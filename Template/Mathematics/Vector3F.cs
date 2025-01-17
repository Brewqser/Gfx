﻿using System;
using System.Runtime.InteropServices;
using EMBC.Mathematics.Extensions;

namespace EMBC.Mathematics
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vector3F :
        IInterpolateSingle<Vector3F>
    {
        #region // static

        public static readonly Vector3F Zero = new Vector3F(0, 0, 0);

        #endregion

        #region // storage

        public float X { get; }

        public float Y { get; }

        public float Z { get; }

        #endregion

        #region // ctor

        public Vector3F(float x, float y, float z) => (X, Y, Z) = (x, y, z);

        #endregion

        #region // operators

        public static Vector3F operator +(Vector3F left, Vector3F right)
        {
            return new Vector3F
            (
                left.X + right.X,
                left.Y + right.Y,
                left.Z + right.Z
            );
        }

        public static Vector3F operator -(Vector3F left, Vector3F right)
        {
            return new Vector3F
            (
                left.X - right.X,
                left.Y - right.Y,
                left.Z - right.Z
            );
        }

        public static Vector3F operator *(Vector3F left, float right)
        {
            return new Vector3F
            (
                left.X * right,
                left.Y * right,
                left.Z * right
            );
        }

        public static Vector3F operator /(Vector3F left, float right)
        {
            return new Vector3F
            (
                left.X / right,
                left.Y / right,
                left.Z / right
            );
        }

        public static float operator *(Vector3F left, Vector3F right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        public static Vector3F operator ^(Vector3F left, Vector3F right)
        {
            return new Vector3F
            (
                left.Y * right.Z - left.Z * right.Y,
                left.Z * right.X - left.X * right.Z,
                left.X * right.Y - left.Y * right.X
            );
        }

        #endregion

        #region // interpolation

        public Vector3F InterpolateMultiply(float multiplier)
        {
            return new Vector3F
            (
                X.InterpolateMultiply(multiplier),
                Y.InterpolateMultiply(multiplier),
                Z.InterpolateMultiply(multiplier)
            );
        }

        public Vector3F InterpolateLinear(in Vector3F other, float alpha)
        {
            return new Vector3F
            (
                X.InterpolateLinear(other.X, alpha),
                Y.InterpolateLinear(other.Y, alpha),
                Z.InterpolateLinear(other.Z, alpha)
            );
        }

        public Vector3F InterpolateBarycentric(in Vector3F other0, in Vector3F other1, Vector3F barycentric)
        {
            return new Vector3F
            (
                X.InterpolateBarycentric(other0.X, other1.X, barycentric),
                Y.InterpolateBarycentric(other0.Y, other1.Y, barycentric),
                Z.InterpolateBarycentric(other0.Z, other1.Z, barycentric)
            );
        }

        #endregion

        #region // routines

        public override string ToString() => $"{X:0.000000}, {Y:0.000000}, {Z:0.000000}";

        #endregion
    }
}
