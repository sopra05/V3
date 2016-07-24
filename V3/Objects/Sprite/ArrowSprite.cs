using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Objects.Sprite
{
    /// <summary>
    /// A simple arrow from different directions without animations. 
    /// </summary>
    public sealed class ArrowSprite : ISpriteCreature
    {
        private Texture2D mTexture;
        private const int Size = 64;

        public void Load(ContentManager contentManager)
        {
            mTexture = contentManager.Load<Texture2D>("Sprites/arrows");
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, MovementState movementState, MovementDirection movementDirection)
        {
            spriteBatch.Draw(mTexture, position - new Vector2(Size / 2f), new Rectangle(0, Size * (int) movementDirection, Size, Size), Color.White);
        }

        public void DrawStatic(SpriteBatch spriteBatch,
            Point position,
            MovementState movementState,
            MovementDirection movementDirection)
        {
            Draw(spriteBatch, position.ToVector2(), movementState, movementDirection);
        }

        public void PlayAnimation(GameTime gameTime)
        {
        }

        public void PlayOnce(MovementState animation, TimeSpan duration)
        {
        }
    }
}