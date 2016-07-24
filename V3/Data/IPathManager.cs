namespace V3.Data
{
    /// <summary>
    /// Provides access to the default applications path, i. e. the
    /// directories where save games, achievements and other persistent data
    /// can be stored.
    /// </summary>
    public interface IPathManager
    {
        /// <summary>
        /// The base directory for persistent application data.
        /// </summary>
        string AppDirectory { get; }

        /// <summary>
        /// The directory for save games.
        /// </summary>
        string SaveGameDirectory { get; }

        /// <summary>
        /// The file to store the options in.
        /// </summary>
        string OptionsFile { get;  }

        /// <summary>
        /// Creates the application directories that do not already exist.
        /// </summary>
        void CreateMissingDirectories();
    }
}