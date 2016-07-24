using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace V3.Input
{
    /// <summary>
    /// An event that is sent when a mouse button is pressed or released.
    /// </summary>
    public interface IMouseEvent
    {
        /// <summary>
        /// The mouse button that was pressed or released.
        /// </summary>
        MouseButton MouseButton { get; }
        /// <summary>
        /// The state of the mouse button (pressed or released?).
        /// </summary>
        ButtonState ButtonState { get; }
        /// <summary>
        /// The position where the mouse button was pressed the last time.
        /// </summary>
        Point PositionPressed { get; }
        /// <summary>
        /// The position where the mouse button was released if this is a
        /// release event, null otherwise.
        /// </summary>
        Point? PositionReleased { get; }
        /// <summary>
        /// True if PositionReleased is a valid on-screen position, otherwise
        /// false.
        /// </summary>
        bool ReleasedOnScreen { get; }
    }
}
