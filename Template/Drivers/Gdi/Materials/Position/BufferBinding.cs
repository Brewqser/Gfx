using EMBC.Drivers.Gdi.Render.Rasterization;
using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials.Position
{
    public class BufferBinding
        : IBufferBinding<VsIn>
    {
        public Vector3F[] Positions { get; }

        public BufferBinding(Vector3F[] positions)
        {
            Positions = positions;
        }

        public VsIn GetVsIn(uint index)
        {
            return new VsIn
            (
                Positions[index]
            );
        }
    }
}
