using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using EMBC.Utils;

namespace EMBC.Materials
{
    public class TextureResource :
        ITextureResource
    {
        #region // storage

        public int Id { get; }

        public string Name { get; private set; }

        public Size Size { get; private set; }

        public Bitmap Source { get; private set; }

        #endregion

        #region // ctor

        internal TextureResource(string name, Size size)
        {
            Name = name;
            Id = Name.GetHashCode();
            Size = size;
        }

        internal TextureResource(string name, Func<Bitmap> getSource)
        {
            Name = name;
            Id = Name.GetHashCode();

            // get initial data
            var source = getSource();
            // ensure it's 32bit argb
            if (source.PixelFormat != PixelFormat.Format32bppArgb)
            {
                var bitmap32argb = source.ToBitmap(Color.FromArgb);
                source.Dispose();
                source = bitmap32argb;
            }

            Source = source;
            Size = Source.Size;
        }

        public void Dispose()
        {
            Source?.Dispose();
            Source = default;

            Size = default;
            Name = default;
        }

        #endregion

        #region // hashing

        public override int GetHashCode()
        {
            return Id;
        }

        public int GetHashCode(ITextureResource obj)
        {
            return obj.GetHashCode();
        }

        public static bool AreEqual(ITextureResource left, ITextureResource right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.GetHashCode() == right.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ITextureResource textureResource)
            {
                return AreEqual(this, textureResource);
            }
            return false;
        }
        public bool Equals(ITextureResource left, ITextureResource right)
        {
            return AreEqual(left, right);
        }

        #endregion
    }
}
