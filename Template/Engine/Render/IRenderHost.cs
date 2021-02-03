using EMBC.Common.Camera;
using EMBC.Inputs;
using System;

namespace EMBC.Engine.Render
{
    public interface IRenderHost : 
        IDisposable
    {
        IntPtr HostHandle { get; }
        IInput  HostInput { get; }

        ICameraInfo CameraInfo { get; set; }

        FpsCounter FpsCounter { get; }

        void Render();

        event EventHandler<ICameraInfo> CameraInfoChanged;

    }
}
