using System;

namespace EMBC.Inputs
{
    public abstract class Input :
        IInput
    {
        #region // storage

        public abstract System.Drawing.Size Size { get; }

        public abstract event SizeEventHandler SizeChanged;

        public abstract event MouseEventHandler MouseMove;

        public abstract event MouseEventHandler MouseDown;

        public abstract event MouseEventHandler MouseUp;

        public abstract event MouseEventHandler MouseWheel;

        public abstract event KeyEventHandler KeyDown;

        public abstract event KeyEventHandler KeyUp;

        #endregion

        #region // ctor

        public abstract void Dispose();

        #endregion
    }
}
