using MathNet.Numerics.LinearAlgebra;
using EMBC.Mathematics.Extensions;

namespace EMBC.Common.Camera.Projections
{
    public class ProjectionOrthographic :
        Projection,
        IProjectionOrthographic
    {
        #region // storage

        public double FieldWidth { get; }

        public double FieldHeight { get; }

        #endregion

        #region // ctor

        public ProjectionOrthographic(double nearPlane, double farPlane, double fieldWidth, double fieldHeight) :
            base(nearPlane, farPlane)
        {
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
        }

        public static IProjectionOrthographic FromDistance(double nearPlane, double farPlane, double cameraPositionToTargetDistance, double aspectRatio)
        {
            return new ProjectionOrthographic(nearPlane, farPlane, cameraPositionToTargetDistance * aspectRatio, cameraPositionToTargetDistance);
        }

        #endregion

        #region // routines

        public override object Clone()
        {
            return new ProjectionOrthographic(NearPlane, FarPlane, FieldWidth, FieldHeight);
        }

        public override Matrix<double> GetMatrixProjection()
        {
            return MatrixEx.OrthoRH(FieldWidth, FieldHeight, NearPlane, FarPlane);
        }

        public override IProjection GetAdjustedProjection(double aspectRatio)
        {
            return new ProjectionOrthographic(NearPlane, FarPlane, FieldHeight * aspectRatio, FieldHeight);
        }

        #endregion
    }
}
