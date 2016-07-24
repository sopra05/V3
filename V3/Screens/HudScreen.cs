using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using System.Collections.Generic;
using V3.Data;
using V3.Camera;
using V3.Input;
using V3.Map;
using V3.Objects;
using V3.Widgets;

namespace V3.Screens
{
    /// <summary>
    /// Creates a new HUD screen.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class HudScreen : AbstractScreen, IInitializable
    {
        private readonly MenuActions mMenuActions;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;
        private readonly ContentManager mContentManager;
        private readonly WidgetFactory mWidgetFactory;
        private readonly IOptionsManager mOptionsManager;
        private readonly CameraManager mCameraManager;
        private readonly IMapManager mMapManager;
        private readonly IObjectsManager mObjectsManager;
        private AchievementsAndStatistics mAchievementsAndStatistics;

        private KeyboardState mCurrentState;
        private KeyboardState mPreviousState;
        private SpriteFont mFont;

        private List<Button> mButtons = new List<Button>();
        private Button mPauseButton;
        private Button mCameraButton;
        private Button mButton1;
        private Button mButton2;
        private Button mButton3;
        private Button mButton4;
        private Button mButton5;

        private Rectangle mLifeBarRectangle;
        private Rectangle mBackgroundRectangle;
        private Rectangle mUnitRectangle;
        private Rectangle mUnitInformationRectangle;
        private Rectangle mMiniMapRectangle;
        private Rectangle mMiniMapCamera;

        private Texture2D mRectangle;

        private Point mRectanglePos;
        private Point mRectangleSize;
        private int mCurrentHealth;
        private int mMaxHealth;
        private int mCounter;

        private Vector2 mCreatureNamePosition;
        private readonly Selection mSelection;
        private int mScaleFactorX;
        private int mScaleFactorY;

        public HudScreen(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager,
                IOptionsManager optionsManager, MenuActions menuActions, 
                WidgetFactory widgetFactory, Selection selection, CameraManager cameraManager,
                IMapManager mapManager, IObjectsManager objectsManager, 
                AchievementsAndStatistics achievementsAndStatistics): base(true, true)
        {
            mGraphicsDeviceManager = graphicsDeviceManager;
            mContentManager = contentManager;
            mOptionsManager = optionsManager;
            mMenuActions = menuActions;
            mWidgetFactory = widgetFactory;
            mSelection = selection;
            mCameraManager = cameraManager;
            mMapManager = mapManager;
            mObjectsManager = objectsManager;
            mAchievementsAndStatistics = achievementsAndStatistics;
        }

        public void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");
            mFont = mContentManager.Load<SpriteFont>("Fonts/UnitFont");

            mPauseButton = CreateButton("Buttons/Button-06",
                "Pause",
                "Pausiert das Spiel und \n" +
                "öffnet das Pausemenü.");
            mCameraButton = CreateButton("Buttons/Button-07",
                "Kamera",
                "Ändert die Kameraeinstellung zu \n" +
                "zentrierter oder schiebender Kamera. ");
            mButton1 = CreateButton("Buttons/Button-01",
                "Explosion",
                "Lasst den Fleischklops explodieren \n" +
                "und fügt im Umkreis Schaden zu.");
            mButton2 = CreateButton("Buttons/Button-02",
                "Zombie beschwören",
                "Wiederbelebt alle toten Gegner im Umkreis \n" +
                "und lasst sie für euch kämpfen.");
            mButton3 = CreateButton("Buttons/Button-03",
                "Zombies verwandeln",
                "Verschmelzt 5 ausgewählte Zombies \n" +
                "zu einem Fleischklops.");
            mButton4 = CreateButton("Buttons/Button-04",
                "Skelett erschaffen",
                "Verwandelt einen Zombie zu einem \n" +
                "Skelett.");
            mButton5 = CreateButton("Buttons/Button-05",
                "Totenpferd beschwören",
                "Vereinigt 3 Skelette zu einem \n" +
                "Totenpferd.");

            if (mObjectsManager.PlayerCharacter != null)
            {
                mCurrentHealth = mObjectsManager.PlayerCharacter.Life;
                mMaxHealth = mObjectsManager.PlayerCharacter.MaxLife;
            }
        }

