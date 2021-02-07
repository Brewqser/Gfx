using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Drivers.Gdi.Materials.Position
{
    public class Shader :
        Shader<EMBC.Materials.Position.Vertex, Vertex>
    {
        #region // storage

        private Matrix4D MatrixWorldViewProjection { get; set; } = Matrix4D.Identity;

        private Vector4F Color { get; set; } = new Vector4F(0, 0, 0, 0);

        #endregion

        #region // routines

        public void Update(in Matrix4D matrixWorldViewProjection, System.Drawing.Color color)
        {
            MatrixWorldViewProjection = matrixWorldViewProjection;
            Color = color.ToVector4F();
        }

        #endregion

        #region // shaders

        public override Vertex VertexShader(in EMBC.Materials.Position.Vertex vertex)
        {
            return new Vertex
            (
                MatrixWorldViewProjection.Transform(vertex.Position.ToVector4F(1))
            );
        }

        /// <inheritdoc />
        public override Vector4F? PixelShader(in Vertex vertex)
        {
            return Color.W > 0 ? Color : default;
        }

        #endregion
    }
}
