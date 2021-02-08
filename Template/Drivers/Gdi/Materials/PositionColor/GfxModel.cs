using EMBC.Drivers.Gdi.Render;
using EMBC.Materials;
using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials.PositionColor
{
    public class GfxModel :
        GfxModel<VsIn, PsIn>
    {
        private Shader Shader { get; }

        public GfxModel(RenderHost renderHost, IModel model) :
            base(model, renderHost.ShaderLibrary.ShaderPositionColor, new BufferBinding(model.Positions, model.Colors))
        {
            Shader = renderHost.ShaderLibrary.ShaderPositionColor;
        }

        protected override void ShaderUpdate(in Matrix4D matrixToClip)
        {
            Shader.Update(matrixToClip);
        }
    }
}
