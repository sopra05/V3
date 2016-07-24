using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace V3.Objects
{
    /// <summary>
    /// A game object which is placed on the map.
    /// </summary>
    public interface IGameObject
    {
        Vector2 Position { get; set; }

        /// <summary>
        /// A unique ID for this game object, with which it can be identified.
        /// All implementations should use the IdGenerator to generate this ID.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Draws the game object on the screen.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used for drawing.</param>
        void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// The size of the object.
        /// </summary>
        Rectangle BoundaryRectangle { get; }

        /// <summary>
        /// Loads needed graphics.
        /// </summary>
        /// <param name="contentManager">Content manager used. </param>
        void LoadContent(ContentManager contentManager);

        /// <summary>
        /// Returns the object instance without modifications.
        /// </summary>
        /// <returns>This object.</returns>
        IGameObject GetSelf();
    }
}
