using EMBC.Drivers.Gdi.Render;
using EMBC.Materials;
using EMBC.Mathematics;


namespace EMBC.Drivers.Gdi.Materials.Position
{
    public class GfxModel :
        GfxModel<VsIn, PsIn>
    {
        /// <inheritdoc cref="IShader"/>
        private Shader Shader { get; }

        /// <summary />
        public GfxModel(RenderHost renderHost, IModel model) :
            base(model, renderHost.ShaderLibrary.ShaderPosition, new BufferBinding(model.Positions))
        {
            Shader = renderHost.ShaderLibrary.ShaderPosition;
        }

        /// <inheritdoc />
        protected override void ShaderUpdate(in Matrix4D matrixToClip)
        {
            Shader.Update(matrixToClip, Model.Color);
        }
    }
}
