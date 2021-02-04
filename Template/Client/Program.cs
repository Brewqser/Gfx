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

        #endregion
    }
}
