using System;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class King : AbstractCreature
    {
        public override string Name { get; protected set; } = "König Harry";
        public override int Life { get; protected set; } = 10000;
        public override int MaxLife { get; protected set; } = 10000;
        public override int Speed { get; } = 10;
        public override int Attack { get; protected set; } = 45;
        public override int AttackRadius { get; protected set; } = 48;
        public override int SightRadius { get; protected set; } = 500;
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(0.8);
        public override TimeSpan Recovery { get; set; }
        protected override ISpriteCreature[] Sprite { get; } = { new KingSprite() };
        protected override IMovable MovementScheme { get; } = new PlayerMovement();
        protected override CreatureType Type { get; } = CreatureType.King;
        public override Faction Faction { get; } = Faction.Kingdom;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }

        public King(ContentManager contentManager, Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics) : base(contentManager, pathfinder, optionsManager, achievementsAndStatistics)
        {
        }
    }
}
