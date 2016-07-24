using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using V3.Objects;

namespace V3.AI.Internal
{
    /// <summary>
    /// Default implementation of IAiPlayer.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AiPlayer : IAiPlayer
    {
        /// <summary>
        /// The current world view of the player.  It stores the knowledge of
        /// the computer player based on the previous percepts.
        /// </summary>
        public IWorldView WorldView { get; } = new WorldView();
        /// <summary>
        /// The strategy of the player.  The strategy is a state machine that
        /// defines the current state.
        /// </summary>
        public IStrategy Strategy { get; } = new AttackStrategy();
        /// <summary>
        /// The current state of the player.  The state is one step of the
        /// strategy, and defines the specific actions to take.
        /// </summary>
        public AiState State { get; set; } = AiState.Idle;
        /// <summary>
        /// The actions that the player wants to be executed.  Updated by
        /// Act.
        /// </summary>
        public IList<IAction> Actions { get; } = new List<IAction>();

        private readonly IActionFactory mActionFactory;
        private readonly IBasicCreatureFactory mCreatureFactory;
        private readonly IObjectsManager mObjectsManager;
        private readonly UpdatesPerSecond mUpS = new UpdatesPerSecond(1);
        private readonly Random mRandom = new Random();
        private TimeSpan mTimeSpan = TimeSpan.Zero;
        private TimeSpan mTimeSpanSpawn = TimeSpan.Zero;
        private int mMaxWaitBaseMs = (int) TimeSpan.FromSeconds(20).TotalMilliseconds;
        private int mMaxWaitAddMs = (int) TimeSpan.FromSeconds(60).TotalMilliseconds;

        /// <summary>
        /// Creates a new AI player.
        /// </summary>
        public AiPlayer(IActionFactory actionFactory, IBasicCreatureFactory creatureFactory,
                    IObjectsManager objectsManager)
        {
            mActionFactory = actionFactory;
            mCreatureFactory = creatureFactory;
            mObjectsManager = objectsManager;
        }

        /// <summary>
        /// Executes one update cycle -- perception, acting and the execution
        /// of actions.
        /// </summary>
        /// <param name="gameTime">the time since the last update</param>
        public void Update(GameTime gameTime)
        {
            mTimeSpan += gameTime.ElapsedGameTime;
            if (!mUpS.IsItTime(gameTime))
                return;
            Percept();
            Act();
            foreach (var action in Actions)
            {
                if (action.State == ActionState.Waiting)
                    action.Start();
                action.Update();
            }
        }

        /// <summary>
        /// Update the AI's view of the game world.
        /// </summary>
        public void Percept()
        {
            WorldView.EnemyCount = 0;
            WorldView.PlebsCount = 0;
            WorldView.NecromancerHealth = 0;
            WorldView.IdlingKnights.Clear();
            WorldView.Targets.Clear();
            WorldView.Plebs.Clear();

            foreach (var creature in mObjectsManager.CreatureList)
            {
                if (creature.Faction == Faction.Kingdom)
                {
                    if (!creature.IsDead)
                    {
                        if (creature is Knight)
                        {
                            if (creature.MovementState == MovementState.Idle && creature.IsAttacking == null)
                                WorldView.IdlingKnights.Add(creature);
                        }
                    }
                }
                else if (creature.Faction == Faction.Undead)
                {
                    if (!creature.IsDead)
                    {
                        if (!(creature is Necromancer))
                            WorldView.EnemyCount++;
                        else
                            WorldView.NecromancerHealth = (float) creature.Life / creature.MaxLife;
                        WorldView.Targets.Add(creature);
                    }
                }
                else if (creature.Faction == Faction.Plebs)
                {
                    if (!creature.IsDead)
                    {
                        WorldView.Plebs.Add(creature);
                    }
                }

                WorldView.PlebsCount = WorldView.Plebs.Count;
                if (WorldView.InitialPlebsCount < WorldView.PlebsCount)
                    WorldView.InitialPlebsCount = WorldView.PlebsCount;
            }
        }

        private TimeSpan GetRandomTimeSpanSpawn()
        {
            var factor = Math.Max(0, 500 - WorldView.EnemyCount) / 500;
            return TimeSpan.FromMilliseconds(mRandom.Next(mMaxWaitBaseMs)) +
                TimeSpan.FromSeconds(mRandom.Next(factor * mMaxWaitAddMs));
        }

        /// <summary>
        /// Take actions based on the previous percepts, the current strategy
        /// and state.
        /// </summary>
        public void Act()
        {
            State = Strategy.Update(State, WorldView);

            var completedActions = Actions.Where(
                a => a.State == ActionState.Done || a.State == ActionState.Failed).ToList();
            completedActions.ForEach(a => Actions.Remove(a));

            if (State != AiState.Idle)
            {
                if (mTimeSpan >= mTimeSpanSpawn)
                {
                    mTimeSpan -= mTimeSpanSpawn;
                    mTimeSpanSpawn = GetRandomTimeSpanSpawn();
                    SpawnKnight();
                }
            }

            switch (State)
            {
                case AiState.Idle:
                    // nothing do to when idling
                    return;
                case AiState.AttackCreatures:
                    // let all idling soldiers attack some creatures
                    if (WorldView.Targets.Count > 0)
                    {
                        foreach (var creature in WorldView.IdlingKnights)
                        {
                            ICreature target = null;
                            var distance = float.MaxValue;
                            foreach (var c in WorldView.Targets)
                            {
                                var d = Vector2.Distance(c.Position, creature.Position);
                                if (d < distance)
                                {
                                    distance = d;
                                    target = c;
                                }
                            }
                            creature.IsAttacking = target;
                        }
                    }
                    break;
                case AiState.DefendPeasants:
                    if (WorldView.Plebs.Count > 0)
                    {
                        foreach (var creature in WorldView.IdlingKnights)
                        {
                            ICreature target = null;
                            var distance = float.MaxValue;
                            var threshold = (int) (creature.AttackRadius * 0.8);
                            foreach (var c in WorldView.Plebs)
                            {
                                var d = Vector2.Distance(c.Position, creature.Position);
                                if (d <= threshold)
                                {
                                    target = null;
                                    break;
                                }
                                // attempt to avoid clustering
                                /*if (mObjectsManager.GetObjectsInRectangle(c.SelectionRectangle).OfType<Knight>().Count() > 0)
                                {
                                    continue;
                                }*/
                                if (d < distance)
                                {
                                    distance = d;
                                    target = c;
                                }
                            }
                            if (target != null)
                            {
                                var offset = target.Position - creature.Position;
                                offset.Normalize();
                                offset *= threshold;
                                Move(creature, target.Position + offset);
                            }
                        }
                    }
                    break;
                case AiState.AttackNecromancer:
                    foreach (var c in WorldView.IdlingKnights)
                    {
                        c.IsAttacking = mObjectsManager.PlayerCharacter;
                    }
                    break;
            }
        }

        private void SpawnKnight()
        {
            var knight = mCreatureFactory.CreateKnight();
            var position = new Vector2(500, 500);
            if (mObjectsManager.Castle != null)
                position = mObjectsManager.Castle.Position;
            var spawnAction = mActionFactory.CreateSpawnAction(knight, position);
            Actions.Add(spawnAction);
        }

        private void Move(ICreature creature, Vector2 destination)
        {
            var moveAction = mActionFactory.CreateMoveAction(creature, destination);
            Actions.Add(moveAction);
        }
    }
}
