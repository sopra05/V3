using Microsoft.Xna.Framework;

namespace V3.Map
{
    class SearchNode
    {
        // Location on the map
        public Vector2 mPosition;

        // If true, the sprite can walk on
        public bool mWalkable;

        //
        public SearchNode[] mNeighbors;

        // Previous node 
        public SearchNode mParent;

        // Check whether a node is in the open list
        public bool mInOpenList;

        // Check whether a node is in the closed list
        public bool mInClosedList;

        // DIstance from the start node to the goal node (F value)
        public float mDistanceToGoal;

        // Distance traveled from the spawn point (G value)
        public float mDistanceTraveled;
    }
}