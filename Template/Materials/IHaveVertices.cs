using System.Collections.Generic;

namespace EMBC.Materials
{
    public interface IHaveVertices<out TVertex>
    {
        IReadOnlyList<TVertex> Vertices { get; }
    }
}
