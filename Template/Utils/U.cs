using System;
using System.Collections.Generic;

namespace EMBC.Utils
{
    public static class U
    {
        public static int EnvironmentProcessorCount { get; } = Environment.ProcessorCount;

        public static System.Threading.Tasks.ParallelOptions ParallelOptionsDefault = new System.Threading.Tasks.ParallelOptions { MaxDegreeOfParallelism = EnvironmentProcessorCount };

        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
                return value;
            }
            if (value > max)
            {
                value = max;
            }
            return value;
        }

        public static T Cloned<T>(this T cloneable) where T : ICloneable
        {
            return (T)cloneable.Clone();
        }

        public static void Fill<T>(this T[] array, T value)
        {
            var length = array.Length;
            if (length == 0) return;

            var seed = Math.Min(32, array.Length);
            for (var i = 0; i < seed; i++)
            {
                array[i] = value;
            }

            int count;
            for (count = seed; count <= length / 2; count *= 2)
            {
                Array.Copy(array, 0, array, count, count);
            }

            var leftover = length - count;
            if (leftover > 0)
            {
                Array.Copy(array, 0, array, count, leftover);
            }
        }

        public static void ForEach<T> (this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static IntPtr Hnadle(this System.Windows.Forms.Control window)
        {
            return window.IsDisposed ? default : Handle((System.Windows.Forms.IWin32Window)window);
        }

        public static IntPtr Handle(this System.Windows.Forms.IWin32Window window)
        {
            return window.Handle;
        }

        public static IntPtr Handle(this System.Windows.Media.Visual window)
        {
            var handleSource = window.HandleSource();
            return handleSource == null || handleSource.IsDisposed ? default : handleSource.Handle;
        }

        public static System.Windows.Interop.HwndSource HandleSource(this System.Windows.Media.Visual window)
        {
            return System.Windows.PresentationSource.FromVisual(window) as System.Windows.Interop.HwndSource;
        }
        public static void Swap<T>(ref T value0, ref T value1)
        {
            var temp = value0;
            value0 = value1;
            value1 = temp;
        }
        public static int ToRgba(this System.Drawing.Color color)
        {
            return ((((color.A << 8) + color.B) << 8) + color.G << 8) + color.R;
        }

        public static System.Drawing.Color FromRgbaToColor(this int color)
        {
            return System.Drawing.Color.FromArgb
            (
                (color >> 24) & 0xFF,
                (color >> 0) & 0xFF,
                (color >> 8) & 0xFF,
                (color >> 16) & 0xFF
            );
        }
    }
}
