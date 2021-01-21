using System;

namespace EMBC.Inputs
{
    public interface IInput :
        IDisposable
    {
        System.Drawing.Size Size { get; }
        event SizeEventHandler SizeChanged;
        event MouseEventHandler MouseMove;
        event MouseEventHandler MouseDown;
        event MouseEventHandler MouseUp;
        event MouseEventHandler MouseWheel;
        event KeyEventHandler KeyDown;
        event KeyEventHandler KeyUp;

    }
}
