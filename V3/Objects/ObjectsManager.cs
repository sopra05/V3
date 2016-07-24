using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Camera;
using V3.Effects;
using V3.Map;

namespace V3.Objects
{
    /// <summary>
    /// Objects manager for all game objects, be is creatures or buildings or even simple landscape objects.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ObjectsManager : IObjectsManager
    {
        private List<List<ICreature>> mCreaturesByFactions;
        private List<ICreature> mCreatureList;
        private List<ICreature> mAddToSelectables;
        private Quadtree mTextureQuadtree;
        private Quadtree mInteractableQuadtree;
        private readonly ContentManager mContentManager;
        private readonly CreatureFactory mCreatureFactory;
        private readonly IEffectsManager mEffectsManager;
        private Rectangle mMapRectangle;
        private List<Area> mAreas;
        private readonly UpdatesPerSecond mFewerUpdates = new UpdatesPerSecond(15);
        private readonly UpdatesPerSecond mEffectsCounter = new UpdatesPerSecond(0.2);

        public List<ICreature> AddToSelectables => mAddToSelectables;
        public List<ICreature> CreatureList => mCreatureList;
        public List<ICreature> UndeadCreatures => mCreaturesByFactions[(int) Faction.Undead];
        //public List<ICreature> KingdromCreatures => mCreaturesByFactions[(int) Faction.Kingdom];
        //public List<ICreature> PlebCreatures => mCreaturesByFactions[(int) Faction.Plebs];
        public ICreature PlayerCharacter { get; private set; }
        public ICreature Boss { get; private set; }
        public ICreature Prince { get; private set; }
        public Castle Castle { get; private set; }

        public ObjectsManager(ContentManager contentManager, CreatureFactory creatureFactory,
            IEffectsManager effectsManager)
        {
            mContentManager = contentManager;
            mCreatureFactory = creatureFactory;
            mEffectsManager = effectsManager;
        }

        public void Initialize(IMapManager mapManager)
        {
            Clear();
            mAddToSelectables = new List<ICreature>();
            mCreatureList = new List<ICreature>();
            mCreaturesByFactions = new List<List<ICreature>>();
            // Make three lists because we have three factions.
            for (int i = 0; i < 3; i++)
            {
                mCreaturesByFactions.Add(new List<ICreature>());
            }
            // Gets the initial values for the Quad Tree from the current map.
            mInteractableQuadtree = new Quadtree(mapManager.SizeInPixel + new Point(128, 128));
            mTextureQuadtree = new Quadtree(mapManager.SizeInPixel + new Point(128, 128));
            mInteractableQuadtree.LoadContent(mContentManager);
            mTextureQuadtree.LoadContent(mContentManager);

            // Import map objects for drawing.
            ImportMapObjects(mapManager.GetObjects());

            // Save Map size in ObjectsManager.
            mMapRectangle = new Rectangle(new Point(0), mapManager.SizeInPixel);

            mAreas = mapManager.Areas;
        }

        public void Clear()
        {
            CreatureList?.Clear();
            mCreaturesByFactions?.Clear();
            for (int i = 0; i < 3; i++)
            {
                mCreaturesByFactions?.Add(new List<ICreature>());
            }
            PlayerCharacter = null;
            Boss = null;
            Prince = null;
            Castle = null;
            mInteractableQuadtree?.Clear();
            mTextureQuadtree?.Clear();
        }

        public void CreatePlayerCharacter(Necromancer necromancer)
        {
            PlayerCharacter = necromancer;
            AddCreature(necromancer);
        }

        public void CreateBoss(ICreature boss)
        {
            Boss = boss;
            AddCreature(boss);
        }

        public void CreatePrince(ICreature prince)
        {
            Prince = prince;
            AddCreature(prince);
        }

        public void CreateCreature(ICreature creature)
        {
            AddCreature(creature);
        }

        public void RemoveCreature(ICreature creature)
        {
            mCreatureList.Remove(creature);
            mCreaturesByFactions[(int) creature.Faction].Remove(creature);
            mInteractableQuadtree.RemoveItem(creature);
        }

        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            var objectsToDraw = mInteractableQuadtree.GetObjectsInRectangle(camera.ScreenRectangle);
            var texturesToDraw = mTextureQuadtree.GetObjectsInRectangle(camera.ScreenRectangle);
            IEnumerable<IGameObject> ordered = from obj in objectsToDraw.Concat(texturesToDraw) orderby obj.Position.Y select obj;
            foreach (var obj in ordered)
            {
                obj.Draw(spriteBatch);
            }
        }

