using Microsoft.Xna.Framework;
using V3.Objects;

namespace V3.Camera
{
    /// <summary>
    /// This is the Camera Class for a player-centered camera mode. Does not go outside the map.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CameraCentered : ICamera
    {
        public int MapPixelWidth { get; set; }
        public int MapPixelHeight { get; set; }
        private int mMaxX;
        private int mMaxY;
        public Matrix Transform { get; set; }
        private Point mCamCenter;
        public Vector2 Location { get { return mCamCenter.ToVector2(); } set { mCamCenter = value.ToPoint(); } }
        public Point ScreenSize => new Point(mGraphicsDeviceManager.GraphicsDevice.Viewport.Width, mGraphicsDeviceManager.GraphicsDevice.Viewport.Height);
        public Rectangle ScreenRectangle => new Rectangle(mCamCenter, ScreenSize);

        private readonly GraphicsDeviceManager mGraphicsDeviceManager;

        public CameraCentered(GraphicsDeviceManager graphicsDeviceManager)
        {
            mGraphicsDeviceManager = graphicsDeviceManager;
        }

        private int ScreenWidth()
        {
            return mGraphicsDeviceManager.GraphicsDevice.Viewport.Width; 
        }

        private int ScreenHeight()
        {
            return mGraphicsDeviceManager.GraphicsDevice.Viewport.Height;
        }

        /// <summary>
        /// Updates the position of the camera related to the player's position.
        /// </summary>
        public void Update(ICreature player)
        {
            /*The center of the camera is by default in the upper left corner. To get the player in center, 
            simply substract the half of the screen values. Does not go outside the map.
            */
            mMaxX = MapPixelWidth - ScreenWidth();
            mMaxY = MapPixelHeight / 2 - ScreenHeight();

            mCamCenter = new Point(MathHelper.Clamp((int)(player.Position.X - ScreenWidth() * 0.5f), 0, mMaxX),
                                     MathHelper.Clamp((int)(player.Position.Y - ScreenHeight() * 0.5f), 0, mMaxY));

            //Transform matrix for the camera to make sure it is moving.
            Transform = Matrix.CreateTranslation(new Vector3(-mCamCenter.ToVector2(), 0));
        }
    }
}