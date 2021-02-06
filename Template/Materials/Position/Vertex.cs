using System.Runtime.InteropServices;
using EMBC.Mathematics;

namespace EMBC.Materials.Position
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex :
        IVertex
    {
        #region // storage

        public Vector3F Position { get; }

        #endregion

        #region // ctor

        public Vertex(Vector3F position)
        {
            Position = position;
        }

        #endregion
    }
}
