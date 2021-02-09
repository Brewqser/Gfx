using EMBC.Mathematics;

namespace EMBC.Materials
{
    public interface IModel
    {
        ShaderType ShaderType { get; set; }

        Space Space { get; set; }

        PrimitiveTopology PrimitiveTopology { get; set; }

        Vector3F[] Positions { get; set; }

        int[] Colors { get; set; }

        Vector2F[] TextureCoordinates { get; set; }

        int Color { get; set; }

        ITextureResource TextureResource { get; set; }
    }
}
