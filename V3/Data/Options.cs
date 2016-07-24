using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using V3.Camera;

namespace V3.Data
{
    /// <summary>
    /// The graphics options.
    /// </summary>
    [Serializable]
    public sealed class Options
    {
        /// <summary>
        /// All available screen resolutions.
        /// </summary>
        public static List<Point> Resolutions { get; } = new List<Point>
        {
            new Point(800, 480),
            new Point(800, 600),
            new Point(1024, 768),
            new Point(1280, 800),
            new Point(1280, 1024),
            new Point(1366, 768),
            new Point(1920, 1080)
        };

        /// <summary>
        /// All available camera types.
        /// </summary>
        public static List<CameraType> CameraTypes { get; } =
            Enum.GetValues(typeof (CameraType)).Cast<CameraType>().ToList();

        /// <summary>
        /// All available debug modes.
        /// </summary>
        public static List<DebugMode> DebugModes { get; } =
            Enum.GetValues(typeof (DebugMode)).Cast<DebugMode>().ToList();

        /// <summary>
        /// All available volume settings.
        /// </summary>
        public static List<int> Volumes { get; } = new List<int>()
        {
            10,
            20,
            30,
            40,
            50,
            60,
            70,
            80,
            90,
            100
        };
        private static readonly Point sDefaultResolution = Resolutions[0];
        private static readonly bool sDefaultIsFullScreen = false;
        private static readonly DebugMode sDefaultDebugMode = DebugMode.Off;
        private static readonly CameraType sDefaultCameraType = CameraType.Scrolling;
        private static readonly bool sDefaultIsMuted = false;
        private static readonly int sDefaultVolume = 100;

        /// <summary>
        /// The current screen resolution.
        /// </summary>
        public Point Resolution { get; set; } = sDefaultResolution;

        /// <summary>
        /// True if the game should be run in full screen, otherwise false.
        /// </summary>
        public bool IsFullScreen { get; set; } = sDefaultIsFullScreen;

        /// <summary>
        /// The current debug mode.
        /// </summary>
        public DebugMode DebugMode { get; set; } = sDefaultDebugMode;

        /// <summary>
        /// The current camera type.
        /// </summary>
        public CameraType CameraType { get; set; } = sDefaultCameraType;

        /// <summary>
        /// True if the sound is muted, otherwise false.
        /// </summary>
        public bool IsMuted { get; set; } = sDefaultIsMuted;

        /// <summary>
        /// The volume to use for the sound (if the sound is not muted), range
        /// 0 .. 100.
        /// </summary>
        public int Volume { get; set; } = sDefaultVolume;

        /// <summary>
        /// Returns the effective volume with regard to the mute and volume
        /// settings.
        /// </summary>
        /// <returns>the effective volume that should be used for sound</returns>
        public float GetEffectiveVolume()
        {
            return IsMuted ? 0 : ((float) Volume) / 100;
        }
    }
}
