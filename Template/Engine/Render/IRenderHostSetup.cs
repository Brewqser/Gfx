using EMBC.Inputs;
using System;

namespace EMBC.Engine.Render
{
    public interface IRenderHostSetup
    {
        IntPtr HostHandle { get; }
        IInput HostInput { get; }
    }
}
