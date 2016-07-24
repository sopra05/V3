namespace V3.Objects.Sprite
{
    public class SkeletonEliteSprite : AbstractSpriteCreature
    {
        protected override string TextureFile { get; } = "skeleton_elite";
        protected override int AttackingFrames { get; } = 4;
        protected override int DyingTextureIndex { get; } = 22;
    }
}