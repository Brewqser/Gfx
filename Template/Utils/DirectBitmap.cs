using System.Drawing;
using System.Drawing.Imaging;

namespace EMBC.Utils
{
    public class DirectBitmap :
        Buffer2D<int>
    {
        #region // storage

        public Bitmap Bitmap { get; private set; }

        public Graphics Graphics { get; private set; }

        #endregion

        #region // ctor

        public DirectBitmap(Size size, int[] data) :
            base(size, data)
        {
            Bitmap = new Bitmap(Width, Height, Width * sizeof(int), PixelFormat.Format32bppPArgb, Address);
            Graphics = Graphics.FromImage(Bitmap);
        }

        public DirectBitmap(Size size) :
            this(size, new int[size.Width * size.Height])
        {
        }

        public DirectBitmap(int width, int height) :
            this(new Size(width, height))
        {
        }

        public override void Dispose()
        {
            Graphics.Dispose();
            Graphics = default;

            Bitmap.Dispose();
            Bitmap = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        public void SetPixel(int x, int y, Color color) => Write(x, y, color.ToArgb());

        public Color GetPixel(int x, int y) => Color.FromArgb(Read<int>(x, y));

        public void Clear(Color color) => Clear(color.ToArgb());

        #endregion
    }
}
