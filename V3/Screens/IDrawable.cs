using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Screens
{
    /// <summary>
    /// An object that can be drawn using a sprite batch.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws this object using the given sprite batch.
        /// </summary>
        /// <param name="gameTime">a snapshot of the game time</param>
        /// <param name="spriteBatch">the sprite batch to use for drawing
        /// this object</param>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
