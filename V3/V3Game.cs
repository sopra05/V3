using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using V3.Data;
using V3.Screens;

namespace V3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public sealed class V3Game : Game
    {
        private readonly IKernel mKernel;
        private SpriteBatch mSpriteBatch;
        private IScreenManager mScreenManager;
        private IScreen mMainScreen;
        //private float mReculate = 1.0f;
        /// <summary>
        /// Creates a new V3 game instance.
        /// </summary>
        public V3Game()
        {
            mKernel = new StandardKernel(new Bindings(this, new GraphicsDeviceManager(this)));
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";
            mKernel.Get<IPathManager>().CreateMissingDirectories();
            mScreenManager = mKernel.Get<IScreenManager>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            mMainScreen = mKernel.Get<IScreenFactory>().CreateMainScreen();
            mScreenManager.AddScreen(mMainScreen);
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// It delegates the update to the screen manager.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mScreenManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.  It delegates the
        /// drawing to the screen manager.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            mScreenManager.Draw(gameTime, mSpriteBatch);
            base.Draw(gameTime);
        }
    }
}
