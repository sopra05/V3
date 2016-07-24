namespace V3.AI.Internal
{
    /// <summary>
    /// Abstract implementation of IAction.
    /// </summary>
    public abstract class AbstractAction : IAction
    {
        /// <summary>
        /// The current state of the action.
        /// </summary>
        public ActionState State { get; private set; } = ActionState.Waiting;

        /// <summary>
        /// Start the execution of the action.
        /// </summary>
        public virtual void Start()
        {
            State = ActionState.Executing;
        }

        /// <summary>
        /// Update the execution state.  This method should be repateatingly
        /// called as long as State is Executing.
        /// </summary>
        public virtual void Update()
        {
            if (State != ActionState.Executing)
                return;
            State = GetNextState();
        }

        /// <summary>
        /// Returns the next state of this action.  It is guaranteed that the
        /// current state is Executing.
        /// </summary>
        protected abstract ActionState GetNextState();
    }
}
