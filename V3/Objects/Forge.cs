using Microsoft.Xna.Framework;

namespace V3.Objects
{
    /// <summary>
    /// A Forge which can be attacked.
    /// </summary>
    public sealed class Forge : AbstractBuilding
    {
        public override string Name { get; } = "Schmiede";
        protected override int MaxRobustness { get; } = 200;
        public override int Robustness { get; protected set; } = 200;
        public override int MaxGivesWeapons { get; protected set; } = 10;

        public Forge(Vector2 position, Rectangle size, string textureName, BuildingFace facing) : base(position, size, textureName, facing)
        {
        }
    }
}
