using System;

namespace V3.Objects
{
    /// <summary>
    /// Static helper class that generates unique IDs for game objects.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class IdGenerator
    {
        private static int sCurrentId;

        private static int? sIdOnce;

        private IdGenerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get an ID for a new game object.
        /// </summary>
        /// <returns>the ID to use for a new game object</returns>
        public static int GetNextId()
        {
            int id;
            if (sIdOnce.HasValue)
            {
                id = sIdOnce.Value;
                ClearIdOnce();
            }
            else
            {
                id = sCurrentId;
                sCurrentId++;
            }

            return id;
        }

        /// <summary>
        /// Sets the ID to use only for the next object that is created.
        /// </summary>
        /// <param name="id">the id for the next object</param>
        public static void SetIdOnce(int id)
        {
            sIdOnce = id;
        }

        /// <summary>
        /// Clear the ID stored by SetIdOnce that is used only for the
        /// next object.
        /// </summary>
        public static void ClearIdOnce()
        {
            sIdOnce = null;
        }
    }
}
