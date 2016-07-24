namespace V3.Data
{
    /// <summary>
    /// Stores the current game state.
    /// </summary>
    public interface IGameStateManager
    {
        /// <summary>
        /// Stores the current game state and returns it.
        /// </summary>
        /// <returns>the current game state</returns>
        GameState GetGameState();

        /// <summary>
        /// Restores the given game state.
        /// </summary>
        /// <param name="gameState">the game state to restore</param>
        void LoadGameState(GameState gameState);
    }
}
