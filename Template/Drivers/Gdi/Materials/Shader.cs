using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials
{
    public abstract class Shader<TVertex, TVertexShader> :
        IShader<TVertex, TVertexShader>
        where TVertex : struct
        where TVertexShader : struct, IVertexShader
    {
        #region // shaders

        public abstract TVertexShader VertexShader(in TVertex vertex);

        public virtual Vector4F? PixelShader(in TVertexShader vertex)
        {
            return new Vector4F(1, 1, 1, 1);
        }

        #endregion
    }
}
