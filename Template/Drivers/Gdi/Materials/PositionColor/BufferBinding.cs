using EMBC.Drivers.Gdi.Render.Rasterization;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Drivers.Gdi.Materials.PositionColor
{
    public class BufferBinding
       : IBufferBinding<VsIn>
    {
        public Vector3F[] Positions { get; }

        public int[] Colors { get; }

        public BufferBinding(Vector3F[] positions, int[] colors)
        {
            Positions = positions;
            Colors = colors;
        }

        public VsIn GetVsIn(uint index)
        {
            return new VsIn
            (
                Positions[index],
                Colors[index].FromRgbaToVector4F()
            );
        }
    }
}