        public override bool HandleMouseEvent(IMouseEvent mouseEvent)
        {
            mButtons.ForEach(b => b.HandleMouseEvent(mouseEvent));
            if (mMiniMapRectangle.Contains(mouseEvent.PositionPressed))
            {
                return true;
            }
            foreach (var button in mButtons)
            {
                if (button.Rectangle.Contains(mouseEvent.PositionPressed))
                    return true;
                if (mouseEvent.PositionReleased.HasValue && button.Rectangle.Contains(mouseEvent.PositionReleased.Value))
                    return true;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            var viewport = mGraphicsDeviceManager.GraphicsDevice.Viewport;
            mRectanglePos = new Point(viewport.Width / 2 - viewport.Width / 8, viewport.Height - viewport.Height / 20 - 2);
            mRectangleSize = new Point(viewport.Width / 4, viewport.Height / 20);
            mCurrentHealth = mObjectsManager.PlayerCharacter.Life;

            mLifeBarRectangle = new Rectangle(mRectanglePos.X, mRectanglePos.Y, mRectangleSize.X * mCurrentHealth / mMaxHealth, mRectangleSize.Y);
            mBackgroundRectangle = new Rectangle(mRectanglePos - new Point(2, 2), mRectangleSize + new Point(4, 4));
            mUnitInformationRectangle = new Rectangle(viewport.Width * 2/3, viewport.Height * 7/8, viewport.Width / 3, viewport.Height / 8);
            mUnitRectangle = new Rectangle(mUnitInformationRectangle.X + mUnitInformationRectangle.Width * 2 / 3, mUnitInformationRectangle.Y + 5, mUnitInformationRectangle.Width / 3 - 15, mUnitInformationRectangle.Height - 10);
            mCreatureNamePosition = new Vector2(mUnitInformationRectangle.X + mUnitInformationRectangle.Width / 20, mUnitInformationRectangle.Y + mUnitInformationRectangle.Height / 20);

            // Logic for camera toggle
            mPreviousState = mCurrentState;
            mCurrentState = Keyboard.GetState();

            if (mCameraButton.IsClicked || (mCurrentState.IsKeyDown(Keys.C) && mPreviousState.IsKeyUp(Keys.C)))
            {
                if (mOptionsManager.Options.CameraType == CameraType.Centered)
                {
                    mOptionsManager.Options.CameraType = CameraType.Scrolling;
                }
                else if (mOptionsManager.Options.CameraType == CameraType.Scrolling)
                {
                    mOptionsManager.Options.CameraType = CameraType.Centered;
                }
                mMenuActions.SaveOptions();
                mMenuActions.ApplyOptions();
            }

            // Logic for opening the death screen if the player's life reaches 0 or the victory screen if the boss is defeated.
            if (mObjectsManager.PlayerCharacter.IsDead)
            {
                mCounter++;
                if (mCounter == 120)
                {
                    mMenuActions.OpenDeathScreen();
                    mCounter = 0;
                }
            }

            else if (mObjectsManager.Boss != null && mObjectsManager.Boss.IsDead)
            {
                mCounter++;
                if (mCounter == 120)
                {
                    mAchievementsAndStatistics.mUsedTime -= TimeSpan.FromSeconds(2);
                    if (gameTime.TotalGameTime <= TimeSpan.FromMinutes(5))
                        mAchievementsAndStatistics.mHellsNotWaiting = true;
                    mMenuActions.OpenVictoryScreen();
                    mCounter = 0;
                }
        }

            // Define the Minimap into the upper right corner and the scale factors needed.

            mMiniMapRectangle = new Rectangle(viewport.Width * 3 / 4, 0, viewport.Width * 1 / 4, viewport.Height * 1 / 4);
            mScaleFactorX = mCameraManager.GetCamera().MapPixelWidth / mMiniMapRectangle.Width;
            mScaleFactorY = mCameraManager.GetCamera().MapPixelHeight / 2 / mMiniMapRectangle.Height;
            mMiniMapCamera = new Rectangle(mMiniMapRectangle.X + (int)mCameraManager.GetCamera().Location.X / mScaleFactorX,
                                           mMiniMapRectangle.Y + (int)mCameraManager.GetCamera().Location.Y / mScaleFactorY,
                                           viewport.Width / mScaleFactorX, viewport.Height / mScaleFactorY);

            if (mMiniMapRectangle.Contains(Mouse.GetState().Position))
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    float newCameraLocationX = MathHelper.Clamp(((float) Mouse.GetState().Position.X - mMiniMapRectangle.X) * mScaleFactorX -
                                               mMiniMapCamera.Width / 2f * mScaleFactorX, 0, mCameraManager.GetCamera().MapPixelWidth - viewport.Width);
                    float newCameraLocationY = MathHelper.Clamp(((float) Mouse.GetState().Position.Y - mMiniMapRectangle.Y) * mScaleFactorY -
                                               mMiniMapCamera.Height / 2f * mScaleFactorY, 0, mCameraManager.GetCamera().MapPixelHeight / 2f - viewport.Height);
                    mCameraManager.GetCamera().Location = new Vector2(newCameraLocationX, newCameraLocationY);
                }
            }

