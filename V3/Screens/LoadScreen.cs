using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using System.Collections.Generic;
using V3.Data;
using V3.Input;
using V3.Widgets;

namespace V3.Screens
{
    /// <summary>
    /// The screen for the load menu.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class LoadScreen : AbstractScreen, IInitializable
    {
        private readonly ContentManager mContentManager;
        private readonly IMenu mMenu;
        private readonly ISaveGameManager mSaveGameManager;
        private readonly MenuActions mMenuActions;
        private readonly WidgetFactory mWidgetFactory;

        private Texture2D mRectangle;
        private Button mButtonBack;
        private SelectButton mButtonSaveGame;
        private Button mButtonLoad;
        private List<ISaveGame> mSaveGames;

        /// <summary>
        /// Creates a new load screen.
        /// </summary>
        public LoadScreen(ContentManager contentManager, FormMenu menu,
                ISaveGameManager saveGameManager, MenuActions menuActions,
                WidgetFactory widgetFactory)
            : base(false, true)
        {
            mContentManager = contentManager;
            mMenu = menu;
            mSaveGameManager = saveGameManager;
            mMenuActions = menuActions;
            mWidgetFactory = widgetFactory;
        }

        public void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");

            mButtonBack = mWidgetFactory.CreateButton("Zurück");
            mButtonLoad = mWidgetFactory.CreateButton("Laden");
            mButtonSaveGame = mWidgetFactory.CreateSelectButton();
            mSaveGames = mSaveGameManager.GetSaveGames();
            foreach (var saveGame in mSaveGames)
            {
                mButtonSaveGame.Values.Add(GetSaveGameString(saveGame));
            }
            mButtonSaveGame.SelectedIndex = mSaveGames.Count - 1;

            mMenu.Widgets.Add(mWidgetFactory.CreateEmptyWidget());
            mMenu.Widgets.Add(mButtonBack);
            mMenu.Widgets.Add(mWidgetFactory.CreateLabel("Spielstand"));
            mMenu.Widgets.Add(mButtonSaveGame);
            mMenu.Widgets.Add(mWidgetFactory.CreateEmptyWidget());
            mMenu.Widgets.Add(mButtonLoad);
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
            var backgroundRectangle = new Rectangle((int)mMenu.Position.X,
                    (int)mMenu.Position.Y, (int)mMenu.Size.X, (int)mMenu.Size.Y);
            backgroundRectangle.X -= 30;
            backgroundRectangle.Y -= 30;
            backgroundRectangle.Width += 60;
            backgroundRectangle.Height += 60;

            spriteBatch.Begin();
            spriteBatch.Draw(mRectangle, backgroundRectangle, Color.LightGray);
            spriteBatch.End();

            mMenu.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (mButtonBack.IsClicked)
            {
                mMenuActions.Close(this);
            }
            else if (mButtonLoad.IsClicked)
            {
                var saveGame = mSaveGames[mButtonSaveGame.SelectedIndex];
                mMenuActions.LoadGame(saveGame);
            }

            mMenu.Update();
        }

        private static string GetSaveGameString(ISaveGame saveGame)
        {
            return saveGame.Timestamp.ToString("s");
        }
    }
}