        public void DrawQuadtree(SpriteBatch spriteBatch)
        {
            mInteractableQuadtree.Draw(spriteBatch);
            mTextureQuadtree.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime, bool rightButtonPressed, Vector2 rightButtonPosition, ICamera camera)
        {
            foreach (var creature in mCreatureList)
            {
                if (creature.GetSelf() == null) continue;
                creature.Update(gameTime, PlayerCharacter, rightButtonPressed, rightButtonPosition, mInteractableQuadtree, camera);
                

                // Create a boss if castle is down
                if (creature.IsAttackingBuilding != null && creature.IsAttackingBuilding.Name == "Burg")
                {
                    if (creature.IsAttackingBuilding.Robustness <= 50 && Boss == null)
                    {
                        var king = mCreatureFactory.CreateKing(new Vector2(creature.IsAttackingBuilding.Position.X - 300, creature.IsAttackingBuilding.Position.Y + 130), MovementDirection.S); // 
                        var knight1 = mCreatureFactory.CreateKnight(new Vector2(creature.IsAttackingBuilding.Position.X - 320, creature.IsAttackingBuilding.Position.Y + 110), MovementDirection.S); // 
                        var knight2 = mCreatureFactory.CreateKnight(new Vector2(creature.IsAttackingBuilding.Position.X - 280, creature.IsAttackingBuilding.Position.Y + 150), MovementDirection.S); // 
                        CreateBoss(king);
                        AddCreature(knight1);
                        AddCreature(knight2);
                        break;
                    }
                    //break;
                }
                // Checks if the creature moved out of the map and resets its position as appropriate.
                if (!mMapRectangle.Contains(creature.Position))
                {
                    creature.ResetPosition();
                }
            }
            if (mFewerUpdates.IsItTime(gameTime))
            {
                mInteractableQuadtree.Update();
            }

            #region Boss Special Attack
            // Play some cool effects when boss is spawned so he looks cooler.
            // Added a small special attack which does at bit AoE damage.
            if (mEffectsCounter.IsItTime(gameTime))
            {
                if (Boss != null && camera.ScreenRectangle.Contains(Boss.Position))
                {
                    mEffectsManager.PlayOnce(new Quake(), Boss.Position.ToPoint(), new Point(256, 128));
                    var aoeRadius = new Rectangle(Boss.Position.ToPoint() - new Point(128), new Point(256));
                    var effectedCreatures = mInteractableQuadtree.GetObjectsInRectangle(aoeRadius);
                    foreach (var obj in effectedCreatures)
                    {
                        var creature = obj as ICreature;
                        if (creature != null && creature.Faction == Faction.Undead && creature.BoundaryRectangle.Intersects(aoeRadius))
                        {
                            creature.TakeDamage(10);
                        }
                    }
                }
            }
            #endregion

            #region Necromancer stuff
            if (PlayerCharacter.IsSelected && rightButtonPressed)
            {
                var objectsUnderMouse =
                    mInteractableQuadtree.GetObjectsInRectangle(new Rectangle(rightButtonPosition.ToPoint(), new Point(1, 1)));
                foreach (var obj in objectsUnderMouse)
                {
                    var creature = obj as ICreature;
                    if (creature == null || !creature.SelectionRectangle.Contains(rightButtonPosition) || !creature.IsDead || creature.Faction == Faction.Undead) continue;
                    if (Vector2.Distance(PlayerCharacter.Position, creature.Position) > PlayerCharacter.AttackRadius) continue;
                    RemoveCreature(creature);
                    if (creature is KingsGuard || creature is King || creature is Prince)
                    {
                        CreateCreature(mCreatureFactory.CreateSkeletonElite(creature.Position, creature.MovementDirection));
                    }
                    else
                    {
                        CreateCreature(mCreatureFactory.CreateZombie(creature.Position, creature.MovementDirection));
                    }
                    PlayerCharacter.PlayAnimationOnce(MovementState.Special, TimeSpan.FromSeconds(0.5d));
                    mEffectsManager.PlayOnce(new BloodBang(), creature.Position.ToPoint(), new Point(128));
                    break;
                }
            }
            #endregion Necromancer stuff

            #region SkeletonRider

            var recMouse = new Rectangle(rightButtonPosition.ToPoint(), new Point(1, 1));
            foreach (var creature in UndeadCreatures)
            {
                Skeleton skeleton = creature as Skeleton;
                if (skeleton != null)
                {
                    if(skeleton.IsSelected && rightButtonPressed)
                    {
                        var atMousPosition = mInteractableQuadtree.GetObjectsInRectangle(recMouse);
                        foreach (var wannabehorse in atMousPosition)
                        {
                            var horse = wannabehorse as SkeletonHorse;
                            if (horse != null && Vector2.Distance(skeleton.Position, horse.Position) < 32)
                            {
                                horse.Mount(skeleton);
                                horse.IsSelected = true;
                                mAddToSelectables.Add(horse);
                            }
                        }
                    }
                }
            }
            #endregion SkeletonRider
        }

        private void AddCreature(ICreature creature)
        {
            if (!mMapRectangle.Contains(creature.BoundaryRectangle)) return;
            mCreatureList.Add(creature);
            mCreaturesByFactions[(int) creature.Faction].Add(creature);
            mInteractableQuadtree.Insert(creature);
        }

        public void ImportMapObjects(List<IGameObject> textureObjects)
        {
            foreach (var obj in textureObjects)
            {
                if (obj is IBuilding)
                {
                    mInteractableQuadtree.Insert(obj);
                    if (obj is Castle)
                        Castle = (Castle) obj;
                }
                else
                {
                    mTextureQuadtree.Insert(obj);
                }
            }
        }

        public List<IGameObject> GetObjectsInRectangle(Rectangle rectangle)
        {
            return mInteractableQuadtree.GetObjectsInRectangle(rectangle);
        }

        public List<ICreature> GetCreaturesInEllipse(Ellipse ellipse)
        {
            var setToReturn = new List<ICreature>();
            foreach (var obj in mInteractableQuadtree.GetObjectsInRectangle(ellipse.BoundaryRectangle))
            {
                ICreature creature = obj as ICreature;
                if (creature != null && ellipse.Contains(creature.Position))
                {
                    setToReturn.Add(creature);
                }
            }
            return setToReturn;
        }

        public void ExposeTheLiving()
        {
            foreach (var creature in mCreatureList)
            {
                if (creature.Faction == Faction.Plebs)
                {
                    (creature as MalePeasant)?.RemoveArmor();
                    (creature as FemalePeasant)?.RemoveArmor();
                }
                else if (creature.Faction == Faction.Kingdom)
                {
                    (creature as Knight)?.RemoveArmor();
                    (creature as KingsGuard)?.RemoveArmor();
                }
            }
        }

        public bool InGraveyardArea(ICreature creature)
        {
            return mAreas.Where(area => area.Type == AreaType.Graveyard).Select(area => area.Contains(creature)).Any(contains => contains);
        }
    }
}
