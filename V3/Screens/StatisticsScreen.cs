using System.Collections.Generic;
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
    /// The screen for the statistics.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class StatisticsScreen : AbstractScreen, IInitializable
    {
        private readonly ContentManager mContentManager;
        private readonly IMenuFactory mMenuFactory;
        private readonly MenuActions mMenuActions;
        private readonly WidgetFactory mWidgetFactory;
        private readonly AchievementsAndStatistics mAchievementsAndStatistics;

        private Button mButtonBack;
        private SelectButton mSelectMission;

        private List<IMenu> mMenuList = new List<IMenu>();
        private Texture2D mRectangle;

        public StatisticsScreen(ContentManager contentManager, MenuActions menuActions, WidgetFactory widgetFactory,
               IMenuFactory menuFactory, AchievementsAndStatistics achievementsAndStatistics)
            : base(false, true)
        {
            mContentManager = contentManager;
            mMenuActions = menuActions;
            mWidgetFactory = widgetFactory;
            mMenuFactory = menuFactory;
            mAchievementsAndStatistics = achievementsAndStatistics;
        }

        public void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");

            mButtonBack = mWidgetFactory.CreateButton("Zurück");
            mSelectMission = mWidgetFactory.CreateSelectButton();

            var menu = mMenuFactory.CreateFormMenu();
            mMenuList.Add(menu);

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Auswahl"));
            menu.Widgets.Add(mSelectMission);

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Zeit"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("               " + string.Format("{0:T}",mAchievementsAndStatistics.mUsedTime)));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Getötete Gegner"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + string.Format("{0:00000}", mAchievementsAndStatistics.mKilledCreatures)));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Verlorene Einheiten"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + string.Format("{0:00000}", mAchievementsAndStatistics.mLostServants)));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("zurückgelegte Entfernung"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + string.Format("{0:00000}", mAchievementsAndStatistics.mWalkedDistance) + " m"));

            menu.Widgets.Add(mWidgetFactory.CreateEmptyWidget());
            menu.Widgets.Add(mButtonBack);

            menu = mMenuFactory.CreateFormMenu();

            mMenuList.Add(menu);

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Auswahl"));
            menu.Widgets.Add(mSelectMission);

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Zeit"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("               " + "00:00:00"));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Getötete Gegner"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + "000000"));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Verlorene Einheiten"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + "000000"));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("zurückgelegte Entfernung"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + "000000"));

            menu.Widgets.Add(mWidgetFactory.CreateEmptyWidget());
            menu.Widgets.Add(mButtonBack);

            menu = mMenuFactory.CreateFormMenu();

            mMenuList.Add(menu);

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Auswahl"));
            menu.Widgets.Add(mSelectMission);

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Zeit"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("               " + "00:00:00"));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Getötete Gegner"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + "000000"));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Verlorene Einheiten"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + "000000"));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("zurückgelegte Entfernung"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + "000000"));

            menu.Widgets.Add(mWidgetFactory.CreateEmptyWidget());
            menu.Widgets.Add(mButtonBack);

            for (var i = 0; i < mMenuList.Count; i++)
                mSelectMission.Values.Add($"Mission {i + 1}");

            menu = mMenuFactory.CreateFormMenu();

            mMenuList.Add(menu);

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Auswahl"));
            menu.Widgets.Add(mSelectMission);

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Zeit"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("               " + "00:00:00"));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Getötete Gegner"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + "000000"));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("Verlorene Einheiten"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + "000000"));

            menu.Widgets.Add(mWidgetFactory.CreateLabel("zurückgelegte Entfernung"));
            menu.Widgets.Add(mWidgetFactory.CreateLabel("                " + "000000"));

            menu.Widgets.Add(mWidgetFactory.CreateEmptyWidget());
            menu.Widgets.Add(mButtonBack);

            mSelectMission.Values.Add("Gesamt");
        }

        public override bool HandleKeyEvent(IKeyEvent keyEvent)
        {
            if (keyEvent.KeyState == KeyState.Down && keyEvent.Key == Keys.Escape)
                mMenuActions.Close(this);

            return true;
        }

        public override bool HandleMouseEvent(IMouseEvent mouseEvent)
        {
            GetCurrentMenu().HandleMouseEvent(mouseEvent);
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (mButtonBack.IsClicked)
            {
                mMenuActions.Close(this);
            }
            GetCurrentMenu().Update();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var menu = GetCurrentMenu();
            var backgroundRectangle = new Rectangle((int)menu.Position.X,
                    (int)menu.Position.Y, (int)menu.Size.X, (int)menu.Size.Y);
            backgroundRectangle.X -= 30;
            backgroundRectangle.Y -= 30;
            backgroundRectangle.Width += 60;
            backgroundRectangle.Height += 60;

            spriteBatch.Begin();
            spriteBatch.Draw(mRectangle, backgroundRectangle, Color.LightGray);
            spriteBatch.End();

            GetCurrentMenu().Draw(spriteBatch);
        }

        private IMenu GetCurrentMenu()
        {
            return mMenuList[mSelectMission.SelectedIndex];
        }
    }
}
