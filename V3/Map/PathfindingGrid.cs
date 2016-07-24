using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Camera;

namespace V3.Map
{
    /// <summary>
    /// Tells the pathfinder where you can walk.
    /// </summary>
    public sealed class PathfindingGrid
    {
        private const int CellHeight = Constants.CellHeight;
        private const int CellWidth = Constants.CellWidth;

        private readonly bool[,] mArray;
        public readonly int mGridWidth;
        public readonly int mGridHeight;
        private Texture2D mTexture;
        private Texture2D mMinimapTexture;

        public PathfindingGrid(int mapWidth, int mapHeight, int tileWidth, int tileHeight)
        {
            mGridHeight = (mapHeight - 1) * tileHeight / CellHeight / 2;
            mGridWidth = (mapWidth - 1) * tileWidth / CellWidth;
            mArray = new bool[mGridHeight, mGridWidth];
        }

        /// <summary>
        /// Compares the pathfinding grid with the given collision grid and adjusts the former.
        /// If a cell of the pathfinding grid is false and the cell at the same position of the
        /// collision grid is true, switch false to true.
        /// </summary>
        /// <param name="collisionGrid">A grid of the same size as the pathfinding grid.</param>
        public void CreateCollisions(bool[,] collisionGrid)
        {
            if (collisionGrid.Length == mGridWidth * mGridHeight)
            {
                for (int i = 0; i < mGridHeight; i++)
                {
                    for (int j = 0; j < mGridWidth; j++)
                    {
                        if (!mArray[i, j])
                        {
                            mArray[i, j] = collisionGrid[i, j];
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Error creating the collision grid. Object layer data and collision grid data do not fit.");
            }
        }

        /// <summary>
        /// Load content for visual representation of the pathfinding grid.
        /// </summary>
        /// <param name="contentManager">Use this content manager.</param>
        public void LoadContent(ContentManager contentManager)
        {
            mTexture = contentManager.Load<Texture2D>("Textures/pathfinder");
            //mOnePixelTexture = contentManager.Load<Texture2D>("Sprites/WhiteRectangle");
        }

        /// <summary>
        /// A visual representation of the pathfinding grid. Drawn efficiently.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used for drawing.</param>
        /// <param name="camera">For only drawing on the shown part of the map.</param>
        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            Point startPosition = camera.Location.ToPoint() / new Point(CellWidth, CellHeight);
            Point tilesOnScreen = camera.ScreenSize / new Point(CellWidth, CellHeight) + new Point(1, 1) + startPosition;
            for (int i = startPosition.Y; i < tilesOnScreen.Y && i < mGridHeight; i++)
            {
                for (int j = startPosition.X; j < tilesOnScreen.X && j < mGridWidth; j++)
                {
                    Rectangle destinationRectangle = new Rectangle(j * CellWidth, i * CellHeight, CellWidth, CellHeight);
                    Rectangle sourceRectangle = new Rectangle(mArray[i, j] ? CellWidth : 0, 0, CellWidth, CellHeight);
                    spriteBatch.Draw(mTexture, destinationRectangle, sourceRectangle, Color.White);
                }
            }
        }

        /// <summary>
        /// Gets the value at the specified position of the collision array.
        /// </summary>
        /// <param name="cellX">Position at the horizontal axis.</param>
        /// <param name="cellY">Position at the vertical axis.</param>
        /// <returns>Returns 0 if you can walk at the specified position, 1 otherwise.</returns>
        public int GetIndex(int cellX, int cellY)
        {
            //if (cellX < 0 || cellX > mGridWidth - 1 || cellY < 0 || cellY > mGridHeight - 1)
            //    return 0;
            return mArray[cellY, cellX] ? 1 : 0;
        }

        /// <summary>
        /// Draws a small version of the pathfinding grid to the screen.
        /// Useful for the minimap.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used.</param>
        /// <param name="position">Where to draw in pixel coordinates and which size. In pixels.</param>
        public void DrawSmallGrid(SpriteBatch spriteBatch, Rectangle position)
        {
            spriteBatch.Draw(mMinimapTexture, position, Color.White);
        }

        public void CreateMinimap(GraphicsDevice device)
        {
            Color[] colors = new Color[mGridWidth * mGridHeight];
            for (int i = 0; i < mGridHeight; i++)
            {
                for (int j = 0; j < mGridWidth; j++)
                {
                    colors[i * mGridWidth + j ] = mArray[i, j] ? Color.DarkGray : Color.Green;
                }
            }
            mMinimapTexture = new Texture2D(device, mGridWidth, mGridHeight);
            mMinimapTexture.SetData(colors);
        }
    }
}