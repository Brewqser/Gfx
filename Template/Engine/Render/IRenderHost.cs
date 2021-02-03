using EMBC.Common.Camera;
using EMBC.Inputs;
using System;
using System.Drawing;

namespace EMBC.Engine.Render
{
    public interface IRenderHost : 
        IDisposable
    {
        IntPtr HostHandle { get; }
        IInput  HostInput { get; }

        Size HostSize { get; }

        ICameraInfo CameraInfo { get; set; }

        FpsCounter FpsCounter { get; }

        void Render();

        event EventHandler<ICameraInfo> CameraInfoChanged;

    }
}
