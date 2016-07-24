using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace V3.Input.Internal
{
    /// <summary>
    /// Default implementation of an event that is sent when a mouse button is
    /// pressed or released.
    /// </summary>
    internal sealed class MouseEvent : IMouseEvent
    {
        /// <summary>
        /// The mouse button that was pressed or released.
        /// </summary>
        public MouseButton MouseButton { get; }
        /// <summary>
        /// The state of the mouse button (pressed or released?).
        /// </summary>
        public ButtonState ButtonState { get; }
        /// <summary>
        /// The position where the mouse button was pressed the last time.
        /// </summary>
        public Point PositionPressed { get; }
        /// <summary>
        /// The position where the mouse button was released if this is a
        /// release event, null otherwise.
        /// </summary>
        public Point? PositionReleased { get; }
        /// <summary>
        /// True if PositionReleased is a valid on-screen position, otherwise
        /// false.
        /// </summary>
        public bool ReleasedOnScreen { get; }

        /// <summary>
        /// Creates a new mouse event with the given data.
        /// </summary>
        /// <param name="mouseButton">the mouse button that was pressed or
        ///     released</param>
        /// <param name="buttonState">the type of the event (pressed or
        ///     released?)</param>
        /// <param name="positionPressed">the position of the last press of
        ///     the button</param>
        /// <param name="positionReleased">the position of the release of the
        ///     button if this is a release event, or null otherwise</param>
        /// <param name="releasedOnScreen">true if positionReleased is a valid
        ///     on-screen position.</param>
        public MouseEvent(MouseButton mouseButton, ButtonState buttonState, Point positionPressed, Point? positionReleased, bool releasedOnScreen)
        {
            MouseButton = mouseButton;
            ButtonState = buttonState;
            PositionPressed = positionPressed;
            PositionReleased = positionReleased;
            ReleasedOnScreen = releasedOnScreen;
        }
    }
}
