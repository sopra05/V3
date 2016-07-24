using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using V3.Objects;
using MathHelper = Microsoft.Xna.Framework.MathHelper;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace V3.Camera
{
    /// <summary>
    /// This is the camera class for a map-scrolling camera mode. Does not move over the map.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CameraScrolling : ICamera
    {
        public int MapPixelWidth { get; set; }
        public int MapPixelHeight { get; set; }
        private const int MinOffset = 10;
        private const int CameraSpeed = 10;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;
        private Point mLocation;
        public Matrix Transform { get; set; }
        private int mMaxX;
        private int mMaxY;
        public Vector2 Location {get {return mLocation.ToVector2();} set { mLocation = value.ToPoint(); } }
        public Point ScreenSize => new Point(mGraphicsDeviceManager.GraphicsDevice.Viewport.Width, mGraphicsDeviceManager.GraphicsDevice.Viewport.Height);
        public Rectangle ScreenRectangle => new Rectangle(mLocation, ScreenSize);

        public CameraScrolling(GraphicsDeviceManager graphicsDeviceManager)
        {
            mGraphicsDeviceManager = graphicsDeviceManager;
        }

        public void Update(ICreature player)
        {
            var viewport = mGraphicsDeviceManager.GraphicsDevice.Viewport;

            mMaxX = MapPixelWidth - viewport.Width;
            mMaxY = MapPixelHeight / 2 - viewport.Height;

            var mouse = Mouse.GetState();
            if (mouse.X < MinOffset)
            {
                MoveCameraLeft();
            }

            if (mouse.X > viewport.Width - MinOffset)
            {
                MoveCameraRight();
            }

            if (mouse.Y < MinOffset)
            {
                MoveCameraDown();
            }

            if (mouse.Y > viewport.Height - MinOffset)
            {
                MoveCameraUp();
            }
            Transform = Matrix.CreateTranslation(new Vector3(-mLocation.ToVector2(), 0));
        }

        /// <summary>
        /// Move the Camera to the left. Does not go outside the map.
        /// </summary>
        private void MoveCameraLeft()
        {
            mLocation.X = MathHelper.Clamp(mLocation.X - CameraSpeed, 0, mMaxX);
        }

        /// <summary>
        /// Move the Camera to the right. Does not go outside the map.
        /// </summary>
        private void MoveCameraRight()
        {
            mLocation.X = MathHelper.Clamp(mLocation.X + CameraSpeed, 0, mMaxX);
        }

        /// <summary>
        /// Move the Camera up. Does not go outside the map.
        /// </summary>
        private void MoveCameraUp()
        {
            mLocation.Y = MathHelper.Clamp(mLocation.Y + CameraSpeed, 0, mMaxY);
        }


        /// <summary>
        /// Move the Camera down. Does not go outside the map.
        /// </summary>
        private void MoveCameraDown()
        {
            mLocation.Y = MathHelper.Clamp(mLocation.Y - CameraSpeed, 0, mMaxY);
        }
    }
}