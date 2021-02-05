using System.Runtime.InteropServices;
using MathNet.Spatial.Euclidean;

namespace EMBC.Materials.Position
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex :
        IVertex
    {
        #region // storage

        public Point3D Position { get; }

        #endregion

        #region // ctor

        public Vertex(Point3D position)
        {
            Position = position;
        }

        #endregion
    }
}
