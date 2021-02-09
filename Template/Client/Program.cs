using System;
using System.Collections.Generic;
using EMBC.Engine.Render;
using EMBC.Utils;
using EMBC.Materials;

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

            RenderHosts.ForEach(rh => rh.HostInput.KeyDown += (sender, args) => Seed.HostInputOnKeyDown(args, rh));

            while (!Dispatcher.HasShutdownStarted)
            {

                Render(RenderHosts, Seed.GetModels());
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

        private static void Render(IEnumerable<IRenderHost> renderHosts, IEnumerable<IModel> models)
        {
            renderHosts.ForEach(rh => rh.Render(models));
            
        }

        #endregion
    }
}
