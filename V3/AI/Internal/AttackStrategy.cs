namespace V3.AI.Internal
{
    /// <summary>
    /// A simple strategy for the computer player that tells him to attack the
    /// enemy creatures.
    /// </summary>
    internal class AttackStrategy : IStrategy
    {
        /// <summary>
        /// Updates the current state according to the game situtation.
        /// </summary>
        /// <param name="state">the current state</param>
        /// <param name="worldView">the current view of the game world</param>
        /// <returns>the next state indicated by this strategy</returns>
        public AiState Update(AiState state, IWorldView worldView)
        {
            switch (state)
            {
                case AiState.Idle:
                    if (worldView.InitialPlebsCount - worldView.PlebsCount > 3)
                    {
                        return AiState.DefendPeasants;
                    }
                    break;
                case AiState.DefendPeasants:
                    if (worldView.PlebsCount < worldView.InitialPlebsCount * 0.75 || worldView.EnemyCount > 20)
                    {
                        return AiState.AttackCreatures;
                    }
                    break;
                case AiState.AttackCreatures:
                    if (worldView.NecromancerHealth < 0.1)
                    {
                        return AiState.AttackNecromancer;
                    }
                    break;
                case AiState.AttackNecromancer:
                    if (worldView.NecromancerHealth >= 0.1)
                    {
                        return AiState.AttackCreatures;
                    }
                    break;
            }

            return state;
        }
    }
}
