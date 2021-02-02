using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using EMBC.Engine.Render;
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

        protected override void RenderInternal()
        {
            var graphics = BackBuffer.Graphics;
            graphics.Clear(Color.Black);
            
            DrawWorldAxis();
            DrawGeometry();

            graphics.DrawString(FpsCounter.FpsString, FontConsloe12, Brushes.Red, 0, 0);

            BufferedGraphics.Graphics.DrawImage(BackBuffer.Bitmap, new RectangleF(PointF.Empty, Viewport.Size), new RectangleF(new PointF(-0.5f, -0.5f), BufferSize), GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }


        private void DrawPolyline(IEnumerable<Point3D> points, Space space, Pen pen)
        {
            switch (space)
            {
                case Space.World:
                    var t = GetDeltaTime(new TimeSpan(0, 0, 0, 10));
                    var angle = t * Math.PI * 2;
                    var radius = 2;

                    var cameraPosition = new Vector3D(Math.Sin(angle) * radius, Math.Cos(angle) * radius, 1);
                    var cameraTarget = new Vector3D(0, 0, 0);
                    var cameraUpVector = new UnitVector3D(0 ,0 ,1);
                    var matrixView = MatrixEx.LookAtRH(cameraPosition, cameraTarget, cameraUpVector);

                    // projection matrix
                    var fovY = Math.PI * 0.5;
                    var aspectRatio = (double)BufferSize.Width / BufferSize.Height;
                    var nearPlane = 0.001;
                    var farPlane = 1000;
                    // ReSharper disable once UnusedVariable
                    var matrixPerspective = MatrixEx.PerspectiveFovRH(fovY, aspectRatio, nearPlane, farPlane);

                    var fieldHeight = 3;
                    var fieldWidth = fieldHeight * aspectRatio;
                    // ReSharper disable once UnusedVariable
                    var matrixOrthographic = MatrixEx.OrthoRH(fieldWidth, fieldHeight, nearPlane, farPlane);

                    var matrixProjection = matrixPerspective;
                    //var matrixProjection = matrixOrthographic;

                    // view space (NDC) to screen space matrix
                    var matrixViewport = MatrixEx.Viewport(Viewport);

                    DrawPolylineScreenSpace((matrixView * matrixProjection * matrixViewport).Transform(points), pen);
                    break;

                case Space.View:
                    DrawPolylineScreenSpace(MatrixEx.Viewport(Viewport).Transform(points), pen);
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

        private double GetDeltaTime(TimeSpan periodDuration)
        {
            return GetDeltaTime(FrameStarted, periodDuration);
        }

        private static double GetDeltaTime(DateTime timestamp, TimeSpan periodDuration)
        {
            return (timestamp.Second * 1000 + timestamp.Millisecond) % periodDuration.TotalMilliseconds / periodDuration.TotalMilliseconds;
        }

        private void DrawWorldAxis()
        {
            DrawPolyline(new[] { new Point3D(0, 0, 0), new Point3D(1, 0, 0), }, Space.World, Pens.Red);
            DrawPolyline(new[] { new Point3D(0, 0, 0), new Point3D(0, 1, 0), }, Space.World, Pens.LawnGreen);
            DrawPolyline(new[] { new Point3D(0, 0, 0), new Point3D(0, 0, 1), }, Space.World, Pens.Blue);
        }

        private static readonly Point3D[][] CubePolylines = new[]
        {
            new[]
            {
                new Point3D(0, 0, 0),
                new Point3D(1, 0, 0),
                new Point3D(1, 1, 0),
                new Point3D(0, 1, 0),
                new Point3D(0, 0, 0),
            },
            new[]
            {
                new Point3D(0, 0, 1),
                new Point3D(1, 0, 1),
                new Point3D(1, 1, 1),
                new Point3D(0, 1, 1),
                new Point3D(0, 0, 1),
            },
            new[] { new Point3D(0, 0, 0), new Point3D(0, 0, 1), },
            new[] { new Point3D(1, 0, 0), new Point3D(1, 0, 1), },
            new[] { new Point3D(1, 1, 0), new Point3D(1, 1, 1), },
            new[] { new Point3D(0, 1, 0), new Point3D(0, 1, 1), },
        }.Select(cubePolyline => MatrixEx.Translate(-0.5, -0.5, -0.5).Transform(cubePolyline).ToArray()).ToArray();

        private void DrawGeometry()
        {
            // bigger cube
            var angle = GetDeltaTime(new TimeSpan(0, 0, 0, 5)) * Math.PI * 2;
            var matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(new UnitVector3D(1, 0, 0), angle) *
                MatrixEx.Translate(1, 0, 0);

            foreach (var cubePolyline in CubePolylines)
            {
                DrawPolyline(matrixModel.Transform(cubePolyline), Space.World, Pens.White);
            }

            // smaller cube
            angle = GetDeltaTime(new TimeSpan(0, 0, 0, 1)) * Math.PI * 2;
            matrixModel =
                MatrixEx.Scale(0.5) *
                MatrixEx.Rotate(new UnitVector3D(0, 1, 0), angle) *
                MatrixEx.Translate(0, 1, 0) *
                matrixModel;

            foreach (var cubePolyline in CubePolylines)
            {
                DrawPolyline(matrixModel.Transform(cubePolyline), Space.World, Pens.Yellow);
            }
        }

        #endregion

    }
}
