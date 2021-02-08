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

        public Position.Shader ShaderPosition { get; set; }

        public PositionColor.Shader ShaderPositionColor { get; set; }

        #endregion

        #region // ctor

        public ShaderLibrary(RenderHost renderHost)
        {
            Shaders.Add(ShaderPosition = new Position.Shader(renderHost));
            Shaders.Add(ShaderPositionColor = new PositionColor.Shader(renderHost));
        }

        public void Dispose()
        {
            foreach (var shader in Shaders)
            {
                shader.Dispose();
            }
            Shaders = default;
        }

        #endregion
    }
}
