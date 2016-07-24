using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using V3.Input;

namespace V3.Widgets
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class SelectButton : AbstractTextWidget, IClickable
    {
        public List<string> Values { get; } = new List<string>();

        public int SelectedIndex { get; set; }

        public bool IsClicked { get; set; }

        public bool IsEnabled { get; set; } = true;

        private readonly ContentManager mContentManager;

        private Texture2D mTriangle;
        private Rectangle mBoxArrowLeft;
        private Rectangle mBoxArrowRight;

        public SelectButton(ContentManager contentManager) : base(contentManager)
        {
            mContentManager = contentManager;
        }

        public override void Initialize()
        {
            mTriangle = mContentManager.Load<Texture2D>("Menu/arrow_white");
            base.Initialize();
        }

        public void HandleMouseEvent(IMouseEvent mouseEvent)
        {
            if (mouseEvent.MouseButton == MouseButton.Left && mouseEvent.ButtonState == ButtonState.Released)
            {
                if (!mouseEvent.PositionReleased.HasValue)
                    return;
                var rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
                var position = mouseEvent.PositionReleased.Value;
                if (rectangle.Contains(position))
                    IsClicked = true;

                int change;
                if (mBoxArrowLeft.Contains(position))
                    change = -1;
                else if (mBoxArrowRight.Contains(position))
                    change = 1;
                else
                    return;

                SelectedIndex += change;
                if (SelectedIndex < 0)
                    SelectedIndex += Values.Count;
                if (SelectedIndex >= Values.Count)
                    SelectedIndex -= Values.Count;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            UpdateSelection();

            var arrowPadding = 30;
            var arrowLength = 20;
            var arrowY = (int)Position.Y + (int)(Size.Y / 2) - arrowLength / 2;
            mBoxArrowLeft = new Rectangle((int)Position.X + arrowPadding,
                    arrowY, arrowLength, arrowLength);
            mBoxArrowRight = new Rectangle((int)Position.X + (int)Size.X - arrowPadding,
                    arrowY, arrowLength, arrowLength);


            spriteBatch.Draw(mTriangle, mBoxArrowLeft, null, Color.Gray, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(mTriangle, mBoxArrowRight, Color.Gray);

            base.Draw(spriteBatch);
        }

        public override Vector2 GetMinimumSize()
        {
            Vector2 size;
            try
            {
                size = new Vector2(Values.Max(v => Font.MeasureString(v).X), Font.MeasureString(Text).Y);
            }
            catch (ArgumentException)
            {
                // Return whatever.
                size = new Vector2(100, 40);
            }
            size.X += 2 * PaddingX;
            size.Y += 2 * PaddingY;
            return size;
        }

        private void UpdateSelection()
        {
            Text = Values[SelectedIndex];
        }
    }
}
