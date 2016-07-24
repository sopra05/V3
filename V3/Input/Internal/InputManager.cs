using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ninject;

namespace V3.Input.Internal
{
    /// <summary>
    /// Watches the state of the mouse and keyboard and creates events if a
    /// change (key pressed or released) is detected. To use this class, call
    /// Update in every update round. After the update, you may access the
    /// generated events in KeyEvents and MouseEvents. The input manager only
    /// watches the mouse buttons and keys listed in sWatchedKeys and
    /// sWatchedButtons.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class InputManager : IInputManager, IInitializable
    {
        /// <summary>
        /// The key events that were generated during the last update. Reset
        /// in the next update.
        /// </summary>
        public ICollection<IKeyEvent> KeyEvents { get; } = new HashSet<IKeyEvent>();
        /// <summary>
        /// The mouse events that were generated during the last update. Reset in
        /// the next update.
        /// </summary>
        public ICollection<IMouseEvent> MouseEvents { get; } = new HashSet<IMouseEvent>();

        private static readonly ICollection<Keys> sWatchedKeys = new List<Keys> { Keys.Enter, Keys.Escape, Keys.E, Keys.L, Keys.Q, Keys.S, Keys.F1, Keys.F2 , Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8 };

        private static readonly ICollection<MouseButton> sWatchedButtons = new List<MouseButton> { MouseButton.Left, MouseButton.Right, MouseButton.Middle };

        private readonly GraphicsDeviceManager mGraphicsDeviceManager;

        private readonly IDictionary<Keys, KeyState> mKeyStates = new Dictionary<Keys, KeyState>();

        private readonly IDictionary<MouseButton, ButtonState> mButtonStates = new Dictionary<MouseButton, ButtonState>();

        private readonly IDictionary<MouseButton, Point?> mButtonPositions = new Dictionary<MouseButton, Point?>();

        /// <summary>
        /// Creates a new input manager.
        /// </summary>
        public InputManager(GraphicsDeviceManager graphicsDeviceManager)
        {
            mGraphicsDeviceManager = graphicsDeviceManager;
        }

        public void Initialize()
        {
            foreach (var key in sWatchedKeys)
                mKeyStates.Add(key, KeyState.Up);
            foreach (var button in sWatchedButtons)
            {
                mButtonStates.Add(button, ButtonState.Released);
                mButtonPositions.Add(button, null);
            }
        }

        /// <summary>
        /// Updates the keyboard and mouse status and generates the key and mouse
        /// events in KeyEvents and MouseEvents if changes were detected. Should
        /// be called once every update, before doing something else.
        /// </summary>
        public void Update()
        {
            UpdateKeyboard();
            UpdateMouse();
        }

        private void UpdateKeyboard()
        {
            KeyEvents.Clear();

            var state = Keyboard.GetState();
            foreach (var key in sWatchedKeys)
            {
                var newState = state[key];
                if (newState != mKeyStates[key])
                {
                    mKeyStates[key] = newState;
                    KeyEvents.Add(new KeyEvent(key, newState));
                }
            }
        }

        private void UpdateMouse()
        {
            MouseEvents.Clear();

            var state = Mouse.GetState();
            foreach (var button in sWatchedButtons)
            {
                var newState = GetButtonState(state, button);
                if (newState != mButtonStates[button])
                {
                    var position = new Point(state.X, state.Y);
                    var positionPressed = position;
                    Point? positionReleased = null;
                    if (newState == ButtonState.Released)
                    {
                        if (mButtonPositions[button].HasValue)
                            positionPressed = mButtonPositions[button].Value;
                        positionReleased = position;
                    }

                    mButtonStates[button] = newState;
                    mButtonPositions[button] = position;

                    var releasedOnScreen = false;
                    if (positionReleased.HasValue)
                        releasedOnScreen = IsPointOnScreen(positionReleased.Value);

                    MouseEvents.Add(new MouseEvent(button, newState, positionPressed, positionReleased, releasedOnScreen));
                }
            }
        }

        private bool IsPointOnScreen(Point point)
        {
            var viewport = mGraphicsDeviceManager.GraphicsDevice.Viewport;
            return point.X >= 0 && point.X <= viewport.Width && point.Y >= 0 && point.Y <= viewport.Height;
        }

        private static ButtonState GetButtonState(MouseState state, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return state.LeftButton;
                case MouseButton.Right:
                    return state.RightButton;
                case MouseButton.Middle:
                    return state.MiddleButton;
                default:
                    return state.LeftButton;
            }
        }
    }
}
