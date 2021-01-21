using EMBC.Inputs;
using System;

namespace EMBC.Engine.Render
{
    public class RenderHostSetup :
        IRenderHostSetup
    {
        #region // storage

        public IntPtr HostHandle { get; }

        public IInput HostInput { get; }

        #endregion

        #region // ctor

        public RenderHostSetup(IntPtr hostHandle, IInput hostInput)
        {
            HostHandle = hostHandle;
            HostInput = hostInput;
        }

        #endregion
    }
}
