using System.Diagnostics.CodeAnalysis;

namespace V3.Effects
{
    /// <summary>
    /// A medium sized ring of smoke, spreading over some area.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class SmokeMedium : AbstractEffect
    {
        protected override string TextureFile { get; } = "particlefx_04";
    }
}