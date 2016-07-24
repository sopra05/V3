namespace V3.Effects
{
    /// <summary>
    /// A round explosion of blood.
    /// </summary>
    public sealed class BloodExplosion : AbstractEffect
    {
        protected override string TextureFile { get; } = "blood_hit_08";
        protected override string SoundFile { get; } = "impactsplat01";
    }
}