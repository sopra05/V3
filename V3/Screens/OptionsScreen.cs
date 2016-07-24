using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using V3.Camera;
using V3.Data;
using V3.Input;
using V3.Widgets;

namespace V3.Screens
{
    /// <summary>
    /// The screen for the options menu.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class OptionsScreen : AbstractScreen, IInitializable
    {
        private readonly ContentManager mContentManager;
        private readonly IMenu mMenu;
        private readonly IOptionsManager mOptionsManager;
        private readonly MenuActions mMenuActions;
        private readonly WidgetFactory mWidgetFactory;

        private Texture2D mRectangle;
        private Button mButtonBack;
        private SelectButton mButtonSize;
        private SelectButton mButtonFullscreen;
        private SelectButton mButtonCamera;
        private SelectButton mButtonDebug;
        private SelectButton mButtonMute;
        private SelectButton mButtonVolume;
        private Button mButtonApply;

        /// <summary>
        /// Creates a new options screen.
        /// </summary>
        public OptionsScreen(ContentManager contentManager, FormMenu menu,
                IOptionsManager optionsManager, MenuActions menuActions,
                WidgetFactory widgetFactory)
            : base(false, true)
        {
            mContentManager = contentManager;
            mMenu = menu;
            mOptionsManager = optionsManager;
            mMenuActions = menuActions;
            mWidgetFactory = widgetFactory;
        }

        public void Initialize()
        {
            mRectangle = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");

            mButtonBack = mWidgetFactory.CreateButton("Zurück");
            mButtonApply = mWidgetFactory.CreateButton("Bestätigen");
            mButtonSize = mWidgetFactory.CreateSelectButton();
            foreach (var resolution in Options.Resolutions)
            {
                mButtonSize.Values.Add(GetResolutionString(resolution));
            }
            mButtonSize.SelectedIndex = Options.Resolutions.IndexOf(mOptionsManager.Options.Resolution);
            mButtonFullscreen = mWidgetFactory.CreateSelectButton(new[] { "aus", "an" });
            mButtonFullscreen.SelectedIndex = mOptionsManager.Options.IsFullScreen ? 1 : 0;
            mButtonCamera = mWidgetFactory.CreateSelectButton();
            Options.CameraTypes.ForEach(t => mButtonCamera.Values.Add(GetCameraTypeString(t)));
            mButtonCamera.SelectedIndex = Options.CameraTypes.IndexOf(mOptionsManager.Options.CameraType);
            mButtonDebug = mWidgetFactory.CreateSelectButton();
            Options.DebugModes.ForEach(m => mButtonDebug.Values.Add(GetDebugModeString(m)));
            mButtonDebug.SelectedIndex = Options.DebugModes.IndexOf(mOptionsManager.Options.DebugMode);
            mButtonMute = mWidgetFactory.CreateSelectButton(new[] { "aus", "an" });
            mButtonMute.SelectedIndex = mOptionsManager.Options.IsMuted ? 0 : 1;
            mButtonVolume = mWidgetFactory.CreateSelectButton();
            foreach (var volume in Options.Volumes)
            {
                mButtonVolume.Values.Add(GetVolumeString(volume));
            }
            mButtonVolume.SelectedIndex = Options.Volumes.IndexOf(mOptionsManager.Options.Volume);

            mMenu.Widgets.Add(mWidgetFactory.CreateEmptyWidget());
            mMenu.Widgets.Add(mButtonBack);
            mMenu.Widgets.Add(mWidgetFactory.CreateLabel("Auflösung"));
            mMenu.Widgets.Add(mButtonSize);
            mMenu.Widgets.Add(mWidgetFactory.CreateLabel("Fullscreen"));
            mMenu.Widgets.Add(mButtonFullscreen);
            mMenu.Widgets.Add(mWidgetFactory.CreateLabel("Kamera"));
            mMenu.Widgets.Add(mButtonCamera);
            mMenu.Widgets.Add(mWidgetFactory.CreateLabel("Debug"));
            mMenu.Widgets.Add(mButtonDebug);
            mMenu.Widgets.Add(mWidgetFactory.CreateLabel("Sound"));
            mMenu.Widgets.Add(mButtonMute);
            mMenu.Widgets.Add(mWidgetFactory.CreateLabel("Lautstärke"));
            mMenu.Widgets.Add(mButtonVolume);
            mMenu.Widgets.Add(mWidgetFactory.CreateEmptyWidget());
            mMenu.Widgets.Add(mButtonApply);
        }

        public override bool HandleKeyEvent(IKeyEvent keyEvent)
        {
            if (keyEvent.KeyState == KeyState.Down && keyEvent.Key == Keys.Escape)
                mMenuActions.Close(this);

            return true;
        }

        public override bool HandleMouseEvent(IMouseEvent mouseEvent)
        {
            mMenu.HandleMouseEvent(mouseEvent);
            return true;
        }

        /// <summary>
        /// Draws this object using the given sprite batch.
        /// </summary>
        /// <param name="gameTime">a snapshot of the game time</param>
        /// <param name="spriteBatch">the sprite batch to use for drawing
        /// this object</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var backgroundRectangle = new Rectangle((int)mMenu.Position.X,
                    (int)mMenu.Position.Y, (int)mMenu.Size.X, (int)mMenu.Size.Y);
            backgroundRectangle.X -= 30;
            backgroundRectangle.Y -= 30;
            backgroundRectangle.Width += 60;
            backgroundRectangle.Height += 60;

            spriteBatch.Begin();
            spriteBatch.Draw(mRectangle, backgroundRectangle, Color.LightGray);
            spriteBatch.End();

            mMenu.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (mButtonBack.IsClicked)
            {
                mMenuActions.Close(this);
            }
            else if (mButtonApply.IsClicked)
            {
                UpdateOptions();
                mMenuActions.SaveOptions();
                mMenuActions.ApplyOptions();
            }

            mMenu.Update();
        }

        private void UpdateOptions()
        {
            mOptionsManager.Options.IsFullScreen = mButtonFullscreen.SelectedIndex != 0;
            mOptionsManager.Options.DebugMode = Options.DebugModes[mButtonDebug.SelectedIndex];
            mOptionsManager.Options.Resolution = Options.Resolutions[mButtonSize.SelectedIndex];
            mOptionsManager.Options.CameraType = Options.CameraTypes[mButtonCamera.SelectedIndex];
            mOptionsManager.Options.IsMuted = mButtonMute.SelectedIndex == 0;
            mOptionsManager.Options.Volume = Options.Volumes[mButtonVolume.SelectedIndex];
        }

        private static string GetCameraTypeString(CameraType cameraType)
        {
            switch (cameraType)
            {
                case CameraType.Centered:
                    return "Zentriert";
                case CameraType.Scrolling:
                    return "Schiebend";
                default:
                    return "Unkown";
            }
        }

        private static string GetDebugModeString(DebugMode debugMode)
        {
            switch (debugMode)
            {
                case DebugMode.Off:
                    return "Aus";
                case DebugMode.Fps:
                    return "FPS Zähler";
                case DebugMode.Full:
                    return "An";
                default:
                    return "Unknown";
            }
        }

        private static string GetResolutionString(Point resolution)
        {
            return $"{resolution.X}x{resolution.Y}";
        }

        private static string GetVolumeString(int volume)
        {
            return $"{volume} %";
        }
    }
}
