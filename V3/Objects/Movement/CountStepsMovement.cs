using Microsoft.Xna.Framework;

namespace V3.Objects.Movement
{
    public class CountStepsMovement : PlayerMovement
    {
        public float WalkedPixels { get; private set; }

        public override Vector2 GiveNewPosition(Vector2 currentPosition, int speed)
        {
            var movedDistance = base.GiveNewPosition(currentPosition, speed);
            WalkedPixels += Vector2.Distance(currentPosition, currentPosition + movedDistance);
            return movedDistance;
        }
    }
}