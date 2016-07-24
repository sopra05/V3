using Microsoft.Xna.Framework;

namespace V3.Screens
{
    /// <summary>
    /// An object that can be updated.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Updates the status of this object.
        /// </summary>
        /// <param name="gameTime">a snapshot of the game time</param>
        void Update(GameTime gameTime);
    }
}
