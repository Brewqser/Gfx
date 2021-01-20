using System;

namespace EMBC.Engine.Render
{
    public abstract class RenderHost :
        IRenderHost
    {
        #region // storage

        public IntPtr HostHandle { get; private set; }

        public FpsCounter FpsCounter { get; private set; }

        #endregion

        #region // ctor

        protected RenderHost(IntPtr hostHandle)
        {
            HostHandle = hostHandle;

            FpsCounter = new FpsCounter(new TimeSpan(0, 0, 0, 0, 1000));
        }

        public virtual void Dispose()
        {
            FpsCounter?.Dispose();
            FpsCounter = default;

            HostHandle = default;
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
