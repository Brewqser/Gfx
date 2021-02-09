using System;
using EMBC.Drivers.Gdi.Render;
using EMBC.Drivers.Gdi.Render.Rasterization;
using EMBC.Materials;
using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials
{
    public abstract class GfxModel :
        IGfxModel
    {
        #region // storage

        protected RenderHost RenderHost { get; set; }

        public IModel Model { get; set; }

        public Texture Texture { get; set; }

        #endregion

        #region // ctor

        protected GfxModel(RenderHost renderHost, IModel model)
        {
            RenderHost = renderHost;
            Model = model;
            if (!(Model.TextureResource is null))
            {
                Texture = RenderHost.TextureLibrary.GetTexture(Model.TextureResource);
            }
        }

        public virtual void Dispose()
        {
            Texture = default;
            Model = default;
            RenderHost = default;
        }

        #endregion

        #region // routines

        public static IGfxModel Factory(RenderHost renderHost, IModel model)
        {
            switch (model.ShaderType)
            {
                case ShaderType.Position:
                    return new Position.GfxModel(renderHost, model);

                case ShaderType.PositionColor:
                    return new PositionColor.GfxModel(renderHost, model);

                case ShaderType.PositionTexture:
                    return new PositionTexture.GfxModel(renderHost, model);

                default:
                    throw new ArgumentOutOfRangeException(nameof(model.ShaderType), model.ShaderType, default);
            }
        }

        protected abstract void ShaderUpdate(in Matrix4D matrixToClip);

        protected abstract void Render();

        public void Render(in Matrix4D matrixToClip)
        {
            ShaderUpdate(matrixToClip);
            Render();
        }

        #endregion
    }

    public abstract class GfxModel<TVsIn, TPsIn> :
        GfxModel
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        #region // storage

        private IShader<TVsIn, TPsIn> Shader { get; set; }

        private IBufferBinding<TVsIn> BufferBinding { get; set; }

        #endregion

        #region // ctor

        protected GfxModel(RenderHost renderHost, IModel model, IShader<TVsIn, TPsIn> shader, IBufferBinding<TVsIn> bufferBinding) :
            base(renderHost, model)
        {
            Shader = shader;
            BufferBinding = bufferBinding;
        }

        public override void Dispose()
        {
            Shader = default;
            BufferBinding = default;

            base.Dispose();
        }

        #endregion

        #region // routines

        protected override void Render()
        {
            Shader.Pipeline.Render(BufferBinding, Model.Positions.Length, Model.PrimitiveTopology);
        }

        #endregion
    }
}
