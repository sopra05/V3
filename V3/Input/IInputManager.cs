using System.Collections.Generic;

namespace V3.Input
{
    /// <summary>
    /// Watches the state of the mouse and keyboard and creates events if a
    /// change (key pressed or released) is detected. To use this class, call
    /// Update in every update round. After the update, you may access the
    /// generated events in KeyEvents and MouseEvents. The input manager only
    /// watches the mouse buttons and keys listed in sWatchedKeys and
    /// sWatchedButtons.
    /// </summary>
    public interface IInputManager
    {
        /// <summary>
        /// The key events that were generated during the last update. Reset
        /// in the next update.
        /// </summary>
        ICollection<IKeyEvent> KeyEvents { get; }

        /// <summary>
        /// The mouse events that were generated during the last update. Reset in
        /// the next update.
        /// </summary>
        ICollection<IMouseEvent> MouseEvents { get; }

        /// <summary>
        /// Updates the keyboard and mouse status and generates the key and mouse
        /// events in KeyEvents and MouseEvents if changes were detected. Should
        /// be called once every update, before doing something else.
        /// </summary>
        void Update();
    }
}