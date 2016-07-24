using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    public sealed class SkeletonHorse : AbstractCreature
    {
        public SkeletonHorse(ContentManager contentManager, Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics) : base(contentManager, pathfinder, optionsManager, achievementsAndStatistics)
        {
        }

        private Skeleton mSkeleton;

        protected override ISpriteCreature[] Sprite { get; } = {new SkeletonHorseSprite()};
        protected override IMovable MovementScheme { get; } = new PlayerMovement();
        protected override CreatureType Type { get; } = CreatureType.SkeletonHorse;
        public override string Name { get; protected set; } = "Totenpferd";
        public override int Life { get; protected set; } = 150;
        public override int MaxLife { get; protected set; } = 150;
        public override int Speed { get; } = 20;
        public override int Attack { get; protected set; }
        public override int AttackRadius { get; protected set; }
        public override int SightRadius { get; protected set; }
        public override TimeSpan TotalRecovery { get; } = TimeSpan.FromSeconds(0.5);
        public override TimeSpan Recovery { get; set; }
        public override Faction Faction { get; } = Faction.Undead;
        public override ICreature IsAttacking { get; set; }
        public override IBuilding IsAttackingBuilding { get; set; }

        protected override Point CreatureSize { get; } = new Point(36);
        protected override Point BoundaryShift { get; } = new Point(-18, -24);

        /// <summary>
        /// A skeleton mounts a horse, becoming a skeleton rider.
        /// </summary>
        /// <param name="skeleton">The skeleton which mounts the horse.</param>
        public void Mount(Skeleton skeleton)
        {
            if (IsDead || skeleton.IsDead || mSkeleton != null) return;
            skeleton.Mounted = true;
            mSkeleton = skeleton;
            ChangeEquipment(EquipmentType.Body, new SkeletonRiderSprite());
            Attack = skeleton.Attack;
            AttackRadius = skeleton.AttackRadius * 2;
            // Sight radius is higher because skeleton is mounted.
            SightRadius = skeleton.SightRadius * 2;
            Name = "Skelettreiter";
        }

        protected override void Die()
        {
            if (mSkeleton != null)
            {
                mSkeleton.Position = Position;
                mSkeleton.Mounted = false;
                mSkeleton = null;
                ChangeEquipment(EquipmentType.Body, new SkeletonHorseSprite());
            }
            base.Die();
        }

        public override CreatureData SaveData()
        {
            var data = base.SaveData();
            if (mSkeleton != null)
                data.SkeletonId = mSkeleton.Id;
            return data;
        }

        public override void LoadReferences(CreatureData data, Dictionary<int, ICreature> creatures)
        {
            base.LoadReferences(data, creatures);
            if (creatures.ContainsKey(data.SkeletonId))
            {
                var creature = creatures[data.SkeletonId];
                if (creature is Skeleton)
                    Mount((Skeleton) creature);
            }
        }
    }
}
