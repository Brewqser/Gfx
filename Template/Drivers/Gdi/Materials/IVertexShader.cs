using EMBC.Mathematics;
namespace EMBC.Drivers.Gdi.Materials
{
    public interface IVertexShader
    {
        Vector4F Position { get; }
    }

    public interface IVertexShader<out TVertexShader> :
        IVertexShader
        where TVertexShader : struct, IVertexShader
    {
        TVertexShader CloneWithNewPosition(Vector4F position);
    }
}
