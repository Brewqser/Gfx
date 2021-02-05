using EMBC.Common.Camera;
using EMBC.Inputs;
using System;
using System.Collections.Generic;
using System.Drawing;
using EMBC.Materials;

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

        void Render(IEnumerable<IPrimitive> primitives);

        event EventHandler<ICameraInfo> CameraInfoChanged;

    }
}
