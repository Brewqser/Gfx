
using EMBC.Mathematics;

namespace EMBC.Materials
{
    public class Model :
        IModel
    {
        public ShaderType ShaderType { get; set; }

        public Space Space { get; set; }

        public PrimitiveTopology PrimitiveTopology { get; set; }

        public Vector3F[] Positions { get; set; }

        public int[] Colors { get; set; }

        public int Color { get; set; }
    }
}
