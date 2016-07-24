using System;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    /// <summary>
    /// A class for the player character: The Necromancer.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class Necromancer : AbstractCreature
    {
        public override string Name { get; protected set; } = "Vagram der Grausame";
        public override int Life { get; protected set; } = 500;
        public override int MaxLife { get; protected set; } = 500;
        public override int Speed { get; } = 12;
        public override int Attack { get; protected set; } = 0;
        public override int AttackRadius { get; protected set; } = 500;
        public override int SightRadius { get; protected set; } = 0;
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(1);
        public override TimeSpan Recovery { get; set; }
        protected override ISpriteCreature[] Sprite { get; } = {new NecromancerSprite(), new StaffSprite()};
        protected override IMovable MovementScheme { get; } = new CountStepsMovement();
        protected override CreatureType Type { get; } = CreatureType.Necromancer;
        public override Faction Faction { get; } = Faction.Undead;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }
        public float WalkedPixels => ((CountStepsMovement) MovementScheme).WalkedPixels;

        public Necromancer(ContentManager contentManager,
            Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics)
            : base(contentManager, pathfinder, optionsManager, achievementsAndStatistics)
        {
        }

        public void ChangeSex()
        {
            ISpriteCreature sprite0;
            ISpriteCreature sprite1;
            if (Sprite[0] is NecromancerSprite)
            {
                sprite0 = new NecromancerFemaleSprite();
                sprite1 = new StaffFemaleSprite();
            }
            else
            {
                sprite0 = new NecromancerSprite();
                sprite1 = new StaffSprite();
            }

            ChangeEquipment(EquipmentType.Body, sprite0);
            if (Sprite.Length >= 2 && Sprite[(int) EquipmentType.Head] != null)
            {
                ChangeEquipment(EquipmentType.Head, sprite1);
            }
        }
    }
}
