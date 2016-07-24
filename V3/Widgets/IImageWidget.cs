using Microsoft.Xna.Framework.Graphics;

namespace V3.Widgets
{
    /// <summary>
    /// A widget that displays an image.
    /// </summary>
    public interface IImageWidget : IWidget
    {
        /// <summary>
        /// The image to display.
        /// </summary>
        Texture2D Image { get; set; }
    }
}
