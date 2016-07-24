using System.Collections.Generic;
using V3.Objects;

namespace V3.Map
{
    /// <summary>
    /// The floor of the map consisting of the ground to walk on, grass or water.
    /// </summary>
    public sealed class FloorLayer : AbstractLayer
    {
        public FloorLayer(int tileWidth, int tileHeight, int mapWidth, int mapHeight, int[,] tileArray, SortedList<int, Tileset> tilesets)
            : base(tileWidth, tileHeight, mapWidth, mapHeight, tileArray, tilesets)
        {
        }

        protected override TextureObject GenerateNullObject()
        {
            return new TextureObject();
        }
    }
}
