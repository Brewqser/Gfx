using System;
using System.Drawing;
using EMBC.Utils;

namespace EMBC.Drivers.Gdi.Render
{
    public class FrameBuffers :
       IDisposable
    {
        #region // storage

        public const int LayerCount = 1;

        public Size Size { get; private set; }

        public DirectBitmap[] BufferColor { get; set; }

        public Buffer2D<float> BufferDepth { get; set; }

        #endregion

        #region // ctor

        public FrameBuffers(Size size)
        {
            Size = size;

            BufferColor = new DirectBitmap[LayerCount];
            for (var i = 0; i < BufferColor.Length; i++)
            {
                BufferColor[i] = new DirectBitmap(Size);
            }

            BufferDepth = new Buffer2D<float>(Size);
        }

        public void Dispose()
        {
            BufferColor.ForEach(o => o.Dispose());
            BufferColor = default;

            BufferDepth.Dispose();
            BufferDepth = default;

            Size = default;
        }

        #endregion
    }
}
