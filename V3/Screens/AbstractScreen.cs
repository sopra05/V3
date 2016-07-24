using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using V3.Input;

namespace V3.Screens
{
    public abstract class AbstractScreen : IScreen
    {
        /// <summary>
        /// Indicates whether screens below this one should be updated.
        /// </summary>
        public bool UpdateLower { get; }

        /// <summary>
        /// Indicates whether screens below this one should be drawn.
        /// </summary>
        public bool DrawLower { get; }

        protected AbstractScreen(bool updateLower, bool drawLower)
        {
            UpdateLower = updateLower;
            DrawLower = drawLower;
        }

        /// <summary>
        /// Handles the given key event and returns whether it should be passed
        /// to the screens below this one.
        /// </summary>
        /// <param name="keyEvent">the key event that occurred</param>
        /// <returns>true if the event has been handeled by this screen and
        /// should not be passed to the lower screens, false otherwise</returns>
        public virtual bool HandleKeyEvent(IKeyEvent keyEvent)
        {
            return false;
        }

        /// <summary>
        /// Handles the given mouse event and returns whether it should be passed
        /// to the screens below this one.
        /// </summary>
        /// <param name="mouseEvent">the mouse event that occurred</param>
        /// <returns>true if the event has been handeled by this screen and
        /// should not be passed to the lower screens, false otherwise</returns>
        public virtual bool HandleMouseEvent(IMouseEvent mouseEvent)
        {
            return true;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
