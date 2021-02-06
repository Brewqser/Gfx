using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace EMBC.Utils
{
    public class DirectBitmap :
        Buffer2D<int>,
        IDisposable
    {
        #region // storage

        private GCHandle BufferHandle { get; set; }

        public Bitmap Bitmap { get; private set; }

        public Graphics Graphics { get; private set; }

        #endregion

        #region // ctor

        public DirectBitmap(Size size) :
            base(size)
        {
            BufferHandle = GCHandle.Alloc(Buffer, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BufferHandle.AddrOfPinnedObject());
            Graphics = Graphics.FromImage(Bitmap);
        }

        public DirectBitmap(int width, int height) :
            this(new Size(width, height))
        {
        }

        public void Dispose()
        {
            Graphics.Dispose();
            Graphics = default;

            Bitmap.Dispose();
            Bitmap = default;

            BufferHandle.Free();
            BufferHandle = default;
        }

        #endregion

        #region // routines

        public void SetPixel(int x, int y, Color color) => SetValue(x, y, color.ToArgb());

        public Color GetPixel(int x, int y) => Color.FromArgb(GetValue(x, y));

        public void Clear(Color color) => Clear(color.ToArgb());

        #endregion
    }
}
