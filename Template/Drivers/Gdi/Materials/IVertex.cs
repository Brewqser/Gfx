using EMBC.Mathematics;
namespace EMBC.Drivers.Gdi.Materials
{
    public interface IVertex
    {
        Vector4F Position { get; }
    }

    public interface IVertex<out TVertex> :
        IVertex
        where TVertex : struct, IVertex
    {
        TVertex CloneWithNewPosition(Vector4F position);
    }
}
