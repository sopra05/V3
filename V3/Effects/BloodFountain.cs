namespace V3.Effects
{
    /// <summary>
    /// A small fountain of blood.
    /// </summary>
    public sealed class BloodFountain : AbstractEffect
    {
        protected override string TextureFile { get; } = "blood_hit_02";
        protected override string SoundFile { get; } = "impactsplat01";
    }
}