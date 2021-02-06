using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;

namespace EMBC.Mathematics.Extensions
{
    public static class Vector4DEx
    {
        #region // from

        public static Vector4D ToVector4D(this in Vector2D value, double z, double w)
        {
            return new Vector4D(value.X, value.Y, z, w);
        }

        public static Vector4D ToVector4D(this in Vector3D value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        public static Vector4D ToVector4D(this in Point2D value, double z, double w)
        {
            return new Vector4D(value.X, value.Y, z, w);
        }

        public static Vector4D ToVector4D(this in Point3D value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        public static Vector4D ToVector4D(this in Vector2F value, double z, double w)
        {
            return new Vector4D(value.X, value.Y, z, w);
        }

        public static Vector4D ToVector4D(this in Vector3F value, double w)
        {
            return new Vector4D(value.X, value.Y, value.Z, w);
        }

        #endregion

        #region // to

        public static Vector3D ToVector3DNormalized(this in Vector4D value)
        {
            return new Vector3D(value.X / value.W, value.Y / value.W, value.Z / value.W);
        }

        public static Point3D ToPoint3DNormalized(this in Vector4D value)
        {
            return new Point3D(value.X / value.W, value.Y / value.W, value.Z / value.W);
        }

        public static Vector3D ToVector3D(this in Vector4D value)
        {
            return new Vector3D(value.X, value.Y, value.Z);
        }

        public static Vector2D ToVector2D(this in Vector4D value)
        {
            return new Vector2D(value.X, value.Y);
        }

        public static Point3D ToPoint3D(this in Vector4D value)
        {
            return new Point3D(value.X, value.Y, value.Z);
        }

        public static Point2D ToPoint2D(this in Vector4D value)
        {
            return new Point2D(value.X, value.Y);
        }

        #endregion

        public static Vector4D Transform(this Matrix<double> m, in Vector4D value)
        {
            return new Vector4D
            (
                m[0, 0] * value.X + m[1, 0] * value.Y + m[2, 0] * value.Z + m[3, 0] * value.W,
                m[0, 1] * value.X + m[1, 1] * value.Y + m[2, 1] * value.Z + m[3, 1] * value.W,
                m[0, 2] * value.X + m[1, 2] * value.Y + m[2, 2] * value.Z + m[3, 2] * value.W,
                m[0, 3] * value.X + m[1, 3] * value.Y + m[2, 3] * value.Z + m[3, 3] * value.W
            );
        }
    }
}
