using MathNet.Numerics.LinearAlgebra;

namespace EMBC.Common.Camera
{

    public interface ICameraInfoCache
    {
        Matrix<double> MatrixView { get; }

        Matrix<double> MatrixViewInverse { get; }

        Matrix<double> MatrixProjection { get; }

        Matrix<double> MatrixProjectionInverse { get; }

        Matrix<double> MatrixViewport { get; }

        Matrix<double> MatrixViewportInverse { get; }

        Matrix<double> MatrixViewProjection { get; }

        Matrix<double> MatrixViewProjectionInverse { get; }

        Matrix<double> MatrixViewProjectionViewport { get; }

        Matrix<double> MatrixViewProjectionViewportInverse { get; }
    }
}
