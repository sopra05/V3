using System;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class Prince : AbstractCreature
    {
        public override string Name { get; protected set; } = "Prinz Erhard";
        public override int Life { get; protected set; } = 6000;
        public override int MaxLife { get; protected set; } = 6000;
        public override int Speed { get; } = 10;
        public override int Attack { get; protected set; } = 35;
        public override int AttackRadius { get; protected set; } = 48;
        public override int SightRadius { get; protected set; } = 500;
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(0.5);
        public override TimeSpan Recovery { get; set; }
        protected override ISpriteCreature[] Sprite { get; } = { new PrinceSprite() };
        protected override IMovable MovementScheme { get; } = new PlayerMovement();
        protected override CreatureType Type { get; } = CreatureType.Prince;
        public override Faction Faction { get; } = Faction.Kingdom;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }

        public Prince(ContentManager contentManager, Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics) : base(contentManager, pathfinder, optionsManager, achievementsAndStatistics)
        {
        }
    }
}
