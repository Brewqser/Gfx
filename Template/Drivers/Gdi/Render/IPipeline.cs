using EMBC.Materials;
using EMBC.Drivers.Gdi.Materials;
using EMBC.Drivers.Gdi.Materials.Position;

namespace EMBC.Drivers.Gdi.Render
{
    public interface IPipeline
    {
        void SetRenderHost(RenderHost renderHost);
    }

    public interface IPipeline<in TVertexIn> :
        IPipeline
        where TVertexIn : struct
    {
        void Render(TVertexIn[] vertices, PrimitiveTopology primitiveTopology);
    }

    public interface IPipeline<TVertexIn, TVertex> :
        IPipeline<TVertexIn>
        where TVertexIn : struct
        where TVertex : struct, Materials.IVertex
    {
        void SetShader(IShader<TVertexIn, TVertex> shader);
    }
}
