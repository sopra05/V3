using Microsoft.Xna.Framework;

namespace V3.Objects
{
    public sealed class Woodhouse : AbstractBuilding
    {
        public override string Name { get; } = "Holzhaus";
        protected override int MaxRobustness { get; } = 130;
        public override int Robustness { get; protected set; } = 130;
        public override int MaxGivesWeapons { get; protected set; } = 10;

        public Woodhouse(Vector2 position, Rectangle size, string textureName, BuildingFace facing) : base(position, size, textureName, facing)
        {
        }
    }
}
