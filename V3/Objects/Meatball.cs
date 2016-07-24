using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    public sealed class Meatball : AbstractCreature
    {
        public Meatball(ContentManager contentManager, Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics) : base(contentManager, pathfinder, optionsManager, achievementsAndStatistics)
        {
        }

        protected override ISpriteCreature[] Sprite { get; } = { new MeatballSprite() };
        protected override IMovable MovementScheme { get; } = new PlayerMovement();
        protected override CreatureType Type { get; } = CreatureType.Meatball;
        public override string Name { get; protected set; } = "Fleischball";
        public override int Life { get; protected set; } = 200;
        public override int MaxLife { get; protected set; } = 200;
        public override int Speed { get; } = 5;
        public override int Attack { get; protected set; } = 70;
        public override int AttackRadius { get; protected set; } = 60;
        public override int SightRadius { get; protected set; } = 100;
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(1);
        public override TimeSpan Recovery { get; set; }
        public override Faction Faction { get; } = Faction.Undead;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }

        protected override Point CreatureSize { get; } = new Point(36);
        protected override Point BoundaryShift { get; } = new Point(-18, -24);
    }
}
