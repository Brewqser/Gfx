using System.Runtime.CompilerServices;
using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials.Position
{
    public readonly struct Vertex :
        IVertex<Vertex>
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

        public Vertex CloneWithNewPosition(Vector4F position)
        {
            return new Vertex(position);
        }

        public override string ToString()
        {
            return $"Position: {Position}";
        }

        #endregion
    }
}
