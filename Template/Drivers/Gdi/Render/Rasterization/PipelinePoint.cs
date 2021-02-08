using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Render.Rasterization
{
    public partial class Pipeline<TVsIn, TPsIn>
    {
        internal struct PrimitivePoint
        {
            public TPsIn PsIn0;
            public Vector4F PositionScreen0;
        }

        private void VertexPostProcessingPoint(ref TPsIn psin)
        {
            PrimitivePoint primitive;
            primitive.PsIn0 = psin;
            VertexPostProcessing(ref primitive.PsIn0, out primitive.PositionScreen0);

            RasterizePoint(primitive);
        }

        private void RasterizePoint(in PrimitivePoint primitive)
        {
            var x = (int)primitive.PositionScreen0.X;
            var y = (int)primitive.PositionScreen0.Y;
            var z = primitive.PositionScreen0.Z;

            StagePixelShader(x, y, z, primitive.PsIn0);
        }
    }
}
