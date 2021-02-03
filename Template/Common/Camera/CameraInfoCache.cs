using MathNet.Numerics.LinearAlgebra;
using EMBC.Mathematics.Extensions;

namespace EMBC.Common.Camera
{
    public class CameraInfoCache :
        ICameraInfoCache
    {
        #region // storage

        public Matrix<double> MatrixView { get; }

        public Matrix<double> MatrixViewInverse { get; }

        public Matrix<double> MatrixProjection { get; }

        public Matrix<double> MatrixProjectionInverse { get; }

        public Matrix<double> MatrixViewport { get; }

        public Matrix<double> MatrixViewportInverse { get; }

        public Matrix<double> MatrixViewProjection { get; }

        public Matrix<double> MatrixViewProjectionInverse { get; }

        public Matrix<double> MatrixViewProjectionViewport { get; }

        public Matrix<double> MatrixViewProjectionViewportInverse { get; }

        #endregion

        #region // ctor

        public CameraInfoCache(ICameraInfo cameraInfo)
        {
            MatrixView = MatrixEx.LookAtRH(cameraInfo.Position.ToVector3D(), cameraInfo.Target.ToVector3D(), cameraInfo.UpVector);
            MatrixViewInverse = MatrixView.Inverse();

            MatrixProjection = cameraInfo.Projection.GetMatrixProjection();
            MatrixProjectionInverse = MatrixProjection.Inverse();

            MatrixViewport = MatrixEx.Viewport(cameraInfo.Viewport);
            MatrixViewportInverse = MatrixViewport.Inverse();

            MatrixViewProjection = MatrixView * MatrixProjection;
            MatrixViewProjectionInverse = MatrixViewProjection.Inverse();

            MatrixViewProjectionViewport = MatrixViewProjection * MatrixViewport;
            MatrixViewProjectionViewportInverse = MatrixViewProjectionViewport.Inverse();
        }

        #endregion
    }
}
