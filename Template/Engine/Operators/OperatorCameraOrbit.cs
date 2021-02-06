using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using EMBC.Common.Camera;
using EMBC.Engine.Render;
using EMBC.Inputs;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;
using EMBC.Utils;

namespace EMBC.Engine.Operators
{
    public class OperatorCameraOrbit :
        Operator
    {
        #region // storage

        private ICameraInfo MouseDownCameraInfo { get; set; }
        private Point3D? MouseDownView { get; set; }
        private Point3D? OrbitOrigin { get; set; }

        #endregion

        #region // ctor

        public OperatorCameraOrbit(IRenderHost renderHost) :
            base(renderHost)
        {
        }

        public override void Dispose()
        {
            MouseDownCameraInfo = default;
            MouseDownView = default;
            OrbitOrigin = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        private static Point3D GetOrbitOrigin(ICameraInfo cameraInfo)
        {
            return cameraInfo.Target;
        }

        protected override void InputOnMouseDown(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseDown(sender, args);

            if (args.Buttons != MouseButtons.Middle || args.ClickCount > 1) return;

            MouseDownCameraInfo = RenderHost.CameraInfo.Cloned();
            MouseDownView = MouseDownCameraInfo.GetTransformationMatrix(Space.Screen, Space.View).Transform(args.Position.ToPoint3D());
            OrbitOrigin = GetOrbitOrigin(MouseDownCameraInfo);
        }

        protected override void InputOnMouseUp(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseUp(sender, args);

            if (args.Buttons != MouseButtons.Middle) return;

            MouseDownCameraInfo = default;
            MouseDownView = default;
            OrbitOrigin = default;
        }

        protected override void InputOnMouseMove(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseMove(sender, args);

            if (!MouseDownView.HasValue || MouseDownCameraInfo == null || !OrbitOrigin.HasValue) return;

            var mouseMoveView = RenderHost.CameraInfo.GetTransformationMatrix(Space.Screen, Space.View).Transform(args.Position.ToPoint3D());
            RenderHost.CameraInfo = Orbit(MouseDownCameraInfo, mouseMoveView - MouseDownView.Value, OrbitOrigin.Value);
        }

        public static ICameraInfo Orbit(ICameraInfo cameraInfoStart, Vector3D mouseOffsetView, Point3D orbitOrigin)
        {
            var eye = cameraInfoStart.Position;
            var target = cameraInfoStart.Target;

            var zAxis = cameraInfoStart.UpVector;
            var yzPlane = new Plane(new Point3D(), cameraInfoStart.GetEyeDirection().ToPoint3D(), zAxis.ToPoint3D());
            var xAxis = yzPlane.Normal;
            var xzPlane = new Plane(new Point3D(), zAxis.ToPoint3D(), xAxis.ToPoint3D());
            var yAxis = xzPlane.Normal;
            var matrixWorldToLocal = Matrix4DEx.CoordinateSystem(new Point3D(), xAxis, yAxis, zAxis);

            orbitOrigin = matrixWorldToLocal.Transform(orbitOrigin);
            eye = matrixWorldToLocal.Transform(eye);
            target = matrixWorldToLocal.Transform(target);

            GetSphereAngles(mouseOffsetView, (target - eye).Normalize(), out var thetaDelta, out var phiDelta);

            var matrixRotationHorizontal = Matrix4DEx.Rotate(UnitVector3D.ZAxis, thetaDelta.Radians).TransformAround(orbitOrigin);
            eye = matrixRotationHorizontal.Transform(eye);
            target = matrixRotationHorizontal.Transform(target);

            var phiPlane = new Plane(eye, target, target + UnitVector3D.ZAxis);
            var matrixRotationVertical = Matrix4DEx.Rotate(phiPlane.Normal, phiDelta.Radians).TransformAround(orbitOrigin);
            eye = matrixRotationVertical.Transform(eye);
            target = matrixRotationVertical.Transform(target);

            var matrixLocalToWorld = matrixWorldToLocal.Inverse();
            eye = matrixLocalToWorld.Transform(eye);
            target = matrixLocalToWorld.Transform(target);

            return new CameraInfo(eye, target, cameraInfoStart.UpVector, cameraInfoStart.Projection.Cloned(), cameraInfoStart.Viewport);
        }

        private static void GetSphereAngles(Vector3D mouseOffsetView, UnitVector3D eyeDirection, out Angle thetaDelta, out Angle phiDelta)
        {
            thetaDelta = new Angle(-mouseOffsetView.X * Math.PI, new Radians());
            phiDelta = new Angle(mouseOffsetView.Y * Math.PI, new Radians());

            var phiStart = UnitVector3D.ZAxis.AngleTo(-eyeDirection);
            var phiEnd = phiStart + phiDelta;

            phiEnd = new Angle(Math.Max(Math.Min(phiEnd.Radians, Math.PI * 0.999), 0.001), new Radians());
            phiDelta = phiEnd - phiStart;
        }

        #endregion
    }
}