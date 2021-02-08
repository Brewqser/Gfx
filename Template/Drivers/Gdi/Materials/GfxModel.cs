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

        public IModel Model { get; set; }

        #endregion

        #region // ctor

        protected GfxModel(IModel model)
        {
            Model = model;
        }

        public virtual void Dispose()
        {
            Model = default;
        }

        #endregion

        #region // routines

        public static IGfxModel Factory(RenderHost renderHost, IModel model)
        {
            // TODO: solve without switch
            switch (model.ShaderType)
            {
                case ShaderType.Position:
                    return new Position.GfxModel(renderHost, model);

                case ShaderType.PositionColor:
                    return new PositionColor.GfxModel(renderHost, model);

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

        protected GfxModel(IModel model, IShader<TVsIn, TPsIn> shader, IBufferBinding<TVsIn> bufferBinding) :
            base(model)
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
