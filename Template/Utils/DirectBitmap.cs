using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace EMBC.Utils
{
    public class DirectBitmap :
        IDisposable
    {
        #region // storage

        public Size Size { get; }
        public int Width => Size.Width;
        public int Height => Size.Height;
        public int[] Buffer { get; private set; }
        private GCHandle BufferHandle { get; set; }
        public Bitmap Bitmap { get; private set; }
        public Graphics Graphics { get; private set; }

        #endregion

        #region // ctor

        public DirectBitmap(Size size)
        {
            Size = size;
            Buffer = new int[Width * Height];
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

            Buffer = default;
        }

        #endregion

        #region // routines

        public int GetIndex(int x, int y) => x + y * Width;

        public void GetXY(int index, out int x, out int y)
        {
            y = index / Width;
            x = index - y * Width;
        }

        public void SetArgb(int x, int y, int argb) => Buffer[GetIndex(x, y)] = argb;

        public int GetArgb(int x, int y) => Buffer[GetIndex(x, y)];

        public void SetPixel(int x, int y, Color color) => SetArgb(x, y, color.ToArgb());

        public Color GetPixel(int x, int y) => Color.FromArgb(GetArgb(x, y));

        #endregion
    }
}
