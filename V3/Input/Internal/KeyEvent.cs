using Microsoft.Xna.Framework.Input;

namespace V3.Input.Internal
{
    /// <summary>
    /// Default implementation of an event that is triggered if a key is
    /// pressed or released on the keyboard.
    /// </summary>
    internal sealed class KeyEvent : IKeyEvent
    {
        /// <summary>
        /// The key that was pressed or released.
        /// </summary>
        public Keys Key { get; }
        /// <summary>
        ///  The type of the event (key pressed or released?).
        /// </summary>
        public KeyState KeyState { get; }

        /// <summary>
        /// Creates a new key event with the given data.
        /// </summary>
        /// <param name="key">the key that was pressed or released</param>
        /// <param name="keyState">the type of the event (presesd or
        /// released?)</param>
        public KeyEvent(Keys key, KeyState keyState)
        {
            Key = key;
            KeyState = keyState;
        }
    }
}
