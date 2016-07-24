using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Objects.Sprite
{
    public abstract class AbstractSpriteCreature : ISpriteCreature
    {
        // How many frames are there for each animation type?
        protected virtual int IdleFrames { get; } = 4;
        protected virtual int MovingFrames { get; } = 8;
        protected virtual int AttackingFrames { get; } = 4;
        protected virtual int DyingFrames { get; } = 6;
        protected virtual int SpecialFrames { get; } = 4;

        /// The row of the specific textures in the texture file.
        protected virtual int IdleTextureIndex { get; } = 0;
        protected virtual int MovingTextureIndex { get; } = 4;
        protected virtual int AttackingTextureIndex { get; } = 12;
        protected virtual int DyingTextureIndex { get; } = 18;
        protected virtual int SpecialTextureIndex { get; } = 24;
        

        // Specify the size of a single animation frame in pixel.
        private const int SpriteWidth = 128;
        private const int SpriteHeight = 128;
        private readonly Point mSpriteShift = new Point(-SpriteWidth / 2, -SpriteHeight * 3 / 4);
        private readonly Point mSpriteSize = new Point(SpriteWidth, SpriteHeight);

        private double mTimeSinceUpdate;
        private Texture2D mTexture;
        // How much time until animation frame is changed (in milliseconds).
        private const int UpdatesPerMSec = 125;  // 8 FPS
        private MovementState mLastMovementState = MovementState.Idle;
        private MovementState mCurrentMovementState = MovementState.Idle;
        private int mMaxAnimationSteps;
        private int mMovementStateRow;
        private bool mIdleBackwardsLoop;
        private int mAnimationState;

        // Fields for PlayOnce method.
        private bool mPriorityAnimation;
        private UpdatesPerSecond mUpS;

        protected abstract string TextureFile { get; }

        /// <summary>
        /// Loads the texture file and prepares animations.
        /// </summary>
        /// <param name="contentManager"></param>
        public void Load(ContentManager contentManager)
        {
            mTexture = contentManager.Load<Texture2D>("Sprites/" + TextureFile);
            mMaxAnimationSteps = IdleFrames;
        }

        /// <summary>
        /// Draws the sprite on the screen.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used for drawing.</param>
        /// <param name="position">Position on the screen where sprite is drawn to.</param>
        /// <param name="movementState">What moveset will be used? (Moving, Attacking...)</param>
        /// <param name="movementDirection">Where does the sprite face?</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, MovementState movementState, MovementDirection movementDirection)
        {
            mCurrentMovementState = movementState;
            Point sourceLocation = new Point((mMovementStateRow + mAnimationState) * SpriteWidth, 
                (int) movementDirection * SpriteHeight);
            Rectangle sourceRectangle = new Rectangle(sourceLocation, mSpriteSize);
            spriteBatch.Draw(mTexture, position + new Vector2(mSpriteShift.X, mSpriteShift.Y), sourceRectangle, Color.White);
        }

        public void DrawStatic(SpriteBatch spriteBatch, Point position, MovementState movementState, MovementDirection movementDirection)
        {
            Point sourceLocation = new Point((int)movementState * SpriteWidth, (int)movementDirection * SpriteHeight);
            Rectangle destinationRectangle = new Rectangle(position + mSpriteShift, mSpriteSize);
            Rectangle sourceRectangle = new Rectangle(sourceLocation, mSpriteSize);
            spriteBatch.Draw(mTexture, destinationRectangle, sourceRectangle, Color.White);
        }

        /// <summary>
        /// Change the sprite to show an animation.
        /// </summary>
        /// <param name="gameTime">Elapsed game time is used for calculating FPS.</param>
        public void PlayAnimation(GameTime gameTime)
        {
            // Playing a single animation cycle just once with higher priority.
            if (mPriorityAnimation)
            {
                if (mAnimationState < mMaxAnimationSteps - 1)
                {
                    if (mUpS.IsItTime(gameTime))
                    {
                        mAnimationState++;
                    }
                    return;
                }
                mAnimationState = 0;
                mCurrentMovementState = mLastMovementState;
                SelectFrames(mCurrentMovementState);
                mPriorityAnimation = false;
                return;
            }

            // Change texture for showing animations according to elapsed game time.
            mTimeSinceUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (mTimeSinceUpdate < UpdatesPerMSec)
            {
                return;
            }
            mTimeSinceUpdate = 0;

            // Check which specific animation sprites need to be used.
            // Only check if movement state is changed.
            if (mCurrentMovementState != mLastMovementState)
            {
                SelectFrames(mCurrentMovementState);
                mAnimationState = 0;
                mLastMovementState = mCurrentMovementState;
            }

            // Idle animation is looped back and forth.
            if (mIdleBackwardsLoop)
            {
                mAnimationState--;
                if (mAnimationState <= 0)
                {
                    mIdleBackwardsLoop = false;
                }
                return;
            }

            if (mAnimationState < mMaxAnimationSteps - 1)
            {
                mAnimationState++;
            }
            else
            {
                if (mLastMovementState == MovementState.Idle)
                {
                    mIdleBackwardsLoop = true;
                    mAnimationState--;
                }
                else if (mLastMovementState == MovementState.Dying)
                {
                }
                else
                {
                    mAnimationState = 0;
                }
            }
        }

        private void SelectFrames(MovementState currentMovementState)
        {
            switch (currentMovementState)
            {
                case MovementState.Idle:
                    mMaxAnimationSteps = IdleFrames;
                    mMovementStateRow = IdleTextureIndex;
                    mIdleBackwardsLoop = false;
                    break;
                case MovementState.Moving:
                    mMaxAnimationSteps = MovingFrames;
                    mMovementStateRow = MovingTextureIndex;
                    break;
                case MovementState.Attacking:
                    mMaxAnimationSteps = AttackingFrames;
                    mMovementStateRow = AttackingTextureIndex;
                    break;
                case MovementState.Dying:
                    mMaxAnimationSteps = DyingFrames;
                    mMovementStateRow = DyingTextureIndex;
                    break;
                case MovementState.Special:
                    mMaxAnimationSteps = SpecialFrames;
                    mMovementStateRow = SpecialTextureIndex;
                    break;
                default:
                    mMaxAnimationSteps = 1; // No Animation if default case is reached.
                    break;
            }
        }

        /// <summary>
        /// Plays the specified animation fully, but only once.
        /// </summary>
        /// <param name="animation">For which movement state the animation should be played.</param>
        /// <param name="duration">How long (or how slow) should the animation be?</param>
        public void PlayOnce(MovementState animation, TimeSpan duration)
        {
            mLastMovementState = mCurrentMovementState;
            mCurrentMovementState = animation;
            mPriorityAnimation = true;
            SelectFrames(animation);
            mAnimationState = 0;
            mUpS = new UpdatesPerSecond(1d / (duration.TotalSeconds / mMaxAnimationSteps));
        }
    }
}