using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace V3.Map
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Pathfinder
    {
        private const int CellHeight = Constants.CellHeight;
        private const int CellWidth = Constants.CellWidth;

        // An array of walkable search nodes
        private SearchNode[,] mSearchNodes;

        // The width of the map
        private int mLevelWidth;

        // the height of the map
        private int mLevelHeight;

        // List for nodes that are available to search
        private readonly List<SearchNode> mOpenList = new List<SearchNode>();

        // List for nodes that are NOT available to search
        private readonly List<SearchNode> mClosedList = new List<SearchNode>();

        //Calculates the distance between two (vector)points
        private float Heuristic(Vector2 position, Vector2 goal)
        {
            return (goal - position).Length(); // Manhattan distance
        }

        public void LoadGrid(PathfindingGrid map)
        {
            mLevelWidth = map.mGridWidth;
            mLevelHeight = map.mGridHeight;
            InitializeSearchNodes(map);
        }

        private void InitializeSearchNodes(PathfindingGrid map)
        {
            mSearchNodes = new SearchNode[mLevelWidth, mLevelHeight];
            
            // Creates a searchnode for each tile
            for (int x = 0; x < mLevelWidth; x++)
            {
                for (int y = 0; y < mLevelHeight; y++)
                {
                    SearchNode node = new SearchNode();

                    node.mPosition = new Vector2(x, y);

                    // Walk only on walkable tiles
                    node.mWalkable = map.GetIndex(x, y) == 0;

                    // Stores nodes that can be walked on
                    if (node.mWalkable)
                    {
                        node.mNeighbors = new SearchNode[4];
                        mSearchNodes[x, y] = node;
                    }
                }
            }

            for (int x = 0; x < mLevelWidth; x++)
            {
                for (int y = 0; y < mLevelHeight; y++)
                {
                    SearchNode node = mSearchNodes[x, y];

                    // Note only walkable nodes
                    if (node == null || node.mWalkable == false)
                        continue;


                    // The neighbors for every node
                    Vector2[] neighbors =
                    {
                        new Vector2(x, y - 1), // Node above the current
                        new Vector2(x, y + 1), // Node below the current
                        new Vector2(x - 1, y), // Node to the left
                        new Vector2(x + 1, y) // Node to the right
                    };

                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        Vector2 position = neighbors[i];

                        // Test whether this neighbor is part of the map
                        if (position.X < 0 || position.X > mLevelWidth - 1 || position.Y < 0 ||
                            position.Y > mLevelHeight - 1)
                            continue;

                        SearchNode neighbor = mSearchNodes[(int)position.X, (int)position.Y];

                        // Keep a reference to the nodes that can be walked on
                        if (neighbor == null || neighbor.mWalkable == false)
                            continue;

                        // A reference to the neighbor
                        node.mNeighbors[i] = neighbor;
                    }
                }
            }
        }

        // Reset the state of the search node
        private void ResetSearchNodes()
        {
            mOpenList.Clear();
            mClosedList.Clear();

            for (int x = 0; x < mLevelWidth; x++)
            {
                for (int y = 0; y < mLevelHeight; y++)
                {
                    SearchNode node = mSearchNodes[x, y];

                    if (node == null)
                        continue;

                    node.mInOpenList = false;
                    node.mInClosedList = false;
                    node.mDistanceTraveled = float.MaxValue;
                    node.mDistanceToGoal = float.MaxValue;
                }
            }
        }

        // Returns the node with the smallest distance
        private SearchNode FindBestNode()
        {
            SearchNode currentTile = mOpenList[0];

            float smallestDistanceToGoal = float.MaxValue;

            // Find the closest node to the goal
            for (int i = 0; i < mOpenList.Count; i++)
            {
                if (mOpenList[i].mDistanceToGoal < smallestDistanceToGoal)
                {
                    currentTile = mOpenList[i];
                    smallestDistanceToGoal = currentTile.mDistanceToGoal;
                }
            }
            return currentTile;
        }

        // Use parent field to trace a path from search node to start node
        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            int counter = 0;

            if (startNode == endNode)
            {
                return new List<Vector2>();
            }

            mClosedList.Add(endNode);

            SearchNode parentTile = endNode.mParent;

            // Find the best path
            while (parentTile != startNode)
            {
                mClosedList.Add(parentTile);
                parentTile = parentTile.mParent;
            }

            // Path from position to goal (from tile to tile)
            List<Vector2> betaPath  = new List<Vector2>();

            // Final path after RayCasting
            List<Vector2> finalPath = new List<Vector2>();

            // Reverse the path and transform into the map
            for (int i = mClosedList.Count - 1; i >= 0; i--)
            {
                betaPath.Add(new Vector2(mClosedList[i].mPosition.X * CellWidth + 8, mClosedList[i].mPosition.Y * CellHeight + 8));
            }

            // Short the path via RayCasting
            for (int i = 1; i < betaPath.Count;)
            {
                if (!RayCast(betaPath[counter], betaPath[i]))
                {
                    finalPath.Add(betaPath[i - 1]);
                    counter = i - 1;
                }
                else
                {
                    i++;
                }
            }
            finalPath.Add(betaPath[betaPath.Count - 1]);
            return finalPath;
        }

        //Test Points
        private Vector2 CheckStartNode(Vector2 startNode)
        {
            var start = startNode;

            var startXPos = startNode;
            var startXNeg = startNode;
            var startYPos = startNode;
            var startYNeg = startNode;
            
            // When sprite is blocked out of map, he returns to the edge of the map
            if (startNode.X > mLevelWidth - 2)
                startNode.X = mLevelWidth - 2;
            if (startNode.X < 2)
                startNode.X = 2;
            if (startNode.Y < 4)
                startNode.Y = 4;
            if (startNode.Y > mLevelHeight - 2)
                startNode.Y = mLevelHeight - 2;
            
            // When sprite stays on a null-position, he goes to the nearest non null-position around that null-position
            while (mSearchNodes[(int)start.X, (int)start.Y] == null)
            {
                if (startXPos.X < mLevelWidth)
                    startXPos.X++;
                if (startXNeg.X > 0)
                    startXNeg.X--;
                if (startYPos.Y < mLevelHeight)
                    startYPos.Y++;
                if (startYNeg.Y > 0)
                    startYNeg.Y--;

                if (mSearchNodes[(int)startXPos.X, (int)start.Y] != null)
                {
                    start.X = startXPos.X;
                    return start;
                }
                if (mSearchNodes[(int)startXNeg.X, (int)start.Y] != null)
                {
                    start.X = startXNeg.X;
                    return start;
                }
                if (mSearchNodes[(int)start.X, (int)startYPos.Y] != null)
                {
                    start.Y = startYPos.Y;
                    return start;
                }
                if (mSearchNodes[(int)start.X, (int)startYNeg.Y] != null)
                {
                    start.Y = startYNeg.Y;
                    return start;
                }
            }
            return start;
        }

        private Vector2 CheckEndNode(Vector2 endNode)
        {
            var end = endNode;

            var endXPos = endNode;
            var endXNeg = endNode;
            var endYPos = endNode;
            var endYNeg = endNode;

            // When goal is null-position, the goal will be the nearest non null-position around that null-position
            while (mSearchNodes[(int) end.X, (int) end.Y] == null)
            {
                if(endXPos.X < mLevelWidth - 3)
                    endXPos.X++;
                if(endXNeg.X > 0)
                    endXNeg.X--;
                if(endYPos.Y < mLevelHeight - 3)
                    endYPos.Y++;
                if(endYNeg.Y > 0)
                    endYNeg.Y--;

                if (endXPos.X > mLevelWidth - 3)
                    break;
                if (endXNeg.X < 0)
                    break;
                if (endYPos.Y > mLevelHeight - 3)
                    break;
                if (endYNeg.Y < 0)
                    break;

                if (mSearchNodes[(int)endXPos.X, (int)end.Y] != null)
                {
                    end.X = endXPos.X;
                    return end;
                }
                if (mSearchNodes[(int)endXNeg.X, (int)end.Y] != null)
                {
                    end.X = endXNeg.X;
                    return end;
                }
                if (mSearchNodes[(int)end.X, (int)endYPos.Y] != null)
                {
                    end.Y = endYPos.Y;
                    return end;
                }
                if (mSearchNodes[(int)end.X, (int)endYNeg.Y] != null)
                {
                    end.Y = endYNeg.Y;
                    return end;
                }
            }
            return end;
        }

        // Finds the best path
        public List<Vector2> FindPath(Vector2 startPoint, Vector2 endPoint)
        {
            // Start to find path if startpoint and endpoint are different
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            // Sprite don't walk out of the map
            if (endPoint.Y > mLevelHeight - 2 || endPoint.Y < 4 || endPoint.X > mLevelWidth - 2 || endPoint.X < 2)
            {
                return new List<Vector2>();
            }

            // Test nodes for their validity
            startPoint = CheckStartNode(startPoint);
            endPoint = CheckEndNode(endPoint);

            /*
            * Clear the open and closed lists.
            * reset each's node F and G values
            */
            ResetSearchNodes();

            // Store references to the start and end nodes for convenience
            SearchNode startNode = mSearchNodes[(int)startPoint.X, (int)startPoint.Y];
            SearchNode endNode = mSearchNodes[(int)endPoint.X, (int)endPoint.Y];

            /*
             * Set the start node’s G value to 0 and its F value to the 
             * estimated distance between the start node and goal node 
             * (this is where our H function comes in) and add it to the open list
             */
            if (startNode != null)
            {
                startNode.mInOpenList = true;

                startNode.mDistanceToGoal = Heuristic(startPoint, endPoint);
                startNode.mDistanceTraveled = 0;

                mOpenList.Add(startNode);
            }

            /*
             * While the OpenList is not empty:
             */
            while (mOpenList.Count > 0)
            {
                // Loop the open list and find the node with the smallest F value
                SearchNode currentNode = FindBestNode();

                // If the open list ist empty or a node can't be found
                if (currentNode == null)
                    break;

                // If the active node ist the goal node, we will find and return the path
                if (currentNode == endNode)
                    return FindFinalPath(startNode, endNode); // Trace our path back to the start

                // Else, for each of the active node's neighbors
                for (int i = 0; i < currentNode.mNeighbors.Length; i++)
                {
                    SearchNode neighbor = currentNode.mNeighbors[i];

                    // Make sure that the neighbor can be walked on
                    if (neighbor == null || !neighbor.mWalkable)
                        continue;

                    // Calculate a new G Value for the neighbors node
                    float distanceTraveled = currentNode.mDistanceTraveled + 1;

                    // An estimate of t he distance from this node to the end node
                    float heuristic = Heuristic(neighbor.mPosition, endPoint);

                    if (!neighbor.mInOpenList && !neighbor.mInClosedList)
                    {
                        // Set the neighbors node G value to the G value
                        neighbor.mDistanceTraveled = distanceTraveled;

                        // Set the neighboring node's F value to the new G value + the estimated
                        // distance between the neighbouring node and goal node
                        neighbor.mDistanceToGoal = distanceTraveled + heuristic;

                        // The neighbouring node's mParent property to point at the active node
                        neighbor.mParent = currentNode;

                        // Add the neighboring node to the open list
                        neighbor.mInOpenList = true;
                        mOpenList.Add(neighbor);
                    }

                    // Else if the neighboring node is in open or closed list
                    else if (neighbor.mInOpenList || neighbor.mInClosedList)
                    {
                        if (neighbor.mDistanceTraveled > distanceTraveled)
                        {
                            neighbor.mDistanceTraveled = distanceTraveled;
                            neighbor.mDistanceToGoal = distanceTraveled + heuristic;

                            neighbor.mParent = currentNode;
                        }
                    }
                }

                // Remove active node from the open list and add to the closed list
                mOpenList.Remove(currentNode);
                currentNode.mInOpenList = true;

            }

            // No path could be found
            return new List<Vector2>();
        }

        // Check whether an area is completely walkable in given rectangle 
        public bool AllWalkable(Rectangle rectangle)
        {
            for (int x = rectangle.X; x <= rectangle.X + rectangle.Width; x++)
            {
                for (int y = rectangle.Y; y <= rectangle.Y + rectangle.Height; y++)
                {
                    if (mSearchNodes[x / 16, y / 16] == null)
                        return false;
                }
            }
            return true;
        }

        //Raycasting
        private bool RayCast(Vector2 start, Vector2 goal)
        {
            var direction = goal - start;
            var currentPos = start;
            direction.Normalize();
            //direction = direction * 8;
            
            while (Vector2.Distance(currentPos, goal) > 1f)
            {
                if (mSearchNodes[(int)currentPos.X / 16, (int)currentPos.Y / 16] == null)
                    return false;
                currentPos += direction;
            }
            return true;
        }
    }
}
