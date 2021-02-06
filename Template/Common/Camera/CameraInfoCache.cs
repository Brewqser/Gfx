using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;

namespace EMBC.Common.Camera
{
    public class CameraInfoCache :
        ICameraInfoCache
    {
        #region // storage

        public Matrix4D MatrixView { get; }

        public Matrix4D MatrixViewInverse { get; }

        public Matrix4D MatrixProjection { get; }

        public Matrix4D MatrixProjectionInverse { get; }

        public Matrix4D MatrixViewport { get; }

        public Matrix4D MatrixViewportInverse { get; }

        public Matrix4D MatrixViewProjection { get; }

        public Matrix4D MatrixViewProjectionInverse { get; }

        public Matrix4D MatrixViewProjectionViewport { get; }

        public Matrix4D MatrixViewProjectionViewportInverse { get; }

        #endregion

        #region // ctor

        public CameraInfoCache(ICameraInfo cameraInfo)
        {
            MatrixView = Matrix4DEx.LookAtRH(cameraInfo.Position.ToVector3D(), cameraInfo.Target.ToVector3D(), cameraInfo.UpVector);
            MatrixViewInverse = MatrixView.Inverse();

            MatrixProjection = cameraInfo.Projection.GetMatrixProjection();
            MatrixProjectionInverse = MatrixProjection.Inverse();

            MatrixViewport = Matrix4DEx.Viewport(cameraInfo.Viewport);
            MatrixViewportInverse = MatrixViewport.Inverse();

            MatrixViewProjection = MatrixView * MatrixProjection;
            MatrixViewProjectionInverse = MatrixViewProjection.Inverse();

            MatrixViewProjectionViewport = MatrixViewProjection * MatrixViewport;
            MatrixViewProjectionViewportInverse = MatrixViewProjectionViewport.Inverse();
        }

        #endregion
    }
}
