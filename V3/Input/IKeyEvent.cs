using Microsoft.Xna.Framework.Input;

namespace V3.Input
{
    /// <summary>
    /// An event that is triggered if a key is pressed or released on the
    /// keyboard.
    /// </summary>
    public interface IKeyEvent
    {
        /// <summary>
        /// The key that was pressed or released.
        /// </summary>
        Keys Key { get; }
        /// <summary>
        ///  The type of the event (key pressed or released?).
        /// </summary>
        KeyState KeyState { get; }
    }
}