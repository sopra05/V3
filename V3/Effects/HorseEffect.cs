using Microsoft.Xna.Framework;

namespace V3.Effects
{
    public sealed class HorseEffect : AbstractEffect
    {
        protected override string TextureFile { get; } = "quake";
        protected override Point SpriteSize { get; } = new Point(256, 128);
        protected override string SoundFile { get; } = "horse";
    }
}
