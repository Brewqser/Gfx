using EMBC.Mathematics;

namespace EMBC.Common.Camera
{

    public interface ICameraInfoCache
    {
        Matrix4D MatrixView { get; }

        Matrix4D MatrixViewInverse { get; }

        Matrix4D MatrixProjection { get; }

        Matrix4D MatrixProjectionInverse { get; }

        Matrix4D MatrixViewport { get; }

        Matrix4D MatrixViewportInverse { get; }

        Matrix4D MatrixViewProjection { get; }

        Matrix4D MatrixViewProjectionInverse { get; }

        Matrix4D MatrixViewProjectionViewport { get; }

        Matrix4D MatrixViewProjectionViewportInverse { get; }
    }
}
