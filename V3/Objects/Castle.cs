using Microsoft.Xna.Framework;

namespace V3.Objects
{
    public sealed class Castle : AbstractBuilding
    {
        public override string Name { get; } = "Burg";
        protected override int MaxRobustness { get; } = 800;
        public override int Robustness { get; protected set; } = 800;

        public Castle(Vector2 position, Rectangle size, string textureName, BuildingFace facing) : base(position, size, textureName, facing)
        {
        }
    }
}
