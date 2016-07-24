using System.Collections.Generic;

namespace V3.Data
{
    /// <summary>
    /// Stores and manages the game state in save games.
    /// </summary>
    public interface ISaveGameManager
    {
        /// <summary>
        /// Creates and persists a new save game of the given data with the
        /// title.
        /// </summary>
        /// <param name="gameState">the data to store</param>
        void CreateSaveGame(GameState gameState);

        /// <summary>
        /// Loads all available save games and returns them ordered by the
        /// creation date.
        /// </summary>
        /// <returns>a list of all available save games, orderd by creation
        /// data</returns>
        List<ISaveGame> GetSaveGames();
    }
}