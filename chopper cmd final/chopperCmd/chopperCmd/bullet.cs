using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace chopperCmd
{
    public class Bullet : gameEntity
    {
        public int direction = 0;
        public string creator;
        string type;
        public  Bullet(int dir, string whoMadeMe)//basic constructor for bullet
        {
            direction = dir;
            creator = whoMadeMe;
            type = "bullet";
        }
        public Bullet(int dir, string whoMadeMe, string type)//advance constructor to be used by boss
        {
            direction = dir;
            creator = whoMadeMe;
            this.type = type;
        }
        public override void LoadContent()
        {
            alive = true;
            Sprite = Game1.instance.Content.Load<Texture2D>(type);
        }
        public override void Update(GameTime gameTime)
        {
            bounds = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
            float delta= (float)gameTime.ElapsedGameTime.TotalSeconds;
            float speed = 500.0f;
            Position.X += speed * delta *direction;
            //set alive to false if bullet goes of the screen
            if (Position.X > Game1.instance.scrWidth || Position.X < 0)
            {
                alive = false;
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            Game1.instance.spriteBatch.Draw(Sprite, Position, Color.White);
            base.Draw(gameTime);
        }
    }
}