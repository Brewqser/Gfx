using EMBC.Materials;
using EMBC.Drivers.Gdi.Materials;

namespace EMBC.Drivers.Gdi.Render
{
    public interface IPipeline
    {
        void SetRenderHost(RenderHost renderHost);
    }

    public interface IPipeline<in TVertex> :
        IPipeline
        where TVertex : struct
    {
        void Render(TVertex[] vertices, PrimitiveTopology primitiveTopology);
    }

    public interface IPipeline<TVertex, TVertexShader> :
        IPipeline<TVertex>
        where TVertex : struct
        where TVertexShader : struct, IVertexShader
    {
        void SetShader(IShader<TVertex, TVertexShader> shader);
    }
}
