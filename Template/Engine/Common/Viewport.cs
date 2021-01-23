namespace EMBC.Engine.Common
{
    public struct Viewport
    {
        #region // storage

        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public double MinZ { get; }
        public double MaxZ { get; }

        #endregion

        #region // queries
        public System.Drawing.Point Location => new System.Drawing.Point(X, Y);
        public System.Drawing.Size Size => new System.Drawing.Size(Width, Height);
        public double AspectRatio => (double)Width / Height;

        #endregion

        #region // ctor

        public Viewport(int x, int y, int width, int height, double minZ, double maxZ)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            MinZ = minZ;
            MaxZ = maxZ;
        }

        public Viewport(System.Drawing.Point location, System.Drawing.Size size, double minZ, double maxZ) :
            this(location.X, location.Y, size.Width, size.Height, minZ, maxZ)
        {
        }

        #endregion
    }
}
