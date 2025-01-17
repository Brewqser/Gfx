﻿namespace EMBC.Utils
{
    public static class RandomSync
    {
        #region // pure

        private static readonly object m_Lock = new object();
        private static readonly System.Random m_Random = new System.Random((int)System.DateTime.UtcNow.Ticks);

        public static int Next()
        {
            lock (m_Lock)
            {
                return m_Random.Next();
            }
        }

        public static int Next(int maxValue)
        {
            lock (m_Lock)
            {
                return m_Random.Next(maxValue);
            }
        }

        public static int Next(int minValue, int maxValue)
        {
            lock (m_Lock)
            {
                return m_Random.Next(minValue, maxValue);
            }
        }

        public static void NextBytes(byte[] buffer)
        {
            lock (m_Lock)
            {
                m_Random.NextBytes(buffer);
            }
        }

        public static double NextDouble()
        {
            lock (m_Lock)
            {
                return m_Random.NextDouble();
            }
        }

        #endregion

        #region // extensions

        public static double NextDouble(double maxValue)
        {
            lock (m_Lock)
            {
                return NextDouble() * maxValue;
            }
        }

        public static double NextDouble(double minValue, double maxValue)
        {
            lock (m_Lock)
            {
                return minValue + NextDouble(maxValue - minValue);
            }
        }

        #endregion
    }
}
