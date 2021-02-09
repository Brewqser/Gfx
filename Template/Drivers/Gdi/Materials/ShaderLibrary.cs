using System;
using System.Collections.Generic;
using EMBC.Drivers.Gdi.Render;
using EMBC.Materials;

namespace EMBC.Drivers.Gdi.Materials
{
    public class ShaderLibrary :
        IDisposable
    {
        #region // storage

        private List<IShader> Shaders { get; set; } = new List<IShader>();

        public Position.Shader ShaderPosition { get; }

        public PositionColor.Shader ShaderPositionColor { get; }

        public PositionTexture.Shader ShaderPositionTexture { get; }

        #endregion

        #region // ctor

        public ShaderLibrary(RenderHost renderHost)
        {
            Shaders.Add(ShaderPosition = new Position.Shader(renderHost));
            Shaders.Add(ShaderPositionColor = new PositionColor.Shader(renderHost));
            Shaders.Add(ShaderPositionTexture = new PositionTexture.Shader(renderHost));
        }

        public void Dispose()
        {
            Shaders.ForEach(shader => shader.Dispose());
        }

        #endregion
    }
}
