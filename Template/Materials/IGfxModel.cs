using EMBC.Mathematics;
using System;

namespace EMBC.Materials
{
    public interface IGfxModel :
        IDisposable
    {
        void Render(in Matrix4D matrixToClip);
    }
}
