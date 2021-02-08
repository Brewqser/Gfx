using EMBC.Drivers.Gdi.Render;
using EMBC.Drivers.Gdi.Render.Rasterization;
using EMBC.Mathematics;

namespace EMBC.Drivers.Gdi.Materials
{
    public abstract class Shader<TVsIn, TPsIn> :
        IShader<TVsIn, TPsIn>
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        public RenderHost RenderHost { get; private set; }

        public IPipeline<TVsIn, TPsIn> Pipeline { get; private set; }

        #region // ctor

        protected Shader(RenderHost renderHost)
        {
            RenderHost = renderHost;
            Pipeline = new Pipeline<TVsIn, TPsIn>(this);
        }

        public virtual void Dispose()
        {
            Pipeline.Dispose();
            Pipeline = default;

            RenderHost = default;
        }

        #endregion

        public abstract bool VertexShader(in TVsIn vsin, out TPsIn vsout);
        public abstract bool PixelShader(in TPsIn psin, out Vector4F color);
    }
}
