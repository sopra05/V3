using System.Diagnostics.CodeAnalysis;

namespace V3.Objects.Sprite
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class HeadBaldSprite : AbstractSpriteCreature
    {
        protected override string TextureFile { get; } = "head_bald";
    }
}