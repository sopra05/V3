using System;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    /// <summary>
    /// A skeleton controlled by the necromancer.
    /// </summary>
    public class Skeleton : AbstractCreature
    {
        /// <summary>
        /// If the skeleton is mounted, it is not updated and drawn anymore because
        /// instead the horse sprite is moved.
        /// </summary>
        public bool Mounted { private get; set; }

        public override string Name { get; protected set; } = "Skelett";
        public override int Life { get; protected set; } = 100;
        public override int MaxLife { get; protected set; } = 100;
        public override int Speed { get; } = 10;
        public override int Attack { get; protected set; } = 10;
        public override int AttackRadius { get; protected set; } = 48;
        public override int SightRadius { get; protected set; } = 150;
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(0.5);
        public override TimeSpan Recovery { get; set; }
        protected override ISpriteCreature[] Sprite { get; } = {new SkeletonSprite()};
        protected override IMovable MovementScheme { get; } = new PlayerMovement();
        protected override CreatureType Type { get; } = CreatureType.Skeleton;
        public override Faction Faction { get; } = Faction.Undead;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }

        // ReSharper disable once MemberCanBeProtected.Global
        public Skeleton(ContentManager contentManager, 
            Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics)
            : base(contentManager, pathfinder, optionsManager,achievementsAndStatistics)
        {
        }

        public override IGameObject GetSelf()
        {
            return Mounted ? null : base.GetSelf();
        }

        public override CreatureData SaveData()
        {
            var data = base.SaveData();
            data.Mounted = Mounted;
            return data;
        }

        public override void LoadData(CreatureData data)
        {
            base.LoadData(data);
            Mounted = data.Mounted;

            if (IsUpgraded)
                ChangeEquipment(EquipmentType.Body, new SkeletonArcherSprite());
        }
    }
}
