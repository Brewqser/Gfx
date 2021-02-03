using System;
using System.Collections.Generic;
using MathNet.Spatial.Euclidean;
using EMBC.Common.Camera;
using EMBC.Engine.Render;
using EMBC.Utils;

namespace EMBC.Client
{
    internal class Program :
        System.Windows.Application, 
        IDisposable
    {

        #region //storage

        private IReadOnlyList<IRenderHost> RenderHosts { get; set; }

        #endregion

        #region // ctor

        public Program()
        {
            Startup += (sender, args) => Ctor();
            Exit += (sender, args) => Dispose();
        }

        private void Ctor()
        {
            RenderHosts = WindowFactory.SeedWindows();

            while (!Dispatcher.HasShutdownStarted)
            {
                DebugCameras(RenderHosts);

                Render(RenderHosts);
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public void Dispose()
        {
            RenderHosts.ForEach(host => host.Dispose());

            RenderHosts = default;
        }

        #endregion

        #region //routines

        private static void Render(IEnumerable<IRenderHost> renderhosts)
        {
            renderhosts.ForEach(rh => rh.Render());
        }

        private static void DebugCameras(IReadOnlyList<IRenderHost> renderHosts)
        {
            var utcNow = DateTime.UtcNow;
            const int radius = 2;

            for (var i = 0; i < renderHosts.Count; i++)
            {
                var t = Drivers.Gdi.Render.RenderHost.GetDeltaTime(utcNow, new TimeSpan(0, 0, 0, i % 2 == 0 ? 10 : 30));
                var angle = t * Math.PI * 2;
                angle *= i % 2 == 0 ? 1 : -1;

                var cameraInfo = renderHosts[i].CameraInfo;
                renderHosts[i].CameraInfo = new CameraInfo
                (
                    new Point3D(Math.Sin(angle) * radius, Math.Cos(angle) * radius, 1),
                    new Point3D(0, 0, 0),
                    cameraInfo.UpVector,
                    cameraInfo.Projection,
                    cameraInfo.Viewport
                );
            }
        }
        #endregion
    }
}
