using System;
using EMBC.Mathematics;
using MathNet.Spatial.Euclidean;

namespace EMBC.Common.Camera.Projections
{
    public interface IProjection :
        ICloneable
    {
        double NearPlane { get; }

        double FarPlane { get; }

        Matrix4D GetMatrixProjection();

        IProjection GetAdjustedProjection(double aspectRatio);

        Ray3D GetMouseRay(ICameraInfo cameraInfo, Point3D mouseWorld);
    }
}
