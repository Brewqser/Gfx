using MathNet.Numerics.LinearAlgebra;

namespace EMBC.Common.Camera.Projections
{
    public abstract class Projection :
        IProjection
    {
        #region // storage

        public double NearPlane { get; }

        public double FarPlane { get; }

        #endregion

        #region // ctor

        protected Projection(double nearPlane, double farPlane)
        {
            NearPlane = nearPlane;
            FarPlane = farPlane;
        }

        #endregion

        #region // routines

        public abstract object Clone();

        public abstract Matrix<double> GetMatrixProjection();

        public abstract IProjection GetAdjustedProjection(double aspectRatio);

        #endregion
    }
}
