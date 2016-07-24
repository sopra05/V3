using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace V3.Widgets
{
    /// <summary>
    /// A menu that arranges widgets in two columns.  All widgets in on column
    /// are made the same size (the maximum widget size).
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class FormMenu : AbstractMenu
    {
        private const float PaddingInnerX = 10;
        private const float PaddingOuterX = 10;
        private const float PaddingInnerY = 10;
        private const float PaddingOuterY = 10;

        public FormMenu(GraphicsDeviceManager graphicsDeviceManager) : base(graphicsDeviceManager)
        {
        }

        protected override Vector2 GetTotalSize()
        {
            var size = new Vector2(2 * PaddingOuterX, 2 * PaddingOuterY);
            var maxXLeft = 0f;
            var maxXRight = 0f;
            for (var i = 0; i < Widgets.Count; i += 2)
            {
                maxXLeft = Math.Max(maxXLeft, Widgets[i].Size.X);
                if (i + 1 < Widgets.Count)
                {
                    maxXRight = Math.Max(maxXRight, Widgets[i + 1].Size.X);
                    size.Y += Math.Max(Widgets[i].Size.Y, Widgets[i + 1].Size.Y);
                }
                else
                {
                    size.Y += Widgets[i].Size.Y;
                }
                if (i + 2 < Widgets.Count)
                    size.Y += PaddingInnerY;
            }
            size.X += maxXLeft + maxXRight + PaddingInnerX;
            return size;
        }

        protected override void UpdateWidgetRelativePositions()
        {
            var y = PaddingOuterY;
            for (var i = 0; i < Widgets.Count; i += 2)
            {
                var x = PaddingOuterX;
                Widgets[i].Position = new Vector2(x, y);
                if (i + 1 < Widgets.Count)
                {
                    x += Widgets[i].Size.X;
                    x += PaddingInnerX;
                    Widgets[i + 1].Position = new Vector2(x, y);
                    y += Math.Max(Widgets[i].Size.Y, Widgets[i + 1].Size.Y);
                }
                else
                {
                    y += Widgets[i].Size.Y;
                }
                y += PaddingInnerY;
            }
        }

        protected override void UpdateWidgetSizes()
        {
            MakeWidgetsSameSize(Widgets.Where((w, i) => i % 2 == 0));
            MakeWidgetsSameSize(Widgets.Where((w, i) => i % 2 == 1));
        }
    }
}
