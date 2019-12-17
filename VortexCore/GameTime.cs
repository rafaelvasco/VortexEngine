using System;

namespace VortexCore
{
    public class GameTime
    {
        public TimeSpan TotalGameTime { get; internal set; }

        public TimeSpan ElapsedGameTime { get; internal set; }

        public bool IsRunningSlowly { get; internal set; }

        public GameTime()
        {
            TotalGameTime = TimeSpan.Zero;
            ElapsedGameTime = TimeSpan.Zero;
            IsRunningSlowly = false;
        }

        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime, bool isRunningSlowly = false)
        {
            TotalGameTime = totalGameTime;
            ElapsedGameTime = elapsedGameTime;
            IsRunningSlowly = isRunningSlowly;
        }

    }
}
