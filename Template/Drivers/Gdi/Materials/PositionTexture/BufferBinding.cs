using EMBC.Drivers.Gdi.Render.Rasterization;
using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials.PositionTexture
{
    public class BufferBinding
        : IBufferBinding<VsIn>
    {
        public Vector3F[] Positions { get; }

        public Vector2F[] TextureCoordinates { get; }

        public BufferBinding(Vector3F[] positions, Vector2F[] textureCoordinates)
        {
            Positions = positions;
            TextureCoordinates = textureCoordinates;
        }
            
        public VsIn GetVsIn(uint index)
        {
            return new VsIn
            (
                Positions[index],
                TextureCoordinates[index]
            );
        }
    }
}
