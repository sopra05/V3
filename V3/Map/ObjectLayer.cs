using System.Collections.Generic;

namespace V3.Map
{
    /// <summary>
    /// The map objects which are the same layer as the moving creatutes. 
    /// Buildings, flowers, trees etc.
    /// </summary>
    public sealed class ObjectLayer : AbstractLayer
    {
        public ObjectLayer(int tileWidth, int tileHeight, int mapWidth, int mapHeight, int[,] tileArray, SortedList<int, Tileset> tilesets)
            : base(tileWidth, tileHeight, mapWidth, mapHeight, tileArray, tilesets)
        {
        }
    }
}