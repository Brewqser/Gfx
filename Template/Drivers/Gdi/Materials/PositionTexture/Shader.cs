using EMBC.Drivers.Gdi.Render;
using EMBC.Drivers.Gdi.Render.Rasterization;
using EMBC.Materials;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Drivers.Gdi.Materials.PositionTexture
{
    public class Shader :
        Shader<VsIn, PsIn>
    {
        private Matrix4D MatrixToClip { get; set; } = Matrix4D.Identity;
        private Texture Texture { get; set; }

        public Shader(RenderHost renderHost) :
            base(renderHost)
        {
        }

        public void Update(in Matrix4D matrixToClip, Texture texture)
        {
            MatrixToClip = matrixToClip;
            Texture = texture;
        }

        public override bool VertexShader(in VsIn vsin, out PsIn vsout)
        {
            vsout = new PsIn
            (
                MatrixToClip.Transform(vsin.Position.ToVector4F(1)),
                vsin.TextureCoordinate
            );
            return true;
        }

        public override bool PixelShader(in PsIn psin, out Vector4F psout)
        {
            psout = TextureSampler.Sample(Texture, psin.TextureCoordinate.X, psin.TextureCoordinate.Y).ToVector4F();

            if (psout.W <= 0)
            {
                psout = default;
                return false;
            }

            return true;
        }
    }
}
