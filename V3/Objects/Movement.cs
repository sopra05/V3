// ReSharper disable InconsistentNaming
namespace V3.Objects
{
    /// <summary>
    /// Cardinal directions to show where the specific creature is facing.
    /// Because of the isometric viewpoint N(orth) is the upper right.
    /// </summary>
    public enum MovementDirection
    {
        W, NW, N, NO, O, SO, S, SW
    }

    /// <summary>
    /// The basic movement states a creature must hold.
    /// </summary>
    public enum MovementState
    {
        Idle, Moving, Attacking, Dying, Special
    }
}