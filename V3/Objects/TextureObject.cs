using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace V3.Objects
{
    /// <summary>
    /// Texture objects represent map data like tiles and map objects which are placed on the screen.
    /// </summary>
    public sealed class TextureObject : IGameObject
    {
        public Vector2 Position { get; set; }
        public int Id { get; }
        public Rectangle BoundaryRectangle => new Rectangle(mDrawPosition, mTextureSize);
        private readonly string mTextureName;
        private readonly Point mDrawPosition;
        private readonly Point mTextureSize;
        private Texture2D mTexture;
        private readonly Point mTextureSource;

        /// <summary>
        /// Creates an empty TextureObject.
        /// </summary>
        public TextureObject()
        {
            Id = IdGenerator.GetNextId();
            Position = Vector2.Zero;
            mDrawPosition = Point.Zero;
            mTextureSize = Point.Zero;
            mTextureName = "EmptyPixel";
            mTextureSource = Point.Zero;
        }

        public TextureObject(Point position, Point drawPosition, Point textureSize, Point textureSource, string textureName)
        {
            Position = position.ToVector2();
            mDrawPosition = drawPosition;
            mTextureSize = textureSize;
            mTextureName = textureName;
            mTextureSource = textureSource;
        }

        public void LoadContent(ContentManager contentManager)
        {
            mTexture = contentManager.Load<Texture2D>("Textures/" + mTextureName);
            //mOnePixelTexture = contentManager.Load<Texture2D>("Sprites/WhiteRectangle");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, BoundaryRectangle, new Rectangle(mTextureSource, mTextureSize), Color.White);
        }

        public IGameObject GetSelf()
        {
            return this;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;
            if (obj == this)
                return true;
            if (!(obj is IGameObject))
                return false;
            return Id.Equals(((IGameObject) obj).Id);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
