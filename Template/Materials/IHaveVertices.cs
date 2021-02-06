using System.Collections.Generic;

namespace EMBC.Materials
{
    public interface IHaveVertices<out TVertex>
    {
        TVertex[] Vertices { get; }
    }
}
