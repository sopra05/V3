using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Data;
using V3.Effects;

namespace V3.Objects
{
    /// <summary>
    /// Class for selecting creatures, holding the selection and drawing the selection rectangle on the screen.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class Selection
    {
        private IObjectsManager mObjectsManager;
        private IEffectsManager mEffectsManager;
        private readonly Color mColor = Color.Black;
        private Texture2D mTexture;
        public List<ICreature> SelectedCreatures { get; private set; } = new List<ICreature>();
        private List<ICreature> TransformList { get; } = new List<ICreature>();
        private readonly CreatureFactory mCreatureFactory;
        private int mDeadBodies;
        private int mExplosionDeaths;
        private Ellipse NecroArea { get; set; }
        public Rectangle LastSelection { get; private set; }
        private SoundEffect mSoundEffect;
        private SoundEffectInstance mSoundEffectInstance;
        private IOptionsManager mOptionsManager;
        private AchievementsAndStatistics mAchievementsAndStatistics;
        
        public Selection(ContentManager contentManager, CreatureFactory creatureFactory, IObjectsManager objectsManager,
            IEffectsManager effectsManager, IOptionsManager optionsmanager, AchievementsAndStatistics achievementsAndStatistics)
        {
            mCreatureFactory = creatureFactory;
            mObjectsManager = objectsManager;
            mEffectsManager = effectsManager;
            mOptionsManager = optionsmanager;
            mAchievementsAndStatistics = achievementsAndStatistics;
            LoadContent(contentManager);
        }

        private void LoadContent(ContentManager contentManager)
        {
            mTexture = contentManager.Load<Texture2D>("Sprites/WhiteRectangle");
            mSoundEffect = contentManager.Load<SoundEffect>("Sounds/zonk2");
            mSoundEffectInstance = mSoundEffect.CreateInstance();
        }

        /// <summary>
        /// Select all objects in the area of the rectangle between origin and destination.
        /// </summary>
        /// <param name="origin">Position in pixels where the mouse button was pressed down.</param>
        /// <param name="destination">Position in pixels where the mouse button was released.</param>
        public void Select(Point origin, Point destination)
        {
            // Clean last selection
            SelectedCreatures.ForEach(e => e.IsSelected=false);
            SelectedCreatures.Clear();

            // New selection
            int x, y, width, height;
            if (origin.X > destination.X)
            {
                x = destination.X;
                width = origin.X - destination.X;
            }
            else
            {
                x = origin.X;
                width = destination.X - origin.X;
            }
            if (origin.Y > destination.Y)
            {
                y = destination.Y;
                height = origin.Y - destination.Y;
            }
            else
            {
                y = origin.Y;
                height = destination.Y - origin.Y;
            }

            // Creates area for necromancer
            var necroPos = mObjectsManager.PlayerCharacter;
            NecroArea = new Ellipse(new Vector2((int)necroPos.Position.X, (int)necroPos.Position.Y), 1280, 640);

            // List for all objects in the current selection
            LastSelection = new Rectangle(x, y, width, height);
            var selectedObjects = mObjectsManager.GetObjectsInRectangle(LastSelection);

            // Lists to seperate undead from enemy creatures
            var enemyCreatures = new List<ICreature>();
            var undeadCreatures = new List<ICreature>();

            foreach (var obj in selectedObjects)
            {
                var creature = obj as ICreature;
                // Give priority when selecting undead creatures.
                if (creature != null && !creature.IsDead && LastSelection.Intersects(creature.SelectionRectangle) && NecroArea.Contains(creature.Position))
                {
                    if (creature.Faction != Faction.Undead)
                    {
                        enemyCreatures.Add(creature);
                    }
                    else
                    {
                        undeadCreatures.Add(creature);
                    }
                }
            }
            SelectedCreatures = undeadCreatures.Count == 0 ? enemyCreatures : undeadCreatures;

            // Make all selectable creatures selected
            foreach (var selectedCreature in SelectedCreatures)
            {
                selectedCreature.IsSelected = true;
            }
        }

        /// <summary>
        /// Draw the selection rectangle on the screen.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used.</param>
        /// <param name="origin">Position in pixels where the mouse button was pressed down.</param>
        /// <param name="destination">Position in pixels where the mouse button was released.</param>
        public void Draw(SpriteBatch spriteBatch, Point origin, Point destination)
        {
            if (origin.X > destination.X)
            {
                spriteBatch.Draw(mTexture, new Rectangle(destination.X, origin.Y, origin.X - destination.X, 2), mColor);
                spriteBatch.Draw(mTexture, new Rectangle(destination.X, destination.Y, origin.X - destination.X, 2), mColor);
            }
            else
            {
                spriteBatch.Draw(mTexture, new Rectangle(origin.X, origin.Y, destination.X - origin.X, 2), mColor);
                spriteBatch.Draw(mTexture, new Rectangle(origin.X, destination.Y, destination.X - origin.X, 2), mColor);
            }
            if (origin.Y > destination.Y)
            {
                spriteBatch.Draw(mTexture, new Rectangle(origin.X, destination.Y, 2, origin.Y - destination.Y), mColor);
                spriteBatch.Draw(mTexture, new Rectangle(destination.X, destination.Y, 2, origin.Y - destination.Y), mColor);
            }
            else
            {
                spriteBatch.Draw(mTexture, new Rectangle(origin.X, origin.Y, 2, destination.Y - origin.Y), mColor);
                spriteBatch.Draw(mTexture, new Rectangle(destination.X, origin.Y, 2, destination.Y - origin.Y), mColor);
            }
        }

        /// <summary>
        /// Press 1 to trigger the specialattack of the meatball
        /// All non undead creatures at the distance of 200 pixels take damage
        /// After the attack the meatball disappears and three zombies will be created
        /// </summary>
        public void Specialattack()
        {
            foreach (var creature in SelectedCreatures)
            {
                var meatball = creature as Meatball;
                if (meatball != null)
                {
                    // Creates radius for meatballs explosion
                    var explosionRadius = new Rectangle((int)meatball.Position.X - 200, (int)meatball.Position.Y - 200, 400, 400);
                    var objectsInMeatballArea = mObjectsManager.GetObjectsInRectangle(explosionRadius);

                    // Plays sounds and effects
                    mObjectsManager.PlayerCharacter.PlayAnimationOnce(MovementState.Special, TimeSpan.FromSeconds(0.5d));
                    mEffectsManager.PlayOnce(new Explosion(), meatball.Position.ToPoint(), explosionRadius.Size * new Point(3, 2));

                    // All attackable creatures take damage (own undead creatures won't take damage)
                    foreach (var attackable in objectsInMeatballArea)
                    {
                        var toAttack = attackable as ICreature;
                        if (toAttack != null && toAttack.Faction != Faction.Undead)
                        {
                            toAttack.TakeDamage(meatball.Attack);
                            if (toAttack.IsDead)
                                mExplosionDeaths += 1;
                        }
                    }

                    if (mExplosionDeaths >= 10)
                        mAchievementsAndStatistics.mKaboom = true;
                    mExplosionDeaths = 0;

                    // The new zombies after explosion
                    var zombie1 = mCreatureFactory.CreateZombie(meatball.Position, meatball.MovementDirection);
                    var zombie2 = mCreatureFactory.CreateZombie(new Vector2(meatball.Position.X + 50, meatball.Position.Y + 50), meatball.MovementDirection);
                    var zombie3 = mCreatureFactory.CreateZombie(new Vector2(meatball.Position.X - 50, meatball.Position.Y - 50), meatball.MovementDirection);

                    // Makes zombies be selected after explosion
                    zombie1.IsSelected = true;
                    zombie2.IsSelected = true;
                    zombie3.IsSelected = true;
                    meatball.IsSelected = false;
                    SelectedCreatures.Add(zombie1);
                    SelectedCreatures.Add(zombie2);
                    SelectedCreatures.Add(zombie3);
                    SelectedCreatures.Remove(meatball);

                    // Remove meatball from game
                    mObjectsManager.RemoveCreature(meatball);

                    // Add zombies to game
                    mObjectsManager.CreateCreature(zombie1);
                    mObjectsManager.CreateCreature(zombie2);
                    mObjectsManager.CreateCreature(zombie3);

                    // For every zombie the necromancer gets healed
                    var heal = mObjectsManager.PlayerCharacter.Life * 0.12;
                    mObjectsManager.PlayerCharacter.Heal((int)heal);

                    break;
                }
            }
        }

        /// <summary>
        /// Press 2 to create zombies from dead bodies
        /// At least one dead body should be in necromancers area
        /// </summary>
        public void TransformZombie()
        {
            // Creates area for necromancer
            var necroPos = mObjectsManager.PlayerCharacter;
            NecroArea = new Ellipse(new Vector2((int) necroPos.Position.X, (int) necroPos.Position.Y), 1280, 640);

            // Get all creatures in necromancers area
            var necroArea = mObjectsManager.GetCreaturesInEllipse(NecroArea);

            // Every dead body in necromancer area will be transformed to an zombie
            // The prince will be transformed to an elite zombie
            foreach (var creature in necroArea)
            {
                if (creature.Faction == Faction.Plebs || creature.Faction == Faction.Kingdom)
                {
                    if (creature.IsDead)
                    {
                        ICreature zombie;
                        if (creature is KingsGuard || creature is King || creature is Prince)
                            zombie = mCreatureFactory.CreateSkeletonElite(creature.Position, creature.MovementDirection);
                        else
                            zombie = mCreatureFactory.CreateZombie(creature.Position, creature.MovementDirection);

                        // Add zombie to game
                        mObjectsManager.CreateCreature(zombie);

                        // Remove dead body from game
                        mObjectsManager.RemoveCreature(creature);

                        // Play sounds and effects
                        mEffectsManager.PlayOnce(new BloodBang(), zombie.Position.ToPoint(), new Point(128));
                        mObjectsManager.PlayerCharacter.PlayAnimationOnce(MovementState.Special,
                            TimeSpan.FromSeconds(0.5d));

                        // The number of dead bodies
                        mDeadBodies++;

                        // For every zombie the necromancer gets healed
                        var heal = mObjectsManager.PlayerCharacter.Life * 0.04;
                        mObjectsManager.PlayerCharacter.Heal((int)heal);
                    }
                }
            }

            // Counts dead bodies in necromancers area and undead creatures
            var set = mDeadBodies + mObjectsManager.UndeadCreatures.Count;
            if (mObjectsManager.InGraveyardArea(mObjectsManager.PlayerCharacter) && set < 6)
            {
                mObjectsManager.PlayerCharacter.PlayAnimationOnce(MovementState.Special, TimeSpan.FromSeconds(0.5d));
                for (int i = 1; i <= 6 - set; i++)
                {
                    var positionX = mObjectsManager.PlayerCharacter.Position.X + (float)Math.Sin(i) * 75;
                    var positionY = mObjectsManager.PlayerCharacter.Position.Y + (float)Math.Cos(i) * 75;
                    var zombie = mCreatureFactory.CreateZombie(new Vector2(positionX, positionY), mObjectsManager.PlayerCharacter.MovementDirection);
                    mObjectsManager.CreateCreature(zombie);
                    zombie.PlayAnimationOnce(MovementState.Special, TimeSpan.FromSeconds(1));
                }
            }
            mDeadBodies = 0;
        }

        /// <summary>
        /// Press 3 to create a meatball in exchange for five zombie
        /// At least five zombies should be selected
        /// </summary>
        public void TransformMeatball()
        {
            // The cost for this transformation
            var cost = mObjectsManager.PlayerCharacter.MaxLife * 0.12;

            // Transform only when necromancer life minus the transformations cost is not lower than 1
            if (mObjectsManager.PlayerCharacter.Life - (int)cost >= 1)
            {
                // Puts all zombies in a seperate list called TransformList
                foreach (var creature in SelectedCreatures)
                {
                    var zombie = creature as Zombie;
                    if (zombie != null && TransformList.Count < 5)
                    {
                        TransformList.Add(zombie);
                    }
                }

                // Creates the meatball and makes him selected
                if (TransformList.Count >= 5)
                {
                    // Delete zombies from game and SelectedCratures
                    foreach (var zombie in TransformList)
                    {
                        SelectedCreatures.Remove(zombie);
                        mObjectsManager.RemoveCreature(zombie);
                    }

                    // Add meatball to game
                    // Position will be randomly chosen from one of the zombies
                    var randomNumber = new Random();
                    var positionMeatball = randomNumber.Next(5);
                    var meatball = mCreatureFactory.CreateMeatball(TransformList[positionMeatball].Position,
                        TransformList[positionMeatball].MovementDirection);
                    meatball.IsSelected = true;
                    SelectedCreatures.Add(meatball);
                    mObjectsManager.CreateCreature(meatball);

                    // Plays sounds and effects
                    mObjectsManager.PlayerCharacter.PlayAnimationOnce(MovementState.Special, TimeSpan.FromSeconds(0.5d));
                    mEffectsManager.PlayOnce(new BloodExplosion(), meatball.Position.ToPoint(), new Point(256));

                    // Necromancer takes damage
                    mObjectsManager.PlayerCharacter.TakeDamage((int)cost);
                }
                TransformList.Clear();
            }
            else
            {
                // Play sound when transformation not possible
                mSoundEffectInstance.Volume = mOptionsManager.Options.GetEffectiveVolume();
                mSoundEffectInstance.Play();
            }
        }
        

        /// <summary>
        /// Press 4 to create an skeleton in exchange for an zombie
        /// At least one zombie should be selected
        /// </summary>
        public void TransformSkeleton()
        {
            // Iterates through SelectedCreatures and takes the first zombie
            foreach (var creature in SelectedCreatures)
            {
                var zombie = creature as Zombie;
                if (zombie != null)
                {
                    // Delete zombie
                    SelectedCreatures.Remove(zombie);
                    mObjectsManager.RemoveCreature(zombie);

                    // Add skeleton
                    var skeleton = mCreatureFactory.CreateSkeleton(zombie.Position, zombie.MovementDirection);
                    skeleton.IsSelected = true;
                    SelectedCreatures.Add(skeleton);
                    mObjectsManager.CreateCreature(skeleton);
                    
                    // Plays sounds and effects
                    mObjectsManager.PlayerCharacter.PlayAnimationOnce(MovementState.Special, TimeSpan.FromSeconds(0.5d));
                    mEffectsManager.PlayOnce(new BloodFountain(), zombie.Position.ToPoint() - new Point(0, 64), new Point(128));

                    break;
                }
            }
        }

        /// <summary>
        /// Press 5 to create an skeletonhorse in exchange for three skeletons
        /// At least three skeletons should be selected
        /// </summary>
        public void TransformSkeletonHorse()
        {
            // The cost for this transformation
            var cost = mObjectsManager.PlayerCharacter.MaxLife * 0.06;

            // Transform only when necromancer life minus the transformations cost is not lower than 1
            if (mObjectsManager.PlayerCharacter.Life - (int)cost >= 1)
            {

                // Puts all skeletons in a seperate list called TransformList
                foreach (var creature in SelectedCreatures)
                {
                    var skeleton = creature as Skeleton;
                    if (skeleton != null && TransformList.Count < 3)
                    {
                        TransformList.Add(skeleton);
                    }
                }

                // Creates the skeletonhorse and makes him selected
                if (TransformList.Count >= 3)
                {
                    // Delete zombies from game and SelectedCratures
                    foreach (var skeleton in TransformList)
                    {
                        SelectedCreatures.Remove(skeleton);
                        mObjectsManager.RemoveCreature(skeleton);
                    }

                    // Add skeletonhorse to game
                    // Position will be randomly chosen from one of the skeletons
                    var randomNumber = new Random();
                    var positionHorse = randomNumber.Next(3);
                    var skeletonHorse = mCreatureFactory.CreateSkeletonHorse(TransformList[positionHorse].Position,
                        TransformList[positionHorse].MovementDirection);
                    skeletonHorse.IsSelected = true;
                    SelectedCreatures.Add(skeletonHorse);
                    mObjectsManager.CreateCreature(skeletonHorse);

                    // Plays sounds and effects
                    mObjectsManager.PlayerCharacter.PlayAnimationOnce(MovementState.Special, TimeSpan.FromSeconds(0.5d));
                    mEffectsManager.PlayOnce(new HorseEffect(), skeletonHorse.Position.ToPoint() - new Point(0, 16), new Point(128));


                    // Necromancer takes damage
                    mObjectsManager.PlayerCharacter.TakeDamage((int)cost);
                }
                TransformList.Clear();
            }
            else
            {
                // Play sound when transformation not possible
                mSoundEffectInstance.Volume = mOptionsManager.Options.GetEffectiveVolume();
                mSoundEffectInstance.Play();
            }
        }

        /// <summary>
        /// If creature leaves area og the necromancer, he gets disselected
        /// </summary>
        public void UpdateSelection()
        {
            // Creates area for necromancer
            var necroPos = mObjectsManager.PlayerCharacter;
            NecroArea = new Ellipse(new Vector2((int)necroPos.Position.X, (int)necroPos.Position.Y), 1280, 640);

            foreach (var creature in SelectedCreatures)
            {
                if (!NecroArea.Contains(creature.Position) || creature.IsDead)
                    creature.IsSelected = false;
            }

            if (mObjectsManager.AddToSelectables.Count > 0)
            {
                SelectedCreatures.Add(mObjectsManager.AddToSelectables[0]);
                mObjectsManager.AddToSelectables.Clear();
            }
        }

        /// <summary>
        /// Give movement command to selected creatures.
        /// </summary>
        /// <param name="destination">Desired destination in pixels.</param>
        public void Move(Vector2 destination)
        {
            if (SelectedCreatures.Count == 0) return;
            var centreCreature = SelectedCreatures[0];
            // You can not move enemy creatures.
            if (centreCreature.Faction != Faction.Undead) return;
            centreCreature.Move(destination);

            for (int i = 1; i < SelectedCreatures.Count; i++)
            {
                Vector2 flockDestination = destination;
                Vector2 distance = centreCreature.Position - SelectedCreatures[i].Position;
                distance.Normalize();
                distance *= 70;
                flockDestination -= distance;
                SelectedCreatures[i].Move(flockDestination);
            }
        }
    }
}
