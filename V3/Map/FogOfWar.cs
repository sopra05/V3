using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Objects;

namespace V3.Map
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class FogOfWar
    {
        private const int FogRange = 64;
        private const int SightRadius = 1000;
        private Point mMapSize;
        private Texture2D mFog;
        private readonly List<Rectangle> mFogRectangle = new List<Rectangle>();

        /// <summary>
        /// Get the size of the map and create an boolean array
        /// </summary>
        /// <param name="size">the size of the map</param>
        public void LoadGrid(Point size)
        {
            mMapSize = size;
            CreateArray();
        }

        /// <summary>
        /// An array so save whether the sprites already walked on this area
        /// </summary>
        private void CreateArray()
        {
            for (int i = -FogRange; i < mMapSize.Y; i += FogRange)
            {
                for (int j = -FogRange * 2; j < mMapSize.X; j += FogRange)
                {
                    mFogRectangle.Add(new Rectangle(j, i, mFog.Width, mFog.Height));
                }
            }
        }

        /// <summary>
        /// The position from creatures which can open the fog
        /// </summary>
        /// <param name="creature">creatures which are able to open the fog</param>
        public void Update(ICreature creature)
        {
            Ellipse creatureEllipse = new Ellipse(creature.Position, SightRadius, SightRadius);
            var markedForDeletion = new List<Rectangle>();
            foreach (var fog in mFogRectangle)
            {
                if (!creature.IsDead && creatureEllipse.Contains(fog.Center.ToVector2()))
                {
                    markedForDeletion.Add(fog);
                }
            }
            foreach (var fogToDelete in markedForDeletion)
            {
                mFogRectangle.Remove(fogToDelete);
            }
        }

        /// <summary>
        /// The sprite for the fog
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            mFog = content.Load<Texture2D>("Sprites/cloud");
        }

        /// <summary>
        /// Try to draw fog of war efficiently.
        /// </summary>
        /// <param name="spriteBatch">Sprite batch used.</param>
        public void DrawFog(SpriteBatch spriteBatch)
        {
            /*
            var screen = camera.ScreenRectangle;
            int fogPerRow = (mMapSize.X + FogRange) / FogRange;
            int fogPerColumn = (mMapSize.Y + FogRange) / FogRange;
            for (int i = screen.Y / FogRange; i < (screen.Y + screen.Height) / FogRange; i++)
            {
                for (int j = screen.X / FogRange; j < (screen.X + screen.Width) / FogRange; j++)
                {
                    spriteBatch.Draw(mFog, mFogRectangle[i * fogPerRow], Color.Black);
                }
            }
            */
            foreach (var fog in mFogRectangle)
            {
                spriteBatch.Draw(mFog, fog, Color.Black);
            }
        }

        public void SetFog(List<Rectangle> fog)
        {
            mFogRectangle.Clear();
            mFogRectangle.AddRange(fog);
        }

        public List<Rectangle> GetFog()
        {
            return mFogRectangle;
        }
    }
}
