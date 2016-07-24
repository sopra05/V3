using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using V3.Camera;
using V3.Data;

namespace V3.Screens
{
    /// <summary>
    /// Provides common actions for the main and pause menu, like saving and
    /// loading the game or removing and adding a screen to the screen stack.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class MenuActions
    {
        private readonly Game mGame;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;
        private readonly IGameStateManager mGameStateManager;
        private readonly IOptionsManager mOptionsManager;
        private readonly ISaveGameManager mSaveGameManager;
        private readonly IScreenManager mScreenManager;
        private readonly IScreenFactory mScreenFactory;
        private readonly CameraManager mCameraManager;

        /// <summary>
        /// Creates a new menu actions instance.
        /// </summary>
        public MenuActions(Game game, GraphicsDeviceManager graphicsDeviceManager,
                IGameStateManager gameStateManager, IOptionsManager optionsManager,
                ISaveGameManager saveGameManager, IScreenManager screenManager,
                IScreenFactory screenFactory, CameraManager cameraManager)
        {
            mGame = game;
            mGraphicsDeviceManager = graphicsDeviceManager;
            mGameStateManager = gameStateManager;
            mOptionsManager = optionsManager;
            mSaveGameManager = saveGameManager;
            mScreenManager = screenManager;
            mScreenFactory = screenFactory;
            mCameraManager = cameraManager;
        }

        /// <summary>
        /// Checks whether it is possible to load a game, i. e. whether there
        /// are save games.
        /// </summary>
        /// <returns>true if there are save games to load, false otherwise
        /// </returns>
        public bool CanLoadGame()
        {
            return mSaveGameManager.GetSaveGames().Count > 0;
        }

        /// <summary>
        /// Removes the given screen from the screen manager.
        /// </summary>
        /// <param name="screen">the screen to remove</param>
        public void Close(IScreen screen)
        {
            // TODO: something like RemoveUntil
            mScreenManager.RemoveScreen(screen);
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void Exit()
        {
            mGame.Exit();
        }

        public void LoadGame(ISaveGame saveGame)
        {
            mScreenManager.Clear();
            var gameScreen = mScreenFactory.CreateGameScreen();
            var gameState = saveGame.GameState;
            mGameStateManager.LoadGameState(gameState);
            gameScreen.SetFog(gameState.mFog);
            gameScreen.SetAiState(gameState.mAiState);
            mScreenManager.AddScreen(gameScreen);
            mScreenManager.AddScreen(mScreenFactory.CreateHudScreen());
        }

        /// <summary>
        /// Opens the main screen and removes all other screens.
        /// </summary>
        public void OpenMainScreen()
        {
            mScreenManager.Clear();
            mScreenManager.AddScreen(mScreenFactory.CreateMainScreen());
        }

        /// <summary>
        /// Opens the options screen in the front.
        /// </summary>
        public void OpenOptionsScreen()
        {
            mScreenManager.AddScreen(mScreenFactory.CreateOptionsScreen());
        }

        public void OpenLoadScreen()
        {
            mScreenManager.AddScreen(mScreenFactory.CreateLoadScreen());
        }

        /// <summary>
        /// Opens the pause screen in the front.
        /// </summary>
        public void OpenPauseScreen()
        {
            mScreenManager.AddScreen(mScreenFactory.CreatePauseScreen());
        }

        /// <summary>
        /// Opens the death screen in the front.
        /// </summary>
        public void OpenDeathScreen()
        {
            mScreenManager.AddScreen(mScreenFactory.CreateDeathScreen());
        }

        /// <summary>
        /// Opens the victory screen in the front.
        /// </summary>
        public void OpenVictoryScreen()
        {
            mScreenManager.AddScreen(mScreenFactory.CreateVictoryScreen());
        }

        /// <summary>
        /// Opens the statistics screen in the front.
        /// </summary>
        public void OpenStatisticsScreen()
        {
            mScreenManager.AddScreen(mScreenFactory.CreateStatisticsScreen());
        }

        /// <summary>
        /// Opens the achievements screen in the front.
        /// </summary>
        public void OpenAchievementsScreen()
        {
            mScreenManager.AddScreen(mScreenFactory.CreateAchievementsScreen());
        }

        /// <summary>
        /// Opens the tech demo.
        /// </summary>
        public void OpenTechDemo()
        {
            mScreenManager.Clear();
            mScreenManager.AddScreen(mScreenFactory.CreaTechdemoScreen());
            mScreenManager.AddScreen(mScreenFactory.CreateHudScreen());
        }

        /// <summary>
        /// Creates a new save game from the current game state.
        /// </summary>
        public void SaveGame()
        {
            var gameState = mGameStateManager.GetGameState();
            var gameScreen = mScreenManager.GetGameScreen();
            if (gameScreen != null)
            {
                gameState.mFog = gameScreen.GetFog();
                gameState.mAiState = gameScreen.GetAiState();
            }
            mSaveGameManager.CreateSaveGame(gameState);
        }

        /// <summary>
        /// Apply the current graphics options.
        /// </summary>
        public void ApplyOptions()
        {
            mGraphicsDeviceManager.PreferredBackBufferWidth = mOptionsManager.Options.Resolution.X;
            mGraphicsDeviceManager.PreferredBackBufferHeight = mOptionsManager.Options.Resolution.Y;
            mGraphicsDeviceManager.IsFullScreen = mOptionsManager.Options.IsFullScreen;
            mGraphicsDeviceManager.ApplyChanges();

#if NO_AUDIO
#else
            MediaPlayer.Volume = mOptionsManager.Options.GetEffectiveVolume();
#endif
        }

        /// <summary>
        /// Save the current graphics options.
        /// </summary>
        public void SaveOptions()
        {
            mOptionsManager.SaveOptions();
        }

        /// <summary>
        /// Opens the game screen with a new game.
        /// </summary>
        public void StartNewGame()
        {
            mScreenManager.Clear();
            var gameScreen = mScreenFactory.CreateGameScreen();
            gameScreen.CreateInitialPopulation();
            mScreenManager.AddScreen(gameScreen);
            mScreenManager.AddScreen(mScreenFactory.CreateHudScreen());
            if (mCameraManager.GetCamera() is CameraScrolling)
                mCameraManager.GetCamera().Location = Vector2.Zero;
        }
    }
}
