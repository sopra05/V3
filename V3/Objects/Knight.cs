using System;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    /// <summary>
    /// A knight which fights against the necromancer.
    /// </summary>
    public sealed class Knight : AbstractCreature
    {
        public override string Name { get; protected set; } = "Ritter";
        public override int Life { get; protected set; } = 120;
        public override int MaxLife { get; protected set; } = 120;
        public override int Speed { get; } = 8;
        public override int Attack { get; protected set; } = 15;
        public override int AttackRadius { get; protected set; } = 48;
        public override int SightRadius { get; protected set; } = 200;
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(0.5);
        public override TimeSpan Recovery { get; set; }
        protected override ISpriteCreature[] Sprite { get; } = {new ChainSprite(), new HeadChainSprite(), new ShortswordSprite(), new BucklerSprite()};
        protected override IMovable MovementScheme { get; } = new PlayerMovement();
        protected override CreatureType Type { get; } = CreatureType.Knight;
        public override Faction Faction { get; } = Faction.Kingdom;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }

        public Knight(ContentManager contentManager,
            Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics)
            : base(contentManager, pathfinder, optionsManager, achievementsAndStatistics)
        {
        }

        public void MakeFemale()
        {
            ChangeEquipment(EquipmentType.Body, new ChainFemaleSprite());
            ChangeEquipment(EquipmentType.Head, new HeadChainFemaleSprite());
            ChangeEquipment(EquipmentType.Weapon, new ShortswordFemaleSprite());
            ChangeEquipment(EquipmentType.Offhand, new BucklerFemaleSprite());
        }

        public void RemoveArmor()
        {
            if (Sprite[(int) EquipmentType.Body] is ChainSprite)
            {
                ChangeEquipment(EquipmentType.Body, new NudeSprite());
            }
            else if (Sprite[(int) EquipmentType.Body] is ChainFemaleSprite)
            {
                ChangeEquipment(EquipmentType.Body, new NudeFemaleSprite());
            }
            else if (Sprite[(int)EquipmentType.Body] is NudeSprite)
            {
                ChangeEquipment(EquipmentType.Body, new ChainSprite());
            }
            else if (Sprite[(int)EquipmentType.Body] is NudeFemaleSprite)
            {
                ChangeEquipment(EquipmentType.Body, new ChainFemaleSprite());
            }
        }
    }
}
