using System;
using EMBC.Drivers.Gdi.Materials;
using EMBC.Materials;

namespace EMBC.Drivers.Gdi.Render.Rasterization
{
    public interface IPipeline<in TVsIn, TPsIn> :
        IDisposable
        where TVsIn : unmanaged
        where TPsIn : unmanaged, IPsIn<TPsIn>
    {
        void Render(IBufferBinding<TVsIn> bufferBinding, int countVertices, PrimitiveTopology primitiveTopology);
    }
}
