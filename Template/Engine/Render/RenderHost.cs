using EMBC.Engine.Common;
using EMBC.Inputs;
using System;
using System.Drawing;

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
        protected Viewport Viewport { get; private set; }


        #endregion

        #region // ctor

        protected RenderHost(IRenderHostSetup renderHostSetup)
        {
            HostHandle = renderHostSetup.HostHandle;
            HostInput = renderHostSetup.HostInput;

            HostSize = HostInput.Size;
            BufferSize = HostInput.Size;
            Viewport = new Viewport(Point.Empty, HostSize, 0, 1);

            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));

            HostInput.SizeChanged += HostInputOnSizeChanged;
        }

        public virtual void Dispose()
        {
            FpsCounter.Dispose();
            FpsCounter = default;

            Viewport = default;
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

            var hostsize = Sanitize(HostInput.Size);
            if (HostSize != hostsize)
            {
                ResizeHost(hostsize);
            }

            var bufferSize = Sanitize(args.NewSize);
            if (BufferSize != bufferSize)
            {
                ResizeBuffers(bufferSize);
            }
                
        }

        protected virtual void ResizeHost(Size size)
        {
            HostSize = size;
            Viewport = new Viewport(Point.Empty, size, 0, 1);
        }

        protected virtual void ResizeBuffers(Size size)
        {
            BufferSize = size;
        }


        #endregion

        #region //render

        public void Render()
        {
            FpsCounter.StartFrame();

            RenderInternal();

            FpsCounter.StopFrame();
        }

        protected abstract void RenderInternal();

        #endregion

    }
}
