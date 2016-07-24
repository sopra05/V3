namespace V3.Objects.Sprite
{
    public sealed class SkeletonSprite : AbstractSpriteCreature
    {
        protected override string TextureFile { get; } = "skeleton";
        protected override int AttackingFrames { get; } = 4;
        protected override int DyingTextureIndex { get; } = 22;
    }
}