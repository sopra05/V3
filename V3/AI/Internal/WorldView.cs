using System.Collections.Generic;
using V3.Objects;

namespace V3.AI.Internal
{
    /// <summary>
    /// Default implementation of IWorldView.
    /// </summary>
    internal class WorldView : IWorldView
    {
        public int EnemyCount { get; set; }
        public int InitialPlebsCount { get; set; }
        public int PlebsCount { get; set; }
        public float NecromancerHealth { get; set; }
        public List<ICreature> IdlingKnights { get; } = new List<ICreature>();
        public List<ICreature> Targets { get; } = new List<ICreature>();
        public List<ICreature> Plebs { get; } = new List<ICreature>();
    }
}
