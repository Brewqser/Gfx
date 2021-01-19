using System;

namespace EMBC.Drivers.Gdi.Render
{
    public class RenderHost :
        Engine.Render.RenderHost
    {
        public RenderHost(IntPtr hostHandle) : 
            base(hostHandle)
        {
        }
    }
}
