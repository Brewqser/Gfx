using EMBC.Materials;

namespace EMBC.Drivers.Gdi.Materials
{
    public class TextureLibrary :
        TextureLibrary<ITextureResource, Texture>
    {
        protected override Texture CreateTexture(ITextureResource textureResource)
        {
            return textureResource.Source is null
                ? new Texture(textureResource.Size)
                : new Texture(textureResource.Source);
        }

        protected override void DeleteTexture(Texture texture)
        {
            texture.Dispose();
        }
    }
}
