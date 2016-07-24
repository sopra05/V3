namespace V3.Objects.Sprite
{
    public sealed class MeatballSprite : AbstractSpriteCreature
    {
        protected override string TextureFile { get; } = "fleischklops";
        protected override int AttackingFrames { get; } = 12;
        protected override int DyingFrames { get; } = 8;
        protected override int AttackingTextureIndex { get; } = 12;
        protected override int DyingTextureIndex { get; } = 24;
    }
}