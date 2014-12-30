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
    public class rockets:gameEntity
    {
        /*public rockets(int dir, string whoMadeMe)
            : base(dir, whoMadeMe){}*/
        public int direction = 0;
        public string creator;
        public float speed;
        int R_interval = 50;
        public  rockets(int dir, string whoMadeMe)
        {
            direction = dir;
            creator = whoMadeMe;
        }

        public override void LoadContent()
        {
            alive = true;
            Sprite = Game1.instance.Content.Load<Texture2D>("rockets");
            rectangle = new Rectangle(currentframe * 35, 0, 35, 22);
        }
        public override void Update(GameTime gameTime)
        {
            bounds = new Rectangle((int)Position.X, (int)Position.Y, 35, 22);
            float delta= (float)gameTime.ElapsedGameTime.TotalSeconds;
            speed = 1000.0f;
            Position.X += speed * delta *direction;
            int width = Game1.instance.GraphicsDevice.Viewport.Width;
            if (Position.X > width)
            {
                alive = false;
            }
            Animate(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 Center = new Vector2(0, 0);
            Game1.instance.spriteBatch.Draw(Sprite, Position, rectangle, Color.White, 0, Center, 1, SpriteEffects.None, 0);
            base.Draw(gameTime);
        }

        public void Animate(GameTime gametime)
        {
            //int tinter = 100;
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds / 4;
            if (timer > R_interval)
            {
                currentframe++;
                timer = 0;
                if (currentframe >= 4)
                {
                    currentframe = 0;
                }

            }
        }
    }
}
