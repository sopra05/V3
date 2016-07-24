using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Data;

namespace V3.Effects
{
    /// <summary>
    /// Interface for a single effect.
    /// </summary>
    public interface IEffect
    {
        /// <summary>
        /// Is the effect playing at the moment?
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Play the specific effect once, do not loop.
        /// </summary>
        /// <param name="position">Position where effect should be played. Points to the middle of the effect animation.</param>
        /// <param name="size">Size of the effect.</param>
        /// <param name="optionsManager">For checking the volume of the sound if there is one.</param>
        void PlayOnce(Point position, Point size, IOptionsManager optionsManager);

        /// <summary>
        /// Update the effect.
        /// </summary>
        /// <param name="gameTime">Game time used for checking animation duration.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Draw the effect.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used.</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Load graphics and possibly sound for the effect.
        /// </summary>
        /// <param name="contentManager">Content manager used.</param>
        void LoadContent(ContentManager contentManager);
    }
}