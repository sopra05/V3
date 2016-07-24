using Microsoft.Xna.Framework.Content;

namespace V3.Widgets
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class Label : AbstractTextWidget
    {
        public Label(ContentManager contentManager) : base(contentManager)
        {
            HorizontalOrientation = HorizontalOrientation.Left;
            PaddingX = 20;
        }
    }
}
