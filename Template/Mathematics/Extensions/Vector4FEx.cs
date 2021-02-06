using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;

namespace EMBC.Mathematics.Extensions
{
    public static class Vector4FEx
    {
        #region // from

        public static Vector4F ToVector4F(this Vector2F value, float z, float w)
        {
            return new Vector4F(value.X, value.Y, z, w);
        }

        public static Vector4F ToVector4F(this Vector3F value, float w)
        {
            return new Vector4F(value.X, value.Y, value.Z, w);
        }

        public static Vector4F ToVector4F(this in Vector4D value)
        {
            return new Vector4F((float)value.X, (float)value.Y, (float)value.Z, (float)value.W);
        }
        public static Vector4F ToVector4F(this Point2D value, float z, float w)
        {
            return new Vector4F((float)value.X, (float)value.Y, z, w);
        }

        public static Vector4F ToVector4F(this Point3D value, float w)
        {
            return new Vector4F((float)value.X, (float)value.Y, (float)value.Z, w);
        }

        public static Vector4F ToVector4F(this Vector2D value, float z, float w)
        {
            return new Vector4F((float)value.X, (float)value.Y, z, w);
        }

        public static Vector4F ToVector4F(this Vector3D value, float w)
        {
            return new Vector4F((float)value.X, (float)value.Y, (float)value.Z, w);
        }

        public static Vector4F ToVector4F(this Vector4D value)
        {
            return new Vector4F((float)value.X, (float)value.Y, (float)value.Z, (float)value.W);
        }

        #endregion

        #region // to

        public static Vector3F ToVector3FNormalized(this in Vector4F value)
        {
            return new Vector3F(value.X / value.W, value.Y / value.W, value.Z / value.W);
        }

        public static Vector4D ToVector4D(this Vector4F value)
        {
            return new Vector4D(value.X, value.Y, value.Z, value.W);
        }

        public static float[] ToFloats(this in Vector4F value)
        {
            return new[] { value.X, value.Y, value.Z, value.W, };
        }

        public static double[] ToDoubles(this in Vector4F value)
        {
            return new double[] { value.X, value.Y, value.Z, value.W, };
        }

        #endregion

        #region // colors

        public static Vector4F FromRgbaToVector4F(this int value)
        {
            var r = value & 0xFF;
            var g = (value >> 8) & 0xFF;
            var b = (value >> 16) & 0xFF;
            var a = (value >> 24) & 0xFF;
            return new Vector4F(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        public static Vector4F FromArgbToVector4F(this int value)
        {
            var b = value & 0xFF;
            var g = (value >> 8) & 0xFF;
            var r = (value >> 16) & 0xFF;
            var a = (value >> 24) & 0xFF;
            return new Vector4F(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        public static int ToArgb(this Vector4F value)
        {
            var r = (byte)(value.X * byte.MaxValue);
            var g = (byte)(value.Y * byte.MaxValue);
            var b = (byte)(value.Z * byte.MaxValue);
            var a = (byte)(value.W * byte.MaxValue);
            return ((((a << 8) + r) << 8) + g << 8) + b;
        }

        public static int ToRgba(this Vector4F value)
        {
            var r = (byte)(value.X * byte.MaxValue);
            var g = (byte)(value.Y * byte.MaxValue);
            var b = (byte)(value.Z * byte.MaxValue);
            var a = (byte)(value.W * byte.MaxValue);
            return ((((r << 8) + g) << 8) + b << 8) + a;
        }

        public static Vector4F ToVector4F(this System.Drawing.Color value)
        {
            return new Vector4F(value.R / 255f, value.G / 255f, value.B / 255f, value.A / 255f);
        }

        #endregion

        public static Vector4F Transform(this Matrix<double> m, in Vector4F value)
        {
            return new Vector4F
            (
                (float)(m[0, 0] * value.X + m[1, 0] * value.Y + m[2, 0] * value.Z + m[3, 0] * value.W),
                (float)(m[0, 1] * value.X + m[1, 1] * value.Y + m[2, 1] * value.Z + m[3, 1] * value.W),
                (float)(m[0, 2] * value.X + m[1, 2] * value.Y + m[2, 2] * value.Z + m[3, 2] * value.W),
                (float)(m[0, 3] * value.X + m[1, 3] * value.Y + m[2, 3] * value.Z + m[3, 3] * value.W)
            );
        }
    }
}
