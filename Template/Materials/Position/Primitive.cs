using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace EMBC.Materials.Position
{
    public class Primitive :
        Primitive<IMaterial, IVertex>,
        IPrimitive
    {
        #region // ctor

        protected Primitive(PrimitiveBehaviour primitiveBehaviour, PrimitiveTopology primitiveTopology, IReadOnlyList<IVertex> vertices, Color color) :
            base(primitiveBehaviour, new Material(color), primitiveTopology, vertices)
        {
        }

        public Primitive(PrimitiveBehaviour primitiveBehaviour, PrimitiveTopology primitiveTopology, IEnumerable<IVertex> vertices, Color color) :
            this(primitiveBehaviour, primitiveTopology, vertices is IReadOnlyList<IVertex> readOnlyList ? readOnlyList : vertices.ToArray(), color)
        {
        }

        #endregion
    }
}
