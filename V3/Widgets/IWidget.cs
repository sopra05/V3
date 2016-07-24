using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Widgets
{
    /// <summary>
    /// A simple widget that has a size and a position and that can be drawn.
    /// </summary>
    public interface IWidget
    {
        /// <summary>
        /// The current position of this widget on the screen.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// The size of this widget on the screen.  The widget should try to
        /// fill this size.  Should not be smaller than the value returned by
        /// GetMinimumSize().
        /// </summary>
        Vector2 Size { get; set; }

        /// <summary>
        /// Returns the minimum size this widgets needs.
        /// </summary>
        Vector2 GetMinimumSize();

        /// <summary>
        /// Drawst this widget on the given sprite batch.  It is assumed that
        /// spriteBatch.Begin() has already been called.
        /// </summary>
        void Draw(SpriteBatch spriteBatch);
    }
}
