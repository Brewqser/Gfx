using MathNet.Numerics.LinearAlgebra;
using EMBC.Mathematics.Extensions;

namespace EMBC.Common.Camera.Projections
{
    public class ProjectionPerspective :
        Projection,
        IProjectionPerspective
    {
        #region // routines

        public double FieldOfViewY { get; }

        public double AspectRatio { get; }

        #endregion

        #region // ctor

        public ProjectionPerspective(double nearPlane, double farPlane, double fieldOfViewY, double aspectRatio) :
            base(nearPlane, farPlane)
        {
            FieldOfViewY = fieldOfViewY;
            AspectRatio = aspectRatio;
        }

        #endregion

        #region // routines

        public override object Clone()
        {
            return new ProjectionPerspective(NearPlane, FarPlane, FieldOfViewY, AspectRatio);
        }

        public override Matrix<double> GetMatrixProjection()
        {
            return MatrixEx.PerspectiveFovRH(FieldOfViewY, AspectRatio, NearPlane, FarPlane);
        }

        public override IProjection GetAdjustedProjection(double aspectRatio)
        {
            return new ProjectionPerspective(NearPlane, FarPlane, FieldOfViewY, aspectRatio);
        }

        #endregion
    }
}
