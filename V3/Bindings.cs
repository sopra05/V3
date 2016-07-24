using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using V3.AI;
using V3.AI.Internal;
using V3.Camera;
using V3.Data;
using V3.Data.Internal;
using V3.Effects;
using V3.Input;
using V3.Input.Internal;
using V3.Map;
using V3.Screens;
using V3.Objects;
using V3.Widgets;

namespace V3
{
    /// <summary>
    /// Defines the bindings of constants, factories and singletons for the
    /// Ninject dependency injection framework.
    /// </summary>
    public sealed class Bindings : NinjectModule
    {
        private readonly V3Game mGame;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;

        /// <summary>
        /// Creates a new Bindings instance for the given game and graphics
        /// device manager.
        /// </summary>
        /// <param name="game">the game that uses this instance</param>
        /// <param name="graphicsDeviceManager">the graphics device manager
        /// instance to use in this instance</param>
        public Bindings(V3Game game, GraphicsDeviceManager graphicsDeviceManager)
        {
            mGame = game;
            mGraphicsDeviceManager = graphicsDeviceManager;
        }

        public override void Load()
        {
            // constants
            Bind<ContentManager>().ToConstant(mGame.Content);
            Bind<Game>().ToConstant(mGame);
            Bind<GraphicsDeviceManager>().ToConstant(mGraphicsDeviceManager);

            // factories
            Bind<IActionFactory>().ToFactory();
            Bind<IBasicCreatureFactory>().ToFactory();
            Bind<IBasicWidgetFactory>().ToFactory();
            Bind<IMenuFactory>().ToFactory();
            Bind<IScreenFactory>().ToFactory();

            // singletons
            Bind<IGameStateManager>().To<GameStateManager>().InSingletonScope();
            Bind<IInputManager>().To<InputManager>().InSingletonScope();
            Bind<IOptionsManager>().To<OptionsManager>().InSingletonScope();
            Bind<IScreenManager>().To<ScreenManager>().InSingletonScope();
            Bind<CameraManager>().ToSelf().InSingletonScope();
            Bind<IMapManager>().To<MapManager>().InSingletonScope();
            Bind<IObjectsManager>().To<ObjectsManager>().InSingletonScope();
            Bind<Pathfinder>().ToSelf().InSingletonScope();
            Bind<Selection>().ToSelf().InSingletonScope();
            Bind<IEffectsManager>().To<EffectsManager>().InSingletonScope();
            Bind<AchievementsAndStatistics>().ToSelf().InSingletonScope();

            // regular bindings
            Bind<IAiPlayer>().To<AiPlayer>();
            Bind<IPathManager>().To<PathManager>();
            Bind<ISaveGameManager>().To<SaveGameManager>();
        }
    }
}
