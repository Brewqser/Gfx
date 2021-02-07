using System;
using System.Collections.Generic;
using EMBC.Drivers.Gdi.Materials;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;
using PrimitiveTopology = EMBC.Materials.PrimitiveTopology;

namespace EMBC.Drivers.Gdi.Render
{
    public class Pipeline<TVertexIn, TVertex> :
        IPipeline<TVertexIn, TVertex>
        where TVertexIn : struct
        where TVertex : struct, IVertex
    {
        #region // singleton

        public static IPipeline<TVertexIn, TVertex> Instance { get; } = new Pipeline<TVertexIn, TVertex>();

        #endregion

        #region // storage

        private RenderHost RenderHost { get; set; }

        private IShader<TVertexIn, TVertex> Shader { get; set; }

        #endregion

        #region // public access

        public void SetRenderHost(RenderHost renderHost) => RenderHost = renderHost;

        public void SetShader(IShader<TVertexIn, TVertex> shader) => Shader = shader;

        public void Render(TVertexIn[] vertices, PrimitiveTopology primitiveTopology)
        {
            StageVertexShader(vertices, primitiveTopology);
        }

        #endregion

        #region // stages

        private void StageVertexShader(TVertexIn[] vertices, PrimitiveTopology primitiveTopology)
        {
            var verticesVsOut = new TVertex[vertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                verticesVsOut[i] = Shader.VertexShader(vertices[i]);
            }

            StageVertexPostProcessing(verticesVsOut, primitiveTopology);
        }

        private void StageVertexPostProcessing(TVertex[] vertices, PrimitiveTopology primitiveTopology)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.PointList:
                    for (var i = 0; i < vertices.Length; i++)
                    {
                        VertexPostProcessingPoint(vertices[i]);
                    }
                    break;

                case PrimitiveTopology.LineList:
                    for (var i = 0; i < vertices.Length; i += 2)
                    {
                        VertexPostProcessingLine(vertices[i], vertices[i + 1]);
                    }
                    break;

                case PrimitiveTopology.LineStrip:
                    for (var i = 0; i < vertices.Length - 1; i++)
                    {
                        VertexPostProcessingLine(vertices[i], vertices[i + 1]);
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

        private void StagePixelShader(int x, int y, in TVertex vertex)
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

        #region // vertex post-processing

        #region // primitives

        private struct PrimitivePoint
        {
            public Vector4F PositionScreen0;
        }

        private struct PrimitiveLine
        {
            public Vector4F PositionScreen0;
            public Vector4F PositionScreen1;
        }

        #endregion

        private void VertexPostProcessing(in TVertex vertex, out Vector4F positionScreen)
        {
            positionScreen = RenderHost.CameraInfo.Cache.MatrixViewport
                .Transform(vertex.Position)
                .ToVector3FNormalized()
                .ToVector4F(1);
        }

        private void VertexPostProcessingPoint(in TVertex vertex0)
        {
            PrimitivePoint primitive;
            VertexPostProcessing(vertex0, out primitive.PositionScreen0);

            RasterizePoint(primitive);
        }

        private void VertexPostProcessingLine(in TVertex vertex0, in TVertex vertex1)
        {
            PrimitiveLine primitive;
            VertexPostProcessing(vertex0, out primitive.PositionScreen0);
            VertexPostProcessing(vertex1, out primitive.PositionScreen1);

            RasterizeLine(primitive);
        }

        #endregion

        #region // rasterization

        #region // point

        private void RasterizePoint(in PrimitivePoint primitive)
        {
            var x = (int)primitive.PositionScreen0.X;
            var y = (int)primitive.PositionScreen0.Y;

            StagePixelShader(x, y, default);
        }

        #endregion

        #region // line

        private void RasterizeLine(in PrimitiveLine primitive)
        {
            var x0 = (int)primitive.PositionScreen0.X;
            var y0 = (int)primitive.PositionScreen0.Y;
            var x1 = (int)primitive.PositionScreen1.X;
            var y1 = (int)primitive.PositionScreen1.Y;

            // get pixel stream
            var pixels = BresenhamLine(x0, y0, x1, y1);

            // draw pixels
            foreach (var pixel in pixels)
            {
                StagePixelShader(pixel.X, pixel.Y, default);
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
