using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using V3.Camera;
using V3.Data;
using V3.Objects.Sprite;

namespace V3.Objects
{
    /// <summary>
    /// A moving game object.
    /// </summary>
    public interface ICreature : IGameObject
    {
        string Name { get; }
        Vector2 InitialPosition { set; }
        int Life { get; }
        int MaxLife { get; }
        int Speed { get; }
        int Attack { get; }
        int AttackRadius { get; }
        int SightRadius { get; }
        TimeSpan TotalRecovery { get; }
        TimeSpan Recovery { get; set; }
        /// <summary>
        /// Area where the creature is standing. Used for collisions.
        /// </summary>
        /// <summary>
        /// Where you can click to select the creature.
        /// </summary>
        Rectangle SelectionRectangle { get; }
        bool IsSelected { get; set; }
        ICreature IsAttacking { get; set; }
        IBuilding IsAttackingBuilding { get; set; }
        MovementDirection MovementDirection { get; set; }
        MovementState MovementState { get; set; }
        Faction Faction { get; }
        bool IsDead { get; }
        bool IsUpgraded { get; set; }

        /// <summary>
        /// Creature takes specific amount of damage. Substracted from Life.
        /// </summary>
        /// <param name="damage">TakeDamage taken.</param>
        void TakeDamage(int damage);

        /// <summary>
        /// Give command to move to desired destination. Not instant movement.
        /// </summary>
        /// <param name="destination">Destination in pixels.</param>
        void Move (Vector2 destination);

        //void ImportentMove(IGameObject creature);

        /// <summary>
        /// Draws a static non-animated sprite (for HUD) at specified position.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used for drawing.</param>
        /// <param name="position">Position where to draw the sprite.</param>
        void DrawStatic(SpriteBatch spriteBatch, Point position);

        /// <summary>
        /// Update the creature behaviour and animation.
        /// </summary>
        void Update(GameTime gameTime, ICreature playerCharacter, 
            bool rightButtonPressed, Vector2 rightButtonPosition, Quadtree quadtree, ICamera camera);

        /// <summary>
        /// Change the equipment/sprite of the creature to something other.
        /// If in debug mode the function throws an exception if the creature does not have the specified equipment slots.
        /// </summary>
        /// <param name="equipmentType">Which part of the equipment should be changed.</param>
        /// <param name="sprite">Which sprite to use instead.</param>
        void ChangeEquipment(EquipmentType equipmentType, ISpriteCreature sprite);

        /// <summary>
        /// Sets back the position of the creature to its state when created.
        /// </summary>
        void ResetPosition();

        /// <summary>
        /// Plays the specified animation fully, but only once.
        /// </summary>
        /// <param name="animation">For which movement state the animation should be played.</param>
        /// <param name="duration">How long (or how slow) should the animation be?</param>
        void PlayAnimationOnce(MovementState animation, TimeSpan duration);

        /// <summary>
        /// Heals the creature. Can not go over MaxLife.
        /// </summary>
        /// <param name="amount">How much HP the creature gains.</param>
        void Heal(int amount);

        /// <summary>
        /// Creature gets more life and maxlife. Used for testing in Techdemo.
        /// </summary>
        /// <param name="modifier">Multiplyier for Life.</param>
        void Empower(int modifier);

        /// <summary>
        /// Save this creature’s data to a CreatureData object.
        /// </summary>
        /// <returns>the CreatureData object with the status of this creature</returns>
        CreatureData SaveData();

        /// <summary>
        /// Restore the creature's state from the given data.
        /// </summary>
        /// <param name="data">the state of the creature to restore</param>
        void LoadData(CreatureData data);

        /// <summary>
        /// Restore the creature's references to other creatures from the given data.
        /// </summary>
        /// <param name="data">the state of the creature to restore</param>
        /// <param name="creatures">the list of all creatures by ID</param>
        void LoadReferences(CreatureData data, Dictionary<int, ICreature> creatures);
    }
}
