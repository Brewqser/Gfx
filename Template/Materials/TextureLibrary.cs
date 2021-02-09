using EMBC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMBC.Materials
{
    public abstract class TextureLibrary<TTextureResource, TTexture> :
         IDisposable
         where TTextureResource : ITextureResource
    {
        #region // storage

        protected Dictionary<TTextureResource, TTexture> Textures { get; set; } = new Dictionary<TTextureResource, TTexture>();

        #endregion

        #region // ctor

        public virtual void Dispose()
        {
            Textures.Select(pair => pair.Key).ToArray().ForEach(DeleteTexture);
            Textures = default;
        }

        #endregion

        #region // routines

        public TTexture GetTexture(TTextureResource textureResource)
        {
            if (!Textures.TryGetValue(textureResource, out var texture))
            {
                Textures.Add(textureResource, texture = CreateTexture(textureResource));
            }
            return texture;
        }

        protected abstract TTexture CreateTexture(TTextureResource textureResource);

        public void DeleteTexture(TTextureResource textureResource)
        {
            var texture = Textures[textureResource];
            DeleteTexture(texture);
            Textures.Remove(textureResource);
        }

        protected abstract void DeleteTexture(TTexture texture);

        #endregion
    }
}
