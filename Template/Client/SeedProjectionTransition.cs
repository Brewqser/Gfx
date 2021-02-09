using System;
using EMBC.Common.Camera;
using EMBC.Common.Camera.Projections;
using EMBC.Engine.Render;

namespace EMBC.Client
{
    public static class SeedProjectionTransition
    {
        public static void Switch(IRenderHost renderHost)
        {
            IProjection projectionNew;
            switch (renderHost.CameraInfo.Projection)
            {
                case IProjectionOrthographic po:
                    projectionNew = new ProjectionPerspective
                    (
                        po.NearPlane,
                        po.FarPlane,
                        Math.PI / 2,
                        renderHost.CameraInfo.Viewport.AspectRatio
                    );
                    break;

                case IProjectionPerspective pp:
                    projectionNew = ProjectionOrthographic.FromDistance
                    (
                        pp.NearPlane,
                        pp.FarPlane,
                        (renderHost.CameraInfo.Target - renderHost.CameraInfo.Position).Length,
                        renderHost.CameraInfo.Viewport.AspectRatio
                    );
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(renderHost.CameraInfo.Projection));
            }

            renderHost.CameraInfo = new CameraInfo
            (
                renderHost.CameraInfo.Position,
                renderHost.CameraInfo.Target,
                renderHost.CameraInfo.UpVector,
                projectionNew,
                renderHost.CameraInfo.Viewport
            );
        }
    }
}
