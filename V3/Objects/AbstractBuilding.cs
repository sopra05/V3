using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Objects
{
    public abstract class AbstractBuilding : IBuilding
    {
        private readonly string mTextureName;
        private Texture2D mTexture;
        private readonly BuildingFace mFacing;
        private Rectangle mTilesetRectangle;

        public Vector2 Position { get; set; }
        protected abstract int MaxRobustness { get; }
        public abstract int Robustness { get; protected set; }
        public abstract string Name { get; }
        public virtual int MaxGivesWeapons { get; protected set; }
        private bool mIsDestroyed;
        public Rectangle BoundaryRectangle { get; }
        public int Id { get; }

        protected AbstractBuilding(Vector2 position, Rectangle size, string textureName, BuildingFace facing)
        {
            Position = position;
            Id = IdGenerator.GetNextId();
            // TODO: Hella ugly code
            mTilesetRectangle = size;
            // Boundary rectangle is smaller than the texture size:
            if (this is Castle)
            {
                BoundaryRectangle = new Rectangle(size.X + 96, size.Y + 296, 900, 500);
            }
            else
            {
                BoundaryRectangle = new Rectangle(size.X, size.Y + size.Height / 2, size.Width * 4 / 5, size.Height / 2);
            }
            mTextureName = textureName;
            mFacing = facing;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int status = 0;
            if (mIsDestroyed)
            {
                status = 2;
            }
            else if (Robustness < MaxRobustness / 2)
            {
                status = 1;
            }
            Rectangle source = new Rectangle(status * mTilesetRectangle.Width, (mFacing == BuildingFace.SW ? 0 : 1) * mTilesetRectangle.Height + (this is Forge ? 384 : 0), mTilesetRectangle.Width, mTilesetRectangle.Height);
            spriteBatch.Draw(mTexture, mTilesetRectangle, source, Color.White);
        }


        public void LoadContent(ContentManager contentManager)
        {
            mTexture = contentManager.Load<Texture2D>("Textures/" + mTextureName);
            //mOnePixelTexture = contentManager.Load<Texture2D>("Sprites/WhiteRectangle");
        }

        public void TakeDamage(int damage)
        {
            if (Robustness > 0)
            {
                Robustness -= damage;
            }

            if (Robustness <= 0)
            {
                Destroyed();
            }
        }

        public void UpgradeCounter()
        {
            MaxGivesWeapons -= 1;
        }


        private void Destroyed()
        {
            mIsDestroyed = true;
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
