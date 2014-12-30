using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace chopperCmd
{
    class Background
    {
        public Vector2 Pos = new Vector2(0, 0);
        public Rectangle size;
        public float scale = 1.0f;
        private Texture2D mBackgroundTexture;


        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mBackgroundTexture = theContentManager.Load<Texture2D>(theAssetName);
            size = new Rectangle(0, 0, (int)(mBackgroundTexture.Width * scale), (int)(mBackgroundTexture.Height * scale));
        }
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mBackgroundTexture, Pos,
                new Rectangle(0, 0, mBackgroundTexture.Width, mBackgroundTexture.Height), Color.White,
                0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
