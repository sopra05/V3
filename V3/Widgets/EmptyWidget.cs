using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Widgets
{
    /// <summary>
    /// A placeholder widget.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class EmptyWidget : IWidget
    {
        /// <summary>
        /// The current position of this widget on the screen.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The size of this widget on the screen.  The widget should try to
        /// fill this size.  Should not be smaller than the value returned by
        /// GetMinimumSize().
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Returns the minimum size this widgets needs.
        /// </summary>
        public Vector2 GetMinimumSize()
        {
            return new Vector2(0, 0);
        }

        /// <summary>
        /// Draws this widget on the given sprite batch.  It is assumed that
        /// spriteBatch.Begin() has already been called.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}

