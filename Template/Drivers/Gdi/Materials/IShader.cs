using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials
{
    public interface IShader<TVertex, TVertexShader>
        where TVertex : struct
        where TVertexShader : struct, IVertexShader
    {
        TVertexShader VertexShader(in TVertex vertex);

        Vector4F? PixelShader(in TVertexShader vertex);
    }
}
