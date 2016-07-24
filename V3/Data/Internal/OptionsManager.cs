using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Ninject;

namespace V3.Data.Internal
{
    /// <summary>
    /// Default implementation of IOptionsManager.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class OptionsManager : IOptionsManager, IInitializable
    {
        /// <summary>
        /// The current options.
        /// </summary>
        public Options Options { get; private set; }

        private readonly IPathManager mPathManager;
        private readonly XmlSerializer mSerializer = new XmlSerializer(typeof(Options));

        /// <summary>
        /// Creates a new OptionsManager.
        /// </summary>
        public OptionsManager(IPathManager pathManager)
        {
            mPathManager = pathManager;
        }

        public void Initialize()
        {
            Options = LoadOptions();
        }

        /// <summary>
        /// Saves the current options to the hard disk.
        /// </summary>
        public void SaveOptions()
        {
            var stream = new FileStream(mPathManager.OptionsFile, FileMode.Create, FileAccess.Write);
            mSerializer.Serialize(stream, Options);
            stream.Close();
        }

        private Options LoadOptions()
        {
            if (!File.Exists(mPathManager.OptionsFile))
                return new Options();
            var stream = new FileStream(mPathManager.OptionsFile, FileMode.Open, FileAccess.Read);
            try
            {
                return (Options)mSerializer.Deserialize(stream);
            }
            catch (SerializationException)
            {
                // ignore so far
            }
            finally
            {
                stream.Close();
            }
            return new Options();
        }
    }
}
