using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using V3.Input;
using V3.Widgets;

namespace V3.Screens
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DeathScreen : AbstractScreen, IInitializable
    {
        private static TimeSpan sTotalDelay = TimeSpan.FromSeconds(2);

        private readonly ContentManager mContentManager;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;
        private readonly MenuActions mMenuActions;
        private readonly WidgetFactory mWidgetFactory;

        private Button mMenuButton;
        private Button mCloseGameButton;
        private Vector2 mButtonPosition;
        private Vector2 mCenter;
        private Vector2 mFontCenter;
        private SpriteFont mDeathFont;
        private Texture2D mRectangle;

        private TimeSpan mDelayTimer = sTotalDelay;

        /// <summary>
        /// Creates a death screen if the players health reaches 0.
        /// </summary>

        public DeathScreen(ContentManager contentManager,
            GraphicsDeviceManager graphicsDeviceManager,
            MenuActions menuActions,
            WidgetFactory widgetFactory)
            : base(false, true)
        {
            mContentManager = contentManager;
            mGraphicsDeviceManager = graphicsDeviceManager;
            mMenuActions = menuActions;
            mWidgetFactory = widgetFactory;
        }

        public override bool HandleKeyEvent(IKeyEvent keyEvent)
        {
            return false;
        }

        public override bool HandleMouseEvent(IMouseEvent mouseEvent)
        {
            if (mDelayTimer <= TimeSpan.Zero)
            {
                mMenuButton.HandleMouseEvent(mouseEvent);
                mCloseGameButton.HandleMouseEvent(mouseEvent);
            }
            return false;
        }

        public void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");
            mDeathFont = mContentManager.Load<SpriteFont>("Fonts/DeathFont");

            mMenuButton = mWidgetFactory.CreateButton("Zum Hauptmenü");
            mMenuButton.Position = Vector2.Zero;
            mMenuButton.PaddingX = 10;
            mMenuButton.PaddingY = 0;
            mMenuButton.Size = mMenuButton.GetMinimumSize();
            mMenuButton.BackgroundColor = Color.Gray * 0.7f;

            mCloseGameButton = mWidgetFactory.CreateButton("Spiel schließen");
            mCloseGameButton.Position = Vector2.Zero;
            mCloseGameButton.PaddingX = 10;
            mCloseGameButton.PaddingY = 0;
            mCloseGameButton.Size = mCloseGameButton.GetMinimumSize();
            mCloseGameButton.BackgroundColor = Color.Gray * 0.7f;
        }

        public override void Update(GameTime gameTime)
        {
            var viewport = mGraphicsDeviceManager.GraphicsDevice.Viewport;

            mCenter = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            mFontCenter = mDeathFont.MeasureString("Ihr        seid        tot") / 2;
            mButtonPosition = new Vector2(mCenter.X - mFontCenter.X * 2/3, mCenter.Y + mFontCenter.Y * 2);

            if (mDelayTimer > TimeSpan.Zero)
                mDelayTimer -= gameTime.ElapsedGameTime;

            mMenuButton.Position = mButtonPosition;
            mCloseGameButton.Position = mMenuButton.Position + new Vector2(mFontCenter.X * 3/ 4, 0);

            if (mMenuButton.IsClicked)
            {
                mMenuActions.OpenMainScreen();
            }
            else if (mCloseGameButton.IsClicked)
            {
                mMenuActions.Exit();
            }

            mMenuButton.IsSelected = mMenuButton.CheckSelected(Mouse.GetState().Position);
            mMenuButton.IsClicked = false;

            mCloseGameButton.IsSelected = mCloseGameButton.CheckSelected(Mouse.GetState().Position);
            mCloseGameButton.IsClicked = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float displayRatio = (float) (1 - mDelayTimer.TotalMilliseconds / sTotalDelay.TotalMilliseconds);

            spriteBatch.Begin();
            spriteBatch.Draw(mRectangle,
                mGraphicsDeviceManager.GraphicsDevice.Viewport.Bounds,
                Color.Black * 0.5f * displayRatio);

            spriteBatch.DrawString(mDeathFont,
                "Ihr     seid     tot",
                mCenter,
                Color.Firebrick * displayRatio,
                0,
                mFontCenter,
                1.0f,
                SpriteEffects.None,
                0.5f);

            if (mDelayTimer <= TimeSpan.Zero)
            {
                mMenuButton.Draw(spriteBatch);
                mCloseGameButton.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
