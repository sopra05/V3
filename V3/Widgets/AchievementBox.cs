using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Widgets
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class AchievementBox : AbstractTextWidget
    {
        public bool IsEnabled { private get; set; }
        private Color BackgroundColor { get; } = Color.Gray;

        private readonly ContentManager mContentManager;
        private readonly WidgetFactory mWidgetFactory;
        private SpriteFont mTitleFont;
        private SpriteFont mDescriptionFont;

        private Label mButtonTitle;
        private Label mButtonDescription;

        private Texture2D mRectangle;

        public AchievementBox(ContentManager contentManager, WidgetFactory widgetFactory) : base(contentManager)
        {
            mContentManager = contentManager;
            mWidgetFactory = widgetFactory;
        }

        public override void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");
            mTitleFont = mContentManager.Load<SpriteFont>("Fonts/MenuFont");
            mDescriptionFont = mContentManager.Load<SpriteFont>("Fonts/UnitFont");

            mButtonTitle = mWidgetFactory.CreateLabel("");
            mButtonTitle.PaddingY = 0;
            mButtonTitle.PaddingX = 10;
            mButtonTitle.HorizontalOrientation = HorizontalOrientation.Left;
            mButtonDescription = mWidgetFactory.CreateLabel("");
            mButtonDescription.PaddingX = 10;
            mButtonDescription.PaddingY = 0;
            mButtonDescription.HorizontalOrientation = HorizontalOrientation.Left;

            base.Initialize();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            
            if (IsEnabled)
            {
                var borderRectangle = new Rectangle((int)Position.X - 2, (int)Position.Y - 2, (int)Size.X + 4, (int)Size.Y + 4);
                spriteBatch.Draw(mRectangle, borderRectangle, Color.Gray);
            }

            mButtonTitle.Color = IsEnabled ? Color.Black : Color.Gray;
            mButtonDescription.Color = IsEnabled ? Color.Black : Color.Gray;

            spriteBatch.Draw(mRectangle, rectangle, GetBackgroundColor());
            mButtonTitle.Position = Position + new Vector2(0, 10);
            mButtonDescription.Position = mButtonTitle.Position + new Vector2(0, mButtonDescription.Size.Y);
            mButtonDescription.Draw(spriteBatch);
            mButtonTitle.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public override Vector2 GetMinimumSize()
        {
            var titleSize = mButtonTitle.GetMinimumSize();
            var descriptionSize = mButtonDescription.GetMinimumSize();

            return new Vector2(MathHelper.Max(titleSize.X, descriptionSize.X), titleSize.Y + descriptionSize.Y + 20);
        }

        private Color GetBackgroundColor()
        {
            return IsEnabled ? BackgroundColor : Color.LightGray;
        }

        public void SetText(string title, string description)
        {
            mButtonTitle.Text = title;
            mButtonDescription.Text = description;
            mButtonTitle.Size = mButtonTitle.GetMinimumSize();
            mButtonDescription.Size = mButtonDescription.GetMinimumSize();
            mButtonTitle.Font = mTitleFont;
            mButtonDescription.Font = mDescriptionFont;
        }
    }
}
