namespace EMBC.Materials
{
    public interface IPrimitive :
        IHavePrimitiveBehaviour
    {
    }

    public interface IPrimitive<out TMaterial> :
        IPrimitive,
        IHaveMaterial<TMaterial>
        where TMaterial : IMaterial
    {
    }

    public interface IPrimitive<out TMaterial, out TVertex> :
        IPrimitive<TMaterial>,
        IHavePrimitiveTopology,
        IHaveVertices<TVertex>
        where TMaterial : IMaterial
        where TVertex : IVertex
    {
    }
}
