using EMBC.Common.Camera;
using EMBC.Common.Camera.Projections;
using EMBC.Engine.Render;
using EMBC.Inputs;
using EMBC.Utils;

namespace EMBC.Engine.Operators
{
    public class OperatorCameraZoom :
        Operator
    {
        #region // ctor

        public OperatorCameraZoom(IRenderHost renderHost) :
            base(renderHost)
        {
        }

        #endregion

        #region // routines

        protected override void InputOnMouseWheel(object sender, IMouseEventArgs args)
        {
            base.InputOnMouseWheel(sender, args);

            var cameraInfo = RenderHost.CameraInfo;

            const double scale = 0.15;
            const double scaleForward = 1.0 + scale;
            const double scaleBackwards = 2.0 - 1.0 / (1.0 - scale);

            var scaleCurrent = args.WheelDelta > 0 ? scaleForward : scaleBackwards;
            var eyeVector = cameraInfo.Target - cameraInfo.Position;
            var offset = eyeVector.ScaleBy(scaleCurrent) - eyeVector;

            var position = cameraInfo.Position + offset;

            var projection = cameraInfo.Projection is ProjectionOrthographic po
                ? ProjectionOrthographic.FromDistance(po.NearPlane, po.FarPlane, (cameraInfo.Target - position).Length, cameraInfo.Viewport.AspectRatio)
                : cameraInfo.Projection.Cloned();

            RenderHost.CameraInfo = new CameraInfo(position, cameraInfo.Target, cameraInfo.UpVector, projection, cameraInfo.Viewport);
        }

        #endregion
    }
}
