using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using V3.Input;

namespace V3.Widgets
{
    /// <summary>
    /// A menu that displays a list of widgets.
    /// </summary>
    public interface IMenu : IMouseEventHandler
    {
        /// <summary>
        /// The widgets in this menu.  The order of the widgets in this list
        /// is the order in which they are displayed.
        /// </summary>
        List<IWidget> Widgets { get; }

        /// <summary>
        /// The total size of the widgets in this menu.
        /// </summary>
        Vector2 Size { get; }

        /// <summary>
        /// The current position of the widgets.
        /// </summary>
        Vector2 Position { get; }

        void Draw(SpriteBatch spriteBatch);

        void Update();
    }
}
