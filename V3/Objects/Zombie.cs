using System;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    /// <summary>
    /// A simple zombie controlled by the necromancer.
    /// </summary>
    public sealed class Zombie : AbstractCreature
    {
        public override string Name { get; protected set; } = "Zombie";
        public override int Life { get; protected set; } = 150;
        public override int MaxLife { get; protected set; } = 150;
        public override int Speed { get; } = 5;
        public override int Attack { get; protected set; } = 8;
        public override int AttackRadius { get; protected set; } = 48;
        public override int SightRadius { get; protected set; } = 150;
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(1);
        public override TimeSpan Recovery { get; set; }
        protected override ISpriteCreature[] Sprite { get; } = {new ZombieSprite()};
        protected override IMovable MovementScheme { get; } = new PlayerMovement();
        protected override CreatureType Type { get; } = CreatureType.Zombie;
        public override Faction Faction { get; } = Faction.Undead;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }

        public Zombie(ContentManager contentManager,
            Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics)
            : base(contentManager, pathfinder, optionsManager,achievementsAndStatistics)
        {
        }

        public override void LoadData(CreatureData data)
        {
            base.LoadData(data);

            if (IsUpgraded)
                ChangeEquipment(EquipmentType.Body, new ZombieWithClubSprite());
        }
    }
}
