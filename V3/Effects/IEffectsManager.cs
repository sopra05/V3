using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Effects
{
    /// <summary>
    /// Interface for managing visual effects like explosions and stuff.
    /// </summary>
    public interface IEffectsManager
    {
        /// <summary>
        /// Update all effects.
        /// </summary>
        /// <param name="gameTime">Game time used for calculation effects duration.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draw all effects.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used.</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Play an effect once, then delete it.
        /// </summary>
        /// <param name="effect">Which effect to play.</param>
        /// <param name="position">Position where effect should be played. Points to the middle of the effect animation.</param>
        /// <param name="size">Size of the effect.</param>
        void PlayOnce(IEffect effect, Point position, Point size);
    }
}