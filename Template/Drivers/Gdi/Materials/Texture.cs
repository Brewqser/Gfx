using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using EMBC.Utils;

namespace EMBC.Drivers.Gdi.Materials
{
    public class Texture :
        IDisposable
    {
        #region // storage

        public DirectBitmap DirectBitmap { get; private set; }

        #endregion

        #region // ctor

        public Texture(Size size)
        {
            DirectBitmap = new DirectBitmap(size);
            DirectBitmap.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        public Texture(Image source) :
            this(source.Size)
        {
            DirectBitmap.Graphics.DrawImage
            (
                source,
                new Rectangle(Point.Empty, DirectBitmap.Size),
                new Rectangle(Point.Empty, source.Size),
                GraphicsUnit.Pixel
            );
        }

        public void Dispose()
        {
            DirectBitmap.Dispose();
            DirectBitmap = default;
        }

        #endregion
    }
}
