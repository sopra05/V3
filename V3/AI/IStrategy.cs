namespace V3.AI
{
    /// <summary>
    /// A strategy for the computer player.  A strategy is a finite state
    /// machine.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Updates the current state according to the game situtation.
        /// </summary>
        /// <param name="state">the current state</param>
        /// <param name="worldView">the current view of the game world</param>
        /// <returns>the next state indicated by this strategy</returns>
        AiState Update(AiState state, IWorldView worldView);
    }
}
