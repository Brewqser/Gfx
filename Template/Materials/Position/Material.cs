using System.Drawing;

namespace EMBC.Materials.Position
{
    public class Material :
        Materials.Material,
        IMaterial
    {
        #region // storage

        public Color Color { get; }

        #endregion

        #region // ctor

        public Material(Color color)
        {
            Color = color;
        }

        #endregion
    }
}
