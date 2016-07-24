using System;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    /// <summary>
    /// Class for simple peasants which will be transformed into zombies.
    /// </summary>
    public sealed class MalePeasant : AbstractCreature
    {
        public override string Name { get; protected set; } = "Dorfbewohner";
        public override int Life { get; protected set; } = 24;
        public override int MaxLife { get; protected set; } = 24;
        public override int Speed { get; } = 10;
        public override int Attack { get; protected set; } = 5;
        public override int AttackRadius { get; protected set; } = 0;
        public override int SightRadius { get; protected set; } = 20;
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(0.3);
        public override TimeSpan Recovery { get; set; }
        protected override ISpriteCreature[] Sprite { get; } = {new ClothSprite(), new HeadSprite(), null};
        protected override IMovable MovementScheme { get; } = new PlayerMovement();
        protected override CreatureType Type { get; } = CreatureType.MalePeasant;
        public override Faction Faction { get; } = Faction.Plebs;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }

        public MalePeasant(ContentManager contentManager,
            Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics)
            : base(contentManager, pathfinder, optionsManager, achievementsAndStatistics)
        {
        }

        public void RemoveArmor()
        {
            if (Sprite[0] is NudeSprite)
            {
                ChangeEquipment(EquipmentType.Body, new ClothSprite());
            }
            else if (Sprite[0] is ClothSprite)
            {
                ChangeEquipment(EquipmentType.Body, new NudeSprite());
            }
        }
    }
}
