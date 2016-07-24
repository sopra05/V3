using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using V3.AI;
using V3.Objects;

namespace V3.Data
{
    /// <summary>
    /// Stores the current state of the game (the data that must be stored in
    /// a save game).  All members should be public and serializable.
    /// </summary>
    [Serializable]
    public sealed class GameState
    {
        public List<CreatureData> mCreatures = new List<CreatureData>();
        public List<Rectangle> mFog = new List<Rectangle>();
        public Vector2 mCameraPosition;
        public AiState mAiState = AiState.Idle;
    }

    public enum CreatureType
    {
        FemalePeasant,
        King,
        KingsGuard,
        Knight,
        MalePeasant,
        Meatball,
        Necromancer,
        Prince,
        Skeleton,
        SkeletonHorse,
        Zombie
    }

    public sealed class CreatureData
    {
        public CreatureType Type { get; set; }
        public int Id { get; set; }
        public int Life { get; set; }
        public int MaxLife { get; set; }
        public int Attack { get; set; }
        public TimeSpan Recovery { get; set; }
        public bool IsUpgraded { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public MovementDirection MovementDirection { get; set; }
        public MovementState MovementState { get; set; }
        public MovementData MovementData { get; set; }
        public int IsAttackingId { get; set; }
        public bool Mounted { get; set; }
        public int SkeletonId { get; set; }
        // IsAttackingBuilding
    }

    public sealed class MovementData
    {
        public List<Vector2> Path { get; set; }
        public int Step { get; set; }
        public Vector2 LastMovement { get; set; }
        public bool IsMoving { get; set; }
    }
}