            // Call for transformations
            if (mButton1.IsClicked)
                mSelection.Specialattack();
            if (mButton2.IsClicked)
                mSelection.TransformZombie();
            if (mButton3.IsClicked)
                mSelection.TransformMeatball();
            if (mButton4.IsClicked)
                mSelection.TransformSkeleton();
            if (mButton5.IsClicked)
                mSelection.TransformSkeletonHorse();

            UpdateButtons();

            foreach (var unit in mSelection.SelectedCreatures)
            {
                if (unit is Meatball)
                    mAchievementsAndStatistics.mMeatballCompany += 1;
                else if (unit is SkeletonHorse)
                    mAchievementsAndStatistics.mSkeletonHorseCavalry += 1;
            }

            mAchievementsAndStatistics.mWalkedDistance += ((Necromancer)mObjectsManager.PlayerCharacter).WalkedPixels / 64;
            mAchievementsAndStatistics.mMarathonRunner += ((Necromancer)mObjectsManager.PlayerCharacter).WalkedPixels / 64;
            mAchievementsAndStatistics.mIronMan += ((Necromancer)mObjectsManager.PlayerCharacter).WalkedPixels / 64;
            mAchievementsAndStatistics.mUsedTime += gameTime.ElapsedGameTime;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mButtons.ForEach(b => b.Draw(spriteBatch));
            spriteBatch.Draw(mRectangle, mBackgroundRectangle, Color.Black * 0.7f);
            spriteBatch.Draw(mRectangle, mLifeBarRectangle, Color.Firebrick);
            spriteBatch.Draw(mRectangle, mUnitInformationRectangle, Color.Chocolate * 0.7f);
            spriteBatch.Draw(mRectangle, mUnitRectangle, Color.Gainsboro * 0.6f);
            
            // Draw the minimap.
            mMapManager.DrawMinimap(spriteBatch, mMiniMapRectangle);

            // Draw a rectangle on the part of the MiniMap that is shown by the camera.
            // Upper line
            spriteBatch.Draw(mRectangle, new Rectangle(mMiniMapCamera.X, mMiniMapCamera.Y, mMiniMapCamera.Width, 1), Color.White);
            // Left line
            spriteBatch.Draw(mRectangle, new Rectangle(mMiniMapCamera.X, mMiniMapCamera.Y, 1, mMiniMapCamera.Height), Color.White);
            // Right line
            spriteBatch.Draw(mRectangle, new Rectangle(mMiniMapCamera.X + mMiniMapCamera.Width, mMiniMapCamera.Y, 1, mMiniMapCamera.Height), Color.White);
            // Bottom line
            spriteBatch.Draw(mRectangle, new Rectangle(mMiniMapCamera.X, mMiniMapCamera.Y + mMiniMapCamera.Height, mMiniMapCamera.Width, 1), Color.White);

