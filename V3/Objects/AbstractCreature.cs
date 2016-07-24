using System;
using System.Collections.Generic;
using Castle.Core.Internal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Camera;
using V3.Data;
using V3.Map;
using V3.Objects.Movement;
using V3.Objects.Sprite;

namespace V3.Objects
{
    /// <summary>
    /// Abstract class for deriving all moving objects in the game,
    /// be it the player, his minions or the enemy.
    /// </summary>
    public abstract class AbstractCreature : ICreature
    {
        // **************************************************
        // Internal variables, managers and values.
        protected virtual Point CreatureSize { get; } = new Point(24);
        protected virtual Point BoundaryShift { get; } = new Point(-12, -16);
        protected virtual Point SelectionSize { get; } = new Point(48, 64);
        protected virtual Point SelectionShift { get; } = new Point(-24, -40);
        /// <summary>
        /// Array of sprites for drawing the creature. Can have up to four entries for (ordered): Body, Head, Weapon, Offhand
        /// </summary>
        protected abstract ISpriteCreature[] Sprite { get; }
        protected abstract IMovable MovementScheme { get; }
        protected abstract CreatureType Type { get; }
        private Texture2D mOnePixelTexture;
        private Texture2D mSelectionTexture;
        private readonly Pathfinder mPathfinder;
        private readonly ContentManager mContentManager;
        private readonly IOptionsManager mOptionsManager;
        private AchievementsAndStatistics mAchievementsAndStatistics;
        private static readonly Random sRandom = new Random();
        private static readonly object sYncLock = new object();
        private List<Arrow> mArrowList = new List<Arrow>();

#if NO_AUDIO
#else
        private SoundEffect mSoundEffect;
        private SoundEffectInstance mSoundEffectInstance;
        private SoundEffect mSoundEffectHorse;
        private SoundEffectInstance mSoundEffectInstanceHorse;
        private SoundEffect mSoundEffectKnight;
        private SoundEffectInstance mSoundEffectInstanceKnight;
        private SoundEffect mSoundEffectFight;
        private SoundEffectInstance mSoundEffectInstanceFight;
        private SoundEffect mSoundEffectMeatball;
        private SoundEffectInstance mSoundEffectInstanceMeatball;
#endif
        private bool mIsDead;
        //private float mReculate = 1.0f;

