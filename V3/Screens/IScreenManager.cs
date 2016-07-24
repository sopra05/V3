namespace V3.Screens
{
    /// <summary>
    /// Handles screens using a screen stack.  You can add screens to the
    /// foreground using the AddScreen method, remove screens from the
    /// foreground using the RemoveScreen method and remove all screens using
    /// the Clear method.  The top screen is always drawn and updated, and
    /// it can decide whether lower screens should be drawn and/or updated
    /// too.  The screens are updated from top to bottom, and drawn from
    /// bottom to top.
    /// </summary>
    public interface IScreenManager : IDrawable, IUpdateable
    {
        /// <summary>
        /// Adds a screen to the foreground.
        /// </summary>
        /// <param name="screen">the screen to add in the foreground</param>
        void AddScreen(IScreen screen);

        /// <summary>
        /// Removes the given screen if it is on the top of the screen stack.
        /// </summary>
        /// <param name="screen">the screen to remove</param>
        /// <returns>true if the screen was on top and has been removed,
        /// false otherwise</returns>
        void RemoveScreen(IScreen screen);

        /// <summary>
        /// Clears the screen stack.
        /// </summary>
        void Clear();

        GameScreen GetGameScreen();
    }
}
