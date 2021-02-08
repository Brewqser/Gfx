using System;
using EMBC.Drivers.Gdi.Materials;
using EMBC.Materials;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Drivers.Gdi.Render.Rasterization
{
    public unsafe partial class Pipeline<TVsIn, TPsIn> :
        IPipeline<TVsIn, TPsIn>
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        #region // storage

        private IShader<TVsIn, TPsIn> Shader { get; set; }

        private RenderHost RenderHost => Shader.RenderHost;

        #endregion

        #region // ctor

        public Pipeline(IShader<TVsIn, TPsIn> shader)
        {
            Shader = shader;
        }

        public void Dispose()
        {
            Shader = default;
        }

        #endregion

        #region // public access

        public void Render(IBufferBinding<TVsIn> bufferBinding, int countVertices, PrimitiveTopology primitiveTopology)
        {
            void Point(uint index0)
            {
                var vsin = stackalloc TVsIn[1];
                *vsin = bufferBinding.GetVsIn(index0);
                StageVertexShader(vsin, 1, primitiveTopology);
            }
            void Line(uint index0, uint index1)
            {
                var vsin = stackalloc TVsIn[2];
                *vsin = bufferBinding.GetVsIn(index0);
                *(vsin + 1) = bufferBinding.GetVsIn(index1);
                StageVertexShader(vsin, 2, primitiveTopology);
            }
            void Triangle(uint index0, uint index1, uint index2)
            {
                var vsin = stackalloc TVsIn[3];
                *vsin = bufferBinding.GetVsIn(index0);
                *(vsin + 1) = bufferBinding.GetVsIn(index1);
                *(vsin + 2) = bufferBinding.GetVsIn(index2);
                StageVertexShader(vsin, 3, primitiveTopology);
            }

            TraversePrimitives(primitiveTopology, countVertices, Point, Line, Triangle);
        }

        #endregion

        #region // routines

        private delegate void ProcessPointDelegate(uint index0);

        private delegate void ProcessLineDelegate(uint index0, uint index1);

        private delegate void ProcessTriangleDelegate(uint index0, uint index1, uint index2);

        private static void TraversePrimitives(PrimitiveTopology primitiveTopology, int countVertices,
            ProcessPointDelegate processPoint, ProcessLineDelegate processLine, ProcessTriangleDelegate processTriangle)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.PointList:
                    for (var i = 0u; i < countVertices; i++)
                    {
                        processPoint(i);
                    }
                    break;

                case PrimitiveTopology.LineList:
                    for (var i = 0u; i < countVertices; i += 2)
                    {
                        processLine(i, i + 1);
                    }
                    break;

                case PrimitiveTopology.LineStrip:
                    for (var i = 0u; i < countVertices - 1; i++)
                    {
                        processLine(i, i + 1);
                    }
                    break;

                case PrimitiveTopology.TriangleList:
                    for (var i = 0u; i < countVertices; i += 3)
                    {
                        processTriangle(i, i + 1, i + 2);
                    }
                    break;

                case PrimitiveTopology.TriangleStrip:
                    var flip = false;
                    for (var i = 0u; i < countVertices - 2; i++)
                    {
                        if (flip)
                        {
                            processTriangle(i, i + 2, i + 1);
                        }
                        else
                        {
                            processTriangle(i, i + 1, i + 2);
                        }
                        flip = !flip;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(primitiveTopology), primitiveTopology, default);
            }
        }

        private bool DepthTest(int index, float z)
        {
            ref var refDepth = ref RenderHost.FrameBuffers.BufferDepth.Data[index];

            if (refDepth < z)
            {
                return false;
            }

            refDepth = z;
            return true;
        }

        #endregion

        #region // stages

        private void StageVertexShader(TVsIn* vsin, int count, PrimitiveTopology primitiveTopology)
        {
            var psin = stackalloc TPsIn[count];
            for (var i = 0; i < count; i++)
            {
                Shader.VertexShader(*(vsin + i), out *(psin + i));
            }
            StageVertexPostProcessing(psin, count, primitiveTopology);
        }

        private void StageVertexPostProcessing(TPsIn* psin, int count, PrimitiveTopology primitiveTopology)
        {
            void Point(uint index0)
            {
                VertexPostProcessingPoint(ref *(psin + index0));
            }
            void Line(uint index0, uint index1)
            {
                VertexPostProcessingLine(ref *(psin + index0), ref *(psin + index1));
            }
            void Triangle(uint index0, uint index1, uint index2)
            {
                VertexPostProcessingTriangle(ref *(psin + index0), ref *(psin + index1), ref *(psin + index2));
            }

            TraversePrimitives(primitiveTopology, count, Point, Line, Triangle);
        }

        private void StagePixelShader(int x, int y, in TPsIn psin)
        {
            if (x < 0 || y < 0 || x >= RenderHost.FrameBuffers.Size.Width || y >= RenderHost.FrameBuffers.Size.Height)
            {
                return;
            }

            var success = Shader.PixelShader(psin, out var psout);

            if (!success)
            {
                return;
            }

            StageOutputMerger(x, y, psout);
        }

        private void StageOutputMerger(int x, int y, Vector4F psout)
        {
            RenderHost.FrameBuffers.BufferColor[0].Write(x, y, psout.ToArgb());
        }

        private void StagePixelShader(int x, int y, float z, in TPsIn psin)
        {
            if (x < 0 || y < 0 || x >= RenderHost.FrameBuffers.Size.Width || y >= RenderHost.FrameBuffers.Size.Height)
            {
                return;
            }

            var success = Shader.PixelShader(psin, out var psout);

            if (!success)
            {
                return;
            }

            StageOutputMerger(x, y, z, psout);
        }

        private void StageOutputMerger(int x, int y, float z, Vector4F psout)
        {
            var index = RenderHost.FrameBuffers.BufferDepth.GetIndex(x, y);

            if (!DepthTest(index, z)) return;

            RenderHost.FrameBuffers.BufferColor[0].Write(index, psout.ToArgb());
        }


        #endregion

        #region // vertex post-processing

        private void VertexPostProcessing(ref TPsIn psin, out Vector4F positionScreen)
        {
            var ndc = psin.Position / psin.Position.W;
            psin = psin.ReplacePosition(ndc);

            positionScreen = RenderHost.CameraInfo.Cache.MatrixViewport.Transform(psin.Position);
        }

        #endregion
    }
}
