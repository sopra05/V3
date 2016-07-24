using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using V3.Objects;

namespace V3.Map
{
    public enum AreaType
    {
        Village,    // has no soldiers or knights, only peasants
        Castle,     // many knights patrolling around
        Graveyard   // here you can respawn zombies
    }

    /// <summary>
    /// Holding area data of the map. Later used for generating enemies.
    /// </summary>
    public sealed class Area
    {
        private const int DistanceHorizontal = 64;
        private const int DistanceVertical = 32;
        private readonly AreaType mType;
        private Rectangle mArea;
        private readonly double mDensity;
        private readonly double mChance;

        /// <summary>
        /// Gets the area type.
        /// </summary>
        public AreaType Type => mType;

        /// <summary>
        /// Creates a new area for generating population.
        /// </summary>
        /// <param name="type">Which type of area. Determines which population is spawned.</param>
        /// <param name="data">The size and position of the area.</param>
        /// <param name="density">The population density. Together with chance.</param>
        /// <param name="chance">The chance that a creature is actually created.</param>
        /// <param name="name">Name of the area as shown in the game.</param>
        // ReSharper disable once UnusedParameter.Local
        public Area(string type, Rectangle data, double density = 0d, double chance = 0d, string name = "")
        {
            switch (type)
            {
                case "village":
                    mType = AreaType.Village;
                    break;
                case "castle":
                    mType = AreaType.Castle;
                    break;
                case "graveyard":
                    mType = AreaType.Graveyard;
                    break;
                default:
                    throw new Exception("Error parsing the map. There is no behaviour defined for objects of type " + type + ".");
            }
            if (density > 1d || chance > 1d || density < 0d || chance < 0d)
            {
                throw new Exception("Error when parsing area data from map. Density and/or chance is not in range 0.0 to 1.0.");
            }
            mArea = data;
            mDensity = density;
            mChance = chance;
        }

        /// <summary>
        /// Creates the initial population for this area.
        /// </summary>
        /// <param name="creatureFactory">The factory used for creating creatures.</param>
        /// <param name="pathfinder">Used for checking collisions when creating population.</param>
        /// <returns></returns>
        public List<ICreature> GetPopulation(CreatureFactory creatureFactory, Pathfinder pathfinder)
        {
            var population = new List<ICreature>();
            if (mDensity <= 0) return population;   // Catch division by zero.
            var rndInt = new Random();
            var rnd = new Random();
            for (double i = DistanceVertical / mDensity  + mArea.Y; i < mArea.Height + mArea.Y; i += DistanceVertical / mDensity )
            {
                for (double j = DistanceHorizontal / mDensity + mArea.X; j < mArea.Width + mArea.X; j += DistanceHorizontal / mDensity )
                {
                    if (mChance < rnd.NextDouble()) continue;
                    var position = new Vector2((float) j, (float) i);
                    if (mType == AreaType.Village)
                    {
                        ICreature peasant;
                        if (rnd.NextDouble() < 0.5d)
                        {
                            peasant = creatureFactory.CreateMalePeasant(position, (MovementDirection)rndInt.Next(8));
                        }
                        else
                        {
                            peasant = creatureFactory.CreateFemalePeasant(position, (MovementDirection)rndInt.Next(8));
                        }
                        if (!pathfinder.AllWalkable(peasant.BoundaryRectangle)) continue;
                        population.Add(peasant);
                    }
                    else if (mType == AreaType.Castle)
                    {
                        ICreature guard = creatureFactory.CreateKingsGuard(position, (MovementDirection)rndInt.Next(8));
                        if (!pathfinder.AllWalkable(guard.BoundaryRectangle)) continue;
                        population.Add(guard);
                    }
                }
            }
            return population;
        }

        /// <summary>
        /// Is a given creature standing in the area?
        /// </summary>
        /// <param name="creature">Check for this creature.</param>
        /// <returns></returns>
        public bool Contains(ICreature creature)
        {
            return mArea.Contains(creature.Position.ToPoint());
        }
    }
}