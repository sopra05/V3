using System.Collections.Generic;
using Microsoft.Xna.Framework;
using V3.Data;
using V3.Map;

namespace V3.Objects.Movement
{
    // TODO: Rename the class. Current name is unfitting. (But what is fitting?)
    /// <summary>
    /// Movement scheme for moving an object with the pathfinder.
    /// </summary>
    public class PlayerMovement : IMovable
    {
        private const int CellHeight = Constants.CellHeight;
        private const int CellWidth = Constants.CellWidth;
        private const float SpeedModifier = 0.25f;

        private List<Vector2> mPath;
        private int mStep;
        private Vector2 mLastMovement;

        public bool IsMoving { get; private set; }

        /// <summary>
        /// Calculates a path without collisions to desired destination.
        /// </summary>
        /// <param name="pathfinder">Pathfinder to use.</param>
        /// <param name="position">Current position in pixel.</param>
        /// <param name="destination">Destination in pixel.</param>
        public void FindPath(Pathfinder pathfinder, Vector2 position, Vector2 destination)
        {
            mStep = 0;
            mPath = pathfinder.FindPath(new Vector2((int)(position.X / CellWidth), (int)(position.Y / CellHeight)),
                new Vector2((int)(destination.X / CellWidth), (int)(destination.Y / CellHeight)));
            IsMoving = mPath.Count > 0;
        }

        /// <summary>
        /// Uses pathfinder to for steady movement to new transition.
        /// </summary>
        /// <param name="currentPosition">Current position in pixel.</param>
        /// <param name="speed">Movement speed of the creature.</param>
        /// <returns>Normalized vector * speed which represents a small step in the direction of desired destination.(</returns>
        public virtual Vector2 GiveNewPosition(Vector2 currentPosition, int speed)
        {
            Vector2 nextPosition = mPath[mStep];
            Vector2 newPosition = nextPosition - currentPosition;
            newPosition.Normalize();
            float distanceToDestination = Vector2.Distance(nextPosition, currentPosition);
            if (distanceToDestination < SpeedModifier * speed)
            {
                if (mStep == mPath.Count - 1)
                {
                    IsMoving = false;
                }
                else
                {
                    mStep++;
                }
            }
            mLastMovement = newPosition;
            return newPosition * SpeedModifier * speed;
        }

        /// <summary>
        /// Calculates the direction the creature is looking when moving.
        /// </summary>
        public MovementDirection GiveMovementDirection()
        {
            //   |\
            //   |α\        α == 22.5°
            //  b|  \ 1     β == 67.5°
            //   |  β\
            //   ──────
            //     a
            const float b = 0.92f;  // b == sin β
            const float a = 0.38f;  // a == sin α
            Vector2 direction = mLastMovement;
            MovementDirection movementDirection;
            if (direction.X < -b)
            {
                movementDirection = MovementDirection.W;
            }
            else if (direction.X > b)
            {
                movementDirection = MovementDirection.O;
            }
            else if (direction.Y > 0)
            {
                if (direction.X < -a)
                {
                    movementDirection = MovementDirection.SW;
                }
                else if (direction.X > a)
                {
                    movementDirection = MovementDirection.SO;
                }
                else
                {
                    movementDirection = MovementDirection.S;
                }
            }
            else
            {
                if (direction.X < -a)
                {
                    movementDirection = MovementDirection.NW;
                }
                else if (direction.X > a)
                {
                    movementDirection = MovementDirection.NO;
                }
                else
                {
                    movementDirection = MovementDirection.N;
                }
            }
            return movementDirection;
        }
        /// <summary>
        /// Save the movement data to a MovementData object.
        /// </summary>
        /// <returns>the MovementData object with the current status</returns>
        public MovementData SaveData()
        {
            var data = new MovementData();
            data.IsMoving = IsMoving;
            if (IsMoving)
            {
                data.Path = mPath;
                data.Step = mStep;
                data.LastMovement = mLastMovement;
            }
            return data;
        }

        /// <summary>
        /// Restore the movement state from the given data.
        /// </summary>
        /// <param name="movementData">the state of the movement to restore</param>
        public void LoadData(MovementData movementData)
        {
            if (movementData == null)
                return;

            IsMoving = movementData.IsMoving;
            if (IsMoving)
            {
                mPath = movementData.Path;
                mStep = movementData.Step;
                mLastMovement = movementData.LastMovement;
            }
        }
    }
}
