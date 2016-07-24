﻿namespace V3.Objects.Sprite
{
    public sealed class ZombieWithClubSprite : AbstractSpriteCreature
    {
        protected override string TextureFile { get; } = "zombie_club";
        protected override int AttackingFrames { get; } = 8;
        protected override int DyingTextureIndex { get; } = 22;
        protected override int SpecialTextureIndex { get; } = 36;
        protected override int SpecialFrames { get; } = 8;
    }
}