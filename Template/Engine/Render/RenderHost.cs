using System;
using System.Collections.Generic;
using System.Drawing;
using MathNet.Spatial.Euclidean;
using EMBC.Common.Camera;
using EMBC.Common.Camera.Projections;
using EMBC.Inputs;
using EMBC.Engine.Operators;
using EMBC.Materials;
using EMBC.Utils;

namespace EMBC.Engine.Render
{
    public abstract class RenderHost :
        IRenderHost
    {
        #region // storage

        public IntPtr HostHandle { get; private set; }
        public IInput HostInput { get; private set; }
         
        public FpsCounter FpsCounter { get; private set; }
        public Size HostSize { get; private set; }
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

        protected IEnumerable<IOperator> Operators { get; set; }

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

            Operators = new List<IOperator>
            {
                new OperatorResize(this, ResizeHost), 
                new OperatorCameraZoom(this),
                new OperatorCameraPan(this),
                new OperatorCameraOrbit(this),
            };


            OperatorResize.Resize(this, HostSize, ResizeHost);
        }

        public virtual void Dispose()
        {
            Operators.ForEach(o => o.Dispose());
            Operators = default;

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

        public void Render(IEnumerable<IModel> models)
        {
            EnsureBufferSize();
            FrameStarted = DateTime.UtcNow;
            FpsCounter.StartFrame();
            RenderInternal(models);
            FpsCounter.StopFrame();
        }

        protected abstract void RenderInternal(IEnumerable<IModel> models);

        #endregion

    }
}
