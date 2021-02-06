using System.Runtime.CompilerServices;
using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials.Position
{
    public readonly struct VertexShader :
        IVertexShader<VertexShader>
    {
        #region // storage

        public Vector4F Position { get; }

        #endregion

        #region // ctor

        public VertexShader(Vector4F position)
        {
            Position = position;
        }

        #endregion

        #region // routines

        public VertexShader CloneWithNewPosition(Vector4F position)
        {
            return new VertexShader(position);
        }

        public override string ToString()
        {
            return $"Position: {Position}";
        }

        #endregion
    }
}
