namespace V3.Objects.Sprite
{
    public class SkeletonArcherSprite : AbstractSpriteCreature
    {
        protected override string TextureFile { get; } = "skeleton_archer";
        protected override int AttackingFrames { get; } = 4;
        protected override int AttackingTextureIndex { get; } = 28;
        protected override int DyingTextureIndex { get; } = 22;
    }
}