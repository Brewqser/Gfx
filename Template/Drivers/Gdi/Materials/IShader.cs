using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials
{
    public interface IShader<TVertexIn, TVertex>
        where TVertexIn : struct
        where TVertex : struct, IVertex
    {
        TVertex VertexShader(in TVertexIn vertex);

        Vector4F? PixelShader(in TVertex vertex);
    }
}
