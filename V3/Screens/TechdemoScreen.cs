using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using V3.Camera;
using V3.Data;
using V3.Effects;
using V3.Input;
using V3.Map;
using V3.Objects;

namespace V3.Screens
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class TechdemoScreen : AbstractScreen, IInitializable
    {
        private enum Creatures
        {
            Empty, Zombie, Skeleton, Peasant, Knight
        }

        private Creatures mCreatePerKey;

        private readonly CameraManager mCameraManager;
        private readonly ContentManager mContentManager;
        private readonly CreatureFactory mCreatureFactory;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;
        private readonly MenuActions mMenuActions;
        private readonly IOptionsManager mOptionsManager;
        private readonly IEffectsManager mEffectsManager;
        private readonly Selection mSelection;
        private readonly Transformation mTransformation;
        private readonly IMapManager mMapManager;
        private readonly IObjectsManager mObjectsManager;
        private readonly Pathfinder mPathfinder;
        private readonly Texture2D mOnePixelTexture;
        // Fields for handling mouse input.
        private Point mInitialClickPosition;
        private bool mLeftButtonPressed;
        private bool mRightButtonPressed;
        private Vector2 mRightButtonPosition;

        /// <summary>
        /// Creates a new game screen.
        /// </summary>
        public TechdemoScreen(IOptionsManager optionsManager, CameraManager cameraManager,
                ContentManager contentManager, CreatureFactory creatureFactory,
                GraphicsDeviceManager graphicsDeviceManager, IMapManager mapManager,
                MenuActions menuActions, IObjectsManager objectsManager,
                Pathfinder pathfinder, Selection selection, Transformation transformation,
                IEffectsManager effectsManager) : base(false, false)
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
            mPathfinder = pathfinder;
            mSelection = selection;
            mOnePixelTexture = contentManager.Load<Texture2D>("Sprites/WhiteRectangle");
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
            if (mouseEvent.ReleasedOnScreen)
            {
                var position = mouseEvent.PositionPressed.ToVector2() + mCameraManager.GetCamera().Location;
                switch (mCreatePerKey)
                {
                    case Creatures.Zombie:
                        mObjectsManager.CreateCreature(mCreatureFactory.CreateZombie(position, MovementDirection.S));
                        break;
                    case Creatures.Skeleton:
                        mObjectsManager.CreateCreature(mCreatureFactory.CreateSkeleton(position, MovementDirection.S));
                        break;
                    case Creatures.Peasant:
                        mObjectsManager.CreateCreature(mCreatureFactory.CreateMalePeasant(position, MovementDirection.S));
                        break;
                    case Creatures.Knight:
                        mObjectsManager.CreateCreature(mCreatureFactory.CreateKnight(position, MovementDirection.S));
                        break;
                }
            }
            return true;
        }

        public void Initialize()
        {
            mMapManager.Load("techdemo");
            mObjectsManager.Initialize(mMapManager);
            mCameraManager.Initialize(mMapManager.SizeInPixel);
            mPathfinder.LoadGrid(mMapManager.GetPathfindingGrid());
            mTransformation.mSelection = mSelection;
            mTransformation.LoadArea(mContentManager);

            // Add creatures for testing purposes.
            var necromancer = mCreatureFactory.CreateNecromancer(new Vector2(1592, 1609), MovementDirection.S);
            var king = mCreatureFactory.CreateKing(new Vector2(1500, 200), MovementDirection.S);
            mObjectsManager.CreatePlayerCharacter(necromancer);
            mObjectsManager.CreateBoss(king);
            int makeCreatureStrongerModifier = 100;
            int makePeasantsNotSoMuchStrongerModifier = 10;
            AddCreature<MalePeasant>(new Point(4), new Point(200, 2000), new Point(2800, 3000), makePeasantsNotSoMuchStrongerModifier);
            AddCreature<FemalePeasant>(new Point(4), new Point(232, 2032), new Point(2800, 3000), makePeasantsNotSoMuchStrongerModifier);
            AddCreature<Knight>(new Point(4), new Point(200, 200), new Point(2800, 1000), makeCreatureStrongerModifier);
            AddCreature<KingsGuard>(new Point(4), new Point(232, 232), new Point(2800, 1000), makeCreatureStrongerModifier);
            AddCreature<Zombie>(new Point(3), new Point(264, 264), new Point(2800, 1000), makeCreatureStrongerModifier);
            AddCreature<Skeleton>(new Point(3), new Point(290, 290), new Point(2800, 1000), makeCreatureStrongerModifier);
            AddCreature<Zombie>(new Point(4), new Point(264, 2064), new Point(2800, 3000), makeCreatureStrongerModifier);

            mTransformation.ObjectsManager = mObjectsManager;
        }

        /// <summary>
        /// Add a generic creature to Techdemo.
        /// Using pixel coordinates for calculating start and end point of map.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="density">Density on X and Y axis. More creatures when lower.</param>
        /// <param name="start">Start point for creating the creature batallion.</param>
        /// <param name="end">At which point to stop creating creatures.</param>
        /// <param name="makeStronger">Give more HP. Better for testing.</param>
        private void AddCreature<T>(Point density, Point start, Point end, int makeStronger) where T:ICreature
        {
            var type = typeof(T);
            for (int i = start.Y; i < end.Y; i += 32 * density.Y)
            {
                for (int j = start.X; j < end.X; j += 32 * density.X)
                {
                    Vector2 position = new Vector2(j, i);
                    ICreature creature;
                    if (type == typeof(MalePeasant))
                    {
                        creature = mCreatureFactory.CreateMalePeasant(position, MovementDirection.S);
                    }
                    else if (type == typeof(FemalePeasant))
                    {
                        creature = mCreatureFactory.CreateFemalePeasant(position, MovementDirection.S);
                    }
                    else if (type == typeof(Skeleton))
                    {
                        creature = mCreatureFactory.CreateSkeleton(position, MovementDirection.S);
                    }
                    else if (type == typeof(Zombie))
                    {
                        creature = mCreatureFactory.CreateZombie(position, MovementDirection.S);
                    }
                    else if (type == typeof(Knight))
                    {
                        creature = mCreatureFactory.CreateKnight(position, MovementDirection.S);
                    }
                    else if (type == typeof(SkeletonHorse))
                    {
                        creature = mCreatureFactory.CreateSkeletonHorse(position, MovementDirection.S);
                    }
                    else if (type == typeof(Meatball))
                    {
                        creature = mCreatureFactory.CreateMeatball(position, MovementDirection.S);
                    }
                    else if (type == typeof(KingsGuard))
                    {
                        creature = mCreatureFactory.CreateKingsGuard(position, MovementDirection.S);
                    }
                    else if (type == typeof(SkeletonElite))
                    {
                        creature = mCreatureFactory.CreateSkeletonElite(position, MovementDirection.S);
                    }
                    else
                    {
                        creature = mCreatureFactory.CreateZombie(position, MovementDirection.S);
                    }
                    creature.Empower(makeStronger);
                    mObjectsManager.CreateCreature(creature);
                }
            }
            
        }

        public override bool HandleKeyEvent(IKeyEvent keyEvent)
        {
            if (keyEvent.KeyState == KeyState.Down)
            {
                Creatures createPerKey = Creatures.Empty;
                switch (keyEvent.Key)
                {
                    case Keys.Escape:
                        mMenuActions.OpenPauseScreen();
                        break;
                    case Keys.F1:
                        createPerKey = Creatures.Zombie;
                        break;
                    case Keys.F2:
                        createPerKey = Creatures.Skeleton;
                        break;
                    case Keys.F3:
                        createPerKey = Creatures.Peasant;
                        break;
                    case Keys.F4:
                        createPerKey = Creatures.Knight;
                        break;
                    case Keys.F5:
                        Rectangle cameraRectangle = mCameraManager.GetCamera().ScreenRectangle;
                        mEffectsManager.PlayOnce(new SmokeBig(), cameraRectangle.Center, cameraRectangle.Size);
                        mObjectsManager.ExposeTheLiving();
                        break;
                    case Keys.F6:
                        (mObjectsManager.PlayerCharacter as Necromancer)?.ChangeSex();
                        mEffectsManager.PlayOnce(new SmokeSmall(), mObjectsManager.PlayerCharacter.Position.ToPoint(), new Point(256));
                        break;
                }
                if (createPerKey == mCreatePerKey)
                {
                    mCreatePerKey = Creatures.Empty;
                }
                else
                {
                    mCreatePerKey = createPerKey;
                }
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
                mObjectsManager.Update(gameTime, mRightButtonPressed, mRightButtonPosition, mCameraManager.GetCamera());
                mCameraManager.Update(mObjectsManager.PlayerCharacter);
                mEffectsManager.Update(gameTime);

                // Call for Transformations
                mTransformation.Transform();

                mSelection.UpdateSelection();
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
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, mCameraManager.GetCamera().Transform);
            mMapManager.DrawFloor(spriteBatch, mCameraManager.GetCamera());
            mObjectsManager.Draw(spriteBatch, mCameraManager.GetCamera());
            mTransformation.DrawNecroArea(spriteBatch);
            mEffectsManager.Draw(spriteBatch);
            if (mLeftButtonPressed)
                mSelection.Draw(spriteBatch, mInitialClickPosition + mCameraManager.GetCamera().Location.ToPoint(), Mouse.GetState().Position + mCameraManager.GetCamera().Location.ToPoint());
            if (mOptionsManager.Options.DebugMode == DebugMode.Full)
            {
                mMapManager.DrawPathfindingGrid(spriteBatch, mCameraManager.GetCamera());
                mObjectsManager.DrawQuadtree(spriteBatch);
                DrawLastSelection(spriteBatch, mSelection.LastSelection);
            }
            // Draws the selection rectangle when left mouse button is pressed.
            spriteBatch.End();
        }

        private void DrawLastSelection(SpriteBatch spriteBatch, Rectangle selection)
        {
            spriteBatch.Draw(mOnePixelTexture, selection, new Color(Color.Black, 100));
        }
    }
}