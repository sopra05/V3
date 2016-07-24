using System;
using Microsoft.Xna.Framework;

namespace V3.Screens
{
    /// <summary>
    /// Counts the frames per second based on the elapsed time and the number
    /// of frames that have been drawn.  Call Update in each Update, and
    /// AddFrame in each Draw.
    /// </summary>
    public sealed class FpsCounter
    {
        /// <summary>
        /// The current frames per second.
        /// </summary>
        public int Fps { get; private set; }

        private int mFrameCount;
        private TimeSpan mTimeSpan = TimeSpan.Zero;

        /// <summary>
        /// Updates the elapsed time and -- once every second -- the fps value.
        /// </summary>
        /// <param name="gameTime">the elapsed game time</param>
        public void Update(GameTime gameTime)
        {
            mTimeSpan += gameTime.ElapsedGameTime;

            if (mTimeSpan > TimeSpan.FromSeconds(1))
            {
                mTimeSpan -= TimeSpan.FromSeconds(1);
                Fps = mFrameCount;
                mFrameCount = 0;
            }
        }

        /// <summary>
        /// Registers that a frame has been drawn. Should be called once for
        /// every Draw.
        /// </summary>
        public void AddFrame()
        {
            mFrameCount++;
        }
    }
}
