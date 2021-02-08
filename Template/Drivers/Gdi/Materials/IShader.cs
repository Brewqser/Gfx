using System;
using EMBC.Drivers.Gdi.Render;
using EMBC.Drivers.Gdi.Render.Rasterization;
using EMBC.Mathematics;


namespace EMBC.Drivers.Gdi.Materials
{
    public interface IShader :
        IDisposable
    {
        RenderHost RenderHost { get; }
    }

    public interface IShader<TVsIn, TPsIn> :
        IShader
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        IPipeline<TVsIn, TPsIn> Pipeline { get; }

        bool VertexShader(in TVsIn vsin, out TPsIn vsout);

        bool PixelShader(in TPsIn psin, out Vector4F color);
    }
}
