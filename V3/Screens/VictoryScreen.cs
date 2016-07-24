using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using V3.Input;

namespace V3.Screens
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class VictoryScreen : AbstractScreen, IInitializable
    {
        private static TimeSpan sTotalDelay = TimeSpan.FromSeconds(1);

        private readonly ContentManager mContentManager;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;

        private Rectangle mVictoryRectangle;
        private Vector2 mFontCenter;
        private Vector2 mCenter;
        private SpriteFont mVictoryFont;
        private Texture2D mRectangle;

        private TimeSpan mDelayTimer = sTotalDelay;

        /// <summary>
        /// Creates a victory screen if the player defeats the boss enemy.
        /// </summary>

        public VictoryScreen(ContentManager contentManager,
            GraphicsDeviceManager graphicsDeviceManager
            )
            : base(false, true)
        {
            mContentManager = contentManager;
            mGraphicsDeviceManager = graphicsDeviceManager;
        }

        public override bool HandleKeyEvent(IKeyEvent keyEvent)
        {
            return false;
        }

        public void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");
            mVictoryFont = mContentManager.Load<SpriteFont>("Fonts/VictoryFont");
        }

        public override void Update(GameTime gameTime)
        {
            var viewport = mGraphicsDeviceManager.GraphicsDevice.Viewport;
            if (mDelayTimer > TimeSpan.Zero)
                mDelayTimer -= gameTime.ElapsedGameTime;

            mCenter = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            mFontCenter = mVictoryFont.MeasureString("Die    Rache    ist    euer!") / 2;
            mVictoryRectangle = new Rectangle(0, (int)mCenter.Y - (int)mFontCenter.Y - 10, viewport.Width, (int)mFontCenter.Y * 2 - 10);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float displayRatio = (float)(1 - mDelayTimer.TotalMilliseconds / sTotalDelay.TotalMilliseconds);
                spriteBatch.Begin();
                spriteBatch.Draw(mRectangle,
                    mVictoryRectangle,
                    Color.Black * 0.8f * displayRatio);

                spriteBatch.DrawString(mVictoryFont,
                    "Die    Rache    ist    euer!",
                    mCenter,
                    Color.DarkGoldenrod * displayRatio, 0, mFontCenter, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.End();
            }
        }
    }
