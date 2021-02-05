using EMBC.Mathematics;

namespace EMBC.Materials
{
    public struct PrimitiveBehaviour
    {
        #region // storage

        public Space Space { get; }

        #endregion

        #region // ctor

        public PrimitiveBehaviour(Space space)
        {
            Space = space;
        }

        #endregion

        #region // factory

        public static PrimitiveBehaviour Default = new PrimitiveBehaviour(Space.World);

        #endregion
    }
}
