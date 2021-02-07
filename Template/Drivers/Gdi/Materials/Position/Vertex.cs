using System.Runtime.CompilerServices;
using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials.Position
{
    public readonly struct Vertex :
        IVertex
    {
        #region // storage

        public Vector4F Position { get; }

        #endregion

        #region // ctor

        public Vertex(Vector4F position)
        {
            Position = position;
        }

        #endregion

        #region // routines

        public override string ToString()
        {
            return $"Position: {Position}";
        }

        #endregion
    }
}
