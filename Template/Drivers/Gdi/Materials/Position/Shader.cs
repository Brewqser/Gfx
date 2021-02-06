using MathNet.Numerics.LinearAlgebra;
using EMBC.Materials.Position;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Drivers.Gdi.Materials.Position
{
    public class Shader :
        Shader<Vertex, VertexShader>
    {
        #region // storage

        private Matrix<double> MatrixWorldViewProjection { get; set; } = MatrixEx.Identity;

        private Vector4F Color { get; set; } = new Vector4F(0, 0, 0, 0);

        #endregion

        #region // routines

        public void Update(Matrix<double> matrixWorldViewProjection, System.Drawing.Color color)
        {
            MatrixWorldViewProjection = matrixWorldViewProjection;
            Color = color.ToVector4F();
        }

        #endregion

        #region // shaders

        public override VertexShader VertexShader(in Vertex vertex)
        {
            return new VertexShader
            (
                MatrixWorldViewProjection.Transform(vertex.Position.ToVector4F(1))
            );
        }

        public override Vector4F? PixelShader(in VertexShader vertex)
        {
            return Color.W > 0 ? Color : default;
        }

        #endregion
    }
}
