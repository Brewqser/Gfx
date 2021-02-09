using System;
using System.Collections.Generic;
using System.Drawing;

namespace EMBC.Materials
{
    public interface ITextureResource :
        IEqualityComparer<ITextureResource>,
        IDisposable
    {
        int Id { get; }

        string Name { get; }

        Size Size { get; }

        Bitmap Source { get; }
    }
}
