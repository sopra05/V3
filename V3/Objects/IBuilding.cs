namespace V3.Objects
{
    public interface IBuilding : IGameObject
    {
        int Robustness { get; }
        string Name { get; }
        int MaxGivesWeapons { get; }

        /// <summary>
        /// Building takes specific amount of damage. Substracted from Robustness.
        /// </summary>
        /// <param name="damage">TakeDamage taken</param>
        void TakeDamage(int damage);

        /// <summary>
        /// Building can give a fixed amount of upgrades.
        /// </summary>
        void UpgradeCounter();
    }
}
