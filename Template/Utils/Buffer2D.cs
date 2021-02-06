using System.Drawing;

namespace EMBC.Utils
{
    public class Buffer2D<T>
    {
        #region // storage

        public Size Size { get; }

        public T[] Buffer { get; }

        #endregion

        #region // ctor

        public Buffer2D(Size size)
        {
            Size = size;
            Buffer = new T[Width * Height];
        }

        #endregion

        #region // routines

        public int Width => Size.Width;

        public int Height => Size.Height;

        public T this[int x, int y]
        {
            get => GetValue(x, y);

            set => SetValue(x, y, value);
        }

        public int GetIndex(int x, int y) => x + y * Width;

        public (int x, int y) GetXY(int index)
        {
            var y = index / Width;
            var x = index - y * Width;
            return (x, y);
        }

        public void SetValue(int x, int y, in T value) => Buffer[GetIndex(x, y)] = value;

        public T GetValue(int x, int y) => Buffer[GetIndex(x, y)];

        public void Clear(T value = default) => Buffer.Fill(value);

        #endregion
    }
}
