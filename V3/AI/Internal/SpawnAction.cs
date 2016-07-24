using Microsoft.Xna.Framework;
using V3.Objects;

namespace V3.AI.Internal
{
    /// <summary>
    /// Spawns a creature at a given position.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SpawnAction : AbstractAction
    {
        private readonly IObjectsManager mObjectsManager;
        private ICreature mCreature;
        private Vector2 mPosition;

        /// <summary>
        /// Creates a new SpawnAction that spawns the given creature at the
        /// given position.
        /// </summary>
        /// <param name="objectsManager">the linked objects manager</param>
        /// <param name="creature">the creature to spawn</param>
        /// <param name="position">the spawn position</param>
        public SpawnAction(IObjectsManager objectsManager, ICreature creature, Vector2 position)
        {
            mObjectsManager = objectsManager;
            mCreature = creature;
            mPosition = position;
        }

        public override void Start()
        {
            mCreature.Position = mPosition;
            mObjectsManager.CreateCreature(mCreature);
            base.Start();
        }

        protected override ActionState GetNextState()
        {
            return ActionState.Done;
        }
    }
}
