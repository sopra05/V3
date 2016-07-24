namespace V3.Objects
{
    public interface IBasicCreatureFactory
    {
        MalePeasant CreateMalePeasant();

        FemalePeasant CreateFemalePeasant();

        Necromancer CreateNecromancer();

        Skeleton CreateSkeleton();
        SkeletonElite CreateSkeletonElite();

        Zombie CreateZombie();

        Knight CreateKnight();

        SkeletonHorse CreateSkeletonHorse();

        Meatball CreateMeatball();

        Prince CreatePrince();

        King CreateKing();

        KingsGuard CreateKingsGuard();
    }
}
