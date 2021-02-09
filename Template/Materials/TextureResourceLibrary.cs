using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace EMBC.Materials
{
    public static class TextureResourceLibrary
    {
        #region // storage

        private static Dictionary<int /* id = name hash */, ITextureResource> Cache { get; } = new Dictionary<int, ITextureResource>();

        #endregion

        #region // routines

        public static ITextureResource Get(int id)
        {
            return Cache[id];
        }

        public static bool TryGet(int id, out ITextureResource textureResource)
        {
            return Cache.TryGetValue(id, out textureResource);
        }

        public static bool TryGet(string name, out ITextureResource textureResource)
        {
            return TryGet(name.GetHashCode(), out textureResource);
        }

        private static ITextureResource GetOrCreate(string name, Func<TextureResource> ctorTextureResource)
        {
            var id = name.GetHashCode();
            if (!Cache.TryGetValue(id, out var textureResource))
            {
                Cache.Add(id, textureResource = ctorTextureResource());
            }
            return textureResource;
        }

        public static ITextureResource GetOrCreate(string name, Size size)
        {
            return GetOrCreate(name, () => new TextureResource(name, size));
        }

        public static ITextureResource GetOrCreate(string name, Func<Bitmap> getBitmap)
        {
            return GetOrCreate(name, () => new TextureResource(name, getBitmap));
        }

        public static ITextureResource GetOrCreateFromFile(string filePathRelativeOrAbsolute)
        {
            var filePathAbsolute = new FileInfo(filePathRelativeOrAbsolute).FullName;
            return GetOrCreate(filePathAbsolute, () => new Bitmap(filePathAbsolute));
        }

        public static void Delete(ITextureResource textureResource)
        {
            Cache.Remove(textureResource.Id);
            textureResource.Dispose();
        }

        #endregion
    }
}
