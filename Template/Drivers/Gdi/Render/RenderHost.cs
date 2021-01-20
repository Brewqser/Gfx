using EMBC.Win;
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
        private BufferedGraphics BufferedGraphics { get; set; }

        #endregion

        #region //ctor

        public RenderHost(IntPtr hostHandle) : 
            base(hostHandle)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);

            BufferedGraphics = BufferedGraphicsManager.Current.Allocate(GraphicsHost, new Rectangle(Point.Empty, W.GetClientRectangle(HostHandle).Size));

            FontConsloe12 = new Font("Console", 12);
        }

        public override void Dispose()
        {
            FontConsloe12.Dispose();
            FontConsloe12 = default;

            BufferedGraphics.Dispose();
            BufferedGraphics = default;

            GraphicsHost.Dispose();
            GraphicsHost = default;

            base.Dispose(); 
        }

        #endregion

        #region // render

        protected override void RenderInternal()
        {
            BufferedGraphics.Graphics.Clear(Color.Black);
            BufferedGraphics.Graphics.DrawString(FpsCounter.FpsString, FontConsloe12, Brushes.Red, 0, 0);

            BufferedGraphics.Render();
        }

        #endregion

    }
}
