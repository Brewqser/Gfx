using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using System;

namespace EMBC.Common.Camera
{
    public static class ICameraInfoEx
    {
        public static Vector3D GetEyeVector(this ICameraInfo cameraInfo) => cameraInfo.Target - cameraInfo.Position;

        public static UnitVector3D GetEyeDirection(this ICameraInfo cameraInfo) => cameraInfo.GetEyeVector().Normalize();

        public static Matrix<double> GetTransformationMatrix(this ICameraInfo cameraInfo, Space from, Space to)
        {
            switch (from)
            {
                case Space.World:
                    switch (to)
                    {
                        case Space.World:
                            return MatrixEx.Identity;

                        case Space.View:
                            return cameraInfo.Cache.MatrixViewProjection;

                        case Space.Screen:
                            return cameraInfo.Cache.MatrixViewProjectionViewport;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(to), to, null);
                    }

                case Space.View:
                    switch (to)
                    {
                        case Space.World:
                            return cameraInfo.Cache.MatrixViewProjectionInverse;

                        case Space.View:
                            return MatrixEx.Identity;

                        case Space.Screen:
                            return cameraInfo.Cache.MatrixViewport;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(to), to, null);
                    }

                case Space.Screen:
                    switch (to)
                    {
                        case Space.World:
                            return cameraInfo.Cache.MatrixViewProjectionViewportInverse;

                        case Space.View:
                            return cameraInfo.Cache.MatrixViewportInverse;

                        case Space.Screen:
                            return MatrixEx.Identity;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(to), to, null);
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(from), from, null);
            }
        }

        public static Ray3D GetMouseRay(this ICameraInfo cameraInfo, Point3D mouseWorld)
        {
            return cameraInfo.Projection.GetMouseRay(cameraInfo, mouseWorld);
        }

        public static Ray3D GetMouseRay(this ICameraInfo cameraInfo, Space space, Point3D mouseSpace)
        {
            return cameraInfo.GetMouseRay(cameraInfo.GetTransformationMatrix(space, Space.World).Transform(mouseSpace));
        }
    }
}
