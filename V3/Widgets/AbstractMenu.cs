using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using V3.Input;

namespace V3.Widgets
{
    /// <summary>
    /// An abstract menu that handles the updating and drawing of widgtes.
    /// </summary>
    public abstract class AbstractMenu : IMenu
    {
        public List<IWidget> Widgets { get; } = new List<IWidget>();
        public Vector2 Size { get; private set; }
        public Vector2 Position { get; private set; }

        private readonly GraphicsDeviceManager mGraphicsDeviceManager;

        protected AbstractMenu(GraphicsDeviceManager graphicsDeviceManager)
        {
            mGraphicsDeviceManager = graphicsDeviceManager;
        }

        public void HandleMouseEvent(IMouseEvent mouseEvent)
        {
            foreach (var clickable in Widgets.OfType<IClickable>())
                clickable.HandleMouseEvent(mouseEvent);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateWidgetRelativePositions();
            UpdateWidgetAbsolutePositions();
            spriteBatch.Begin();
            Widgets.ForEach(w => w.Draw(spriteBatch));
            spriteBatch.End();
        }

        public void Update()
        {
            ResetClicked();
            UpdateMouseSelection();
            UpdateWidgetSizes();
        }

        protected abstract void UpdateWidgetSizes();

        protected abstract void UpdateWidgetRelativePositions();

        protected abstract Vector2 GetTotalSize();

        private void UpdateWidgetAbsolutePositions()
        {
            var viewport = mGraphicsDeviceManager.GraphicsDevice.Viewport;
            Size = GetTotalSize();
            var viewportSize = new Vector2(viewport.Bounds.Width, viewport.Bounds.Height);
            Position = (viewportSize - Size) / 2;
            Widgets.ForEach(w => w.Position = w.Position + Position);
        }

        private void ResetClicked()
        {
            foreach (var clickable in Widgets.OfType<IClickable>())
                clickable.IsClicked = false;
        }

        private void UpdateMouseSelection()
        {
            var position = Mouse.GetState().Position;
            foreach (var selectable in Widgets.OfType<ISelectable>())
                selectable.IsSelected = selectable.CheckSelected(position);
        }

        protected static void MakeWidgetsSameSize(IEnumerable<IWidget> widgets)
        {
            var xMax = 0f;
            var yMax = 0f;

            var widgetsCopy = widgets as IList<IWidget> ?? widgets.ToList();
            foreach (var widget in widgetsCopy)
            {
                var size = widget.GetMinimumSize();
                if (size.X > xMax)
                    xMax = size.X;
                if (size.Y > yMax)
                    yMax = size.Y;
            }

            var sizeMax = new Vector2(xMax, yMax);
            foreach (var widget in widgetsCopy)
                widget.Size = sizeMax;
        }
    }
}
