using EMBC.Mathematics;
namespace EMBC.Drivers.Gdi.Materials
{
    public interface IPsIn<TPsIn> :
        IInterpolateSingle<TPsIn>
    {
        Vector4F Position { get; }
    }
}