            foreach (var creature in mSelection.SelectedCreatures)
            {
                if (creature.IsSelected && mSelection.SelectedCreatures.Count == 1)
                {
                    spriteBatch.DrawString(mFont, creature.Name, mCreatureNamePosition, Color.Black);
                    spriteBatch.DrawString(mFont,
                        "Leben: " + creature.Life,
                        mCreatureNamePosition + new Vector2(0, 20),
                        Color.Black);
                    spriteBatch.DrawString(mFont,
                        "Kraft: " + creature.Attack,
                        mCreatureNamePosition + new Vector2(0, 40),
                        Color.Black);
                    creature.DrawStatic(spriteBatch,
                        new Point(mUnitRectangle.X + mUnitRectangle.Width / 2, mUnitRectangle.Y + mUnitRectangle.Height * 3/4));
                }
                else if (mSelection.SelectedCreatures.Count > 1)
                {
                    var showedCreature = mSelection.SelectedCreatures[0];
                    spriteBatch.DrawString(mFont, showedCreature.Name, mCreatureNamePosition, Color.Black);
                    spriteBatch.DrawString(mFont,
                        "Leben: " + showedCreature.Life,
                        mCreatureNamePosition + new Vector2(0, 20),
                        Color.Black);
                    spriteBatch.DrawString(mFont,
                        "Kraft: " + showedCreature.Attack,
                        mCreatureNamePosition + new Vector2(0, 40),
                        Color.Black);
                    showedCreature.DrawStatic(spriteBatch,
                        new Point(mUnitRectangle.X + mUnitRectangle.Width / 2, mUnitRectangle.Y + mUnitRectangle.Height * 3 / 4));
                }
            }
            spriteBatch.End();
        }

        private void UpdateButtons()
        {
            var viewport = mGraphicsDeviceManager.GraphicsDevice.Viewport;
            // Button Size
            var buttonLength = viewport.Height / 12;
            var buttonSize = new Vector2(buttonLength, buttonLength);
            mButtons.ForEach(b => b.Size = buttonSize);

            // Rectangles for the buttons
            var buttonXVector = new Vector2(buttonLength + 2, 0);
            mPauseButton.Position = new Vector2(5, 5);
            mCameraButton.Position = new Vector2(mPauseButton.Position.X + mPauseButton.Size.X + 5, 5);
            mButton1.Position = new Vector2(2, viewport.Height - buttonLength);
            mButton2.Position = mButton1.Position + buttonXVector;
            mButton3.Position = mButton2.Position + buttonXVector;
            mButton4.Position = mButton3.Position + buttonXVector;
            mButton5.Position = mButton4.Position + buttonXVector;

            if (mPauseButton.IsClicked)
                mMenuActions.OpenPauseScreen();

            var mousePosition = Mouse.GetState().Position;
            mButtons.ForEach(b => b.IsSelected = b.CheckSelected(mousePosition));
            mButtons.ForEach(b => b.IsClicked = false);
        }

        private Button CreateButton(string textureName, string tooltipTitle, string tooltip)
        {
            return CreateButton(mContentManager.Load<Texture2D>(textureName), tooltipTitle, tooltip);
        }

        private Button CreateButton(Texture2D texture, string tooltipTitle, string tooltip)
        {
            var button = mWidgetFactory.CreateButton("");
            button.Position = Vector2.Zero;
            button.PaddingX = 15;
            button.PaddingY = 5;
            button.Size = button.GetMinimumSize();
            button.Image = texture;
            button.Tooltip = tooltip;
            button.TooltipTitle = tooltipTitle;
            mButtons.Add(button);
            return button;
        }
    }
}
