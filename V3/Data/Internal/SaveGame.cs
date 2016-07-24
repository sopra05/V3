using System;

namespace V3.Data.Internal
{
    // TODO: once the game state is getting larger, we have to separate the
    // save game metadata from the game state.

    /// <summary>
    /// A save game that has a timestamp and a title, and that can store
    /// the game state.
    /// </summary>
    [Serializable]
    public sealed class SaveGame : ISaveGame
    {
        /// <summary>
        /// The creation time of this save game in local time.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The compability version of this save game.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// The data stored in this save game.
        /// </summary>
        public GameState GameState { get; set; }

        /// <summary>
        /// Empty constructor for serialization.
        /// </summary>
        private SaveGame()
        {
        }

        /// <summary>
        /// Creates a new save game from the given data.
        /// </summary>
        /// <param name="timestamp">the creation time of the save game</param>
        /// <param name="version">the compability version of the save game</param>
        /// <param name="gameState">the game state to store in the save game</param>
        internal SaveGame(DateTime timestamp, int version, GameState gameState)
        {
            Timestamp = timestamp;
            Version = version;
            GameState = gameState;
        }

        /// <summary>
        /// Compares this save game object to another save game object based
        /// on the creation time.
        /// </summary>
        /// <param name="saveGame">the save game to compare this save game
        /// with</param>
        /// <returns>a value less than zero if this save game has been saved
        /// before the given save game, zero if they have been saved at the
        /// same time and a value greater than zero if the given save game has
        /// been saved before the given one</returns>
        public int CompareTo(ISaveGame saveGame)
        {
            return Timestamp.CompareTo(saveGame.Timestamp);
        }
    }
}
