using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Sprite;

namespace V3.Objects
{
    public sealed class SkeletonElite : Skeleton
    {
        public override int Life { get; protected set; } = 200;
        public override int MaxLife { get; protected set; } = 200;
        public override int Attack { get; protected set; } = 30;
        protected override ISpriteCreature[] Sprite { get; } = { new SkeletonEliteSprite() };

        public SkeletonElite(ContentManager contentManager, Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics) : base(contentManager, pathfinder, optionsManager, achievementsAndStatistics)
        {
        }
    }
}