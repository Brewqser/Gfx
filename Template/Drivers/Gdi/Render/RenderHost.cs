using System;
using System.Drawing;

namespace EMBC.Drivers.Gdi.Render
{
    public class RenderHost :
        Engine.Render.RenderHost
    {

        #region //storage

        private Graphics GraphicsHost { get; set; }
        private Font FontConsloe12 { get; set; }

        #endregion

        #region //ctor

        public RenderHost(IntPtr hostHandle) : 
            base(hostHandle)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);

            FontConsloe12 = new Font("Console", 12);
        }

        public override void Dispose()
        {
            FontConsloe12?.Dispose();
            FontConsloe12 = default;

            GraphicsHost.Dispose();
            GraphicsHost = default;

            base.Dispose();
        }

        #endregion

        #region // render

        protected override void RenderInternal()
        {
            GraphicsHost.Clear(Color.Black);
            GraphicsHost.DrawString(FpsCounter.FpsString, FontConsloe12, Brushes.Red, 0, 0);
        }

        #endregion

    }
}
