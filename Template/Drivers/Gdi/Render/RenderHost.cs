using EMBC.Engine.Render;
using EMBC.Utils;
using EMBC.Win;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;

namespace EMBC.Drivers.Gdi.Render
{
    public class RenderHost :
        Engine.Render.RenderHost
    {

        #region //storage

        private Graphics GraphicsHost { get; set; }
        private Font FontConsloe12 { get; set; }
        private BufferedGraphics BufferedGraphics { get; set; }
        private IntPtr GraphicsHostDeviceContext { get; set; }
        private DirectBitmap BackBuffer { get; set; }

        #endregion

        #region //ctor

        public RenderHost(IRenderHostSetup renderHostSetup) : 
            base(renderHostSetup)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);
            GraphicsHostDeviceContext = GraphicsHost.GetHdc();
            CreateSurface(HostInput.Size);
            CreateBuffers(BufferSize);

            FontConsloe12 = new Font("Console", 12);
        }

        public override void Dispose()
        {
            FontConsloe12.Dispose();
            FontConsloe12 = default;

            DisposeBuffers();
            DisposeSurface();

            GraphicsHost.ReleaseHdc(GraphicsHostDeviceContext);
            GraphicsHostDeviceContext = default;

            GraphicsHost.Dispose();
            GraphicsHost = default;

            base.Dispose(); 
        }

        #endregion


        #region // routines

        protected override void ResizeHost(Size size)
        {
            base.ResizeBuffers(size);

            DisposeBuffers();
            CreateBuffers(size);
        }

        protected override void ResizeBuffers(Size size)
        {
            base.ResizeBuffers(size);

            DisposeBuffers();
            CreateBuffers(size);
        }

        private void CreateBuffers(Size size)
        {
            BackBuffer = new DirectBitmap(size);
        }

        private void DisposeBuffers()
        {
            BackBuffer.Dispose();
            BackBuffer = default;
        }

        private void CreateSurface(Size size)
        {
            BufferedGraphics = BufferedGraphicsManager.Current.Allocate(GraphicsHostDeviceContext, new Rectangle(Point.Empty, size));
            BufferedGraphics.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        private void DisposeSurface()
        {
            BufferedGraphics.Dispose();
            BufferedGraphics = default;
        }

        #endregion

        #region // render

        protected override void RenderInternal()
        {
            var graphics = BackBuffer.Graphics;

            //graphics.Clear(Color.Black);

            var t = DateTime.UtcNow.Millisecond / 1000.0;
            Color GetColor(int x, int y) => Color.FromArgb
            (
                byte.MaxValue,
                    (byte)((double)x / BufferSize.Width * byte.MaxValue),
                    (byte)((double)y / BufferSize.Height * byte.MaxValue),
                    (byte)(Math.Sin(t * Math.PI) * byte.MaxValue)

            );


            Parallel.For(0, BackBuffer.Buffer.Length, index =>
            {
                BackBuffer.GetXY(index, out var x, out int y);
                BackBuffer.Buffer[index] = GetColor(x, y).ToArgb();
            });

            graphics.DrawString(FpsCounter.FpsString, FontConsloe12, Brushes.Red, 0, 0);
            graphics.DrawString($"Buffer = {BufferSize.Width}, {BufferSize.Height}", FontConsloe12, Brushes.Cyan, 0, 16);
            graphics.DrawString($"Viewport = {Viewport.Width}, {Viewport.Height}", FontConsloe12, Brushes.Cyan, 0, 32);


            BufferedGraphics.Graphics.DrawImage(BackBuffer.Bitmap, new RectangleF(PointF.Empty, Viewport.Size), new RectangleF(new PointF(-0.5f, -0.5f), BufferSize), GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        #endregion

    }
}