        // **************************************************
        // Public variables and important attributes of the creature:
        public abstract string Name { get; protected set; }
        public abstract int Life { get; protected set; }
        public abstract int MaxLife { get; protected set; }
        public abstract int Speed { get; }
        public abstract int Attack { get; protected set; }
        public abstract int AttackRadius { get; protected set; }
        public abstract int SightRadius { get; protected set; }
        public abstract TimeSpan TotalRecovery { get;}
        public abstract TimeSpan Recovery { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 InitialPosition { private get; set; }
        public abstract Faction Faction { get; }
        public int Id { get; }

        public bool IsDead
        {
            get { return Life <= 0; }
        }

        public bool IsUpgraded { get; set; }

        public abstract IBuilding IsAttackingBuilding { get; set; }
        public MovementDirection MovementDirection { get; set; }
        public MovementState MovementState { get; set; } = MovementState.Idle;
        public Rectangle SelectionRectangle => new Rectangle(Position.ToPoint() + SelectionShift, SelectionSize);
        public Rectangle BoundaryRectangle => new Rectangle(Position.ToPoint() + BoundaryShift, CreatureSize);
        public bool IsSelected { get; set; }
        private static int sMaxNumberOfSoundHorse;
        private static int sMaxNumberOfSoundMeatball;
        private static int sMaxNumberOfSoundZombie;
        private static int sMaxNumberOfSoundKnight;

        public abstract ICreature IsAttacking { get; set; }

        /// <summary>
        /// Color of the rectangle displayed when creature is selected.
        /// </summary>
        protected virtual Color SelectionColor
        {
            get
            {
                switch (Faction)
                {
                    case Faction.Undead:
                        return Color.Red;
                    case Faction.Kingdom:
                        return Color.Blue;
                    case Faction.Plebs:
                        return Color.Green;
                    default:
                        return Color.Gray;
                }
            }
        }

        protected AbstractCreature(ContentManager contentManager, Pathfinder pathfinder, IOptionsManager optionsManager, AchievementsAndStatistics achievementsAndStatistics)
        {
            mPathfinder = pathfinder;
            mContentManager = contentManager;
            mOptionsManager = optionsManager;
            mAchievementsAndStatistics = achievementsAndStatistics;
            Id = IdGenerator.GetNextId();
            LoadContent(mContentManager);
        }

        /// <summary>
        /// The specific creature takes damage. If its life falls below zero it dies.
        /// </summary>
        /// <param name="damage">Amount of received damage.</param>
        public void TakeDamage(int damage)
        {
            if (Life > 0)
            {
                Life -= damage;
            }

            if (Life <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Loads content files needed for drawing the creature.
        /// </summary>
        /// <param name="contentManager">Content manager used.</param>
        public void LoadContent(ContentManager contentManager)
        {
            Sprite.ForEach(e => e?.Load(contentManager));
            mOnePixelTexture = contentManager.Load<Texture2D>("Sprites/WhiteRectangle");
            mSelectionTexture = contentManager.Load<Texture2D>("Sprites/selection");
#if NO_AUDIO
#else
            try
            {
                mSoundEffect = contentManager.Load<SoundEffect>("Sounds/walking");
                mSoundEffectInstance = mSoundEffect.CreateInstance();
                mSoundEffectHorse = contentManager.Load<SoundEffect>("Sounds/SkeletonHorse");
                mSoundEffectInstanceHorse = mSoundEffectHorse.CreateInstance();
                mSoundEffectKnight = contentManager.Load<SoundEffect>("Sounds/Knight");
                mSoundEffectInstanceKnight = mSoundEffectKnight.CreateInstance();
                mSoundEffectFight = contentManager.Load<SoundEffect>("Sounds/punch");
                mSoundEffectInstanceFight = mSoundEffectFight.CreateInstance();
                mSoundEffectMeatball = contentManager.Load<SoundEffect>("Sounds/Monster_Gigante-Doberman-1334685792");
                mSoundEffectInstanceMeatball = mSoundEffectMeatball.CreateInstance();
            }
            catch (DllNotFoundException)
            {
                // HACK: ignore sound-related errors as the sound is currently
                // not working on the pool computers
            }
            catch (NoAudioHardwareException)
            {
                // HACK: ignore sound-related errors as the sound is currently
                // not working on the pool computers                
            }
#endif
        }

        /// <summary>
        /// The creature should move to the specified destination if possible.
        /// </summary>
        /// <param name="destination">The desired destination.</param>
        public void Move(Vector2 destination)
        {
            MovementScheme.FindPath(mPathfinder, Position, destination);
        }
        
        public void Update(GameTime gameTime, ICreature playerCharacter, 
            bool rightButtonPressed, Vector2 rightButtonPosition, Quadtree quadtree, ICamera camera)
        {
            // If Creature is dead, don't update anymore.
            Sprite.ForEach(e => e?.PlayAnimation(gameTime));
            if (mIsDead)
            {
                return;
            }
            bool showedByCamera = camera.ScreenRectangle.Contains(Position);
#if NO_AUDIO
#else

            // update the volume according to the current setting
            if (mSoundEffectInstance != null)
                mSoundEffectInstance.Volume = mOptionsManager.Options.GetEffectiveVolume()*0.1f;
            if (mSoundEffectInstanceHorse != null)
                mSoundEffectInstanceHorse.Volume = mOptionsManager.Options.GetEffectiveVolume() * 0.07f;
            if (mSoundEffectInstanceKnight != null)
                mSoundEffectInstanceKnight.Volume = mOptionsManager.Options.GetEffectiveVolume() * 0.07f;
            if (mSoundEffectInstanceFight != null)
                mSoundEffectInstanceFight.Volume = mOptionsManager.Options.GetEffectiveVolume() * 0.07f;
            if (mSoundEffectInstanceMeatball != null)
                mSoundEffectInstanceMeatball.Volume = mOptionsManager.Options.GetEffectiveVolume() * 0.07f;
#endif
            if (MovementScheme.IsMoving)
            {
                MovementState = MovementState.Moving;
                Position += MovementScheme.GiveNewPosition(Position, Speed);
                MovementDirection = MovementScheme.GiveMovementDirection();
#if NO_AUDIO
#else           
                try
                {
                    if (showedByCamera)
                    {
                        if ((this is Zombie || this is FemalePeasant || this is MalePeasant || this is Necromancer) &&
                            sMaxNumberOfSoundZombie < 3)
                        {
                            if (mSoundEffectInstance != null)
                            {
                                mSoundEffectInstance.Play();
                                sMaxNumberOfSoundZombie++;
                            }

                        }
                        if (this is SkeletonHorse && sMaxNumberOfSoundHorse < 3) 
                        {
                            if (mSoundEffectInstanceHorse != null)
                            {
                                mSoundEffectInstanceHorse.Play();
                                sMaxNumberOfSoundHorse++;
                            }

                        }
                        if (this is Knight && sMaxNumberOfSoundKnight < 3)
                        {
                            if (mSoundEffectInstanceKnight != null)
                            {
                                mSoundEffectInstanceKnight.Play();
                                sMaxNumberOfSoundKnight++;
                            }

                        }
                        if (this is Meatball && sMaxNumberOfSoundMeatball < 3)
                        {
                            if (mSoundEffectInstanceMeatball != null)
                            {
                                mSoundEffectInstanceMeatball.Play();
                                sMaxNumberOfSoundMeatball++;
                            }

                        }
                    }
                }
                catch (InstancePlayLimitException)
                {
                    // HACK: ignore sound-related errors as the sound is currently
                    // not working on the pool computers
                }
#endif
            }
            else
            {
#if NO_AUDIO
#else
                if (mSoundEffectInstance != null)
                {
                    mSoundEffectInstance.Stop();
                    sMaxNumberOfSoundZombie--;
                }
                if (mSoundEffectInstanceHorse != null)
                {
                    mSoundEffectInstanceHorse.Stop();
                    sMaxNumberOfSoundHorse--;
                }
                if (mSoundEffectInstanceKnight != null)
                {
                    mSoundEffectInstanceKnight.Stop();
                    sMaxNumberOfSoundKnight--;
                }

                if (mSoundEffectInstanceMeatball != null)
                {
                    mSoundEffectInstanceMeatball.Stop();
                    sMaxNumberOfSoundMeatball--;
                }
#endif
                MovementState = MovementState.Idle;
            }
            if (IsDead)
            {
                if (mSoundEffectInstance != null)
                {
                    mSoundEffectInstance.Stop();
                    sMaxNumberOfSoundZombie--;
                }
                if (mSoundEffectInstanceHorse != null)
                {
                    mSoundEffectInstanceHorse.Stop();
                    sMaxNumberOfSoundHorse--;
                }
                if (mSoundEffectInstanceKnight != null)
                {
                    mSoundEffectInstanceKnight.Stop();
                    sMaxNumberOfSoundKnight--;
                }

                if (mSoundEffectInstanceMeatball != null)
                {
                    mSoundEffectInstanceMeatball.Stop();
                    sMaxNumberOfSoundMeatball--;
                }
            }

            /* 
             * 
             * Random movement of Zombies
             *
             */
            #region Random Movement
            Ellipse necroArea = new Ellipse(new Vector2((int)playerCharacter.Position.X, (int)playerCharacter.Position.Y), 1280, 640);
            float necroDistance = Vector2.Distance(Position, playerCharacter.Position);
            List<Vector2> randomMoveVector = new List<Vector2>();

            
            int rndX = RandomNumber(0, 20);
            int rndY = RandomNumber(0, 20);

            randomMoveVector.Add(new Vector2(rndX, rndY));
            randomMoveVector.Add(new Vector2(-rndX, -rndY));
            randomMoveVector.Add(new Vector2(rndX, -rndY));
            randomMoveVector.Add(new Vector2(-rndX, rndY));

            int rndNumber;

            if (!(this is Necromancer) && 
                Faction.Equals(Faction.Undead) && 
                MovementState == MovementState.Idle && 
                !necroArea.Contains(Position))
            {
                rndNumber = RandomNumber(1, 60);
                if (rndNumber == 7)
                {
                    int rndMoveVector = RandomNumber(0, 400);
                    Move(Position + randomMoveVector[rndMoveVector%4]);
                }
            }
            if (Faction.Equals(Faction.Plebs))
            {
                rndNumber = RandomNumber(1, 60);
                if (rndNumber == 7)
                {
                    int rndMoveVector = RandomNumber(0, 400);
                    Move(Position + randomMoveVector[rndMoveVector%4]);
                }
                
            }
            #endregion

            List<IGameObject> defenders = new List<IGameObject>();
            if (IsAttacking == null)
            {
                // Get the quadtree of the sight radius.
                defenders = quadtree.GetObjectsInRectangle(new Rectangle((int) Position.X - SightRadius/2, (int) Position.Y - SightRadius/2, SightRadius, SightRadius));
                // Returns if nothing is in sight.
                if (defenders.Count == 0) return;
            }

            bool attacking = false;

            /* 
             * 
             * COMMAND ATTACKING OF BUILDINGS 
             *
             */
            #region Command Attack

            // Distance between Undead and Necromancer
            if (Faction.Equals(Faction.Undead) && !(this is Necromancer))
            {
                // If Undead are in sight distance of the Necromancer, do stuff.
                if (IsSelected && (int)necroDistance <= playerCharacter.AttackRadius)
                {
                    // Get all the objects of the quadtree of the sightradius of the Necromancer
                    if (rightButtonPressed)
                    {
                        IsAttackingBuilding = null;
                        var objectsUnderMouse = quadtree.GetObjectsInRectangle(new Rectangle(rightButtonPosition.ToPoint(), new Point(1, 1)));
                        foreach (var obj in objectsUnderMouse)
                        {
                            var building = obj as IBuilding;
                            if (building == null || !building.BoundaryRectangle.Contains(rightButtonPosition) || building.Robustness == 0) continue;
                            IsAttackingBuilding = building;
                        }
                    }
                }
                if (IsAttackingBuilding != null )
                {
                    if ((int)Vector2.Distance(Position, ComputeMoVector(IsAttackingBuilding)) <= AttackRadius && IsAttackingBuilding.Robustness >= 0)  // (int)Vector2.Distance(Position, IsAttackingBuilding.Position) <= AttackRadius * 2
                    {
                        //var building = IsAttacking as IBuilding;
                        // Attacking
                        Recovery -= gameTime.ElapsedGameTime;
                        if (Recovery < TimeSpan.Zero)
                        {
                            Recovery = TimeSpan.Zero;
                        }
                        MovementState = MovementState.Attacking; // PlayAnimationOnce(MovementState.Attacking, TotalRecovery);
                        if (Recovery <= TimeSpan.Zero)
                        {
                            IsAttackingBuilding.TakeDamage(Attack);
                            Recovery = TotalRecovery; // Cooldown

                            // Throw an arrow
                            if (AttackRadius > 300)
                                CreateArrow(Position, IsAttackingBuilding.Position);
                        }
                        // If house is dead.
                        if (IsAttackingBuilding.Robustness <= 20)
                        {
                            MovementState = MovementState.Idle;

                            // Undead getting stronger if building is dead.
                            switch (IsAttackingBuilding.Name)
                            {
                                case "Schmiede":
                                    if (IsAttackingBuilding.MaxGivesWeapons > 0 && !IsUpgraded)
                                    {
                                        MaxLife += 20;
                                        Life += 20;
                                        IsUpgraded = true;
                                        IsAttackingBuilding.UpgradeCounter();
                                    }
                                    break;
                                case "Holzhaus":
                                    if (IsAttackingBuilding.MaxGivesWeapons > 0)
                                    {
                                        if (this is Skeleton && !IsUpgraded)
                                        {
                                            ChangeEquipment(EquipmentType.Body, new SkeletonArcherSprite());
                                            AttackRadius += 500;
                                            SightRadius += 500;
                                            IsUpgraded = true;
                                            IsAttackingBuilding.UpgradeCounter();
                                        }
                                        if (this is Zombie && !IsUpgraded)
                                        {
                                            ChangeEquipment(EquipmentType.Body, new ZombieWithClubSprite());
                                            Attack += 10;
                                            IsUpgraded = true;
                                            IsAttackingBuilding.UpgradeCounter();
                                        }
                                    }
                                    break;
                            }

                            if (IsAttackingBuilding.Robustness <= 0) IsAttackingBuilding = null;
                        }
                    }
                    else
                    {
                        Move(ComputeMoVector(IsAttackingBuilding));
                    }
                }
            }

            #endregion
            foreach (var arrow in mArrowList)
            {
                arrow.UpdateArrow();
            }
            /* 
             * 
             * AUTO ATTACKING OF CREATURES 
             * 
             */
            #region Auto-Attack

            // Don't auto attack if Zombie got command from Necromancer.
            if (IsAttackingBuilding != null) return;

            if (IsAttacking == null)
            {
                List<ICreature> creatures = new List<ICreature>();
                foreach (var defender in defenders)
                {
                    var creature = defender as ICreature;
                    if (creature == null) continue;
                    creatures.Add(creature);
                }

                ICreature attackableCreature = null;
                float sightDistance = SightRadius;

                // Compute the nearest enemy.
                foreach (var defender in creatures)
                {
                    // Zombies and Knights only.
                    if (Faction.Equals(Faction.Undead) || Faction.Equals(Faction.Kingdom))
                    {
                        // If attacker-type == defender-type go to next possible defender.
                        if ((Faction.Equals(Faction.Undead) && !defender.Faction.Equals(Faction.Undead)) ||
                            (Faction.Equals(Faction.Kingdom) && defender.Faction.Equals(Faction.Undead)))
                        {
                            if (defender.Life > 0)
                            {
                                // Compute the distance of attacker and possible defender.
                                float distanceTest = Vector2.Distance(Position, defender.Position);

                                if (distanceTest <= sightDistance)
                                {
                                    sightDistance = distanceTest;
                                    attackableCreature = defender;
                                }
                                // IsAttacking = defender;
                                if (attackableCreature != null)
                                {
                                    IsAttacking = defender;

                                }
                            }
                        }
                    }
                }
                if (attackableCreature == null)
                {
                    IsAttacking = null;
                }
            }

            if (IsAttacking == null) return;
            /* 
             * 
             * ATTACKING OF CREATURES 
             * 
             */
            if ((int)Vector2.Distance(Position, IsAttacking.Position) > AttackRadius &&
                                    !MovementScheme.IsMoving)
            {
                // Do not move if attacker is already in attackrange
                if (!attacking)
                {
                    Move(ComputeMoVector(IsAttacking));
                }
            }

            if (IsAttacking.IsDead || (int)Vector2.Distance(Position, IsAttacking.Position) > SightRadius)
            {
                IsAttacking = null;
                MovementState = MovementState.Idle;
                return;
            }
            // Attacking
            Recovery -= gameTime.ElapsedGameTime;
            if (Recovery < TimeSpan.Zero)
            {
                Recovery = TimeSpan.Zero;
            }

            // If attacker is in the attack radius of defender
            if ((int)Vector2.Distance(Position, IsAttacking.Position) <= AttackRadius)
            {
                if (Recovery <= TimeSpan.Zero)
                {
                    IsAttacking.TakeDamage(Attack);
                    PlayAnimationOnce(MovementState.Attacking, TotalRecovery);
                    Recovery = TotalRecovery; // Cooldown

                    // Throw an arrow
                    if (AttackRadius > 300 && IsAttacking != null)
                        CreateArrow(Position, IsAttacking.Position);
                }
            }

            #endregion
        }

        public void PlayAnimationOnce(MovementState animation, TimeSpan duration)
        {
            Sprite.ForEach(e => e.PlayOnce(animation, duration));
        }

        /// <summary>
        /// Draw the creature on the screen.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used for drawing.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsSelected) DrawSelection(spriteBatch);
            Sprite.ForEach(e => e?.Draw(spriteBatch, Position, MovementState, MovementDirection));
            if (IsSelected) DrawLifeRectangle(spriteBatch);
            foreach (var arrow in mArrowList)
            {
                arrow.DrawArrow(spriteBatch);
            }
        }

        public void DrawStatic(SpriteBatch spriteBatch, Point position)
        {
            Sprite.ForEach(e => e?.DrawStatic(spriteBatch, position, MovementState.Idle, MovementDirection.S));
        }

        protected virtual void Die()
        {
            if (Faction == Faction.Plebs || Faction == Faction.Kingdom)
            {
                mAchievementsAndStatistics.mHundredDeadCorpses += 1;
                mAchievementsAndStatistics.mUndeadArmy += 1;
                mAchievementsAndStatistics.mRightHandOfDeath += 1;
                mAchievementsAndStatistics.mKilledCreatures += 1;
            }
            else if (Faction == Faction.Undead)
            {
                mAchievementsAndStatistics.mLostServants += 1;
            }
            mIsDead = true;
            Life = 0;
            MovementState = MovementState.Dying;
        }

        private void DrawSelection(SpriteBatch spriteBatch)
        {
            // TODO: Remove the magic vector and adjust position.
            spriteBatch.Draw(mSelectionTexture, Position - new Vector2(32, 16), SelectionColor);
            /*
            spriteBatch.Draw(mOnePixelTexture, new Rectangle(BoundaryRectangle.X, BoundaryRectangle.Y, BoundaryRectangle.Width, 2), SelectionColor);
            spriteBatch.Draw(mOnePixelTexture, new Rectangle(BoundaryRectangle.X, BoundaryRectangle.Y + BoundaryRectangle.Height, BoundaryRectangle.Width, 2), SelectionColor);
            spriteBatch.Draw(mOnePixelTexture, new Rectangle(BoundaryRectangle.X, BoundaryRectangle.Y, 2, BoundaryRectangle.Height), SelectionColor);
            spriteBatch.Draw(mOnePixelTexture, new Rectangle(BoundaryRectangle.X + BoundaryRectangle.Width, BoundaryRectangle.Y, 2, BoundaryRectangle.Height), SelectionColor);
            */
        }

        private void DrawLifeRectangle(SpriteBatch spriteBatch)
        {
            var backgroundRectangle = new Rectangle(SelectionRectangle.X, SelectionRectangle.Y - 12, SelectionRectangle.Width, SelectionRectangle.Height / 10);
            var lifeBarRectangle = new Rectangle(SelectionRectangle.X + 2, SelectionRectangle.Y - 11, SelectionRectangle.Width - 3, SelectionRectangle.Height / 10 - 2);
            spriteBatch.Draw(mOnePixelTexture, backgroundRectangle, Color.Black * 0.7f);
            spriteBatch.Draw(mOnePixelTexture, new Rectangle(lifeBarRectangle.X, lifeBarRectangle.Y, lifeBarRectangle.Width * Life / MaxLife, lifeBarRectangle.Height), Color.Firebrick);
        }

        /// <summary>
        /// Change the equipment/sprite of the creature to something other.
        /// </summary>
        /// <param name="equipmentType">Which part of the equipment should be changed.</param>
        /// <param name="sprite">Which sprite to use instead.</param>
        public void ChangeEquipment(EquipmentType equipmentType, ISpriteCreature sprite)
        {
            if (Sprite.Length > (int) equipmentType)
            {
                sprite.Load(mContentManager);
                Sprite[(int)equipmentType] = sprite;
            }
#if DEBUG
            else
            {
                throw new Exception("Creature does not have a " + equipmentType.ToString() + " slot. For further information talk to Thomas.");
            }
#endif
        }

        /// <summary>
        /// Returns the object instance without modifications.
        /// </summary>
        /// <returns>This object.</returns>
        public virtual IGameObject GetSelf()
        {
            return this;
        }

        public void ResetPosition()
        {
            Position = InitialPosition;
        }

        #region Compute Move Distance
        /// <summary>
        /// Returns vector for the moving distance to attack.
        /// </summary>
        /// <returns>Vector.</returns>
        private Vector2 ComputeMoVector(IGameObject gameObject)
        {
            Vector2 goToPosition = new Vector2(Position.X, Position.Y);

            /*
             * P
             *   D
             *   
             */
            if (Position.X <= gameObject.BoundaryRectangle.Left &&
                Position.Y <= gameObject.BoundaryRectangle.Top)
            {
                goToPosition =
                    new Vector2(gameObject.BoundaryRectangle.Left - AttackRadius * 0.5f,
                        gameObject.BoundaryRectangle.Top - AttackRadius * 0.5f);
            }
            /*
             *   P
             *   D
             *   
             */
            if (Position.X >= gameObject.BoundaryRectangle.Left &&
                Position.X <= gameObject.BoundaryRectangle.Right &&
                Position.Y <= gameObject.BoundaryRectangle.Top)
            {
                goToPosition = new Vector2(Position.X,
                    gameObject.BoundaryRectangle.Top - AttackRadius * 0.5f);
            }
            /*
             *     P
             *   D
             *   
             */
            if (Position.X >= gameObject.BoundaryRectangle.Right &&
                Position.Y <= gameObject.BoundaryRectangle.Top)
            {
                goToPosition =
                    new Vector2(gameObject.BoundaryRectangle.Right - AttackRadius * 0.5f,
                        gameObject.BoundaryRectangle.Top - AttackRadius * 0.5f);
            }
            /*
             *     
             *   D P
             *   
             */
            if (Position.X >= gameObject.BoundaryRectangle.Right &&
                Position.Y <= gameObject.BoundaryRectangle.Bottom &&
                Position.Y >= gameObject.BoundaryRectangle.Top)
            {
                goToPosition =
                    new Vector2(gameObject.BoundaryRectangle.Right - AttackRadius * 0.5f,
                        Position.Y);
            }
            /*
             * 
             *   D
             *     P
             */
            if (Position.X >= gameObject.BoundaryRectangle.Right &&
                Position.Y >= gameObject.BoundaryRectangle.Bottom)
            {
                goToPosition =
                    new Vector2(gameObject.BoundaryRectangle.Right - AttackRadius * 0.5f,
                        gameObject.BoundaryRectangle.Bottom - AttackRadius * 0.5f);
            }
            /*
             * 
             *   D
             *   P
             */
            if (Position.X >= gameObject.BoundaryRectangle.Left &&
                Position.X <= gameObject.BoundaryRectangle.Right &&
                Position.Y >= gameObject.BoundaryRectangle.Bottom)
            {
                goToPosition = new Vector2(Position.X,
                    gameObject.BoundaryRectangle.Bottom - AttackRadius * 0.5f);
            }
            /*
             * 
             *   D
             * P
             */
            if (Position.X <= gameObject.BoundaryRectangle.Left &&
                Position.Y >= gameObject.BoundaryRectangle.Bottom)
            {
                goToPosition =
                    new Vector2(gameObject.BoundaryRectangle.Left - AttackRadius * 0.5f,
                        gameObject.BoundaryRectangle.Bottom - AttackRadius * 0.5f);
            }
            /*
             * 
             * P D
             *   
             */
            if (Position.X <= gameObject.BoundaryRectangle.Left &&
                Position.Y <= gameObject.BoundaryRectangle.Bottom &&
                Position.Y >= gameObject.BoundaryRectangle.Top)
            {
                goToPosition =
                    new Vector2(gameObject.BoundaryRectangle.Left - AttackRadius * 0.5f,
                        Position.Y);
            }

            return goToPosition;
        }
        #endregion

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;
            if (obj == this)
                return true;
            if (!(obj is IGameObject))
                return false;
            return Id.Equals(((IGameObject) obj).Id);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>
        /// Returns synchronized random value.
        /// </summary>
        /// <returns>Vector.</returns>
        /// <param name="min">Lower bound for random value.</param>
        /// <param name="max">Upper bound +1 for random value.</param>
        private static int RandomNumber(int min, int max)
        {
            // synchronize
            lock (sYncLock)
            { 
                return sRandom.Next(min, max);
            }
        }

        public void Heal(int amount)
        {
            if (Life + amount < MaxLife)
            {
                Life += amount;
            }
            else
            {
                Life = MaxLife;
            }
        }

        public void Empower(int modifier)
        {
            MaxLife *= modifier;
            Life *= modifier;
        }

        private void CreateArrow(Vector2 start, Vector2 goal)
        {
            if (mArrowList.Count >= 60) mArrowList.Clear();
            Arrow arrow = new Arrow(start, goal);
            arrow.LoadArrow(mContentManager);
            mArrowList.Add(arrow);
        }

        /// <summary>
        /// Save this creature’s data to a CreatureData object.
        /// </summary>
        /// <returns>the CreatureData object with the status of this creature</returns>
        public virtual CreatureData SaveData()
        {
            var creatureData = new CreatureData();
            creatureData.Type = Type;
            creatureData.Id = Id;
            creatureData.Life = Life;
            creatureData.MaxLife = MaxLife;
            creatureData.Attack = Attack;
            creatureData.Recovery = Recovery;
            creatureData.IsUpgraded = IsUpgraded;
            creatureData.PositionX = Position.X;
            creatureData.PositionY = Position.Y;
            creatureData.MovementDirection = MovementDirection;
            creatureData.MovementState = MovementState;
            creatureData.MovementData = MovementScheme.SaveData();
            // references
            if (IsAttacking != null)
                creatureData.IsAttackingId = IsAttacking.Id;
            return creatureData;
        }

        /// <summary>
        /// Restore the creature's state from the given data.
        /// </summary>
        /// <param name="creatureData">the state of the creature to restore</param>
        public virtual void LoadData(CreatureData creatureData)
        {
            // ID is set by IdGenerator.SetIdOnce
            Life = creatureData.Life;
            MaxLife = creatureData.MaxLife;
            Attack = creatureData.Attack;
            Recovery = creatureData.Recovery;
            IsUpgraded = creatureData.IsUpgraded;
            Position = new Vector2(creatureData.PositionX, creatureData.PositionY);
            MovementDirection = creatureData.MovementDirection;
            MovementState = creatureData.MovementState;
            MovementScheme.LoadData(creatureData.MovementData);

            if (Life <= 0)
                Die();
        }

        /// <summary>
        /// Restore the creature's references to other creatures from the given data.
        /// </summary>
        /// <param name="data">the state of the creature to restore</param>
        /// <param name="creatures">the list of all creatures by ID</param>
        public virtual void LoadReferences(CreatureData data, Dictionary<int, ICreature> creatures)
        {
            if (creatures.ContainsKey(data.IsAttackingId))
                IsAttacking = creatures[data.IsAttackingId];
        }
    }
}
