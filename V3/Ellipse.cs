using System;
using Microsoft.Xna.Framework;

namespace V3
{
    public struct Ellipse
    {
        private Vector2 Center { get; }
        private float Width { get; }
        private float Height { get; }

        public Rectangle BoundaryRectangle => new Rectangle((Center - new Vector2(Width / 2, Height / 2)).ToPoint(), new Vector2(Width, Height).ToPoint());

        internal Ellipse(Vector2 center, float width, float height)
        {
            Center = center;
            Width = width;
            Height = height;
        }

        internal bool Contains(Vector2 position)
        {
            if (Math.Pow(position.X - Center.X, 2) / Math.Pow(Width/2, 2) +
                Math.Pow(position.Y - Center.Y, 2) / Math.Pow(Height/2, 2) <= 1)
            {
                return true;
            }
            return false;
        }
    }
}
