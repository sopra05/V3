using V3.Input;

namespace V3.Screens
{
    /// <summary>
    /// A screen that can be handled by a screen manager and that provides
    /// information on how to deal with other screens below this one.
    /// </summary>
    public interface IScreen : IDrawable, IUpdateable
    {
        /// <summary>
        /// Indicates whether screens below this one should be updated.
        /// </summary>
        bool UpdateLower { get; }

        /// <summary>
        /// Indicates whether screens below this one should be drawn.
        /// </summary>
        bool DrawLower { get; }

        /// <summary>
        /// Handles the given key event and returns whether it should be passed
        /// to the screens below this one.
        /// </summary>
        /// <param name="keyEvent">the key event that occurred</param>
        /// <returns>true if the event has been handeled by this screen and
        /// should not be passed to the lower screens, false otherwise</returns>
        bool HandleKeyEvent(IKeyEvent keyEvent);

        /// <summary>
        /// Handles the given mouse event and returns whether it should be passed
        /// to the screens below this one.
        /// </summary>
        /// <param name="mouseEvent">the mouse event that occurred</param>
        /// <returns>true if the event has been handeled by this screen and
        /// should not be passed to the lower screens, false otherwise</returns>
        bool HandleMouseEvent(IMouseEvent mouseEvent);
    }
}
