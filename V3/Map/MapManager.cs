using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Camera;
using V3.Objects;

namespace V3.Map
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MapManager : IMapManager
    {
        private TiledParser mTiledParser;
        private FloorLayer mFloorLayer;
        private ObjectLayer mObjectLayer;
        private List<Area> mAreas; 
        private PathfindingGrid mPathfindingGrid;
        private readonly ContentManager mContentManager;
        private readonly GraphicsDeviceManager mGraphicsDeviceManager;

        public List<Area> Areas => mAreas; 

        public Point SizeInPixel { get; private set; }
        public Point SizeInTiles { get; private set; }
        public Point TileSize { get; private set; }
        public Point PathfindingGridSize { get; private set; }
        public Point PathfindingCellSize { get; private set; }
        public string FileName { get; private set; }

        public MapManager(ContentManager contentManager, GraphicsDeviceManager graphicsDeviceManager)
        {
            mContentManager = contentManager;
            mGraphicsDeviceManager = graphicsDeviceManager;
        }

        public void DrawFloor(SpriteBatch spriteBatch, ICamera camera)
        {
            mFloorLayer.Draw(spriteBatch, camera);
        }

        public void Load(string fileName)
        {
            mTiledParser = new TiledParser();
            // Parse map data.
            mTiledParser.Parse(fileName);
            FileName = fileName;
            TileSize = new Point(mTiledParser.TileWidth, mTiledParser.TileHeight);
            SizeInTiles = new Point(mTiledParser.MapWidth, mTiledParser.MapHeight);
            SizeInPixel = new Point((SizeInTiles.X - 1) * TileSize.X, SizeInTiles.Y / 2 * TileSize.Y - TileSize.Y / 2);
            // Create floor layer of the map.
            mFloorLayer = new FloorLayer(mTiledParser.TileWidth, mTiledParser.TileHeight, mTiledParser.MapWidth, mTiledParser.MapHeight, mTiledParser.MapLayers[0], mTiledParser.TileSets);
            mFloorLayer.CreateObjects();
            mFloorLayer.LoadContent(mContentManager);
            // Create object layer of the map.
            mObjectLayer = new ObjectLayer(mTiledParser.TileWidth, mTiledParser.TileHeight, mTiledParser.MapWidth, mTiledParser.MapHeight, mTiledParser.MapLayers[1], mTiledParser.TileSets);
            mObjectLayer.CreateObjects();
            mObjectLayer.LoadContent(mContentManager);
            // Get areas from the map
            mAreas = mTiledParser.Areas;
            // Create pathfinding grid used in the pathfinder.
            mPathfindingGrid = new PathfindingGrid(mTiledParser.MapWidth, mTiledParser.MapHeight, mTiledParser.TileWidth, mTiledParser.TileHeight);
            mPathfindingGrid.LoadContent(mContentManager);
            mPathfindingGrid.CreateCollisions(mFloorLayer.ExtractCollisions());
            mPathfindingGrid.CreateCollisions(mObjectLayer.ExtractCollisions());
            PathfindingGridSize = new Point(mPathfindingGrid.mGridWidth, mPathfindingGrid.mGridHeight);
            PathfindingCellSize = new Point(Constants.CellWidth, Constants.CellHeight);
            // Create Minimap texture from pathfinding grid.
            mPathfindingGrid.CreateMinimap(mGraphicsDeviceManager.GraphicsDevice);
        }

        public List<IGameObject> GetObjects()
        {
            return mObjectLayer.ExtractObjects();
        }

        public List<ICreature> GetPopulation(CreatureFactory creatureFactory, Pathfinder pathfinder)
        {
            return mAreas.SelectMany(area => area.GetPopulation(creatureFactory, pathfinder)).ToList();
        }

        public PathfindingGrid GetPathfindingGrid()
        {
            return mPathfindingGrid;
        }

        public void DrawPathfindingGrid(SpriteBatch spriteBatch, ICamera camera)
        {
            mPathfindingGrid.Draw(spriteBatch, camera);
        }

        public void DrawMinimap(SpriteBatch spriteBatch, Rectangle position)
        {
            mPathfindingGrid.DrawSmallGrid(spriteBatch, position);
        }
    }
}