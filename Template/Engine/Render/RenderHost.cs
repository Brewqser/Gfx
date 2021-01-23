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
        protected Size BufferSize { get; private set; }
        protected Size ViewportSize { get; private set; }


        #endregion

        #region // ctor

        protected RenderHost(IRenderHostSetup renderHostSetup)
        {
            HostHandle = renderHostSetup.HostHandle;
            HostInput = renderHostSetup.HostInput;

            BufferSize = HostInput.Size;
            ViewportSize = HostInput.Size;

            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));

            HostInput.SizeChanged += HostInputOnSizeChanged;
        }

        public virtual void Dispose()
        {
            FpsCounter.Dispose();
            FpsCounter = default;

            BufferSize = default;
            ViewportSize = default;

            HostInput.Dispose();
            HostInput = default;

            HostHandle = default;
        }

        #endregion

        #region //routines

        private void HostInputOnSizeChanged(object sender, ISizeEventArgs args)
        {
            var size = args.NewSize;

            if (size.Width < 1 || size.Height < 1)
            {
                size = new Size(1, 1);
            }

            ResizeBuffers(size);
            ResizeViewpoint(size);
        }

        protected virtual void ResizeBuffers(Size size)
        {
            BufferSize = size;
        }

        protected virtual void ResizeViewpoint(Size size)
        {
            ViewportSize = size;
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
