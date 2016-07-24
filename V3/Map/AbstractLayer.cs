using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Camera;
using V3.Objects;

namespace V3.Map
{
    /// <summary>
    /// A drawable map layer usually created from a Tiled map file.
    /// </summary>
    public abstract class AbstractLayer
    {
        private const int CellHeight = Constants.CellHeight;
        private const int CellWidth = Constants.CellWidth;

        private readonly int mTileWidth;
        private readonly int mTileHeight;
        private readonly int mMapWidth;
        private readonly int mMapHeight;
        private readonly List<IGameObject> mTextureObjects = new List<IGameObject>();
        private readonly int[,] mTileArray;
        private readonly SortedList<int, Tileset> mTilesets;

        protected AbstractLayer(int tileWidth,
            int tileHeight,
            int mapWidth,
            int mapHeight,
            int[,] tileArray,
            SortedList<int, Tileset> tilesets)
        {
            mTileWidth = tileWidth;
            mTileHeight = tileHeight;
            mMapWidth = mapWidth;
            mMapHeight = mapHeight;
            mTilesets = tilesets;
            if (tileArray.Length == mapWidth * mapHeight)
            {
                mTileArray = tileArray;
            }
            else
            {
                throw new Exception("Error constructing map layer. Map size does not fit the map description.");
            }
        }

        /// <summary>
        /// Create the map objects according to the given map array.
        /// </summary>
        public void CreateObjects()
        {
            var firstgridList = mTilesets.Keys;
            for (int i = 0; i < mMapHeight; i++)
            {
                int horizontalOffset = (i % 2 == 0) ? (-mTileWidth / 2) : 0;
                for (int j = 0; j < mMapWidth; j++)
                {
                    int tileId = mTileArray[i, j];
                    // Checks which tileset needs to be used for the specific tile ID at position [i, j]
                    for (int k = firstgridList.Count - 1; k >= 0; k--)
                    {
                        if (tileId == 0)
                        {
                            // This does generally nothing. But you can overwrite GenerateNullObject() for other behaviour.
                            TextureObject objectToInsert = GenerateNullObject();
                            if (objectToInsert != null)
                            {
                                mTextureObjects.Add(objectToInsert);
                            }
                            break;
                        }
                        else if (tileId >= firstgridList[k])
                        {
                            Tileset tileset = mTilesets.Values[k];
                            int firstgrid = firstgridList[k];
                            Point position = SelectPosition(j, i, horizontalOffset);
                            Point textureSize = new Point(tileset.TileWidth, tileset.TileHeight);
                            Point destination = SelectDestination(j, i, horizontalOffset, tileset.OffsetX, tileset.TileHeight, tileset.OffsetY);
                            Point source = SelectSource(tileId, firstgrid, tileset.TileWidth, tileset.TileHeight, tileset.Columns);
                            IGameObject objectToInsert;
                            if (tileset.Name == "houses_rear" || tileset.Name == "houses_front")
                            {
                                if (source.Y < textureSize.Y * 2)
                                {
                                    int initialDamage = 0;
                                    if (source.X / textureSize.X == 1)
                                    {
                                        initialDamage = 50;
                                    }
                                    else if (source.X / textureSize.X == 2)
                                    {
                                        initialDamage = 80;
                                    }
                                    IBuilding building = new Woodhouse(position.ToVector2(), new Rectangle(destination, textureSize), tileset.Name, source.Y % 384 == 0 ? BuildingFace.SW : BuildingFace.NO);
                                    building.TakeDamage(initialDamage);
                                    objectToInsert = building;
                                }
                                else
                                {
                                    int initialDamage = 0;
                                    if (source.X / textureSize.X == 1)
                                    {
                                        initialDamage = 60;
                                    }
                                    else if (source.X / textureSize.X == 2)
                                    {
                                        initialDamage = 100;
                                    }
                                    IBuilding building = new Forge(position.ToVector2(), new Rectangle(destination, textureSize), tileset.Name, source.Y % 384 == 0 ? BuildingFace.SW : BuildingFace.NO);
                                    building.TakeDamage(initialDamage);
                                    objectToInsert = building;
                                }
                            }
                            else if (tileset.Name == "castle")
                            {
                                IBuilding building = new Objects.Castle(position.ToVector2(), new Rectangle(destination, textureSize), tileset.Name, BuildingFace.SW);
                                objectToInsert = building;
                            }
                            else
                            {
                                objectToInsert = new TextureObject(position,
                                        destination,
                                        textureSize,
                                        source,
                                        tileset.Name);
                            }
                            mTextureObjects.Add(objectToInsert);
                            break;
                        }
                    }
                }
            }
        }

        protected virtual TextureObject GenerateNullObject()
        {
            return null;
        }

