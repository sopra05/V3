using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace V3.Data.Internal
{
    /// <summary>
    /// Default implementation if ISaveGameManager.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class SaveGameManager : ISaveGameManager
    {
        private readonly IPathManager mPathManager;

        private const int CurrentVersion = 1;
        private readonly XmlSerializer mFormatter = new XmlSerializer(typeof(SaveGame));

        /// <summary>
        /// Creates a new SaveGameManager. The save game directory must already
        /// be created.
        /// </summary>
        public SaveGameManager(IPathManager pathManager)
        {
            mPathManager = pathManager;
        }

        /// <summary>
        /// Creates and persists a new save game of the given data with the
        /// title.
        /// </summary>
        /// <param name="gameState">the data to store</param>
        public void CreateSaveGame(GameState gameState)
        {
            var saveGame = new SaveGame(DateTime.Now, CurrentVersion, gameState);
            var fileName = GetUniqueFileName();
            var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            mFormatter.Serialize(stream, saveGame);
            stream.Close();
        }

        /// <summary>
        /// Loads all available save games and returns them ordered by the
        /// creation date.
        /// </summary>
        /// <returns>a list of all available save games, orderd by creation
        /// data</returns>
        public List<ISaveGame> GetSaveGames()
        {
            var saveGames = new List<ISaveGame>();
            foreach (var file in Directory.GetFiles(mPathManager.SaveGameDirectory))
            {
                var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
                try
                {
                    var saveGame = (ISaveGame)mFormatter.Deserialize(stream);
                    if (saveGame.Version == CurrentVersion)
                        saveGames.Add(saveGame);
                }
                catch (SerializationException)
                {
                    // ignore so far
                }
                stream.Close();
            }
            saveGames.Sort();
            return saveGames;
        }

        private string GetUniqueFileName()
        {
            var index = 1;
            var fileName = GetFileName(index);
            while (File.Exists(fileName))
            {
                index++;
                fileName = GetFileName(index);
            }
            return fileName;
        }

        private string GetFileName(int index)
        {
            return $"{mPathManager.SaveGameDirectory}/{index:000}.xml";
        }
    }
}
