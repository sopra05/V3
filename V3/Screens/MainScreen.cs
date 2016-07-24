using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using V3.Input;
using V3.Widgets;

namespace V3.Screens
{
    /// <summary>
    /// The screen for the main menu.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainScreen : AbstractScreen, IInitializable
    {
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;
        private readonly IMenu mMenu;
        private readonly MenuActions mMenuActions;
        private readonly WidgetFactory mWidgetFactory;
        private readonly ContentManager mContentManager;

        private Button mButtonStart;
        private Button mButtonLoad;
        private Button mButtonOptions;
        private Button mButtonTechDemo;
        private Button mButtonExit;
        private Button mButtonStatistics;
        private Button mButtonAchievements;

        private Texture2D mTitleImage;
        private Texture2D mBackgroundImage;
        private Vector2 mImagePosition;
        private Random mRandom;
        private Vector2 mImageScrollDirection;
        private Rectangle TitleImageBounds => mTitleImage.Bounds;
        private Rectangle BackgroundImageBounds => mBackgroundImage.Bounds;
        private Rectangle ScreenBounds => mGraphicsDeviceManager.GraphicsDevice.Viewport.Bounds;

        /// <summary>
        /// Creates a new main screen.
        /// </summary>
        public MainScreen(GraphicsDeviceManager graphicsDeviceManager,
                VerticalMenu menu, MenuActions menuActions,
                WidgetFactory widgetFactory, ContentManager contentManager) : base(false, false)
        {
            mGraphicsDeviceManager = graphicsDeviceManager;
            mMenu = menu;
            mMenuActions = menuActions;
            mWidgetFactory = widgetFactory;
            mContentManager = contentManager;
        }

        public void Initialize()
        {
            mMenuActions.ApplyOptions();

            mButtonStart = mWidgetFactory.CreateButton("Neues Spiel");
            mButtonLoad = mWidgetFactory.CreateButton("Spiel laden");
            mButtonLoad.IsEnabled = mMenuActions.CanLoadGame();
            mButtonOptions = mWidgetFactory.CreateButton("Optionen");
            mButtonStatistics = mWidgetFactory.CreateButton("Statistiken");
            mButtonAchievements = mWidgetFactory.CreateButton("Erfolge");
            mButtonTechDemo = mWidgetFactory.CreateButton("Tech-Demo");
            mButtonExit = mWidgetFactory.CreateButton("Spiel schlieﬂen");

            mMenu.Widgets.Add(mButtonStart);
            mMenu.Widgets.Add(mButtonLoad);
            mMenu.Widgets.Add(mButtonOptions);
            mMenu.Widgets.Add(mButtonStatistics);
            mMenu.Widgets.Add(mButtonAchievements);
            mMenu.Widgets.Add(mButtonTechDemo);
            mMenu.Widgets.Add(mButtonExit);

            mTitleImage = mContentManager.Load<Texture2D>("Menu/Titel");
            mBackgroundImage = mContentManager.Load<Texture2D>("Menu/mainscreen");
            mRandom = new Random();
            mImagePosition = RandomPosition();
            mImageScrollDirection = RandomScrollDirection();
            mButtonStart.BackgroundColor = Color.Red;
            mButtonLoad.BackgroundColor = Color.Red;
            mButtonOptions.BackgroundColor = Color.Red;
            mButtonStatistics.BackgroundColor = Color.Red;
            mButtonAchievements.BackgroundColor = Color.Red;
            mButtonTechDemo.BackgroundColor = Color.Red;
            mButtonExit.BackgroundColor = Color.Red;
        }

        public override bool HandleKeyEvent(IKeyEvent keyEvent)
        {
            return true;
        }

        public override bool HandleMouseEvent(IMouseEvent mouseEvent)
        {
            mMenu.HandleMouseEvent(mouseEvent);
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (mButtonStart.IsClicked)
                mMenuActions.StartNewGame();
            else if (mButtonLoad.IsClicked)
                mMenuActions.OpenLoadScreen();
            else if (mButtonOptions.IsClicked)
                mMenuActions.OpenOptionsScreen();
            else if (mButtonTechDemo.IsClicked)
                mMenuActions.OpenTechDemo();
            else if (mButtonStatistics.IsClicked)
                mMenuActions.OpenStatisticsScreen();
            else if (mButtonAchievements.IsClicked)
                mMenuActions.OpenAchievementsScreen();
            else if (mButtonExit.IsClicked)
                mMenuActions.Exit();

            mMenu.Update();
            UpdateBackgroundPosition();
        }

        /// <summary>
        /// Draws this object using the given sprite batch.
        /// </summary>
        /// <param name="gameTime">a snapshot of the game time</param>
        /// <param name="spriteBatch">the sprite batch to use for drawing
        /// this object</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mBackgroundImage, mImagePosition, Color.White);
            Point titleSize = new Point(ScreenBounds.Width * 9 / 10, ScreenBounds.Width * TitleImageBounds.Height * 9 / 10 / TitleImageBounds.Width);
            Point titlePosition = new Point(ScreenBounds.Width / 2 - titleSize.X / 2, ScreenBounds.Height / 20);
            spriteBatch.Draw(mTitleImage, 
                new Rectangle(titlePosition, titleSize), 
                Color.White);
            spriteBatch.End();

            mMenu.Draw(spriteBatch);
        }

        /// <summary>
        /// Calculate a random section of the background image to show.
        /// If the screen is larger than the image, just draw it to the upper left corner.
        /// </summary>
        /// <returns>A random Vector2 fitting the image to the screen.</returns>
        private Vector2 RandomPosition()
        {
            Vector2 position;
            try
            {
                var randomX = mRandom.Next(-BackgroundImageBounds.Width + ScreenBounds.Width, 0);
                var randomY = mRandom.Next(-BackgroundImageBounds.Height + ScreenBounds.Height, 0);
                position = new Vector2(randomX, randomY);
            }
            catch (ArgumentOutOfRangeException)
            {
                position = Vector2.Zero;
            }
            return position;
        }

        private Vector2 RandomScrollDirection()
        {
            var imageScrollDirection = new Vector2((float)mRandom.NextDouble(), (float)mRandom.NextDouble());
            imageScrollDirection.Normalize();
            switch (mRandom.Next(4))
            {
                case 0:
                    return imageScrollDirection;
                case 1:
                    return new Vector2(imageScrollDirection.X, -imageScrollDirection.Y);
                case 2:
                    return new Vector2(-imageScrollDirection.X, imageScrollDirection.Y);
                case 3:
                    return new Vector2(-imageScrollDirection.X, -imageScrollDirection.Y);
                default:
                    return imageScrollDirection;
            }
        }

        private void UpdateBackgroundPosition()
        {
            if (mImagePosition.X < 0 && mImagePosition.X > -BackgroundImageBounds.Width + ScreenBounds.Width
                 && mImagePosition.Y < 0 && mImagePosition.Y > -BackgroundImageBounds.Height + ScreenBounds.Height)
            {
                mImagePosition += mImageScrollDirection;
            }
            else
            {
                mImagePosition = RandomPosition();
                mImageScrollDirection = RandomScrollDirection();
            }
        }
    }
}
