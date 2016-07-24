namespace V3.Objects.Sprite
{
    public class SkeletonRiderSprite : AbstractSpriteCreature
    {
        protected override string TextureFile { get; } = "skeleton_rider";
        protected override int AttackingFrames { get; } = 5;
        protected override int DyingTextureIndex { get; } = 0;
        protected override int DyingFrames { get; } = 0;
    }
}