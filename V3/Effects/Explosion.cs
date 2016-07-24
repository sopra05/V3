using Microsoft.Xna.Framework;

namespace V3.Effects
{
    /// <summary>
    /// A large explosion with sound.
    /// </summary>
    public class Explosion : AbstractEffect
    {
        protected override string TextureFile { get; } = "explosion";
        protected override Point SpriteSize { get; } = new Point(320, 240);
        protected override string SoundFile { get; } = "explosion1";
    }
}