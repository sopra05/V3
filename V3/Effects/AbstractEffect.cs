using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Data;

namespace V3.Effects
{
    public abstract class AbstractEffect : IEffect
    {
        private Texture2D mTexture;
        private int mSpritesPerRow;
        private int mTotalSprites;
        private int mAnimationState;
        private Rectangle mAnimationRectangle;
        private SoundEffect mSoundEffect;
        private SoundEffectInstance mSoundEffectInstance;

        public bool IsPlaying { get; private set; }
        protected virtual UpdatesPerSecond UpdatesPerSecond { get; } = new UpdatesPerSecond(24);
        protected abstract string TextureFile { get; }
        protected virtual Point SpriteSize { get; } = new Point(128, 128);
        protected virtual string SoundFile { get; } = String.Empty;

        public void LoadContent(ContentManager contentManager)
        {
            mTexture = contentManager.Load<Texture2D>("Effects/" + TextureFile);
            mSpritesPerRow = mTexture.Width / SpriteSize.X;
            mTotalSprites = mSpritesPerRow * (mTexture.Height / SpriteSize.Y);
            if (SoundFile.Length != 0)
            {
                mSoundEffect = contentManager.Load<SoundEffect>("Sounds/" + SoundFile);
                mSoundEffectInstance = mSoundEffect.CreateInstance();
            }
        }

        public void PlayOnce(Point position, Point size, IOptionsManager optionsManager)
        {
            mAnimationRectangle = new Rectangle(position - size / new Point(2, 2), size);
            IsPlaying = true;
            if (SoundFile.Length != 0)
            {
                mSoundEffectInstance.Volume = optionsManager.Options.GetEffectiveVolume();
                mSoundEffectInstance.Play();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!IsPlaying) return;
            if (UpdatesPerSecond.IsItTime(gameTime))
            {
                if (mAnimationState < mTotalSprites)
                {
                    mAnimationState++;
                }
                else
                {
                    mAnimationState = 0;
                    IsPlaying = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsPlaying) return;
            spriteBatch.Draw(mTexture, mAnimationRectangle, 
                new Rectangle(new Point(mAnimationState % mSpritesPerRow * SpriteSize.X, mAnimationState / mSpritesPerRow * SpriteSize.Y), SpriteSize), 
                Color.White);
        }
    }
}