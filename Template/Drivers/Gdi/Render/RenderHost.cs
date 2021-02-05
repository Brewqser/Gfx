using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using EMBC.Engine.Render;
using EMBC.Materials;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;
using EMBC.Utils;
using MathNet.Spatial.Euclidean;

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
            base.ResizeHost(size);

            DisposeSurface();
            CreateSurface(size);
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

        protected override void RenderInternal(IEnumerable<IPrimitive> primitives)
        {
            var graphics = BackBuffer.Graphics;
            graphics.Clear(Color.Black);

            DrawPrimitives(primitives);

            graphics.DrawString(FpsCounter.FpsString, FontConsloe12, Brushes.Red, 0, 0);

            BufferedGraphics.Graphics.DrawImage(BackBuffer.Bitmap, new RectangleF(PointF.Empty, HostSize), new RectangleF(new PointF(-0.5f, -0.5f), BufferSize), GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        private void DrawPrimitives(IEnumerable<IPrimitive> primitives)
        {
            foreach (var primitive in primitives.OfType<Materials.Position.IPrimitive>())
            {
                using (var pen = new Pen(primitive.Material.Color))
                {
                    switch (primitive.PrimitiveTopology)
                    {
                        case PrimitiveTopology.LineStrip:
                            DrawPolyline(primitive.Vertices.Select(vertex => vertex.Position), primitive.PrimitiveBehaviour.Space, pen);
                            break;
                    }
                }
            }
        }


        private void DrawPolyline(IEnumerable<Point3D> points, Space space, Pen pen)
        {
            switch (space)
            {
                case Space.World:
                    DrawPolylineScreenSpace(CameraInfo.Cache.MatrixViewProjectionViewport.Transform(points), pen);
                    break;

                case Space.View:
                    DrawPolylineScreenSpace(CameraInfo.Cache.MatrixViewport.Transform(points), pen);
                    break;

                case Space.Screen:
                    DrawPolylineScreenSpace(points, pen);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(space), space, null);
            }
        }

        private void DrawPolylineScreenSpace(IEnumerable<Point3D> pointsScreen, Pen pen)
        {
            var form = default(Point3D?);
            foreach (var pointScreen in pointsScreen)
            {
                if (form.HasValue)
                {
                    BackBuffer.Graphics.DrawLine(pen, (float)form.Value.X, (float)form.Value.Y, (float)pointScreen.X, (float)pointScreen.Y);
                }
                form = pointScreen;
            }
        }

        #endregion

    }
}
