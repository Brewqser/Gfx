using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace EMBC.Materials.Position
{
    public class Primitive :
        Primitive<IMaterial, Vertex>,
        IPrimitive
    {
        #region // ctor

        public Primitive(PrimitiveBehaviour primitiveBehaviour, PrimitiveTopology primitiveTopology, Vertex[] vertices, Color color) :
            base(primitiveBehaviour, new Material(color), primitiveTopology, vertices)
        {
        }
        #endregion
    }
}
