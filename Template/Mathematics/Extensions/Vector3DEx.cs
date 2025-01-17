﻿using MathNet.Spatial.Euclidean;

namespace EMBC.Mathematics.Extensions
{
    public static class Vector3DEx
    {
        #region // import

        public static Vector3D ToVector3D(this Point2D value, double z) => new Vector3D(value.X, value.Y, z);

        public static Vector3D ToVector3D(this Vector2D value, double z) => new Vector3D(value.X, value.Y, z);

        public static Vector3D ToVector3D(this Vector2F value, double z) => new Vector3D(value.X, value.Y, z);

        public static Vector3D ToVector3D(this in Point3D value) => new Vector3D(value.X, value.Y, value.Z);

        public static Vector3D ToVector3D(this in Vector3D value) => new Vector3D(value.X, value.Y, value.Z);

        public static Vector3D ToVector3D(this in UnitVector3D value) => new Vector3D(value.X, value.Y, value.Z);

        public static Vector3D ToVector3D(this Vector3F value) => new Vector3D(value.X, value.Y, value.Z);

        public static Vector3D ToVector3D(this in Vector4D value) => new Vector3D(value.X, value.Y, value.Z);

        public static Vector3D ToVector3D(this Vector4F value) => new Vector3D(value.X, value.Y, value.Z);

        public static Vector3D ToVector3DNormalized(this in Vector4D value) => new Vector3D(value.X / value.W, value.Y / value.W, value.Z / value.W);

        public static Vector3D ToVector3DNormalized(this Vector4F value) => new Vector3D(value.X / value.W, value.Y / value.W, value.Z / value.W);

        #endregion

        #region // export

        public static double[] ToDoubles(this Vector3D value) => new[] { value.X, value.Y, value.Z };

        public static float[] ToFloats(this Vector3D value) => new[] { (float)value.X, (float)value.Y, (float)value.Z };

        #endregion

        #region // interpolation

        public static Vector3D InterpolateMultiply(this in Vector3D value, double multiplier)
        {
            return new Vector3D
            (
                value.X.InterpolateMultiply(multiplier),
                value.Y.InterpolateMultiply(multiplier),
                value.Z.InterpolateMultiply(multiplier)
            );
        }

        public static Vector3D InterpolateLinear(this in Vector3D value, in Vector3D other, double alpha)
        {
            return new Vector3D
            (
                value.X.InterpolateLinear(other.X, alpha),
                value.Y.InterpolateLinear(other.Y, alpha),
                value.Z.InterpolateLinear(other.Z, alpha)
            );
        }

        public static Vector3D InterpolateBarycentric(this in Vector3D value, in Vector3D other0, in Vector3D other1, Vector3D barycentric)
        {
            return new Vector3D
            (
                value.X.InterpolateBarycentric(other0.X, other1.X, barycentric),
                value.Y.InterpolateBarycentric(other0.Y, other1.Y, barycentric),
                value.Z.InterpolateBarycentric(other0.Z, other1.Z, barycentric)
            );
        }

        #endregion
    }
}