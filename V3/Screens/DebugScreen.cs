using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using V3.Data;
using V3.Input;
using V3.Objects;
using V3.Widgets;

namespace V3.Screens
{
    /// <summary>
    /// A special screen that counts and show the number of frames per
    /// second (if debug is enabled).  This screen is not added to the
    /// screen stack, but is always drawn and updated by the screen manager.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DebugScreen : AbstractScreen, IInitializable
    {
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;
        private readonly IObjectsManager mObjectsManager;
        private readonly IOptionsManager mOptionsManager;
        private readonly WidgetFactory mWidgetFactory;
        private FpsCounter mFpsCounter;
        private Label mFpsLabel;
        private Label mUnitCountLabel;
        private int mUnitCount;

        /// <summary>
        /// Creates a new debug screen.
        /// </summary>
        public DebugScreen(GraphicsDeviceManager graphicsDeviceManager,
            IObjectsManager objectsManager, IOptionsManager optionsManager,
            WidgetFactory widgetFactory) : base(true, true)
        {
            mGraphicsDeviceManager = graphicsDeviceManager;
            mObjectsManager = objectsManager;
            mOptionsManager = optionsManager;
            mWidgetFactory = widgetFactory;
        }

        public void Initialize()
        {
            mFpsCounter = new FpsCounter();

            mFpsLabel = mWidgetFactory.CreateLabel("");
            mFpsLabel.PaddingX = 10;
            mFpsLabel.PaddingY = 0;
            mFpsLabel.HorizontalOrientation = HorizontalOrientation.Left;
            mFpsLabel.Color = Color.Red;

            mUnitCountLabel = mWidgetFactory.CreateLabel("");
            mUnitCountLabel.PaddingX = 10;
            mUnitCountLabel.PaddingY = 0;
            mUnitCountLabel.HorizontalOrientation = HorizontalOrientation.Left;
            mUnitCountLabel.Color = Color.Red;
        }

        public override bool HandleMouseEvent(IMouseEvent mouseEvent)
        {
            return false;
        }

        public override void Update(GameTime gameTime)
        {
            mFpsCounter.Update(gameTime);
            mUnitCount = mObjectsManager.CreatureList?.Count ?? 0;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            mFpsCounter.AddFrame();

            if (mOptionsManager.Options.DebugMode == DebugMode.Off)
                return;

            var viewport = mGraphicsDeviceManager.GraphicsDevice.Viewport;

            mFpsLabel.Text = $"FPS: {mFpsCounter.Fps} " + (gameTime.IsRunningSlowly ? "!" : "");
            mFpsLabel.Size = mFpsLabel.GetMinimumSize();
            mFpsLabel.Position = new Vector2(0, viewport.Height - mFpsLabel.Size.Y);

            mUnitCountLabel.Text = $"# units: {mUnitCount}";
            mUnitCountLabel.Size = mUnitCountLabel.GetMinimumSize();
            mUnitCountLabel.Position = mFpsLabel.Position - new Vector2(0, mFpsLabel.Size.Y);

            spriteBatch.Begin();
            mFpsLabel.Draw(spriteBatch);
            mUnitCountLabel.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
