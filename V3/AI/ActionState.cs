namespace V3.AI
{
    /// <summary>
    /// The state of an action taken by the computer player.
    /// </summary>
    public enum ActionState
    {
        /// <summary>
        /// The action is waiting to be executed.
        /// </summary>
        Waiting,
        /// <summary>
        /// The action is currently being executed.
        /// </summary>
        Executing,
        /// <summary>
        /// The action has been done successfully.
        /// </summary>
        Done,
        /// <summary>
        /// The action failed.
        /// </summary>
        Failed
    }
}
