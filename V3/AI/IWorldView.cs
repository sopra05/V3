using System.Collections.Generic;
using V3.Objects;

namespace V3.AI
{
    /// <summary>
    /// Stores the knowledge of the computer player about the game world, and
    /// is used for the evaluation of the strategy.  It is also used to decide
    /// which actions to take based on the current state.
    /// </summary>
    public interface IWorldView
    {
        int EnemyCount { get; set; }
        int InitialPlebsCount { get; set; }
        int PlebsCount { get; set; }
        float NecromancerHealth { get; set; }
        List<ICreature> IdlingKnights { get; }
        List<ICreature> Targets { get; }
        List<ICreature> Plebs { get; }
    }
}
