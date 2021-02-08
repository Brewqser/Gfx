using EMBC.Mathematics;
namespace EMBC.Drivers.Gdi.Materials
{
    public interface IPsIn<TPsIn> :
        IInterpolate<TPsIn>
    {
        Vector4F Position { get; }
        TPsIn ReplacePosition(Vector4F position);
    }
}
