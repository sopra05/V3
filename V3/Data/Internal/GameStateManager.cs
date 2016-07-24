using System.Collections.Generic;
using V3.Camera;
using V3.Map;
using V3.Objects;

namespace V3.Data.Internal
{
    /// <summary>
    /// Default implementation of IGameStateManager.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal sealed class GameStateManager : IGameStateManager
    {
        private readonly CameraManager mCameraManager;
        private readonly CreatureFactory mCreatureFactory;
        private readonly IMapManager mMapManager;
        private readonly IObjectsManager mObjectsManager;

        public GameStateManager(CameraManager cameraManager, CreatureFactory creatureFactory,
                IMapManager mapManager, IObjectsManager objectsManager)
        {
            mCameraManager = cameraManager;
            mCreatureFactory = creatureFactory;
            mMapManager = mapManager;
            mObjectsManager = objectsManager;
        }

        /// <summary>
        /// Restores the given game state.
        /// </summary>
        public GameState GetGameState()
        {
            var gameState = new GameState();
            foreach (var obj in mObjectsManager.CreatureList)
            {
                gameState.mCreatures.Add(obj.SaveData());
            }
            gameState.mCameraPosition = mCameraManager.GetCamera().Location;
            return gameState;
        }

        /// <summary>
        /// Restores the given game state.
        /// </summary>
        /// <param name="gameState">the game state to restore</param>
        public void LoadGameState(GameState gameState)
        {
            mObjectsManager.Clear();
            mObjectsManager.ImportMapObjects(mMapManager.GetObjects());
            var creatures = new Dictionary<int, ICreature>();

            // load creatures
            foreach (var creatureData in gameState.mCreatures)
            {
                ICreature creature = mCreatureFactory.CreateCreature(creatureData.Type, creatureData.Id);
                if (creature == null)
                    continue;
                creature.LoadData(creatureData);
                if (creature is Necromancer)
                    mObjectsManager.CreatePlayerCharacter((Necromancer) creature);
                else if (creature is King) // || creature is Prince
                    mObjectsManager.CreateBoss(creature);
                else
                    mObjectsManager.CreateCreature(creature);

                creatures.Add(creature.Id, creature);
            }

            // fix references
            foreach (var creatureData in gameState.mCreatures)
            {
                if (!creatures.ContainsKey(creatureData.Id))
                    continue;
                creatures[creatureData.Id].LoadReferences(creatureData, creatures);
            }

            if (mCameraManager.GetCamera() is CameraScrolling)
                mCameraManager.GetCamera().Location = gameState.mCameraPosition;
        }
    }
}
