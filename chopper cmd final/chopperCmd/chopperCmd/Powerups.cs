using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace chopperCmd
{
    public class Powerups:gameEntity
    {
        //animation speed
        private int interval2 = 35;
        public string type;
        public  Powerups(string type)
        {
            this.type = type;
        }

        public override void LoadContent()
        {
            alive = true;
            Sprite = Game1.instance.Content.Load<Texture2D>(type);

            //powerup spawn point
            Position.X = (Game1.instance.scrWidth/2);
            Position.Y = -10;
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //Draw a frame on sprite sheet or animation
            rectangle = new Rectangle(currentframe * 73, 0, 73, 80);
            bounds = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
            if (Position.Y > Game1.instance.scrHeight)
            {
                alive = false;
            }
            float speed = 45.0f;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position.Y += speed * delta;

            Animate(gameTime);

        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.instance.spriteBatch.Draw(Sprite, Position, rectangle, Color.White);

            base.Draw(gameTime);
        }

        public void Animate(GameTime gametime)
        {
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds / 4;
            if (timer > interval2)
            {
                currentframe++;
                timer = 0;
                if (currentframe >= 5)
                {
                    currentframe = 0;
                }

            }
        }
    }
}
