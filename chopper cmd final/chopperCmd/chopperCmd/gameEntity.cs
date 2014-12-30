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
     public class gameEntity
    {
        //image for the sprite
        public Texture2D Sprite;
        //position of the sprite
        public Vector2 Position;
        public Rectangle bounds;
        public Rectangle rectangle;
        public bool alive;
        public int health;

        public int currentframe;
        public float timer;
        public float interval = 5;

        public int fx = 157;
        public int fy = 100;

        public gameEntity()
        { 
        }

        public virtual void LoadContent()
        {
        }
        public virtual void Update(GameTime gameTime)
        {
        }
        public virtual void Draw(GameTime gameTime)
        {
        }
    }
}
