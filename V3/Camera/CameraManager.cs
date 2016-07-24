using Microsoft.Xna.Framework;
using System.Collections.Generic;
using V3.Data;
using V3.Objects;

namespace V3.Camera
{
    /// <summary>
    /// Stores and provides access to the possible cameras.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class CameraManager
    {
        private readonly IOptionsManager mOptionsManager;
        private readonly CameraCentered mCameraCentered;
        private readonly CameraScrolling mCameraScrolling;

        /// <summary>
        /// Creates a new CameraManager.
        /// </summary>
        public CameraManager(CameraCentered cameraCentered,
                CameraScrolling cameraScrolling, IOptionsManager optionsManager)
        {
            mCameraCentered = cameraCentered;
            mCameraScrolling = cameraScrolling;
            mOptionsManager = optionsManager;
        }

        /// <summary>
        /// Initializes the cameras with the given map data.
        /// </summary>
        public void Initialize(Point mapPixelSize)
        {
            // TODO: Warum *2? Irgendwas stimmt bei der Interpretation hier nicht.
            var mapPixelHeight = mapPixelSize.Y * 2;
            var mapPixelWidth = mapPixelSize.X;
            var cameras = new List<ICamera> { mCameraCentered, mCameraScrolling };
            foreach (var camera in cameras)
            {
                camera.MapPixelHeight = mapPixelHeight;
                camera.MapPixelWidth = mapPixelWidth;
            }
        }

        /// <summary>
        /// Updates the cameras.
        /// </summary>
        public void Update(ICreature creature)
        {
            GetCamera().Update(creature);
            if (mOptionsManager.Options.CameraType != CameraType.Scrolling)
                mCameraScrolling.Location = GetCamera().Location;
        }

        /// <summary>
        /// Returns the currently selected camera.
        /// </summary>
        public ICamera GetCamera()
        {
            switch (mOptionsManager.Options.CameraType)
            {
                case CameraType.Centered:
                    return mCameraCentered;
                case CameraType.Scrolling:
                    return mCameraScrolling;
                default:
                    return mCameraScrolling;
            }
        }
    }
}
