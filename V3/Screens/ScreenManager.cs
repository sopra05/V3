using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using V3.Input;

namespace V3.Screens
{
    /// <summary>
    /// Default implementation of IScreenManager.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class ScreenManager : IScreenManager
    {
        private readonly IInputManager mInputManager;
        private readonly DebugScreen mDebugScreen;
        private readonly ContentManager mContentManager;

        private readonly Stack<IScreen> mScreens = new Stack<IScreen>();

        /// <summary>
        /// Creates a new screen manager using the given input manager.
        /// </summary>
        public ScreenManager(IInputManager inputManager, DebugScreen debugScreen, ContentManager contentManager)
        {
            mInputManager = inputManager;
            mDebugScreen = debugScreen;
            mContentManager = contentManager;
        }

        /// <summary>
        /// Adds a screen to the foreground.
        /// </summary>
        /// <param name="screen">the screen to add in the foreground</param>
        public void AddScreen(IScreen screen)
        {
            mScreens.Push(screen);
#if NO_AUDIO
#else
            if (screen is MainScreen)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(mContentManager.Load<Song>("Sounds/Kosta_T_-_06"));
            }
            else if (screen is GameScreen)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(mContentManager.Load<Song>("Sounds/Afraid_to_Go"));
                
            }
            //else if (screen is PauseScreen)
            //{
            //    mAbstractCreature.GetSelf();
            //    mAbstractCreature.mSoundEffectInstance.Stop();
            //    mAbstractCreature.mSoundEffectInstanceFight.Stop();
            //    mAbstractCreature.mSoundEffectInstanceHorse.Stop();
            //    mAbstractCreature.mSoundEffectInstanceKnight.Stop();
            //    mAbstractCreature.mSoundEffectInstanceMeatball.Stop();
            //}
#endif
        }

        /// <summary>
        /// Removes the given screen if it is on the top of the screen stack.
        /// </summary>
        /// <param name="screen">the screen to remove</param>
        public void RemoveScreen(IScreen screen)
        {
            if (mScreens.Count > 0 && screen.Equals(mScreens.Peek()))
            {
                mScreens.Pop();
            }
        }

        /// <summary>
        /// Clears the screen stack.
        /// </summary>
        public void Clear()
        {
            mScreens.Clear();
        }

        /// <summary>
        /// Draws the top screen and, if enabled, the lower screens.  The draw
        /// order is from bottom to top, i. e. the lowest enabled screen is
        /// drawn first, and the top screen is drawn last.
        /// </summary>
        /// <param name="gameTime">a snapshot of the game time</param>
        /// <param name="spriteBatch">the sprite batch to use for drawing
        /// </param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var drawScreens = new Stack<IScreen>();
            foreach (var screen in mScreens)
            {
                drawScreens.Push(screen);
                if (!screen.DrawLower)
                    break;
            }

            foreach (var screen in drawScreens)
            {
                screen.Draw(gameTime, spriteBatch);
            }
            mDebugScreen.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Updates the top screen and, if enabled, the lower screens.  The
        /// update order is from top to bottom, i. e. the lowest enabled screen
        /// is drawn first, and the top screen is drawn last.
        /// </summary>
        /// <param name="gameTime">a snapshot of the game time</param>
        public void Update(GameTime gameTime)
        {
            mInputManager.Update();
            HandleInputEvents();

            mDebugScreen.Update(gameTime);
            var currentScreens = new Stack<IScreen>(new Stack<IScreen>(mScreens));
            foreach (var screen in currentScreens)
            {
                screen.Update(gameTime);
                if (!screen.UpdateLower)
                    break;
            }
        }

        private void HandleInputEvents()
        {
            // We need to clone the stack as the input management methods
            // might want to modify the screen stack.  We need two stacks as
            // each stack reverses the order.
            var currentScreens = new Stack<IScreen>(new Stack<IScreen>(mScreens));
            foreach (var keyEvent in mInputManager.KeyEvents)
            {
                foreach (var screen in currentScreens)
                {
                    if (screen.HandleKeyEvent(keyEvent))
                        break;
                }
            }
            foreach (var mouseEvent in mInputManager.MouseEvents)
            {
                foreach (var screen in currentScreens)
                {
                    if (screen.HandleMouseEvent(mouseEvent))
                        break;
                }
            }
        }

        public GameScreen GetGameScreen()
        {
            foreach (var screen in mScreens)
            {
                if (screen is GameScreen)
                    return (GameScreen) screen;
            }
            return null;
        }
    }
}
