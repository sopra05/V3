using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using V3.Camera;
using V3.Map;

namespace V3.Objects
{
    /// <summary>
    /// Objects manager for all game objects, be is creatures or buildings or even simple landscape objects.
    /// </summary>
    public interface IObjectsManager
    {
        //***** FOR TESTING PURPOSES!
        List<ICreature> AddToSelectables { get; }
        List<ICreature> CreatureList { get; }
        List<ICreature> UndeadCreatures { get; }
        //List<ICreature> KingdromCreatures { get; }
        //List<ICreature> PlebCreatures { get; }
        //***** NOT FOR TESTING PURPOSES ANYMORE!

        /// <summary>
        /// Gets the current player character. Usually the necromancer.
        /// Do not set directly! Use CreatePlayerCharacter() instead!
        /// </summary>
        ICreature PlayerCharacter { get; } 

        ICreature Boss { get; }

        ICreature Prince { get; }

        Castle Castle { get; }

        /// <summary>
        /// If you load a new map with new objects you need to initialize the objects manager again.
        /// (Or else you have all the current objects on the new map.)
        /// </summary>
        /// <param name="mapManager"></param>
        void Initialize(IMapManager mapManager);

        /// <summary>
        /// Removes all objects from the object manager.
        /// </summary>
        void Clear();

        /// <summary>
        /// Creates the player character. This should be the first thing you do
        /// after you created or initialized the objects manager.
        /// </summary>
        /// <param name="necromancer"></param>
        void CreatePlayerCharacter(Necromancer necromancer);

        /// <summary>
        /// Creates the boss of the level. Game is won if the boss is killed.
        /// </summary>
        /// <param name="boss">Some ICreature to kill for winning.</param>
        void CreateBoss(ICreature boss);

        /// <summary>
        /// Create the prince, a small boss.
        /// </summary>
        /// <param name="prince">Some hard ICreature to kill.</param>
        void CreatePrince(ICreature prince);

        /// <summary>
        /// Creates a creature for the game and inserts it in the appropriate data structures.
        /// </summary>
        /// <param name="creature"></param>
        void CreateCreature(ICreature creature);

        /// <summary>
        /// Removes specified creature from the game.
        /// </summary>
        /// <param name="creature">The creature to be removed.</param>
        void RemoveCreature(ICreature creature);

        /// <summary>
        /// Draws all currently shown objects on the map.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch used.</param>
        /// <param name="camera">Camera for calculating which objects need to be drawn.</param>
        void Draw(SpriteBatch spriteBatch, ICamera camera);

        /// <summary>
        /// Draws a visual representation of the Quadtree. For debugging purposes.
        /// </summary>
        /// <param name="spriteBatch"></param>
        void DrawQuadtree(SpriteBatch spriteBatch);

        /// <summary>
        /// Update the behaviour of all creatures on the map.
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        /// <param name="rightButtonPressed">Did the player press the right mouse button?</param>
        /// <param name="rightButtonPosition">Where is the mouse currently?</param>
        /// <param name="camera">Camera for checking where to do important updates.</param>
        void Update(GameTime gameTime, bool rightButtonPressed, Vector2 rightButtonPosition, ICamera camera);

        /// <summary>
        /// Imports the TextureObjects from the objects map layer for drawing things in the right order.
        /// </summary>
        /// <param name="textureObjects"></param>
        void ImportMapObjects(List<IGameObject> textureObjects);

        /// <summary>
        /// Get all objects in the given rectangles.
        /// </summary>
        /// <param name="rectangle">The rectangle which defines the area of the returnes objects.</param>
        /// <returns>Game objects in the rectangle.</returns>
        List<IGameObject> GetObjectsInRectangle(Rectangle rectangle);

        /// <summary>
        /// Gets all creatures which are in the given ellipse area.
        /// </summary>
        /// <param name="ellipse">To check if creature is in ellipse area.</param>
        /// <returns></returns>
        List<ICreature> GetCreaturesInEllipse(Ellipse ellipse);

        /// <summary>
        /// Playing around with some cheating codes.
        /// </summary>
        void ExposeTheLiving();

        /// <summary>
        /// Checks if a creature is standing in a graveyard area.
        /// </summary>
        /// <param name="creature">Check for this creature.</param>
        /// <returns>True when standing in graveyard area.</returns>
        bool InGraveyardArea(ICreature creature);
    }
}
