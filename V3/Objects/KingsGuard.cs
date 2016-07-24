using System;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    public sealed class KingsGuard : AbstractCreature
    {
        public override string Name { get; protected set; } = "Königliche Garde";
        public override int Life { get; protected set; } = 200;
        public override int MaxLife { get; protected set; } = 200;
        public override int Speed { get; } = 7;
        public override int Attack { get; protected set; } = 25;
        public override int AttackRadius { get; protected set; } = 48;
        public override int SightRadius { get; protected set; } = 200;
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(0.5);
        public override TimeSpan Recovery { get; set; }
        protected override ISpriteCreature[] Sprite { get; } = { new PlateSprite(), new HeadPlateSprite(), new LongswordSprite(), new ShieldSprite() };
        protected override IMovable MovementScheme { get; } = new PlayerMovement();
        protected override CreatureType Type { get; } = CreatureType.KingsGuard;
        public override Faction Faction { get; } = Faction.Kingdom;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }

        public KingsGuard(ContentManager contentManager,
            Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics)
            : base(contentManager, pathfinder, optionsManager,achievementsAndStatistics)
        {
        }

        public void MakeFemale()
        {
            ChangeEquipment(EquipmentType.Body, new PlateFemaleSprite());
            ChangeEquipment(EquipmentType.Head, new HeadPlateFemaleSprite());
            ChangeEquipment(EquipmentType.Weapon, new LongswordFemaleSprite());
            ChangeEquipment(EquipmentType.Offhand, new ShieldFemaleSprite());
        }

        public void RemoveArmor()
        {
            if (Sprite[(int)EquipmentType.Body] is PlateSprite)
            {
                ChangeEquipment(EquipmentType.Body, new NudeSprite());
            }
            else if (Sprite[(int)EquipmentType.Body] is PlateFemaleSprite)
            {
                ChangeEquipment(EquipmentType.Body, new NudeFemaleSprite());
            }
            else if (Sprite[(int)EquipmentType.Body] is NudeSprite)
            {
                ChangeEquipment(EquipmentType.Body, new PlateSprite());
            }
            else if (Sprite[(int)EquipmentType.Body] is NudeFemaleSprite)
            {
                ChangeEquipment(EquipmentType.Body, new PlateFemaleSprite());
            }
        }
    }
}
