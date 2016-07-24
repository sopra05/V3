using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using V3.Data;

namespace V3.Effects
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class EffectsManager : IEffectsManager
    {
        private readonly ContentManager mContentManager;
        private readonly IOptionsManager mOptionsManager;
        private readonly List<IEffect> mActiveEffects = new List<IEffect>();

        public EffectsManager(ContentManager contentManager, IOptionsManager optionsManager)
        {
            mContentManager = contentManager;
            mOptionsManager = optionsManager;
        }

        public void Update(GameTime gameTime)
        {
            var doneEffects = new List<IEffect>();
            foreach (var effect in mActiveEffects)
            {
                effect.Update(gameTime);
                if (!effect.IsPlaying)
                {
                    doneEffects.Add(effect);
                }
            }
            foreach (var doneEffect in doneEffects)
            {
                mActiveEffects.Remove(doneEffect);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var effect in mActiveEffects)
            {
                effect.Draw(spriteBatch);
            }
        }

        public void PlayOnce(IEffect effect, Point position, Point size)
        {
            effect.LoadContent(mContentManager);
            mActiveEffects.Add(effect);
            effect.PlayOnce(position, size, mOptionsManager);
        }
    }
}