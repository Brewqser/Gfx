using MathNet.Spatial.Euclidean;

namespace EMBC.Materials
{
    public interface IVertex
    {
    }

    public interface IVertexPosition :
        IVertex
    {
        Point3D Position { get; }
    }
}
