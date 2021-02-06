namespace EMBC.Drivers.Gdi.Materials
{
    public class ShaderLibrary
    {
        #region // storage

        public Position.Shader ShaderPosition { get; set; }

        #endregion

        #region // ctor

        public ShaderLibrary()
        {
            ShaderPosition = new Position.Shader();
        }

        #endregion
    }
}
