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
    public class enemies:gameEntity 
    {
        SoundEffect BulletImpact;
        SoundEffect RocketImpact;
        public float lastShot = 0.0f;       
        float shotlimit;
        float speed;                
        private bool _scoreApplied = false;
        public bool scoreApplied{
            get { return _scoreApplied; }
            set { _scoreApplied = value; }
        }

        public enemies(Vector2 pos) //basic contructor for enemy position and health  
        {
            Position = pos;
            health = 100;
        }
        public enemies(Vector2 pos, float speed, float shotlimit)// advance contructor for enemy contructor and enemy speed 
        {
            Position = pos;
            health = 100;
            this.speed = speed;
            this.shotlimit = shotlimit;
        }
         
        public override void LoadContent() 
        {

            alive = true;
            //Load enemy sprites sound effects
            Sprite = Game1.instance.Content.Load<Texture2D>("Enemies & Bosses\\enemy");
            BulletImpact = Game1.instance.Content.Load<SoundEffect>("Soundeffects\\BulletImpact");
            RocketImpact = Game1.instance.Content.Load<SoundEffect>("Soundeffects\\RocketExp");
            base.LoadContent();
        }

        
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            rectangle = new Rectangle(currentframe * 150, 0, 150, 50);
            bounds = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);

            
            float speed2 = 150.0f;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Position.X > 0 - Sprite.Width)
            {
                Position.X -= speed * delta;    //speed multiplied by gametime 
            }
            else 
            {
                alive = false;              //false if offscreen 
            }
            #region shooting
            if (health > 0)
            {
                if (lastShot > shotlimit)
                {
                    Bullet bullet = new Bullet(-1, "enemy");
                    bullet.LoadContent();
                    bullet.Position = Position;
                    bullet.Position.X += 100 / 2;
                    bullet.Position.Y += 60 / 2;
                    Game1.instance.bullets.Add(bullet);
                    lastShot = 0;
                }
                lastShot += delta;

                collideBullet();         //check for collision with bullet
                collideChopper();       //check for collision with chopper
                Animate(gameTime);
            }
            else if (health <= 0)
            {
                Position.Y += speed2 * delta;
                Position.X -= speed2 * delta;
                Sprite = Game1.instance.Content.Load<Texture2D>("Enemies & Bosses\\enemydeath");
                Animate(gameTime);
            }

            if (health <= 0 && !scoreApplied)  // score applied to chopper only once 
            {
                scoreApplied = true;                    
                Game1.instance.chopper.score += 100;
            }
            #endregion
            
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.instance.spriteBatch.Draw(Sprite,Position, rectangle, Color.White);
            
            base.Draw(gameTime);
        }

        void collideBullet()
        {
            for (int i = 0; i < Game1.instance.bullets.Count(); i++)
            {
                if (health >= 1)
                {       //if bounds of bullet intersect bounds of enemy minus 50 health and remove bullet
                    if (bounds.Intersects(Game1.instance.bullets[i].bounds) && Game1.instance.bullets[i].creator == "chopper")
                    {
                        BulletImpact.Play();
                        health -= 50;
                        Game1.instance.bullets.Remove(Game1.instance.bullets[i]);
                    }
                }
            }
            for (int i = 0; i < Game1.instance.rockets.Count(); i++)
            {//if bounds of rocket intersect bounds of enemy minus 50 health and remove rocket
                if (health >= 1)
                {
                    if (bounds.Intersects(Game1.instance.rockets[i].bounds) && Game1.instance.rockets[i].creator == "chopper")
                    {
                        RocketImpact.Play();
                        health -= 100;
                        Game1.instance.rockets.Remove(Game1.instance.rockets[i]);
                    }
                }
            }
        }
        
        void collideChopper()
        {
            if (bounds.Intersects(Game1.instance.chopper.bounds))
            { // if enemy bounds intersect chopper bounds health is mius 50 
                health -= 50;
                if (health <= 0)
                {// if health is less than 0 alive is false
                    alive = false;
                }
            }
        }
        public void Animate(GameTime gametime)
        {
            //int tinter = 100;
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds / 4;
            if (timer > interval)
            {
                currentframe++;     //animate the enemy sprite sheet 
                timer = 0;
                if (currentframe >= 4)
                {
                    currentframe = 0;
                }

            }
        }
    }
}
