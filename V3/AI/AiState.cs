namespace V3.AI
{
    /// <summary>
    /// An action state for the AI player that is part of a strategy.  A state
    /// defines the specific actions to take (for example, defend peasants, or
    /// attack enemy creatures).
    /// </summary>
    public enum AiState
    {
        /// <summary>
        /// Waiting for the player actions.
        /// </summary>
        Idle,
        /// <summary>
        /// Defend peasants so that they don't become zombies.
        /// </summary>
        DefendPeasants,
        /// <summary>
        /// Attack enemy creatures.
        /// </summary>
        AttackCreatures,
        /// <summary>
        /// Attack the necromancer directly.
        /// </summary>
        AttackNecromancer
    }
}
