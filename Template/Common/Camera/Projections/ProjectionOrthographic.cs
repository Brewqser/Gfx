﻿using MathNet.Numerics.LinearAlgebra;
using EMBC.Mathematics.Extensions;
using MathNet.Spatial.Euclidean;
using EMBC.Mathematics;

namespace EMBC.Common.Camera.Projections
{
    public class ProjectionOrthographic :
        Projection,
        IProjectionOrthographic
    {
        #region // storage

        public double FieldWidth { get; }

        public double FieldHeight { get; }

        #endregion

        #region // ctor

        public ProjectionOrthographic(double nearPlane, double farPlane, double fieldWidth, double fieldHeight) :
            base(nearPlane, farPlane)
        {
            FieldWidth = fieldWidth;
            FieldHeight = fieldHeight;
        }

        public static IProjectionOrthographic FromDistance(double nearPlane, double farPlane, double cameraPositionToTargetDistance, double aspectRatio)
        {
            return new ProjectionOrthographic(nearPlane, farPlane, cameraPositionToTargetDistance * aspectRatio, cameraPositionToTargetDistance);
        }

        #endregion

        #region // routines

        public override object Clone()
        {
            return new ProjectionOrthographic(NearPlane, FarPlane, FieldWidth, FieldHeight);
        }

        public override Matrix4D GetMatrixProjection()
        {
            return Matrix4DEx.OrthoRH(FieldWidth, FieldHeight, NearPlane, FarPlane);
        }

        public override IProjection GetAdjustedProjection(double aspectRatio)
        {
            return new ProjectionOrthographic(NearPlane, FarPlane, FieldHeight * aspectRatio, FieldHeight);
        }

        public override Ray3D GetMouseRay(ICameraInfo cameraInfo, Point3D mouseWorld)
        {
            return new Ray3D(mouseWorld, cameraInfo.GetEyeDirection());
        }

        #endregion
    }
}
