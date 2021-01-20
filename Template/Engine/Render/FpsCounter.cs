using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMBC.Engine.Render
{
    public class FpsCounter:
        IDisposable
    {
        #region // storage

        public TimeSpan UpdateRate { get; }
        private Stopwatch StowatchUpdate { get; set; }
        private Stopwatch StowatchFrame { get; set; }
        private TimeSpan Elapsed { get; set; }
        private int FrameCount { get; set; }
        public double FpsRender { get; private set; }
        public double FpsGlobal { get; private set; }
        public string FpsString => $"FPS = {FpsRender:0} ({FpsGlobal:0})";

        #endregion

        #region // ctor

        public FpsCounter(TimeSpan updateRate)
        {
            UpdateRate = updateRate;

            StowatchUpdate = new Stopwatch();
            StowatchFrame = new Stopwatch();

            StowatchUpdate.Start();

            Elapsed = TimeSpan.Zero;
        }

        public void Dispose()
        {
            StowatchUpdate?.Stop();
            StowatchUpdate = default;

            StowatchFrame?.Stop();
            StowatchFrame = default;
        }

        #endregion

        #region // routines

        public void StartFrame()
        {
            StowatchFrame.Restart();
        }

        public void StopFrame()
        {
            StowatchFrame.Stop();
            Elapsed += StowatchFrame.Elapsed;
            FrameCount++;

            var updateElapsed = StowatchUpdate.Elapsed;

            if (updateElapsed >= UpdateRate)
            {
                FpsRender = FrameCount / Elapsed.TotalSeconds;
                FpsGlobal = FrameCount / updateElapsed.TotalSeconds;

                StowatchUpdate.Restart();
                Elapsed = TimeSpan.Zero;
                FrameCount = 0;
            }
        }
        #endregion
    }
}