        /// <summary>
        /// Loads the image files needed for drawing the tilesets.
        /// </summary>
        /// <param name="contentManager">Content manager used for loading the ressources.</param>
        public void LoadContent(ContentManager contentManager)
        {
            mTextureObjects.ForEach(o => o.LoadContent(contentManager));
        }

        /// <summary>
        /// Draws only the parts of the map which are visible. More efficient than the other Draw-Method.
        /// Not very robust and maybe does not work correctly most layers.
        /// This is because of gaps in the list of game objects.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used.</param>
        /// <param name="camera">Needed to tell which objects of the map are looked upon.</param>
        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            int tilesHorizontal = camera.ScreenSize.X / mTileWidth;
            int tilesVertical = camera.ScreenSize.Y * 2 / mTileHeight;
            int horizontalStart = camera.ScreenRectangle.X / mTileWidth;
            int verticalStart = camera.ScreenRectangle.Y * 2 / mTileHeight;
            /*
            for (int i = 0; i < tilesVertical; i++)
            {
                for (int j = 0; j < tilesHorizontal; j++)
                {
                    mTextureObjects[horizontalStart + j].Draw(spriteBatch);
                }
            }
            */
            for (int j = 0; j < tilesVertical + 2; j++)
            {
                for (int i = 0; i < tilesHorizontal + 2; i++)
                {
                    int index = i + horizontalStart + (j + verticalStart) * mMapWidth;
                    if (index < mTextureObjects.Count)
                    {
                        mTextureObjects[index].Draw(spriteBatch);
                    }
                }
            }
        }

        /// <summary>
        /// Extract a collision grid from the map layer. Used in pathfinding.
        /// </summary>
        /// <returns>A two dimensional boolean collision grid.</returns>
        public bool[,] ExtractCollisions()
        {
            int gridHeight = (mMapHeight - 1) * mTileHeight/ CellHeight / 2;
            int gridWidth = (mMapWidth - 1) * mTileWidth / CellWidth;
            bool[,] collisionGrid = new bool[gridHeight, gridWidth];
            var firstgridList = mTilesets.Keys;
            for (int i = 0; i < mMapHeight; i++)
            {
                for (int j = 0; j < mMapWidth; j++)
                {
                    int tileId = mTileArray[i, j];
                    for (int k = firstgridList.Count - 1; k >= 0; k--)
                    {
                        if (tileId >= firstgridList[k])
                        {
                            Tileset tileset = mTilesets.Values[k];
                            int firstgrid = firstgridList[k];
                            tileId -= firstgrid;
                            bool[,] collisionData;
                            // Is there even collision data for the specific tile ID?
                            if (tileset.TileCollisions.TryGetValue(tileId, out collisionData))
                            {
                                int cellOffset = (i % 2 == 0 ? -mTileWidth / 2 : 0) / CellWidth;
                                int cellsHorizontal = mTileWidth / CellWidth;
                                int cellsVertical = mTileHeight / CellHeight;
                                int iStart = (i - 1) * cellsVertical / 2 + cellsVertical - tileset.CollisionHeight + tileset.OffsetY / CellHeight;
                                int jStart = j * cellsHorizontal + cellOffset + tileset.OffsetX / CellWidth;
                                for (int iData = 0; iData < tileset.CollisionHeight; iData++)
                                {
                                    for (int jData = 0; jData < tileset.CollisionWidth; jData++)
                                    {
                                        // Do we even need to update collisionGrid?
                                        if (iStart + iData >= 0 && iStart + iData < gridHeight && jStart + jData >= 0 && jStart + jData < gridWidth &&
                                            collisionData[iData, jData] && !collisionGrid[iStart + iData, jStart + jData])
                                        {
                                            collisionGrid[iStart + iData, jStart + jData] = collisionData[iData, jData];
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return collisionGrid;
        }

        public List<IGameObject> ExtractObjects()
        {
            return mTextureObjects;
        }

        private Point SelectDestination(int x, int y, int xOffset, int tileXOffset, int tileHeight, int tileYOffset)
        {
            return new Point(x * mTileWidth + xOffset + tileXOffset,
                (y - 1) * (mTileHeight / 2) - tileHeight + mTileHeight + tileYOffset);
        }

        private Point SelectSource(int tileId, int firstgrid, int tileWidth, int tileHeight, int tilesPerRow)
        {
            return new Point((tileId - firstgrid) % tilesPerRow * tileWidth, (tileId - firstgrid) / tilesPerRow * tileHeight);
        }

        private Point SelectPosition(int x, int y, int xOffset)
        {
            return new Point(x * mTileWidth + xOffset + mTileWidth / 2, y * (mTileHeight / 2));
        }
    }
}