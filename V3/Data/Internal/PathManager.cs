using System;
using System.IO;

namespace V3.Data.Internal {
    /// <summary>
    /// Default implementation of IPathManager.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class PathManager : IPathManager
    {
        /// <summary>
        /// The base directory for persistent application data.
        /// </summary>
        public string AppDirectory { get; }

        /// <summary>
        /// The file to store the options in.
        /// </summary>
        public string OptionsFile { get; }

        /// <summary>
        /// The directory for save games.
        /// </summary>
        public string SaveGameDirectory { get; }

        /// <summary>
        /// Creates a new path manager and initializes the paths, but does not
        /// create the directories if they don’t already exist.
        /// </summary>
        public PathManager()
        {
            var localAppDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            AppDirectory = $"{localAppDir}/V3";
            SaveGameDirectory = $"{AppDirectory}/SaveGames";
            OptionsFile = $"{AppDirectory}/Options.xml";
        }

        /// <summary>
        /// Creates the application directories that do not already exist.
        /// </summary>
        public void CreateMissingDirectories()
        {
            string[] directories = { AppDirectory, SaveGameDirectory };
            foreach (var directory in directories)
            {
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
            }
        }
    }
}
