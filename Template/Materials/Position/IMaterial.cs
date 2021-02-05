using System.Drawing;

namespace EMBC.Materials.Position
{
    public interface IMaterial :
        Materials.IMaterial
    {
        Color Color { get; }
    }
}
