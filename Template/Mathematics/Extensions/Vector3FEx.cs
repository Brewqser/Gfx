﻿using MathNet.Spatial.Euclidean;

namespace EMBC.Mathematics.Extensions
{
    public static class Vector3FEx
    {
        #region // import

        public static Vector3F ToVector3F(this Point2D value, float z) => new Vector3F((float)value.X, (float)value.Y, z);

        public static Vector3F ToVector3F(this Vector2D value, float z) => new Vector3F((float)value.X, (float)value.Y, z);

        public static Vector3F ToVector3F(this Vector2F value, float z) => new Vector3F(value.X, value.Y, z);

        public static Vector3F ToVector3F(this in Point3D value) => new Vector3F((float)value.X, (float)value.Y, (float)value.Z);

        public static Vector3F ToVector3F(this in Vector3D value) => new Vector3F((float)value.X, (float)value.Y, (float)value.Z);

        public static Vector3F ToVector3F(this in UnitVector3D value) => new Vector3F((float)value.X, (float)value.Y, (float)value.Z);

        public static Vector3F ToVector3F(this Vector3F value) => new Vector3F(value.X, value.Y, value.Z);

        public static Vector3F ToVector3F(this in Vector4D value) => new Vector3F((float)value.X, (float)value.Y, (float)value.Z);

        public static Vector3F ToVector3F(this Vector4F value) => new Vector3F(value.X, value.Y, value.Z);

        public static Vector3F ToVector3FNormalized(this in Vector4D value) => new Vector3F((float)(value.X / value.W), (float)(value.Y / value.W), (float)(value.Z / value.W));

        public static Vector3F ToVector3FNormalized(this Vector4F value) => new Vector3F(value.X / value.W, value.Y / value.W, value.Z / value.W);

        #endregion

        #region // export

        public static double[] ToDoubles(this Vector3F value) => new double[] { value.X, value.Y, value.Z };

        public static float[] ToFloats(this Vector3F value) => new[] { value.X, value.Y, value.Z };

        #endregion
    }
}

