namespace V3.Input
{
    /// <summary>
    /// Handles mouse events.
    /// </summary>
    public interface IMouseEventHandler
    {
        /// <summary>
        /// Handle the given mouse event, if applicable.
        /// </summary>
        /// <param name="mouseEvent">the mouse event to handle</param>
        void HandleMouseEvent(IMouseEvent mouseEvent);
    }
}
