using System;
using System.Collections.Generic;
using EMBC.Drivers.Gdi.Materials;
using EMBC.Materials;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Drivers.Gdi.Render
{
    public class Pipeline<TVertex, TVertexShader> :
        IPipeline<TVertex, TVertexShader>
        where TVertex : struct
        where TVertexShader : struct, IVertexShader<TVertexShader/* TODO: temporary */>
    {
        #region // singleton

        public static IPipeline<TVertex, TVertexShader> Instance { get; } = new Pipeline<TVertex, TVertexShader>();

        #endregion

        #region // storage

        private RenderHost RenderHost { get; set; }

        private IShader<TVertex, TVertexShader> Shader { get; set; }

        #endregion

        #region // public access

        public void SetRenderHost(RenderHost renderHost) => RenderHost = renderHost;

        public void SetShader(IShader<TVertex, TVertexShader> shader) => Shader = shader;

        public void Render(TVertex[] vertices, PrimitiveTopology primitiveTopology)
        {
            StageVertexShader(vertices, primitiveTopology);
        }

        #endregion

        #region // routines

        private void TransformClipToScreen(ref TVertexShader vertex)
        {
            var positionScreen = RenderHost.CameraInfo.Cache.MatrixViewport
                .Transform(vertex.Position)
                .ToVector3FNormalized()
                .ToVector4F(1);

            vertex = vertex.CloneWithNewPosition(positionScreen);
        }

        #endregion

        #region // stages

        private void StageVertexShader(TVertex[] vertices, PrimitiveTopology primitiveTopology)
        {
            var verticesVsOut = new TVertexShader[vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                verticesVsOut[i] = Shader.VertexShader(vertices[i]);
            }

            StagePrimitiveAssembly(verticesVsOut, primitiveTopology);
        }

        private void StagePrimitiveAssembly(TVertexShader[] vertices, PrimitiveTopology primitiveTopology)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.PointList:
                    for (var i = 0; i < vertices.Length; i++)
                    {
                        RasterizePoint(ref vertices[i]);
                    }
                    break;

                case PrimitiveTopology.LineList:
                    for (var i = 0; i < vertices.Length; i += 2)
                    {
                        RasterizeLine(ref vertices[i], ref vertices[i + 1]);
                    }
                    break;

                case PrimitiveTopology.LineStrip:
                    for (var i = 0; i < vertices.Length - 1; i++)
                    {
                        var copy0 = vertices[i];
                        var copy1 = vertices[i + 1];
                        RasterizeLine(ref copy0, ref copy1);
                    }
                    break;

                case PrimitiveTopology.TriangleList:
                    // TODO:
                    break;

                case PrimitiveTopology.TriangleStrip:
                    // TODO:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveTopology), primitiveTopology, null);
            }
        }

        private void StagePixelShader(int x, int y, ref TVertexShader vertex)
        {
            var color = Shader.PixelShader(vertex);

            if (!color.HasValue)
            {
                return;
            }

            StageOutputMerger(x, y, color.Value);
        }

        private void StageOutputMerger(int x, int y, Vector4F color)
        {
            var backBuffer = RenderHost.BackBuffer;
            var index = backBuffer.GetIndex(x, y);

            if (index < 0 || index >= backBuffer.Buffer.Length)
            {
                return;
            }

            backBuffer.Buffer[index] = color.ToArgb();
        }

        #endregion

        #region // rasterize

        #region // point

        private void RasterizePoint(ref TVertexShader vertex0)
        {
            // TODO: clipping
            TransformClipToScreen(ref vertex0);
            DrawPoint(ref vertex0);
        }

        private void DrawPoint(ref TVertexShader vertex0)
        {
            var x = (int)vertex0.Position.X;
            var y = (int)vertex0.Position.Y;
            StagePixelShader(x, y, ref vertex0);
        }

        #endregion

        #region // line

        private void RasterizeLine(ref TVertexShader vertex0, ref TVertexShader vertex1)
        {
            // TODO: clipping
            TransformClipToScreen(ref vertex0);
            TransformClipToScreen(ref vertex1);
            DrawLine(ref vertex0, ref vertex1);
        }

        private void DrawLine(ref TVertexShader vertex0, ref TVertexShader vertex1)
        {
            var x0 = (int)vertex0.Position.X;
            var y0 = (int)vertex0.Position.Y;
            var x1 = (int)vertex1.Position.X;
            var y1 = (int)vertex1.Position.Y;

            var empty = default(TVertexShader);

            var pixels = BresenhamLine(x0, y0, x1, y1);

            foreach (var point in pixels)
            {
                StagePixelShader(point.X, point.Y, ref empty);
            }
        }

        /// Bresenham's line algorithm line rasterization algorithm.
        /// https://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
        public static IEnumerable<(int X, int Y)> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            var w = x1 - x0;
            var h = y1 - y0;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            var longest = Math.Abs(w);
            var shortest = Math.Abs(h);
            if (longest <= shortest)
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            var numerator = longest >> 1;
            for (var i = 0; i <= longest; i++)
            {
                yield return (x0, y0);
                numerator += shortest;
                if (numerator < longest)
                {
                    x0 += dx2;
                    y0 += dy2;
                }
                else
                {
                    numerator -= longest;
                    x0 += dx1;
                    y0 += dy1;
                }
            }
        }

        #endregion

        #endregion
    }
}
