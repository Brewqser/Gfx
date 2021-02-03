using System;
using System.Drawing;
using MathNet.Spatial.Euclidean;
using EMBC.Common.Camera;
using EMBC.Common.Camera.Projections;
using EMBC.Inputs;

namespace EMBC.Engine.Render
{
    public abstract class RenderHost :
        IRenderHost
    {
        #region // storage

        public IntPtr HostHandle { get; private set; }
        public IInput HostInput { get; private set; }
         
        public FpsCounter FpsCounter { get; private set; }
        protected Size HostSize { get; private set; }
        protected Size BufferSize { get; private set; }
        private ICameraInfo m_CameraInfo;
        public ICameraInfo CameraInfo
        {
            get => m_CameraInfo;
            set
            {
                m_CameraInfo = value;
                CameraInfoChanged?.Invoke(this, m_CameraInfo);
            }
        }

        protected DateTime FrameStarted { get; private set; }


        #endregion


        #region // events

        public event EventHandler<ICameraInfo> CameraInfoChanged;

        #endregion

        #region // ctor

        protected RenderHost(IRenderHostSetup renderHostSetup)
        {
            HostHandle = renderHostSetup.HostHandle;
            HostInput = renderHostSetup.HostInput;

            HostSize = HostInput.Size;
            BufferSize = HostInput.Size;
            CameraInfo = new CameraInfo
            (
                new Point3D(1, 1, 1),
                new Point3D(0, 0, 0),
                new UnitVector3D(0, 0, 1),
                new ProjectionPerspective(0.001, 1000, Math.PI * 0.5, 1),
                //new ProjectionOrthographic(0.001, 1000, 2, 2),
                new Viewport(0, 0, 1, 1, 0, 1)
            );

            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));

            HostInput.SizeChanged += HostInputOnSizeChanged;

            HostInputOnSizeChanged(this, new SizeEventArgs(HostSize));
        }

        public virtual void Dispose()
        {
            FpsCounter.Dispose();
            FpsCounter = default;

            CameraInfo = default;
            BufferSize = default;
            HostSize = default;

            HostInput.Dispose();
            HostInput = default;

            HostHandle = default;
        }

        #endregion

        #region //routines

        private void HostInputOnSizeChanged(object sender, ISizeEventArgs args)
        {
            Size Sanitize(Size size)
            {
                if (size.Width < 1 || size.Height < 1)
                {
                    size = new Size(1, 1);
                }
                return size;
            }

            var hostSize = Sanitize(args.NewSize);
            if (HostSize != hostSize)
            {
                ResizeHost(hostSize);
            }

            var cameraInfo = CameraInfo;
            if (cameraInfo.Viewport.Size != hostSize)
            {
                var viewport = new Viewport
                (
                    cameraInfo.Viewport.X,
                    cameraInfo.Viewport.Y,
                    hostSize.Width,
                    hostSize.Height,
                    cameraInfo.Viewport.MinZ,
                    cameraInfo.Viewport.MaxZ
                );
                CameraInfo = new CameraInfo
                (
                    cameraInfo.Position,
                    cameraInfo.Target,
                    cameraInfo.UpVector,
                    cameraInfo.Projection.GetAdjustedProjection(viewport.AspectRatio),
                    viewport
                );
            }
        }

        protected virtual void ResizeHost(Size size)
        {
            HostSize = size;
        }

        protected virtual void ResizeBuffers(Size size)
        {
            BufferSize = size;
        }

        protected void EnsureBufferSize()
        {
            var size = CameraInfo.Viewport.Size;
            if (BufferSize != size)
            {
                ResizeBuffers(size);
            }
        }


        #endregion

        #region //render

        public void Render()
        {
            EnsureBufferSize();
            FrameStarted = DateTime.UtcNow;
            FpsCounter.StartFrame();
            RenderInternal();
            FpsCounter.StopFrame();
        }

        protected abstract void RenderInternal();

        #endregion

    }
}
