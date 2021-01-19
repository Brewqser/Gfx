﻿using System;

namespace EMBC.Engine.Render
{
    public interface IRenderHost : 
        IDisposable
    {
        IntPtr HostHandle { get; }
    }
}