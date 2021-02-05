using System.Collections.Generic;

namespace EMBC.Materials
{
    public abstract class Primitive :
        IPrimitive
    {
        #region // storage

        public PrimitiveBehaviour PrimitiveBehaviour { get; }

        #endregion

        #region // ctor

        protected Primitive(PrimitiveBehaviour primitiveBehaviour)
        {
            PrimitiveBehaviour = primitiveBehaviour;
        }

        #endregion
    }

    public abstract class Primitive<TMaterial> :
        Primitive,
        IPrimitive<TMaterial>
        where TMaterial : IMaterial
    {
        #region // storage

        public TMaterial Material { get; }

        #endregion

        #region // ctor

        protected Primitive(PrimitiveBehaviour primitiveBehaviour, TMaterial material) :
            base(primitiveBehaviour)
        {
            Material = material;
        }

        #endregion
    }

    public abstract class Primitive<TMaterial, TVertex> :
        Primitive<TMaterial>,
        IPrimitive<TMaterial, TVertex>
        where TMaterial : IMaterial
        where TVertex : IVertex
    {
        #region // storage

        public PrimitiveTopology PrimitiveTopology { get; }

        public IReadOnlyList<TVertex> Vertices { get; }

        #endregion

        #region // ctor

        protected Primitive(PrimitiveBehaviour primitiveBehaviour, TMaterial material, PrimitiveTopology primitiveTopology, IReadOnlyList<TVertex> vertices) :
            base(primitiveBehaviour, material)
        {
            PrimitiveTopology = primitiveTopology;
            Vertices = vertices;
        }

        #endregion
    }
}
