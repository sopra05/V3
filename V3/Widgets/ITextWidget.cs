using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Widgets
{
    /// <summary>
    /// A widget that contains some text.
    /// </summary>
    public interface ITextWidget : IWidget
    {
        /// <summary>
        /// The text of this widget.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// The space that is added before and after the text in the
        /// horizontal direction.
        /// </summary>
        float PaddingX { get; set; }

        /// <summary>
        /// The space that is added before and after the text in the
        /// vertical direction.
        /// </summary>
        float PaddingY { get; set; }

        /// <summary>
        /// The horizontal orientation of the text within the size.
        /// </summary>
        HorizontalOrientation HorizontalOrientation { get; set; }

        /// <summary>
        /// The default color of the text.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// The font for text rendering.
        /// </summary>
        SpriteFont Font { get; set; }
    }
}
