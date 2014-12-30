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
    
    public class BossEnemy:enemies
    {
        SoundEffect BulletPig;
        SoundEffect RocketImpact;
        int interval3 = 50;
        private string type;
        private int dir;
        float shotlimit;
        float speed;
        public BossEnemy(Vector2 pos, string type, int health, float speed, float shotlimit) : base(pos) { this.type = type; dir = -1; this.health = health; this.shotlimit = shotlimit; this.speed = speed; }//call the base constructor then call main constructor

        public override void LoadContent()
        {
            alive = true;
            Sprite = Game1.instance.Content.Load<Texture2D>("Enemies & Bosses\\"+type);
            BulletPig = Game1.instance.Content.Load<SoundEffect>("Soundeffects\\BulletPig");
            RocketImpact = Game1.instance.Content.Load<SoundEffect>("Soundeffects\\RocketExp");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            rectangle = new Rectangle(currentframe * 319, 0, 319, 143);
            bounds = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);


            
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            
                if (Position.X >= (Game1.instance.scrWidth - (319 + 50)))
                {
                    //move into position
                    Position.X -= speed * delta;
                }
                if (Position.X <= (Game1.instance.scrWidth - (319 + 50)))
                {
                    //now in position move up and down 
                    if (Position.Y <= 0 + 31)
                    {
                        dir = 1;
                    }

                    if (Position.Y >= (Game1.instance.scrHeight - Sprite.Height))
                    {
                        dir = -1;
                    }
                    Position.Y += speed * delta * dir;
                }

            if (lastShot > shotlimit)
            {
                Bullet bullet = new Bullet(-1, "enemy", "Enemies & Bosses\\sovietbacon");
                bullet.LoadContent();
                bullet.Position = Position;
                bullet.Position.X += 319 / 2;
                bullet.Position.Y += 130 / 2;
                Game1.instance.bullets.Add(bullet);
                lastShot = 0;
            }
            lastShot += delta;
            Animate(gameTime);

            //check for vaious collisions
            collideBullet();
            collideChopper();
            collideRockets();
        }
        public void collideBullet()
        {
            for (int i = 0; i < Game1.instance.bullets.Count(); i++)
            {
                //if bounds of bullet intersect bounds of boss minus 10 health and remove bullet
                if (bounds.Intersects(Game1.instance.bullets[i].bounds) && Game1.instance.bullets[i].creator == "chopper")
                {
                    BulletPig.Play();
                    health -= 10;
                    if (health <= 0)
                    {
                        alive = false;
                        Game1.instance.boss = false;
                        Game1.instance.chopper.score += 1000;
                    }
                    Game1.instance.bullets.Remove(Game1.instance.bullets[i]);
                }
            }
        }
        void collideRockets()
        {
            for (int i = 0; i < Game1.instance.rockets.Count(); i++)
            {
                //if bounds of rocket intersect bounds of enemy minus 100 health and remove bullet
                if (bounds.Intersects(Game1.instance.rockets[i].bounds))
                {
                    RocketImpact.Play();
                    health -= 100;
                    if (health <= 0)
                    {
                        alive = false;
                        Game1.instance.boss = false;
                        Game1.instance.chopper.score += 1000;
                    }
                    Game1.instance.rockets.Remove(Game1.instance.rockets[i]);
                }
            }
        }

        public void collideChopper()
        {
            if (bounds.Intersects(Game1.instance.chopper.bounds))
            {
                if (Game1.instance.chopper.shield)
                {
                    Game1.instance.chopper.shieldH -= 10;
                    if (Game1.instance.chopper.shieldH <= 0)
                    {
                        Game1.instance.chopper.shield = false;
                    }

                }
                else
                {
                    health -= 25;
                }
                if (health <= 0)
                {
                    alive = false;
                    Game1.instance.boss = false;
                    Game1.instance.chopper.score += 1000;
                }
            }
        }

        public void Animate(GameTime gametime)
        {
            //int tinter = 100;
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds / 4;
            if (timer > interval3)
            {
                currentframe++;
                timer = 0;
                if (currentframe >= 2)
                {
                    currentframe = 0;
                }

            }
        }

    }
     
}
