using System;

namespace EMBC.Inputs
{
    public class SizeEventArgs :
        EventArgs,
        ISizeEventArgs
    {
        #region // storage

        public System.Drawing.Size NewSize { get; set; }

        #endregion

        #region // ctor

        public SizeEventArgs(System.Drawing.Size newSize)
        {
            NewSize = newSize;
        }

        #endregion
    }
}
