using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace V3.AI
{
    /// <summary>
    /// A computer player that takes actions according to a specified strategy.
    /// </summary>
    public interface IAiPlayer
    {
        /// <summary>
        /// The current world view of the player.  It stores the knowledge of
        /// the computer player based on the previous percepts.
        /// </summary>
        IWorldView WorldView { get; }
        /// <summary>
        /// The strategy of the player.  The strategy is a state machine that
        /// defines the current state.
        /// </summary>
        IStrategy Strategy { get; }
        /// <summary>
        /// The current state of the player.  The state is one step of the
        /// strategy, and defines the specific actions to take.
        /// </summary>
        AiState State { get; set; }
        /// <summary>
        /// The actions that the player wants to be executed.  Updated by
        /// Act.
        /// </summary>
        IList<IAction> Actions { get; }

        /// <summary>
        /// Executes one update cycle -- perception, acting and the execution
        /// of actions.
        /// </summary>
        /// <param name="gameTime">the time since the last update</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// Update the AI's view of the game world.
        /// </summary>
        void Percept();

        /// <summary>
        /// Take actions based on the previous percepts, the current strategy
        /// and state.  Updates the list of actions stored in Actions.  These
        /// should be executed by the caller.
        /// </summary>
        void Act();
    }
}
