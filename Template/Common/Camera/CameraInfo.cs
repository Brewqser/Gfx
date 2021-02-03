using MathNet.Spatial.Euclidean;
using EMBC.Common.Camera.Projections;
using EMBC.Utils;

namespace EMBC.Common.Camera
{
    public class CameraInfo :
        ICameraInfo
    {
        #region // storage

        public Point3D Position { get; }

        public Point3D Target { get; }

        public UnitVector3D UpVector { get; }

        public IProjection Projection { get; }

        public Viewport Viewport { get; }

        public ICameraInfoCache Cache { get; }

        #endregion

        #region // ctor

        public CameraInfo(in Point3D position, in Point3D target, in UnitVector3D upVector, in IProjection projection, in Viewport viewport)
        {
            Position = position;
            Target = target;
            UpVector = upVector;
            Projection = projection;
            Viewport = viewport;
            Cache = new CameraInfoCache(this);
        }

        #endregion

        #region // routines

        public object Clone()
        {
            return new CameraInfo(Position, Target, UpVector, Projection.Cloned(), Viewport);
        }

        #endregion
    }
}
