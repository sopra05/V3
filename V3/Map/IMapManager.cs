using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using V3.Camera;
using V3.Objects;

namespace V3.Map
{
    /// <summary>
    /// Manager for loading and drawing game maps. Also holds information about map attributes.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public interface IMapManager
    {
        /// <summary>
        /// A list of map areas (rectangle-sized).
        /// </summary>
        List<Area> Areas { get; }

            /// <summary>
        /// Size of the shown map in pixels.
        /// </summary>
        Point SizeInPixel { get; }
        /// <summary>
        /// Size of the map in tiles. (Some tiles are cut off at the edges.)
        /// </summary>
        Point SizeInTiles { get; }
        /// <summary>
        /// Size of a single tile in pixels.
        /// </summary>
        Point TileSize { get; }
        /// <summary>
        /// Number of cells the pathfinding grid consists of.
        /// </summary>
        Point PathfindingGridSize { get; }
        /// <summary>
        /// Size of a single cell of the pathfinding grid in pixels.
        /// </summary>
        Point PathfindingCellSize { get; }
        /// <summary>
        /// File name of the loaded map (without suffix).
        /// </summary>
        string FileName { get; }
        /// <summary>
        /// Efficiently draw the floor layer. Only draw the tiles seen by the camera.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="camera"></param>
        void DrawFloor(SpriteBatch spriteBatch, ICamera camera);
        /// <summary>
        /// Load a map file and create the map layers and pathfinding information.
        /// </summary>
        /// <param name="fileName">Name of the map file (without suffix).</param>
        void Load(string fileName);
        /// <summary>
        /// Returns all objects in the objects layer.
        /// </summary>
        /// <returns>List of all static game objects imported from the map.</returns>
        List<IGameObject> GetObjects();
        /// <summary>
        /// Returns the pathfinding grid for passing to the pathfinder.
        /// </summary>
        /// <returns>A grid used for pathfinding.</returns>
        PathfindingGrid GetPathfindingGrid();
        /// <summary>
        /// Efficiently draw the pathfinding grid. For debugging purposes.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used.</param>
        /// <param name="camera">Current camera for calculating the shown screen.</param>
        void DrawPathfindingGrid(SpriteBatch spriteBatch, ICamera camera);

        /// <summary>
        /// Draws the minimap to specified position.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used.</param>
        /// <param name="position">Where to draw the minimap and which size.</param>
        void DrawMinimap(SpriteBatch spriteBatch, Rectangle position);

        /// <summary>
        /// Automatically creates an initial population from the map data and returns it.
        /// </summary>
        /// <param name="creatureFactory">Factory for creating creatues.</param>
        /// <param name="pathfinder">Pathfinder is used for checking collisions when creating creatures.</param>
        /// <returns>Initial population in a list.</returns>
        List<ICreature> GetPopulation(CreatureFactory creatureFactory, Pathfinder pathfinder);
    }
}