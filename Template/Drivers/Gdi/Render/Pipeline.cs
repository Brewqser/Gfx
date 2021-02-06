using System;
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
                    // TODO:
                    break;

                case PrimitiveTopology.LineStrip:
                    // TODO:
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

        /// <summary>
        /// Pixel (fragment) shader stage.
        /// </summary>
        private void StagePixelShader(int x, int y, ref TVertexShader vertex)
        {
            var color = Shader.PixelShader(vertex);

            // check if was discarded
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

        #endregion
    }
}
