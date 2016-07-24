using System;
using System.Collections.Generic;

namespace V3.Map
{
    /// <summary>
    /// Class for holding information needed of Tilesets. Needed to draw the map.
    /// </summary>
    public sealed class Tileset
    {
        private const int CellHeight = Constants.CellHeight;
        private const int CellWidth = Constants.CellWidth;

        /// <summary>
        /// Name of the tileset, often the filename.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Tile width of each tile in pixel.
        /// </summary>
        public int TileWidth { get; }
        /// <summary>
        /// Tile height of each tile in pixel.
        /// </summary>
        public int TileHeight { get; }

        /// <summary>
        /// Columns of tiles of the tileset image.
        /// </summary>
        public int Columns { get; private set; }
        /// <summary>
        /// When tile is drawn, is there an offset needed on the X axis for correct display.
        /// </summary>
        public int OffsetX { get; private set; }
        /// <summary>
        ///         
        /// When tile is drawn, is there an offset needed on the Y axis for correct display.
        /// </summary>
        public int OffsetY { get; private set; }
        /// <summary>
        /// Each tile of the tileset, represented by an integer, can hold collision data consisting of a two dimensional
        /// array of boolean values. Its size is described by CollisionWidth and CollisionHeight.
        /// </summary>
        public Dictionary<int, bool[,]> TileCollisions { get; }
        public int CollisionWidth => TileWidth / CellWidth;
        public int CollisionHeight => TileHeight / CellHeight;

        public Tileset(string name, int tileWidth, int tileHeight, int columns, int offsetX = 0, int offsetY = 0)
        {
            Name = name;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Columns = columns;
            OffsetX = offsetX;
            OffsetY = offsetY;
            // TODO: Fill dictionary with TiledParser.
            TileCollisions = new Dictionary<int, bool[,]>();
        }

        /// <summary>
        /// Add an entry to the collision dictionary for the specific tile.
        /// </summary>
        /// <param name="tileId">The tile ID in the tileset.</param>
        /// <param name="collisionData">The corresponding collision data as string of '0' and '1'.</param>
        public void AddCollisionData(int tileId, string collisionData)
        {
            int gridWidth = CollisionWidth;
            int gridHeight = CollisionHeight;
            bool[,] dataArray = new bool[gridHeight, gridWidth];
            for (int i = 0; i < gridHeight; i++)
            {
                for (int j = 0; j < gridWidth; j++)
                {
                    try
                    {
                        dataArray[i, j] = collisionData[i * gridWidth + j] == '1';
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        throw new IndexOutOfRangeException("Inconsistencies with the collision data of Tile " 
                            + tileId + " in tileset " + Name + ". Check corresponding tmx file or" +
                            "contact the programmer: Thomas.", e);
                    }
                }
            }
            TileCollisions.Add(tileId, dataArray);
        }
    }
}