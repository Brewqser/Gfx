using EMBC.Drivers.Gdi.Render;
using EMBC.Materials;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Drivers.Gdi.Materials.Position
{
    public class Shader :
        Shader<VsIn, PsIn>
    {
        private Matrix4D MatrixToClip { get; set; } = Matrix4D.Identity;
        private Vector4F Color { get; set; } = new Vector4F(0, 0, 0, 0);

        public Shader(RenderHost renderHost) :
            base(renderHost)
        {
        }

        public void Update(in Matrix4D matrixToClip, int color)
        {
            MatrixToClip = matrixToClip;
            Color = color.FromRgbaToVector4F();
        }

        public override bool VertexShader(in VsIn vsin, out PsIn vsout)
        {
            vsout = new PsIn(MatrixToClip.Transform(vsin.Position.ToVector4F(1)));
            return true;
        }

        public override bool PixelShader(in PsIn psin, out Vector4F psout)
        {
            if (Color.W <= 0)
            {
                psout = default;
                return false;
            }

            psout = Color;
            return true;
        }
    }
}
