using EMBC.Drivers.Gdi.Render;
using EMBC.Materials;
using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials.PositionTexture
{
    public class GfxModel :
        GfxModel<VsIn, PsIn>
    {
        private Shader Shader { get; }

        public GfxModel(RenderHost renderHost, IModel model) :
            base(renderHost, model, renderHost.ShaderLibrary.ShaderPositionTexture, new BufferBinding(model.Positions, model.TextureCoordinates))
        {
            Shader = renderHost.ShaderLibrary.ShaderPositionTexture;
        }

        protected override void ShaderUpdate(in Matrix4D matrixToClip)
        {
            Shader.Update(matrixToClip, RenderHost.TextureLibrary.GetTexture(Model.TextureResource));
        }
    }
}
