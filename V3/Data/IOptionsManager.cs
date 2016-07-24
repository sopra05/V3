namespace V3.Data
{
    /// <summary>
    /// Handles the storing and loading of game options to the hard disk.
    /// </summary>
    public interface IOptionsManager
    {
        /// <summary>
        /// The current options.
        /// </summary>
        Options Options { get; }

        /// <summary>
        /// Saves the current options to the hard disk.
        /// </summary>
        void SaveOptions();
    }
}
