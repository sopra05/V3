using Microsoft.Xna.Framework;
using V3.Objects;

namespace V3.AI.Internal
{
    /// <summary>
    /// Creates IAction instances.  Automatically implemented by Ninject.
    /// </summary>
    public interface IActionFactory
    {
        /// <summary>
        /// Creates a new MoveAction to move the given creature to the given
        /// destination.
        /// </summary>
        /// <param name="creature">the creature to mvoe</param>
        /// <param name="destination">the destination of the creature</param>
        MoveAction CreateMoveAction(ICreature creature, Vector2 destination);

        /// <summary>
        /// Creates a new SpawnAction that spawns the given creature at the
        /// given position.
        /// </summary>
        /// <param name="creature">the creature to spawn</param>
        /// <param name="position">the spawn position</param>
        SpawnAction CreateSpawnAction(ICreature creature, Vector2 position);
    }
}
