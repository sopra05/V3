using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using V3.Objects;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace V3
{
    public sealed class Node
    {
        /// <summary>
        /// Represents the Rectangle if a Quad gets split
        /// </summary>
        private Node mRt; // The rectangle on the right top
        private Node mLt; // The rectangle on the left top 
        private Node mRb; // The rectangle on the right bottom
        private Node mLb; // The rectangle on the left bottom
        private readonly Node mParent; // the parent of a node
        private Rectangle mRectangle;
        private readonly int mPositionX; // X position of the current rectangle
        private readonly int mPositionY; // Y position of the current rectangle
        private readonly int mSizeX; // length of the current rectangle 
        private readonly int mSizeY; // width of the current rectangle 
        private readonly int mCenterX; // center of the current rectangle 
        private readonly int mCenterY;
        private int mCount;
        private Rectangle mCreatureRectangle;
        private MovementDirection mMovementDirectionItem2;
        private MovementState mMovementStateItem;
        private MovementState mMovementStateItem2;
        
        /// <summary>
        /// The list of each node where the objects in each rectangle are saved
        /// </summary>
        private List<IGameObject> mObjectList = new List<IGameObject>();
        private int Count => ObjectCount();

        /// <summary>
        /// initialize the Node
        /// </summary>
        /// <param name="currentRectangle">the current Rectangle/size of the Node</param>
        /// <param name="p">the parent of the node</param>
        public Node(Rectangle currentRectangle, Node p)
        {
            mParent = p;
            mRectangle = currentRectangle;
            mPositionY = mRectangle.Y;
            mPositionX = mRectangle.X;
            mSizeX = mRectangle.Width;
            mSizeY = mRectangle.Height;
            mCenterY = mRectangle.Y + mRectangle.Height / 2;
            mCenterX = mRectangle.X + mRectangle.Width / 2;
        }

        /// <summary>
        /// divided the rectangle in 4 small rectangles locaded in it self
        /// </summary>
        private void CreateSubnodes()
        {
            if (mLt == null)
            {
                mLt = new Node(new Rectangle(mPositionX - 1, mPositionY - 1, mSizeX / 2 + 2, mSizeY / 2 + 2),
                        this);
                mRt = new Node(new Rectangle(mPositionX + mSizeX / 2, mPositionY - 1, mSizeX / 2 + 1, mSizeY / 2 + 2),
                        this);
                mLb = new Node(new Rectangle(mPositionX - 1, mPositionY + mSizeY / 2, mSizeX / 2 + 2, mSizeY / 2 + 1),
                        this);
                mRb = new Node( new Rectangle(mPositionX + mSizeX / 2, mPositionY + mSizeY / 2, mSizeX / 2 + 1, mSizeY / 2 + 1),
                        this);
            }
        }

        /// <summary>
        /// This method is looking in which rectangle the object is located.
        /// At first it checks in which part (right top, left top...) of the rectangle the object is 
        /// and if its posibible the rectangle gets split in for new rectangles the same is happening again.
        /// </summary>
        /// <param name="item">Type of Creature including their position.</param>
        public void AddtoSubNode(IGameObject item)
        {
            mCreatureRectangle = item.BoundaryRectangle; 

            if (mRectangle.Contains(mCreatureRectangle))
            {
                if (mCreatureRectangle.Right < mCenterX && (mCreatureRectangle.Y + mCreatureRectangle.Height) < mCenterY)
                {
                    CreateSubnodes(); // splits subnode in more subnodes
                    mLt.AddtoSubNode(item);
                }
                else if (mCreatureRectangle.X > mCenterX && (mCreatureRectangle.Y + mCreatureRectangle.Height) < mCenterY)
                {
                    CreateSubnodes();
                    mRt.AddtoSubNode(item);
                }
                else if (mCreatureRectangle.Right < mCenterX && mCreatureRectangle.Y > mCenterY)
                {
                    CreateSubnodes();
                    mLb.AddtoSubNode(item);
                }
                else if (mCreatureRectangle.X > mCenterX && mCreatureRectangle.Y > mCenterY)
                {
                    CreateSubnodes();
                    mRb.AddtoSubNode(item);
                }
                else
                {
                    mObjectList.Add(item); // the object gets added to the objectlist of the current node/subnode
                    //CheckCollission(mObjectList);
                    mCount++;
                    if (mCount == 8)
                    {
                        CheckCollission(mObjectList);
                    }
                    else if (mCount > 8)
                    {
                        mCount = 0;
                    }
                }
            }
        }

        /// <summary>
        /// checks if the Objects in the same Quad are intersecting
        /// </summary>
        /// <param name="objectList">list with the objects in the same quad</param>
        //void CheckCollission(List<AbstractCreature> objectList)
        void CheckCollission(List<IGameObject> objectList)
        {
            //    if (mParent != null)
            //    {
            //        mList2 = mParent.mObjectList;
            //    }

            foreach (var obj in objectList)
            {
                foreach (var obj2 in objectList)
                {
                    if (Equals(obj, obj2))
                    {
                        continue;
                    }
                    if (obj.BoundaryRectangle.Intersects(obj2.BoundaryRectangle))
                        //if (obj.BoundaryRectangle.X < obj2.BoundaryRectangle.X + obj2.BoundaryRectangle.Width &&
                        //obj.BoundaryRectangle.X + obj.BoundaryRectangle.Width > obj2.BoundaryRectangle.X &&
                        //obj.BoundaryRectangle.Y < obj2.BoundaryRectangle.Y + obj2.BoundaryRectangle.Height &&
                        //obj.BoundaryRectangle.Height + obj.BoundaryRectangle.Y > obj2.BoundaryRectangle.Y)
                    {
                        
                        if (!(obj is ICreature)) return;
                        if (!(obj2 is ICreature)) return;
                        ICreature creature = (ICreature)obj;
                        mMovementStateItem = creature.MovementState;
                        ICreature creature2 = (ICreature)obj2;
                        mMovementStateItem2 = creature2.MovementState;
                        if (mMovementStateItem != MovementState.Dying && (mMovementStateItem2 != MovementState.Dying))
                        {
                            HandleCollision(obj, obj2);
                        }
                    }
                }
            }

            //if (mParent != null)
            //{
            //    foreach (var obj in mList2)
            //    {
            //        foreach (var obj2 in objectList)
            //        {
            //            if (obj == obj2)
            //            {
            //                continue;
            //            }
            //            if (obj.BoundaryRectangle.Intersects(obj2.BoundaryRectangle))
            //            //if (obj.BoundaryRectangle.X < obj2.BoundaryRectangle.X + obj2.BoundaryRectangle.Width &&
            //            //    obj.BoundaryRectangle.X + obj.BoundaryRectangle.Width > obj2.BoundaryRectangle.X &&
            //            //    obj.BoundaryRectangle.Y < obj2.BoundaryRectangle.Y + obj2.BoundaryRectangle.Height &&
            //            //    obj.BoundaryRectangle.Height + obj.BoundaryRectangle.Y > obj2.BoundaryRectangle.Y)
            //            {
            //                if (!(obj is ICreature)) return;
            //                if (!(obj2 is ICreature)) return;
            //                //if (obj.GetType() != typeof(ICreature)) return;
            //                ICreature creature = (ICreature)obj;
            //                mMovementStateItem = creature.MovementState;
            //                ICreature creature2 = (ICreature)obj2;
            //                mMovementStateItem2 = creature2.MovementState;
            //                if (mMovementStateItem != MovementState.Dying && (mMovementStateItem2 != MovementState.Dying))
            //                {
            //                    HandleCollision(obj, obj2);
            //                }
            //                //mCollision = true;
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// If collsion is detected the object gets moved away so there is no collision anymore
        /// </summary>
        /// <param name="item">the object which collid with another object</param>
        /// <param name="item2">the other object</param>
        //void HandleCollision(AbstractCreature item, AbstractCreature item2)
        void HandleCollision(IGameObject item, IGameObject item2)
        {
            ICreature creature = (ICreature)item;
            ICreature creature2 = (ICreature)item2;
            mMovementDirectionItem2 = creature2.MovementDirection;
            
                if (mMovementDirectionItem2 == MovementDirection.S)
                {
                    creature.Position = new Vector2(item.Position.X + 1, item.Position.Y);
                }
                if (mMovementDirectionItem2 == MovementDirection.N)
                {
                    creature.Position = new Vector2(item.Position.X + 1, item.Position.Y);
                }
                else if (mMovementDirectionItem2 == MovementDirection.O)
                {
                    creature.Position = new Vector2(item.Position.X, item.Position.Y + 1);
                }
                else if (mMovementDirectionItem2 == MovementDirection.W)
                {
                    creature.Position = new Vector2(item.Position.X, item.Position.Y + 1);
                }
                else if (mMovementDirectionItem2 == MovementDirection.SO)
                {
                    creature.Position = new Vector2(item.Position.X - 1, item.Position.Y);
                }
                else if (mMovementDirectionItem2 == MovementDirection.NO)
                {
                    creature.Position = new Vector2(item.Position.X + 1, item.Position.Y - 1);
                }
                else if (mMovementDirectionItem2 == MovementDirection.NW)
                {
                    creature.Position = new Vector2(item.Position.X - 1, item.Position.Y + 1);
                }
                else if (mMovementDirectionItem2 == MovementDirection.SW)
                {
                    creature.Position = new Vector2(item.Position.X + 1, item.Position.Y);
                }
                else
                {
                    creature.Position = new Vector2(item.Position.X + 1, item.Position.Y + 1);
                }
        }

        /// <summary>
        /// Clears the QuadTree of all objects, including any objects living in its children.
        /// </summary>
        public void Clear()
        {
            // Clear out the children, if we have any
            if (mLt != null)
            {
                mLt.Clear();
                mRt.Clear();
                mLb.Clear();
                mRb.Clear();
            }

            // Clear any objects at this level
            if (mObjectList != null)
            {
                mObjectList.Clear();
                mObjectList = null;
            }

            // Set the children to null
            mLt = null;
            mRt = null;
            mLb = null;
            mRb = null;
        }
        
        /// <summary>
        /// Get the total for all objects in this QuadTree, including children.
        /// </summary>
        /// <returns>The number of objects contained within this QuadTree and its children.</returns>
        private int ObjectCount()
        {
            int count = 0;

            // Add the objects at this level
            if (mObjectList != null) count += mObjectList.Count;

            // Add the objects that are contained in the children
            if (mLt != null)
            {
                count += mLt.ObjectCount();
                count += mRt.ObjectCount();
                count += mLb.ObjectCount();
                count += mRb.ObjectCount();
            }

            return count;
        }


        /// <summary>
        /// Deletes an item from this QuadTree. If the object is removed causes this Quad to have no objects in its children, 
        /// it's children will be removed as well.
        /// </summary>
        /// <param name="item"></param>
        public void Delete(IGameObject item)
        {
            if (mObjectList.Contains(item))
            {
                mObjectList.Remove(item);
            }
            // If we didn't find the object in this tree, try to delete from its children
            else if (mLt != null)
            {
                mLt.Delete(item);
                mRt.Delete(item);
                mLb.Delete(item);
                mRb.Delete(item);
            }

            // If all the children are empty, delete all the children
            if (mLt?.Count == 0 &&
                mRt.Count == 0 &&
                mLb.Count == 0 &&
                mRb.Count == 0)
            {
                mLt = null;
                mRt = null;
                mLb = null;
                mRb = null;
            }
        }

        /// <summary>
        /// If the Object isnt in his old Rectangle anymore it have to move
        /// the object to the parent(s) Rectangle until it fits, and optionally going back down into children
        /// </summary>
        /// <param name="item">Actuell Creature</param>
        private void Move(IGameObject item)
        {
            if (mParent != null && mParent.mRectangle.Contains(item.BoundaryRectangle))
            {
                mParent.AddtoSubNode(item);
            }
            else if (mParent == null)
            {
                AddtoSubNode(item);
            }
            else
            {
                mParent.Move(item);
            }

        }

        /// <summary>
        /// If four Subnodes are empty the get deleted
        /// </summary>
        private void RemoveEmptyNodes()
        {
            // If all the children are empty, delete all the children
            if (mLt?.Count == 0 &&
                mRt.Count == 0 &&
                mLb.Count == 0 &&
                mRb.Count == 0)
            {
                mLt = null;
                mRt = null;
                mLb = null;
                mRb = null;
            }
            mLt?.RemoveEmptyNodes();
            mRt?.RemoveEmptyNodes();
            mLb?.RemoveEmptyNodes();
            mRb?.RemoveEmptyNodes();
        }


        /// <summary>
        /// Gibt einem alle Argumente die Innerhalb des rectangles sind zurück in der Liste objectInRecList
        /// </summary>
        /// <param name="rectangle">Der Bereich aus dem man alle Objecte haben möchte</param>
        /// <param name="objectInRecList">Die Liste, in der alle Objekte enthalten sind, die sich im gefragten Rectangle aufhalten</param>
        /// <returns></returns>
        public List<IGameObject> GetObjectsInRectangle(Rectangle rectangle, List<IGameObject> objectInRecList)
        {
            if (rectangle.Intersects(mRectangle)) 
            {
                if (mObjectList != null)
                {
                    foreach (var obj in mObjectList)
                    {
                        if (obj.GetSelf() != null)
                        {
                            objectInRecList.Add(obj);
                        }
                    }
                }
                if (mLt != null)
                {
                    mLt.GetObjectsInRectangle(rectangle, objectInRecList);
                    mRt.GetObjectsInRectangle(rectangle, objectInRecList);
                    mLb.GetObjectsInRectangle(rectangle, objectInRecList);
                    mRb.GetObjectsInRectangle(rectangle, objectInRecList);
                }
            }
            return objectInRecList;
        }

        
        /// <summary>
        /// Updates the position of the object in the Quadtree. If they changed their position
        /// they get deleted and added again at the correct  position. 
        /// </summary>
        public void Update1()
        {
            List<IGameObject> copyList = new List<IGameObject>(mObjectList);
            //CheckCollission(mObjectList);
            foreach (IGameObject obj in copyList)
            {
                if (!(obj is ICreature))
                    continue;
                Delete(obj);
                if (mRectangle.Contains(obj.BoundaryRectangle))
                {
                    AddtoSubNode(obj);
                }
                // if Creature isnt there anymore -> move
                else
                {
                    Move(obj);
                    break;
                }
            }
            mLt?.Update1();
            mRt?.Update1();
            mRb?.Update1();
            mLb?.Update1();
            RemoveEmptyNodes();
        }

        /// <summary>
        /// Makes the Quadtree visible
        /// </summary>
        /// <param name="spriteBatch">to draw the Rectangles</param>
        /// <param name="texture"></param>
        public void DrawQuadtree(SpriteBatch spriteBatch, Texture2D texture)
        {
            //spriteBatch.Draw(mQuadtree.Texture, mRectangle, Color.Black);
            spriteBatch.Draw(texture, new Rectangle(mPositionX, mPositionY, mSizeX, 2), Color.Black);
            spriteBatch.Draw(texture, new Rectangle(mPositionX, mPositionY, 2, mSizeY), Color.Black);
            spriteBatch.Draw(texture, new Rectangle(mPositionX + mSizeX, mPositionY, 2, mSizeY), Color.Black);
            spriteBatch.Draw(texture, new Rectangle(mPositionX, mPositionY + mSizeY, mSizeX, 2), Color.Black);
            mLt?.DrawQuadtree(spriteBatch, texture);
            mRt?.DrawQuadtree(spriteBatch, texture);
            mLb?.DrawQuadtree(spriteBatch, texture);
            mRb?.DrawQuadtree(spriteBatch, texture);
            
            // Draws the rectangle of each Creature
            //foreach (var obj in mObjectList)
            //{
            //    spriteBatch.Draw(mQuadtree.Texture, new Rectangle(obj.BoundaryRectangle.X, obj.BoundaryRectangle.Y, obj.BoundaryRectangle.Width, 5), Color.Red);
            //    spriteBatch.Draw(mQuadtree.Texture, new Rectangle(obj.BoundaryRectangle.X, obj.BoundaryRectangle.Y, 5, obj.BoundaryRectangle.Height), Color.Red);
            //    spriteBatch.Draw(mQuadtree.Texture, new Rectangle(obj.BoundaryRectangle.X + obj.BoundaryRectangle.Width, obj.BoundaryRectangle.Y, 5, obj.BoundaryRectangle.Height), Color.Red);
            //    spriteBatch.Draw(mQuadtree.Texture, new Rectangle(obj.BoundaryRectangle.X, obj.BoundaryRectangle.Y + obj.BoundaryRectangle.Height, obj.BoundaryRectangle.Width, 5), Color.Red);
            //}
        }
    }
}
