using Microsoft.Xna.Framework;
using System.Linq;

namespace V3.Widgets
{
    /// <summary>
    /// A simple menu with vertically arranged widgets.  All widgets are made
    /// the same size (the maximum widget size).
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class VerticalMenu : AbstractMenu
    {
        private float mPadding = 10;

        public VerticalMenu(GraphicsDeviceManager graphicsDeviceManager) : base(graphicsDeviceManager)
        {
        }

        protected override void UpdateWidgetRelativePositions()
        {
            var y = 0f;
            foreach (var widget in Widgets)
            {
                widget.Position = new Vector2(0, y);
                y += widget.Size.Y;
                y += mPadding;
            }
        }

        protected override Vector2 GetTotalSize()
        {
            var totalX = Widgets.Max(w => w.Size.X);
            var totalY = (Widgets.Count - 1) * mPadding + Widgets.Sum(w => w.Size.Y);
            return new Vector2(totalX, totalY);
        }

        protected override void UpdateWidgetSizes()
        {
            MakeWidgetsSameSize(Widgets);
        }
    }
}
