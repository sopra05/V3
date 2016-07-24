using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Objects.Sprite;

namespace V3.Objects
{
    public sealed class Arrow
    {
        //Drawing an arrow
        private const int SpeedModifier = 10;
        private readonly ArrowSprite mArrow;
        private Vector2 mArrowPosition;
        private Vector2 mArrowGoal;
        private Vector2 mDirection;
        private bool mArrowDraw;
        private readonly MovementDirection mMovementDirection;

        public Arrow(Vector2 start, Vector2 goal)
        {
            mArrowPosition = start;
            mArrowGoal = goal;
            mDirection = goal - start;
            mDirection.Normalize();
            mMovementDirection = GiveMovementDirection(mDirection);
            mArrow = new ArrowSprite();
            mArrowDraw = true;
        }

        public void LoadArrow(ContentManager contentManager)
        {
            mArrow.Load(contentManager);
        }

        public void DrawArrow(SpriteBatch spriteBatch)
        {
            if (mArrowDraw)
                mArrow.Draw(spriteBatch, mArrowPosition, MovementState.Idle, mMovementDirection);
        }

        /// <summary>
        /// The moving for arrow
        /// </summary>
        public void UpdateArrow()
        {
            if (mArrowDraw)
                mArrowPosition += mDirection * SpeedModifier;

            if (Vector2.Distance(mArrowPosition, mArrowGoal) < 1f * SpeedModifier)
                mArrowDraw = false;
        }

        /// <summary>
        /// Calculates the direction the creature is looking when moving.
        /// Method copied from PlayerMovement class.
        /// </summary>
        private MovementDirection GiveMovementDirection(Vector2 direction)
        {
            //   |\
            //   |α\        α == 22.5°
            //  b|  \ 1     β == 67.5°
            //   |  β\
            //   ──────
            //     a
            const float b = 0.92f;  // b == sin β
            const float a = 0.38f;  // a == sin α
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
    }
}
