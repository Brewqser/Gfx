using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials
{
    public abstract class Shader<TVertexIn, TVertex> :
       IShader<TVertexIn, TVertex>
       where TVertexIn : struct
       where TVertex : struct, IVertex
    {
        #region // shaders

        public abstract TVertex VertexShader(in TVertexIn vertex);

        public virtual Vector4F? PixelShader(in TVertex vertex)
        {
            return new Vector4F(1, 1, 1, 1);
        }

        #endregion
    }
}
