using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using EMBC.Drivers.Gdi.Materials;
using EMBC.Engine.Render;
using EMBC.Materials;
using EMBC.Mathematics;
using EMBC.Mathematics.Extensions;
using EMBC.Utils;

namespace EMBC.Drivers.Gdi.Render
{
    public class RenderHost :
        Engine.Render.RenderHost
    {
        #region // storage

        private Graphics GraphicsHost { get; set; }

        private IntPtr GraphicsHostDeviceContext { get; set; }

        private BufferedGraphics BufferedGraphics { get; set; }

        public DirectBitmap BackBuffer { get; private set; }

        public ShaderLibrary ShaderLibrary { get; private set; }

        private Font FontConsolas12 { get; set; }

        #endregion

        #region // ctor

        public RenderHost(IRenderHostSetup renderHostSetup) :
            base(renderHostSetup)
        {
            GraphicsHost = Graphics.FromHwnd(HostHandle);
            GraphicsHostDeviceContext = GraphicsHost.GetHdc();
            CreateSurface(HostInput.Size);
            CreateBuffers(BufferSize);
            ShaderLibrary = new ShaderLibrary(this);
            FontConsolas12 = new Font("Consolas", 12);
        }

        public override void Dispose()
        {
            FontConsolas12.Dispose();
            FontConsolas12 = default;

            ShaderLibrary.Dispose();
            ShaderLibrary = default;

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
            BackBuffer.Clear(Color.Black);

            RenderPrimitives(primitives);

            BackBuffer.Graphics.DrawString(FpsCounter.FpsString, FontConsolas12, Brushes.Red, 0, 0);

            BufferedGraphics.Graphics.DrawImage(
                BackBuffer.Bitmap,
                new RectangleF(PointF.Empty, HostSize),
                new RectangleF(new PointF(-0.5f, -0.5f), BufferSize),
                GraphicsUnit.Pixel);
            BufferedGraphics.Render(GraphicsHostDeviceContext);
        }

        private void RenderPrimitives(IEnumerable<IPrimitive> primitives)
        {
            // TODO: currently we know how to draw only certain type of primitives, so just filter them out
            // TODO: in a future we're gonna solve this generically (without typecasting)
            foreach (var primitive in primitives.OfType<EMBC.Materials.Position.IPrimitive>())
            {
                var gfxModel = GfxModel.Factory(this, new Model
                {
                    ShaderType = ShaderType.Position,
                    Space = primitive.PrimitiveBehaviour.Space,
                    PrimitiveTopology = primitive.PrimitiveTopology,
                    Positions = primitive.Vertices.Select(v => v.Position).ToArray(),
                    Color = primitive.Material.Color.ToRgba(),
                });
                ShaderLibrary.ShaderPosition.Update(GetMatrixForVertexShader(this, primitive.PrimitiveBehaviour.Space), primitive.Material.Color.ToRgba());
                gfxModel.Render(GetMatrixForVertexShader(this, primitive.PrimitiveBehaviour.Space));
            }
        }

        private static Matrix4D GetMatrixForVertexShader(IRenderHost renderHost, Space space)
        {
            switch (space)
            {
                case Space.World:
                    return renderHost.CameraInfo.Cache.MatrixViewProjection;

                case Space.View:
                    return Matrix4D.Identity;

                case Space.Screen:
                    return renderHost.CameraInfo.Cache.MatrixViewportInverse;

                default:
                    throw new ArgumentOutOfRangeException(nameof(space), space, default);
            }
        }

        #endregion
    }
}
