using Microsoft.Xna.Framework;

namespace V3
{
    /// <summary>
    /// A class for telling when to do an update for events with a constant frequency.
    /// </summary>
    public sealed class UpdatesPerSecond
    {
        private double mTimeBetweenUpdates;
        private double mTimeSinceLastUpdate;

        /// <summary>
        /// Initializes class.
        /// </summary>
        /// <param name="frequency">How many updates should be done per second.</param>
        public UpdatesPerSecond(double frequency)
        {
            ChangeFrequency(frequency);
        }

        /// <summary>
        /// Change update frequency.
        /// </summary>
        /// <param name="frequency">How many updates should be done per second.</param>
        private void ChangeFrequency(double frequency)
        {
            mTimeBetweenUpdates = 1d / frequency;
        }

        /// <summary>
        /// Tells if it is time to do an update.
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        /// <returns></returns>
        public bool IsItTime(GameTime gameTime)
        {
            mTimeSinceLastUpdate += gameTime.ElapsedGameTime.TotalSeconds;
            if (mTimeSinceLastUpdate >= mTimeBetweenUpdates)
            {
                Reset();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reset internal timer for checking if update should be done.
        /// </summary>
        private void Reset()
        {
            mTimeSinceLastUpdate = 0;
        }
    }
}