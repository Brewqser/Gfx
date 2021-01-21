using EMBC.Inputs;
using System;

namespace EMBC.Engine.Render
{
    public interface IRenderHost : 
        IDisposable
    {
        IntPtr HostHandle { get; }
        IInput  HostInput { get; }

        FpsCounter FpsCounter { get; }

        void Render();
    }
}
