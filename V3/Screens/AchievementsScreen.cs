using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using V3.Input;
using V3.Objects;
using V3.Widgets;

namespace V3.Screens
{
    /// <summary>
    /// The screen for the AchievementsAndStatistics.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class AchievementsScreen : AbstractScreen, IInitializable
    {
        //private AchievementBox mFirstSteps;
        private AchievementBox mKillPrince;
        private AchievementBox mKillKing;
        //private AchievementBox mWarmasterAndWizard;
        private AchievementBox mKaboom;
        private AchievementBox mMarathonRunner;
        private AchievementBox mIronMan;
        private AchievementBox mMeatballCompany;
        private AchievementBox mSkeletonHorseCavalry;
        private AchievementBox mRightHandOfDeath;
        private AchievementBox mMinimalist;
        private AchievementBox mHundredDeadCorpses;
        private AchievementBox mUndeadArmy;
        private AchievementBox mHellsNotWaiting;


        private readonly ContentManager mContentManager;
        private readonly IMenuFactory mMenuFactory;
        private readonly MenuActions mMenuActions;
        private readonly WidgetFactory mWidgetFactory;
        private readonly AchievementsAndStatistics mAchievementsAndStatistics;
        private readonly ObjectsManager mObjectsManager;

        private Button mButtonBack;
        private SelectButton mSelectPage;

        private List<IMenu> mMenuList = new List<IMenu>();
        private Texture2D mRectangle;

        public AchievementsScreen(ContentManager contentManager, MenuActions menuActions, WidgetFactory widgetFactory,
               IMenuFactory menuFactory, AchievementsAndStatistics achievementsAndStatistics, ObjectsManager objectsManager)
            : base(false, true)
        {
            mContentManager = contentManager;
            mMenuFactory = menuFactory;
            mMenuActions = menuActions;
            mWidgetFactory = widgetFactory;
            mAchievementsAndStatistics = achievementsAndStatistics;
            mObjectsManager = objectsManager;
        }

        public void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");

            mButtonBack = mWidgetFactory.CreateButton("Zurück");
            mSelectPage = mWidgetFactory.CreateSelectButton();

            var menu = mMenuFactory.CreateVerticalMenu();
            mMenuList.Add(menu);

            menu.Widgets.Add(mSelectPage);

            mKillPrince = mWidgetFactory.CreateAchievementBox();
            mKillPrince.SetText("Erbfolge aufgehalten", "Vernichtet Prinz Erhard.");
            menu.Widgets.Add(mKillPrince);

            mKaboom = mWidgetFactory.CreateAchievementBox();
            mKaboom.SetText("KABUMM!!!", "Tötet mindestens 10 Gegner mit einer einzigen Fleischklops-Explosion.");
            menu.Widgets.Add(mKaboom);

            mKillKing = mWidgetFactory.CreateAchievementBox();
            mKillKing.SetText("Königsmord", "Nehmt eure Vergeltung am König und tötet ihn.");
            menu.Widgets.Add(mKillKing);

            menu.Widgets.Add(mButtonBack);

            menu = mMenuFactory.CreateVerticalMenu();
            mMenuList.Add(menu);

            menu.Widgets.Add(mSelectPage);

            mHundredDeadCorpses = mWidgetFactory.CreateAchievementBox();
            mHundredDeadCorpses.SetText("Leichenfledderer", "Tötet in einer Mission mindestens 100 Gegner.");
            menu.Widgets.Add(mHundredDeadCorpses);

            mUndeadArmy = mWidgetFactory.CreateAchievementBox();
            mUndeadArmy.SetText("Untote Armee", "Tötet in einer Mission mindestens 1000 Gegner.");
            menu.Widgets.Add(mUndeadArmy);

            mRightHandOfDeath = mWidgetFactory.CreateAchievementBox();
            mRightHandOfDeath.SetText("Die erbarmungslose rechte Hand des Todes", "Tötet insgesamt 10 000 Gegner.");
            menu.Widgets.Add(mRightHandOfDeath);

            menu.Widgets.Add(mButtonBack);

            menu = mMenuFactory.CreateVerticalMenu();
            mMenuList.Add(menu);

            menu.Widgets.Add(mSelectPage);

            mMeatballCompany = mWidgetFactory.CreateAchievementBox();
            mMeatballCompany.SetText("Fleischpanzer-Kompanie", "Erschafft und befehligt in einer Mission 10 Fleischklopse gleichzeitig.");
            menu.Widgets.Add(mMeatballCompany);

            mSkeletonHorseCavalry = mWidgetFactory.CreateAchievementBox();
            mSkeletonHorseCavalry.SetText("Klappernde Kavallerie", "Erschafft und befehligt in einer Mission 25 Skelettpferde gleichzeitig.");
            menu.Widgets.Add(mSkeletonHorseCavalry);

            mMinimalist = mWidgetFactory.CreateAchievementBox();
            mMinimalist.SetText("Minimalist", "Beendet eine Mission und setzt dabei weniger als 100 Einheiten ein.");
            menu.Widgets.Add(mMinimalist);

            menu.Widgets.Add(mButtonBack);

            menu = mMenuFactory.CreateVerticalMenu();
            mMenuList.Add(menu);

            menu.Widgets.Add(mSelectPage);

            mHellsNotWaiting = mWidgetFactory.CreateAchievementBox();
            mHellsNotWaiting.SetText("Die Hölle wartet nicht", "Beendet eine Mission in weniger als 5 Minuten.");
            menu.Widgets.Add(mHellsNotWaiting);

            mMarathonRunner = mWidgetFactory.CreateAchievementBox();
            mMarathonRunner.SetText("Marathonläufer", "Legt in einer Mission mindestens 1000m zurück.");
            menu.Widgets.Add(mMarathonRunner);

            mIronMan = mWidgetFactory.CreateAchievementBox();
            mIronMan.SetText("Der Iron Man", "Legt insgesamt eine Strecke von 10 000m zurück.");
            menu.Widgets.Add(mIronMan);

            menu.Widgets.Add(mButtonBack);

            for (var i = 0; i < mMenuList.Count; i++)
                mSelectPage.Values.Add($"Seite {i + 1}");
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

            // achievement datas. if one value becomes a given value, the corresponding achievement will be enabled.

            if (mAchievementsAndStatistics.mKillPrince)
                mKillPrince.IsEnabled = true;
            if (mAchievementsAndStatistics.mKillKing)
                mKillKing.IsEnabled = true;
            if (mAchievementsAndStatistics.mHellsNotWaiting)
                mHellsNotWaiting.IsEnabled = true;
            if (mAchievementsAndStatistics.mKaboom)
                mKaboom.IsEnabled = true;

            if (mAchievementsAndStatistics.mMarathonRunner >= 1000)
                mMarathonRunner.IsEnabled = true;
            if (mAchievementsAndStatistics.mIronMan >= 10000)
                mIronMan.IsEnabled = true;
            if (mAchievementsAndStatistics.mMeatballCompany >= 10)
                mMeatballCompany.IsEnabled = true;
            if (mAchievementsAndStatistics.mSkeletonHorseCavalry >= 25)
                mSkeletonHorseCavalry.IsEnabled = true;
            if (mAchievementsAndStatistics.mRightHandOfDeath >= 10000)
                mRightHandOfDeath.IsEnabled = true;
            if (mAchievementsAndStatistics.mMinimalist <= 100 && mObjectsManager.Boss != null && mObjectsManager.Boss.IsDead)
                mMinimalist.IsEnabled = true;
            if (mAchievementsAndStatistics.mHundredDeadCorpses >= 100)
                mHundredDeadCorpses.IsEnabled = true;
            if (mAchievementsAndStatistics.mUndeadArmy >= 1000)
                mUndeadArmy.IsEnabled = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var menu = GetCurrentMenu();
            var backgroundRectangle = new Rectangle((int)menu.Position.X,
                    (int)menu.Position.Y, (int)menu.Size.X, 
                    (int)menu.Size.Y);
            backgroundRectangle.X -= 30;
            backgroundRectangle.Y -= 30;
            backgroundRectangle.Width += 60;
            backgroundRectangle.Height += 60;

            spriteBatch.Begin();
            spriteBatch.Draw(mRectangle, backgroundRectangle, Color.WhiteSmoke);
            spriteBatch.End();

            menu.Draw(spriteBatch);
        }

        private IMenu GetCurrentMenu()
        {
            return mMenuList[mSelectPage.SelectedIndex];
        }
    }
}
