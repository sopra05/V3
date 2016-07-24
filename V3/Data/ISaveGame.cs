using System;

namespace V3.Data
{
    /// <summary>
    /// Stores a game state with some metadata.
    /// </summary>
    public interface ISaveGame : IComparable<ISaveGame>
    {
        /// <summary>
        /// The creation time of this save game in local time.
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// The compability version of this save game.
        /// </summary>
        int Version { get; set; }

        /// <summary>
        /// The data stored in this save game.
        /// </summary>
        GameState GameState { get; set; }
    }
}