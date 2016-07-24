using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using System.Collections.Generic;
using V3.AI;
using V3.Camera;
using V3.Data;
using V3.Effects;
using V3.Input;
using V3.Map;
using V3.Objects;

namespace V3.Screens
{
    /// <summary>
    /// The main game screen.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class GameScreen : AbstractScreen, IInitializable
    {
        private readonly CameraManager mCameraManager;
        private readonly ContentManager mContentManager;
        private readonly CreatureFactory mCreatureFactory;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;
        private readonly MenuActions mMenuActions;
        private readonly IOptionsManager mOptionsManager;
        private readonly Selection mSelection;
        private readonly Transformation mTransformation;
        private readonly IAiPlayer mAiPlayer;
        private readonly IMapManager mMapManager;
        private readonly IEffectsManager mEffectsManager;
        private readonly IObjectsManager mObjectsManager;
        private readonly Pathfinder mPathfinder;
        private readonly Texture2D mOnePixelTexture;
        private readonly FogOfWar mFog;
        private bool mFogOfWarActivaded = true;
        private AchievementsAndStatistics mAchievementsAndStatistics;
        private int mFogCounter;
        // Fields for handling mouse input.
        private Point mInitialClickPosition;
        private bool mLeftButtonPressed;
        private bool mRightButtonPressed;
        private Vector2 mRightButtonPosition;

        /// <summary>
        /// Creates a new game screen.
        /// </summary>
        public GameScreen(IOptionsManager optionsManager, CameraManager cameraManager,
                ContentManager contentManager, CreatureFactory creatureFactory,
                GraphicsDeviceManager graphicsDeviceManager, IMapManager mapManager,
                MenuActions menuActions, IAiPlayer aiPlayer, IObjectsManager objectsManager,
                Pathfinder pathfinder, Selection selection, FogOfWar fog, Transformation transformation,
                IEffectsManager effectsManager, AchievementsAndStatistics achievementsAndStatistics) : base(false, false)
        {
            mMapManager = mapManager;
            mObjectsManager = objectsManager;
            mCameraManager = cameraManager;
            mOptionsManager = optionsManager;
            mContentManager = contentManager;
            mCreatureFactory = creatureFactory;
            mEffectsManager = effectsManager;
            mTransformation = transformation;
            mGraphicsDeviceManager = graphicsDeviceManager;
            mMenuActions = menuActions;
            mAiPlayer = aiPlayer;
            mPathfinder = pathfinder;
            mSelection = selection;
            mFog = fog;
            mOnePixelTexture = contentManager.Load<Texture2D>("Sprites/WhiteRectangle");
            mAchievementsAndStatistics = achievementsAndStatistics;
        }


        public override bool HandleMouseEvent(IMouseEvent mouseEvent)
        {
            if (mouseEvent.MouseButton == MouseButton.Left && mouseEvent.ButtonState == ButtonState.Pressed)
            {
                if (!mLeftButtonPressed)
                {
                    mLeftButtonPressed = true;
                    mInitialClickPosition = mouseEvent.PositionPressed;
                }
            }
            else
            {
                if (mLeftButtonPressed)
                {
                    mSelection.Select(mInitialClickPosition + mCameraManager.GetCamera().Location.ToPoint(),
                        mouseEvent.PositionReleased.GetValueOrDefault() + mCameraManager.GetCamera().Location.ToPoint());
                    mLeftButtonPressed = false;
                }
            }
            if (mouseEvent.MouseButton == MouseButton.Right && mouseEvent.ButtonState == ButtonState.Pressed)
            {
                if (!mRightButtonPressed)
                {
                    mRightButtonPressed = true;
                    mInitialClickPosition = mouseEvent.PositionPressed;
                }
            }
            if (mouseEvent.MouseButton == MouseButton.Right && mouseEvent.ButtonState == ButtonState.Released)
            {
                if (mouseEvent.PositionReleased != null && mouseEvent.ReleasedOnScreen)
                {
                    mRightButtonPressed = false;
                    mRightButtonPosition = mouseEvent.PositionReleased.Value.ToVector2() + mCameraManager.GetCamera().Location;
                    mSelection.Move(mouseEvent.PositionReleased.Value.ToVector2()
                         + mCameraManager.GetCamera().Location);
                }
            }
            return true;
        }

        public void Initialize()
        {
#if NEWMAP
            mMapManager.Load("work_in_progress");
#else
            mMapManager.Load("map_grassland");
#endif
            mObjectsManager.Initialize(mMapManager);
            mCameraManager.Initialize(mMapManager.SizeInPixel);
            mPathfinder.LoadGrid(mMapManager.GetPathfindingGrid());
            mFog.LoadContent(mContentManager);
            mFog.LoadGrid(mMapManager.SizeInPixel);
            mTransformation.mSelection = mSelection;
            mTransformation.LoadArea(mContentManager);
            mTransformation.ObjectsManager = mObjectsManager;
        }

        /// <summary>
        /// Create the initial population as designated by the map manager,
        /// the player character, and possibly other creatures.
        /// </summary>
        public void CreateInitialPopulation()
        {
            // Create initial population.
            foreach (var creature in mMapManager.GetPopulation(mCreatureFactory, mPathfinder))
            {
                mObjectsManager.CreateCreature(creature);
            }

#if NEWMAP
            var necromancer = mCreatureFactory.CreateNecromancer(new Vector2(385, 420), MovementDirection.S);
            mObjectsManager.CreatePlayerCharacter(necromancer);
            // Create Prince and his guard.
            mObjectsManager.CreatePrince(mCreatureFactory.CreatePrince(new Vector2(6139, 3039), MovementDirection.SO));
            mObjectsManager.CreateCreature(mCreatureFactory.CreateKingsGuard(new Vector2(5794, 2900), MovementDirection.N));
            mObjectsManager.CreateCreature(mCreatureFactory.CreateKingsGuard(new Vector2(5858, 2900), MovementDirection.N));
            mObjectsManager.CreateCreature(mCreatureFactory.CreateKingsGuard(new Vector2(5936, 2885), MovementDirection.NW));
            mObjectsManager.CreateCreature(mCreatureFactory.CreateKingsGuard(new Vector2(6000, 2885), MovementDirection.NW));
            mObjectsManager.CreateCreature(mCreatureFactory.CreateKingsGuard(new Vector2(6064, 2885), MovementDirection.NW));
#else
            // Add creatures for testing purposes.
            var necromancer = mCreatureFactory.CreateNecromancer(new Vector2(300, 150), MovementDirection.S);
            var zombie1 = mCreatureFactory.CreateZombie(new Vector2(800, 200), MovementDirection.S);
            var zombie2 = mCreatureFactory.CreateZombie(new Vector2(850, 250), MovementDirection.S);
            var zombie3 = mCreatureFactory.CreateZombie(new Vector2(900, 200), MovementDirection.S);
            var zombie4 = mCreatureFactory.CreateZombie(new Vector2(950, 250), MovementDirection.S);
            var zombie5 = mCreatureFactory.CreateZombie(new Vector2(1000, 200), MovementDirection.S);
            var zombieToGo = mCreatureFactory.CreateZombie(new Vector2(400, 400), MovementDirection.S);
            var knight = mCreatureFactory.CreateKnight(new Vector2(1500, 210), MovementDirection.W);
            var horse = mCreatureFactory.CreateSkeletonHorse(new Vector2(1300, 500), MovementDirection.SW);
            var prince = mCreatureFactory.CreatePrince(new Vector2(1800, 1000), MovementDirection.S);
            var meatball = mCreatureFactory.CreateMeatball(new Vector2(350, 150), MovementDirection.S);
            // Add creatures to obects manager.
            mObjectsManager.CreatePlayerCharacter(necromancer);
            mObjectsManager.CreatePrince(prince);
            mObjectsManager.CreateCreature(zombie1);
            mObjectsManager.CreateCreature(zombie2);
            mObjectsManager.CreateCreature(zombie3);
            mObjectsManager.CreateCreature(zombie4);
            mObjectsManager.CreateCreature(zombie5);
            mObjectsManager.CreateCreature(zombieToGo);
            mObjectsManager.CreateCreature(knight);
            mObjectsManager.CreateCreature(horse);
            mObjectsManager.CreateCreature(meatball);
#endif
        }

        public override bool HandleKeyEvent(IKeyEvent keyEvent)
        {
            if (keyEvent.KeyState == KeyState.Down && keyEvent.Key == Keys.Escape)
                mMenuActions.OpenPauseScreen();
            if (keyEvent.KeyState == KeyState.Down && keyEvent.Key == Keys.F5)
            {
                Rectangle cameraRectangle = mCameraManager.GetCamera().ScreenRectangle;
                mEffectsManager.PlayOnce(new SmokeBig(), cameraRectangle.Center, cameraRectangle.Size);
                mObjectsManager.ExposeTheLiving();
            }
            if (keyEvent.KeyState == KeyState.Down && keyEvent.Key == Keys.F6)
            {
                (mObjectsManager.PlayerCharacter as Necromancer)?.ChangeSex();
                mEffectsManager.PlayOnce(new SmokeSmall(), mObjectsManager.PlayerCharacter.Position.ToPoint(), new Point(256));
            }
            if (keyEvent.KeyState == KeyState.Down && keyEvent.Key == Keys.F8)
            {
                mFogOfWarActivaded = !mFogOfWarActivaded;
            }

            return true;
        }

        /// <summary>
        /// Updates the status of this object.
        /// </summary>
        /// <param name="gameTime">a snapshot of the game time</param>
        public override void Update(GameTime gameTime)
        {
#if DEBUG
#else
            try
            {
#endif
                if (mObjectsManager.Boss != null && mObjectsManager.Boss.IsDead)
                    mAchievementsAndStatistics.mKillKing = true;
                if (mObjectsManager.Prince != null && mObjectsManager.Prince.IsDead)
                    mAchievementsAndStatistics.mKillPrince = true;
                mObjectsManager.Update(gameTime, mRightButtonPressed, mRightButtonPosition, mCameraManager.GetCamera());
                mCameraManager.Update(mObjectsManager.PlayerCharacter);
                mEffectsManager.Update(gameTime);
            mAiPlayer.Update(gameTime);

            // Call for Transformations
            mTransformation.Transform();

                // Check whether creature is in Necromancers Radius
                mSelection.UpdateSelection();

                // Update for FogOfWar.
                for (int i = mFogCounter; i < mObjectsManager.UndeadCreatures.Count; i += 30)
                {
                    mFog.Update(mObjectsManager.UndeadCreatures[i]);
                }
                if (mFogCounter < 30)
                    mFogCounter++;
                else
                    mFogCounter = 0;

#if DEBUG
#else
                }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
#endif
        }

        /// <summary>
        /// Draws this object using the given sprite batch.
        /// </summary>
        /// <param name="gameTime">a snapshot of the game time</param>
        /// <param name="spriteBatch">the sprite batch to use for drawing
        /// this object</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            mGraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null, mCameraManager.GetCamera().Transform);
            mMapManager.DrawFloor(spriteBatch, mCameraManager.GetCamera());
            mObjectsManager.Draw(spriteBatch, mCameraManager.GetCamera());
            mTransformation.DrawNecroArea(spriteBatch);
            mEffectsManager.Draw(spriteBatch);
            // Draws the selection rectangle when left mouse button is pressed.
            if (mLeftButtonPressed)
                mSelection.Draw(spriteBatch, mInitialClickPosition + mCameraManager.GetCamera().Location.ToPoint(),
                    Mouse.GetState().Position + mCameraManager.GetCamera().Location.ToPoint());
            if (mFogOfWarActivaded) mFog.DrawFog(spriteBatch);
            if (mOptionsManager.Options.DebugMode == DebugMode.Full)
            {
                mMapManager.DrawPathfindingGrid(spriteBatch, mCameraManager.GetCamera());
                mObjectsManager.DrawQuadtree(spriteBatch);
                DrawLastSelection(spriteBatch, mSelection.LastSelection);
            }
            spriteBatch.End();
        }

        private void DrawLastSelection(SpriteBatch spriteBatch, Rectangle selection)
        {
            spriteBatch.Draw(mOnePixelTexture, selection, new Color(Color.Black, 100));
        }

        public void SetFog(List<Rectangle> fog)
        {
            mFog.SetFog(fog);
        }

        public List<Rectangle> GetFog()
        {
            return mFog.GetFog();
        }

        public void SetAiState(AiState state)
        {
            mAiPlayer.State = state;
        }

        public AiState GetAiState()
        {
            return mAiPlayer.State;
        }
    }
}
