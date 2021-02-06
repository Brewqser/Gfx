using EMBC.Mathematics;

namespace EMBC.Materials
{
    public interface IVertex
    {
    }

    public interface IVertexPosition :
        IVertex
    {
        Vector3F Position { get; }
    }
}
