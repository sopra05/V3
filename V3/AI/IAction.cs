namespace V3.AI
{
    /// <summary>
    /// An action that can be taken by the computer player.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// The current state of the action.
        /// </summary>
        ActionState State { get; }

        /// <summary>
        /// Start the execution of the action.
        /// </summary>
        void Start();

        /// <summary>
        /// Update the execution state.  This method should be repateatingly
        /// called as long as State is Executing.
        /// </summary>
        void Update();
    }
}
