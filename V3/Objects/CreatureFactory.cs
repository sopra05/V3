using System;
using Microsoft.Xna.Framework;
using V3.Data;

namespace V3.Objects
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CreatureFactory
    {
        private readonly IBasicCreatureFactory mFactory;
        private readonly Random mRnd = new Random();

        public CreatureFactory(IBasicCreatureFactory factory)
        {
            mFactory = factory;
        }

        public ICreature CreateCreature(CreatureType type, int id)
        {
            IdGenerator.SetIdOnce(id);
            switch (type)
            {
                case CreatureType.FemalePeasant:
                    return mFactory.CreateFemalePeasant();
                case CreatureType.King:
                    return mFactory.CreateKing();
                case CreatureType.KingsGuard:
                    return mFactory.CreateKingsGuard();
                case CreatureType.Knight:
                    return mFactory.CreateKnight();
                case CreatureType.MalePeasant:
                    return mFactory.CreateMalePeasant();
                case CreatureType.Meatball:
                    return mFactory.CreateMeatball();
                case CreatureType.Necromancer:
                    return mFactory.CreateNecromancer();
                case CreatureType.Prince:
                    return mFactory.CreatePrince();
                case CreatureType.Skeleton:
                    return mFactory.CreateSkeleton();
                case CreatureType.Zombie:
                    return mFactory.CreateZombie();
                default:
                    IdGenerator.ClearIdOnce();
                    return null;
            }
        }

        public MalePeasant CreateMalePeasant(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreateMalePeasant(), position, movementDirection);
        }

        public FemalePeasant CreateFemalePeasant(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreateFemalePeasant(), position, movementDirection);
        }

        public Necromancer CreateNecromancer(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreateNecromancer(), position, movementDirection);
        }

        public Skeleton CreateSkeleton(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreateSkeleton(), position, movementDirection);
        }

        public SkeletonElite CreateSkeletonElite(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreateSkeletonElite(), position, movementDirection);
        }

        public Zombie CreateZombie(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreateZombie(), position, movementDirection);
        }

        public Knight CreateKnight(Vector2 position, MovementDirection movementDirection)
        {
            Knight knight = CreateCreature(mFactory.CreateKnight(), position, movementDirection);
            if (mRnd.Next(3) == 0)
            {
                knight.MakeFemale();
            }
            return knight;
        }

        public KingsGuard CreateKingsGuard(Vector2 position, MovementDirection movementDirection)
        {
            KingsGuard guard = CreateCreature(mFactory.CreateKingsGuard(), position, movementDirection);
            if (mRnd.Next(3) == 0)
            {
                guard.MakeFemale();
            }
            return guard;
        }

        public SkeletonHorse CreateSkeletonHorse(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreateSkeletonHorse(), position, movementDirection);
        }

        public Meatball CreateMeatball(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreateMeatball(), position, movementDirection);
        }

        public Prince CreatePrince(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreatePrince(), position, movementDirection);
        }

        public King CreateKing(Vector2 position, MovementDirection movementDirection)
        {
            return CreateCreature(mFactory.CreateKing(), position, movementDirection);
        }

        private T CreateCreature<T>(T creature, Vector2 position, MovementDirection movementDirection) where T: ICreature
        {
            creature.Position = position;
            creature.InitialPosition = position;
            creature.MovementDirection = movementDirection;
            return creature;
        }
    }
}
