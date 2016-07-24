using Microsoft.Xna.Framework;
using V3.Data;
using V3.Map;

namespace V3.Objects.Movement
{
    public interface IMovable
    {
        /// <summary>
        /// Calculates a path without collisions to desired destination.
        /// </summary>
        /// <param name="pathfinder">Pathfinder to use.</param>
        /// <param name="position">Current position in pixel.</param>
        /// <param name="destination">Destination in pixel.</param>
        void FindPath(Pathfinder pathfinder, Vector2 position, Vector2 destination);

        /// <summary>
        /// Uses pathfinder to for steady movement to new transition.
        /// </summary>
        /// <param name="currentPosition">Current position in pixel.</param>
        /// <param name="speed">Movement speed of the creature.</param>
        /// <returns>Normalized vector * speed which represents a small step in the direction of desired destination.(</returns>
        Vector2 GiveNewPosition(Vector2 currentPosition, int speed);

        /// <summary>
        /// Calculates the direction the creature is looking when moving.
        /// </summary>
        MovementDirection GiveMovementDirection();
        bool IsMoving { get; }

        /// <summary>
        /// Save the movement data to a MovementData object.
        /// </summary>
        /// <returns>the MovementData object with the current status</returns>
        MovementData SaveData();

        /// <summary>
        /// Restore the movement state from the given data.
        /// </summary>
        /// <param name="movementData">the state of the movement to restore</param>
        void LoadData(MovementData movementData);
    }
}
