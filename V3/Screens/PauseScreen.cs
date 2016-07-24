using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using V3.Input;
using V3.Widgets;

namespace V3.Screens
{
    /// <summary>
    /// The screen for the pause menu.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class PauseScreen : AbstractScreen, IInitializable
    {
        private readonly ContentManager mContentManager;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;
        private readonly IMenu mMenu;
        private readonly MenuActions mMenuActions;
        private readonly WidgetFactory mWidgetFactory;

        private Texture2D mRectangle;
        private Button mButtonBack;
        private Button mButtonSave;
        private Button mButtonLoad;
        private Button mButtonOptions;
        private Button mButtonStatistics;
        private Button mButtonAchievements;
        private Button mButtonMain;
        private Button mButtonExit;

        /// <summary>
        /// Creates a new pause screen.
        /// </summary>
        public PauseScreen(ContentManager contentManager,
                GraphicsDeviceManager graphicsDeviceManager, VerticalMenu menu,
                MenuActions menuActions, WidgetFactory widgetFactory)
            : base(false, true)
        {
            mContentManager = contentManager;
            mGraphicsDeviceManager = graphicsDeviceManager;
            mMenu = menu;
            mMenuActions = menuActions;
            mWidgetFactory = widgetFactory;
        }

        public void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");

            mButtonBack = mWidgetFactory.CreateButton("Zurück zum Spiel");
            mButtonSave = mWidgetFactory.CreateButton("Spiel speichern");
            mButtonLoad = mWidgetFactory.CreateButton("Spiel laden");
            mButtonLoad.IsEnabled = mMenuActions.CanLoadGame();
            mButtonStatistics = mWidgetFactory.CreateButton("Statistiken");
            mButtonAchievements = mWidgetFactory.CreateButton("Erfolge");
            mButtonOptions = mWidgetFactory.CreateButton("Optionen");
            mButtonMain = mWidgetFactory.CreateButton("Hauptmenü");
            mButtonExit = mWidgetFactory.CreateButton("Spiel schließen");

            mMenu.Widgets.Add(mButtonBack);
            mMenu.Widgets.Add(mButtonSave);
            mMenu.Widgets.Add(mButtonLoad);
            mMenu.Widgets.Add(mButtonOptions);
            mMenu.Widgets.Add(mButtonStatistics);
            mMenu.Widgets.Add(mButtonAchievements);
            mMenu.Widgets.Add(mButtonMain);
            mMenu.Widgets.Add(mButtonExit);
        }

        public override bool HandleKeyEvent(IKeyEvent keyEvent)
        {
            if (keyEvent.KeyState == KeyState.Down && keyEvent.Key == Keys.Escape)
                mMenuActions.Close(this);

            return true;
        }

        public override bool HandleMouseEvent(IMouseEvent mouseEvent)
        {
            mMenu.HandleMouseEvent(mouseEvent);
            return true;
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
            spriteBatch.Draw(mRectangle, mGraphicsDeviceManager.GraphicsDevice.Viewport.Bounds, Color.Black * 0.7f);
            spriteBatch.End();

            mMenu.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (mButtonBack.IsClicked)
            {
                mMenuActions.Close(this);
            }
            else if (mButtonSave.IsClicked)
            {
                mMenuActions.SaveGame();
                mMenuActions.Close(this);
            }
            else if (mButtonLoad.IsClicked)
            {
                mMenuActions.OpenLoadScreen();
            }
            else if (mButtonOptions.IsClicked)
            {
                mMenuActions.OpenOptionsScreen();
            }
            else if (mButtonStatistics.IsClicked)
            {
                mMenuActions.OpenStatisticsScreen();
            }
            else if (mButtonAchievements.IsClicked)
            {
                mMenuActions.OpenAchievementsScreen();
            }
            else if (mButtonMain.IsClicked)
            {
                mMenuActions.OpenMainScreen();
            }
            else if (mButtonExit.IsClicked)
            {
                mMenuActions.Exit();
            }

            mMenu.Update();
        }
    }
}
