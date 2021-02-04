using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;

namespace EMBC.Common.Camera.Projections
{
    public interface IProjection :
        ICloneable
    {
        double NearPlane { get; }

        double FarPlane { get; }

        Matrix<double> GetMatrixProjection();

        IProjection GetAdjustedProjection(double aspectRatio);

        Ray3D GetMouseRay(ICameraInfo cameraInfo, Point3D mouseWorld);
    }
}
