using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Objects.Sprite
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISpriteCreature
    {
        /// <summary>
        /// Loads the texture file and prepares animations.
        /// </summary>
        /// <param name="contentManager">Content manager used.</param>
        void Load(ContentManager contentManager);

        /// <summary>
        /// Draws the sprite on the screen.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used for drawing.</param>
        /// <param name="position">Position on the screen in pixels where the sprite should stand.</param>
        /// <param name="movementState">What moveset will be used? (Moving, Attacking...)</param>
        /// <param name="movementDirection">Where does the sprite face to?</param>
        void Draw(SpriteBatch spriteBatch, Vector2 position, MovementState movementState, MovementDirection movementDirection);

        /// <summary>
        /// Draws a static image of the sprite. No animations.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used for drawing.</param>
        /// <param name="position">Position of the sprite in pixels. (Where are the feet of the sprite placed.</param>
        /// <param name="movementState">What moveset will be used? (Moving, Attacking...)</param>
        /// <param name="movementDirection">Where does the sprite face to?</param>
        void DrawStatic(SpriteBatch spriteBatch,
            Point position,
            MovementState movementState,
            MovementDirection movementDirection);

        /// <summary>
        /// Change the sprite to show an animation.
        /// </summary>
        /// <param name="gameTime">Elapsed game time is used for calculating FPS.</param>
        void PlayAnimation(GameTime gameTime);

        /// <summary>
        /// Plays the specified animation fully, but only once.
        /// </summary>
        /// <param name="animation">For which movement state the animation should be played.</param>
        /// <param name="duration">How long (or how slow) should the animation be?</param>
        void PlayOnce(MovementState animation, TimeSpan duration);
    }
}