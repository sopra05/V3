using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Objects;

namespace V3
{
    public sealed class Quadtree
    {
        private Node mRoot; // The first "Node" of the Quadtree
        private ContentManager mContentManager;
        private readonly Point mMaxSize;

        /// <summary>
        /// Generates Quadtree
        /// </summary>
        /// <param name="maxSize">Gives the biggest/first Rectangle of the Quadtree. This should be the sice of the howl map.</param>
        public Quadtree(Point maxSize)
        {
            mRoot = new Node(new Rectangle(new Point(-128, -128), maxSize), null);
            mMaxSize = maxSize;
        }
        
        /// <summary>
        /// Updates the Quadtree. This is importent for the movements of the Objects. 
        /// </summary>
        public void Update()
        {
            mRoot.Update1();
        }

        /// <summary>
        /// You call this method if you want to Inster an Object to the Quadtree.
        /// </summary>
        /// <param name="item">Type of Creature including their position.</param>
        public void Insert(IGameObject item)
        {
            mRoot.AddtoSubNode(item);
        }

        /// <summary>
        /// DMakes the Rectangles of the Quadtree visible.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            mRoot.DrawQuadtree(spriteBatch, Texture);
        }

        public List<IGameObject> GetObjectsInRectangle(Rectangle rectangle)
        {
            List<IGameObject> objectList = new List<IGameObject>();
            return mRoot.GetObjectsInRectangle(rectangle, objectList);
        }

        /// <summary>
        /// Deletes an Object out of the Quadtree.
        /// </summary>
        /// <param name="item">Type of Creature including their position.</param>
        public void RemoveItem(IGameObject item)
        {
            mRoot.Delete(item);
        }

        /// <summary>
        /// Loads the content to draw the Rectangels of the Quadtree.
        /// </summary>
        /// <param name="contentManager"></param>
        public void LoadContent(ContentManager contentManager)
        {
            mContentManager = contentManager;
            Texture = mContentManager.Load<Texture2D>("Sprites/WhiteRectangle");
        }

        /// <summary>
        /// Deletes all elements from the quadtree.
        /// </summary>
        public void Clear()
        {
            mRoot?.Clear();
            mRoot = new Node(new Rectangle(new Point(-50, -50), mMaxSize), null);
        }

        private Texture2D Texture { get; set; }
    }
}
