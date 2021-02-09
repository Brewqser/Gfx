using System;
using System.Threading;
using System.Threading.Tasks;
using EMBC.Common.Camera;
using EMBC.Common.Camera.Projections;
using EMBC.Engine.Render;
using EMBC.Utils;

namespace EMBC.Client
{
    public static class SeedProjectionTransition
    {
        private static TimeSpan TRANSITION_DURATION { get; } = new TimeSpan(0, 0, 0, 0, 2000);

        private static TimeSpan TRANSITION_SLEEP { get; } = new TimeSpan(0, 0, 0, 0, 5);

        private static double Activate(double value)
        {
            var valueSin = Math.Sin(-(Math.PI * 0.5) + value * Math.PI);

            return (valueSin + 1) * 0.5;
        }

        public static void Switch(IRenderHost renderHost)
        {
            // get new projection
            switch (renderHost.CameraInfo.Projection)
            {
                case IProjectionOrthographic po:
                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        new ProjectionPerspective
                        (
                            po.NearPlane,
                            po.FarPlane,
                            Math.PI / 2,
                            renderHost.CameraInfo.Viewport.AspectRatio
                        ),
                        renderHost.CameraInfo.Viewport
                    );
                    break;

                case IProjectionPerspective pp:
                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        ProjectionOrthographic.FromDistance
                        (
                            pp.NearPlane,
                            pp.FarPlane,
                            (renderHost.CameraInfo.Target - renderHost.CameraInfo.Position).Length,
                            renderHost.CameraInfo.Viewport.AspectRatio
                        ),
                        renderHost.CameraInfo.Viewport
                    );
                    break;

                case IProjectionCombined _:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(renderHost.CameraInfo.Projection));
            }
        }

        private static void LaunchTransition(IRenderHost renderHost)
        {
            var synchronizationContextSTA = SynchronizationContext.Current;

            var projectionEndType = default(Type);
            var transitionStarted = DateTime.UtcNow;

            Task.Factory.StartNew(() =>
            {
                var stop = false;
                while (!stop)
                {
                    synchronizationContextSTA.Send(state =>
                    {
                        if (renderHost.CameraInfo is null)
                        {
                            return;
                        }

                        if (renderHost.CameraInfo.Projection is IProjectionCombined pc)
                        {
                            var dateTimeCurrent = DateTime.UtcNow;
                            var projectionEndTypeCurrent = pc.Projection1.GetType();

                            double GetTransitionProgress() => ((dateTimeCurrent - transitionStarted).TotalMilliseconds / TRANSITION_DURATION.TotalMilliseconds).Clamp(0, 1);

                            if (projectionEndType is null)
                            {
                                projectionEndType = projectionEndTypeCurrent;
                            }

                            if (projectionEndType != projectionEndTypeCurrent)
                            {
                                transitionStarted = dateTimeCurrent - new TimeSpan((long)(TRANSITION_DURATION.Ticks * (1.0 - GetTransitionProgress())));
                                projectionEndType = projectionEndTypeCurrent;
                            }

                            var progress = Activate(GetTransitionProgress());
                            if (progress < 1)
                            {
                                renderHost.CameraInfo = new CameraInfo
                                (
                                    renderHost.CameraInfo.Position,
                                    renderHost.CameraInfo.Target,
                                    renderHost.CameraInfo.UpVector,
                                    new ProjectionCombined(pc.Projection0, pc.Projection1, 1 - progress),
                                    renderHost.CameraInfo.Viewport
                                );
                            }
                            else
                            {
                                renderHost.CameraInfo = new CameraInfo
                                (
                                    renderHost.CameraInfo.Position,
                                    renderHost.CameraInfo.Target,
                                    renderHost.CameraInfo.UpVector,
                                    pc.Projection1,
                                    renderHost.CameraInfo.Viewport
                                );
                                stop = true;
                            }
                        }
                        else
                        {
                            stop = true;
                        }
                    }, null);

                    Thread.Sleep(TRANSITION_SLEEP);
                }
            });
        }

        public static void Transit(IRenderHost renderHost)
        {
            switch (renderHost.CameraInfo.Projection)
            {
                case IProjectionOrthographic po:
                    #region // orthographic -> perspective

                    var po2p = new ProjectionPerspective
                    (
                        po.NearPlane,
                        po.FarPlane,
                        Math.PI / 2,
                        renderHost.CameraInfo.Viewport.AspectRatio
                    );
                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        new ProjectionCombined(po, po2p, 1),
                        renderHost.CameraInfo.Viewport
                    );
                    LaunchTransition(renderHost);
                    break;

                #endregion

                case IProjectionPerspective pp:
                    #region // perspective -> orthographic

                    var pp2o = ProjectionOrthographic.FromDistance
                    (
                        pp.NearPlane,
                        pp.FarPlane,
                        (renderHost.CameraInfo.Target - renderHost.CameraInfo.Position).Length,
                        renderHost.CameraInfo.Viewport.AspectRatio
                    );
                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        new ProjectionCombined(pp, pp2o, 1),
                        renderHost.CameraInfo.Viewport
                    );
                    LaunchTransition(renderHost);
                    break;

                #endregion

                case IProjectionCombined pc:
                    #region // in transition at the moment -> flip direction

                    renderHost.CameraInfo = new CameraInfo
                    (
                        renderHost.CameraInfo.Position,
                        renderHost.CameraInfo.Target,
                        renderHost.CameraInfo.UpVector,
                        new ProjectionCombined(pc.Projection1, pc.Projection0, pc.Weight1),
                        renderHost.CameraInfo.Viewport
                    );
                    break;

                #endregion

                default:
                    throw new ArgumentOutOfRangeException(nameof(renderHost.CameraInfo.Projection));
            }
        }
    }
}
