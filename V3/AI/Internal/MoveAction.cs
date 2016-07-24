using Microsoft.Xna.Framework;
using V3.Objects;

namespace V3.AI.Internal
{
    /// <summary>
    /// Moves a creature to a destination point.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MoveAction : AbstractAction
    {
        private ICreature mCreature;
        private Vector2 mDestination;

        /// <summary>
        /// Creates a new MoveAction to move the given creature to the given
        /// destination.
        /// </summary>
        /// <param name="creature">the creature to mvoe</param>
        /// <param name="destination">the destination of the creature</param>
        public MoveAction(ICreature creature, Vector2 destination)
        {
            mCreature = creature;
            mDestination = destination;
        }

        /// <summary>
        /// Start the execution of the action.
        /// </summary>
        public override void Start()
        {
            mCreature.Move(mDestination);
            base.Start();
        }

        protected override ActionState GetNextState()
        {
            switch (mCreature.MovementState)
            {
                case MovementState.Idle:
                    return ActionState.Done;
                case MovementState.Attacking:
                case MovementState.Dying:
                    return ActionState.Failed;
                case MovementState.Moving:
                    return ActionState.Executing;
                default:
                    return ActionState.Failed;
            }
        }

    }
}
