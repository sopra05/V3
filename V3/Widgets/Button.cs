using Castle.Core.Internal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using V3.Input;

namespace V3.Widgets
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class Button : AbstractTextWidget, IClickable, IImageWidget, ISelectable
    {
        public bool IsClicked { get; set; }

        public bool IsEnabled { get; set; } = true;

        public bool IsSelected { get; set; }

        public Color BackgroundColor { private get; set; } = Color.Gray;

        public Texture2D Image { get; set; }

        public string Tooltip { get; set; }
        public string TooltipTitle { private get; set; }

        public Rectangle Rectangle
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y); }
        }

        private readonly ContentManager mContentManager;
        private readonly WidgetFactory mWidgetFactory;

        private Texture2D mRectangle;
        private AchievementBox mTooltipBox;

        public Button(ContentManager contentManager, WidgetFactory widgetFactory) : base(contentManager)
        {
            mContentManager = contentManager;
            mWidgetFactory = widgetFactory;
        }

        public override void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");
            mTooltipBox = mWidgetFactory.CreateAchievementBox();

            base.Initialize();
        }

        public void HandleMouseEvent(IMouseEvent mouseEvent)
        {
            if (!IsEnabled)
                return;
            if (mouseEvent.MouseButton == MouseButton.Left && mouseEvent.ButtonState == ButtonState.Released)
            {
                if (!mouseEvent.PositionReleased.HasValue)
                    return;
                if (Rectangle.Contains(mouseEvent.PositionReleased.Value))
                    IsClicked = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

            if (IsSelected && IsEnabled)
            {
                var borderRectangle = new Rectangle((int)Position.X - 2, (int)Position.Y - 2, (int)Size.X + 4, (int)Size.Y + 4);
                spriteBatch.Draw(mRectangle, borderRectangle, Color.Red);
            }

            if (Image == null)
            {
                spriteBatch.Draw(mRectangle, rectangle, GetBackgroundColor());
            }
            else
            {
                spriteBatch.Draw(Image, rectangle, Color.White);
                if (!IsEnabled)
                {
                    spriteBatch.Draw(mRectangle, rectangle, Color.Black * 0.7f);
                }
            }

            if (IsSelected && !TooltipTitle.IsNullOrEmpty() && !Tooltip.IsNullOrEmpty())
            {
                mTooltipBox.SetText(TooltipTitle, Tooltip);
                mTooltipBox.Size = mTooltipBox.GetMinimumSize();
                mTooltipBox.Position = Position - new Vector2(0, mTooltipBox.Size.Y);
                mTooltipBox.IsEnabled = true;
                if (mTooltipBox.Position.Y < 0)
                    mTooltipBox.Position = Position + new Vector2(0, Size.Y);
                mTooltipBox.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }

        protected override Color GetColor()
        {
            return IsEnabled ? Color : Color.Gray;
        }

        private Color GetBackgroundColor()
        {
            return IsEnabled ? BackgroundColor : Color.LightGray;
        }
    }
}
