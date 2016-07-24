using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Ninject;

namespace V3.Widgets
{
    public abstract class AbstractTextWidget : ITextWidget, IInitializable
    {
        private string mText;

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        public string Text
        {
            get { return mText; }
            set { mText = ReplaceUmlauteWhenOnUnix(value); }
        }

        public float PaddingX { get; set; } = 80;

        public float PaddingY { get; set; } = 5;

        public Color Color { get; set; } = Color.Black;

        public HorizontalOrientation HorizontalOrientation { get; set; } = HorizontalOrientation.Center;

        public SpriteFont Font { get; set; }

        private readonly ContentManager mContentManager;

        public AbstractTextWidget(ContentManager contentManager)
        {
            mContentManager = contentManager;
            Text = "";
        }

        public virtual void Initialize()
        {
            Font = mContentManager.Load<SpriteFont>("Fonts/MenuFont");
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var calculatedSize = Font.MeasureString(Text);
            var position = Position;
            switch (HorizontalOrientation)
            {
                case HorizontalOrientation.Left:
                    position.X += PaddingX;
                    break;
                case HorizontalOrientation.Center:
                    position.X += (Size - calculatedSize).X / 2;
                    break;
                case HorizontalOrientation.Right:
                    position.X += (Size - calculatedSize).X;
                    position.X -= PaddingX;
                    break;
            }
            position.Y += (Size - calculatedSize).Y / 2;
            spriteBatch.DrawString(Font, Text, position, GetColor());
        }

        public virtual Vector2 GetMinimumSize()
        {
            var size = Font.MeasureString(Text);
            size.X += 2 * PaddingX;
            size.Y += 2 * PaddingY;
            return size;
        }

        public bool CheckSelected(Point position)
        {
            var rectangle = new Rectangle((int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y);
            return rectangle.Contains(position);
        }

        protected virtual Color GetColor()
        {
            return Color;
        }

        /// <summary>
        /// Test if execution platform is UNIX and replace german Umlaute
        /// accordingly because a strange ArgumentException is thrown otherwise. 
        /// </summary>
        /// <param name="originalString">The original input string.</param>
        /// <returns></returns>
        private string ReplaceUmlauteWhenOnUnix(string originalString)
        {
            // Taken from <http://mono.wikia.com/wiki/Detecting_the_execution_platform>.
            int p = (int) Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128))  // Running on Unix
            {
                var sb = new StringBuilder();
                foreach (char c in originalString)
                {
                    if (c < 128)
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString();
            }
            // NOT running on Unix.
            return originalString;
        }
    }
}
