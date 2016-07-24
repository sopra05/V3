using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace V3.Objects
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class Transformation
    {
        public IObjectsManager ObjectsManager { private get; set; }
        private KeyboardState mOldStateSpecialattack;
        private KeyboardState mOldStateZombie;
        private KeyboardState mOldStateMeatball;
        private KeyboardState mOldStateSkeleton;
        private KeyboardState mOldStateHorse;
        internal Selection mSelection;
        private Texture2D mNecroArea;

        /// <summary>
        /// Call for transformations
        /// </summary>
        public void Transform()
        {
            KeyboardState newState = Keyboard.GetState();

            // Specialattack for Meatball (press 1)
            if (newState.IsKeyDown(Keys.D1))
            {
                if (!mOldStateSpecialattack.IsKeyDown(Keys.D1))
                {
                    mSelection.Specialattack();
                }
            }
            mOldStateSpecialattack = newState;

            // Transform Zombie (press 2)
            if (newState.IsKeyDown(Keys.D2))
            {
                if (!mOldStateZombie.IsKeyDown(Keys.D2))
                {
                    mSelection.TransformZombie();
                }
            }
            mOldStateZombie = newState;

            // Transform Meatball (press 3)
            // Nneed five zombies
            if (newState.IsKeyDown(Keys.D3))
            {
                if (!mOldStateMeatball.IsKeyDown(Keys.D3))
                {
                    mSelection.TransformMeatball();
                }
            }
            mOldStateMeatball = newState;

            // Transform Skeleton (press 4)
            // Need one zombie
            if (newState.IsKeyDown(Keys.D4))
            {
                if (!mOldStateSkeleton.IsKeyDown(Keys.D4))
                {
                    mSelection.TransformSkeleton();
                }
            }
            mOldStateSkeleton = newState;

            // Transform SkeletonHorse (press 5)
            // Nneed 3 skeletons
            if (newState.IsKeyDown(Keys.D5))
            {
                if (!mOldStateHorse.IsKeyDown(Keys.D5))
                {
                    mSelection.TransformSkeletonHorse();
                }
            }
            mOldStateHorse = newState;
        }

        //Draw(SpriteBatch spriteBatch, Vector2 position, MovementState movementState, MovementDirection movementDirection)
        /// <summary>
        /// Load the selection and ellipse sprites for necromancers area
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        public void LoadArea(ContentManager contentManager)
        {
            //mNecroArea = contentManager.Load<Texture2D>("Sprites/selection");
            mNecroArea = contentManager.Load<Texture2D>("Sprites/ellipse");
        }

        /// <summary>
        /// Draw the area for necromancer
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use for drawing the object</param>
        public void DrawNecroArea(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(mNecroArea, ObjectsManager.PlayerCharacter.Position - new Vector2(800, 400), null, Color.Red*0.3f, 0, Vector2.Zero, 25, SpriteEffects.None, 0);
            spriteBatch.Draw(mNecroArea, ObjectsManager.PlayerCharacter.Position - new Vector2(640, 320), null, Color.Red*0.5f, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 0);
        }
    }
}