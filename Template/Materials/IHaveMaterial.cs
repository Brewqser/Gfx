namespace EMBC.Materials
{
    public interface IHaveMaterial<out TMaterial>
        where TMaterial : IMaterial
    {
        TMaterial Material { get; }
    }
}