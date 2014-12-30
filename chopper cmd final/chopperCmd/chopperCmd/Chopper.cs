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
    public class Chopper:gameEntity
    {
        //Chopper shooting variables
        public float lastShot = 0;
        public float shotLimitB = 0.5f;
        public float shotlimitR = 0.5f;
        public bool EnRockets = false;
        public int rockets = 0;

        //Shield variable
        public bool shield = false;
        public int shieldH;


        
        private int chHeight= 104, chWidth=59;
        
        
        int rec_Y = 0;
        SpriteFont rocketnumber;
        #region SoundEffects
        SoundEffect Gun;
        SoundEffect Rocket;
        SoundEffect PowerB;
        SoundEffect PowerSh;
        SoundEffect PowerR;
        #endregion

        //private score with getter and setter
        private int _score = 0;
        public int score
        {
            get { return this._score; }
            set { this._score = value; }
        }
        //Set lives
        private int _noLives = 3;

        public int noLives
        {
            get{return this._noLives;}
        }
        public Chopper()
        {
            health = 100;
        }
        
        public override void LoadContent()
        {
            //load the chopper image
            Sprite = Game1.instance.Content.Load<Texture2D>("Chopper Sprites\\chopper");
            //Load ocket image
            rocketnumber = Game1.instance.Content.Load<SpriteFont>("rocketnumber");
            //center the sprite on the y axis
            Position.X = 10;
            Position.Y = (Game1.instance.scrHeight/2) -(Sprite.Height/2);
            alive = true;
            #region LoadSoundEffects
            Gun = Game1.instance.Content.Load<SoundEffect>("Soundeffects\\GunSlow");
            Rocket = Game1.instance.Content.Load<SoundEffect>("Soundeffects\\Rocket");
            PowerB = Game1.instance.Content.Load<SoundEffect>("Soundeffects\\PowerUpBullet");
            PowerSh = Game1.instance.Content.Load<SoundEffect>("Soundeffects\\PowerUpShield");
            PowerR = Game1.instance.Content.Load<SoundEffect>("Soundeffects\\PowerUpRocket");
            #endregion

            base.LoadContent();
        }
        
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            rectangle = new Rectangle(currentframe * fx, rec_Y, fx, fy);
            bounds = new Rectangle((int)Position.X, (int)Position.Y + 5, chHeight, chWidth);

            #region movementLogic
            float speed =200.0f;
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            KeyboardState KeyState = Keyboard.GetState();
            GamePadState pad = GamePad.GetState(PlayerIndex.One);
            if ((Position.Y <= (Game1.instance.scrHeight - (chHeight - 18)) && (Position.Y > 31)) && (Position.X <= (Game1.instance.scrWidth - (chWidth - 25)) && (Position.X > 0)))
            {   
                if (KeyState.IsKeyDown(Keys.Up) || pad.ThumbSticks.Left.Y >0)
                {
                    Position.Y -= speed*delta;
                }
                
                if (KeyState.IsKeyDown(Keys.Down)||pad.ThumbSticks.Left.Y <0)
                {
                    Position.Y += speed*delta;
                }
                
                if (KeyState.IsKeyDown(Keys.Left)||pad.ThumbSticks.Left.X <0)
                {
                    Position.X -= speed*delta;
                }
                
                if (KeyState.IsKeyDown(Keys.Right) || pad.ThumbSticks.Left.X >0)
                {
                    Position.X += speed*delta;
                }
               
                if (KeyState.IsKeyDown(Keys.F) || pad.IsButtonDown(Buttons.RightShoulder))
                {
                    speed = 450.0f;
                    Position.X += speed * delta;
                }

                if (KeyState.IsKeyDown(Keys.B) || pad.IsButtonDown(Buttons.LeftShoulder))
                {
                    speed = 450.0f;
                    Position.X -= speed * delta;
                }
               
            }
            else
            {
                //call bounce to stop the sprite moving off the screen
                bounce();
            }
            #endregion

            //check for various collisions
            collideBullet();
            collidePowerup();
            collideEnemy();
            Animate(gameTime);
            

            #region shooting
            GamePadState previouspad;
            KeyboardState oldKeyState;
            if (/*KeyState.IsKeyDown(Keys.Space) ||*/ pad.Triggers.Right > 0 && (lastShot > shotLimitB))
            {
                if (health > 100 && !shield)
                {
                    rec_Y = 0;
                }
                else if (health <= 100 && !shield)
                {
                    rec_Y = 99;
                }
                Gun.Play();
                Bullet bullet = new Bullet(1, "chopper");
                bullet.LoadContent();
                bullet.Position = Position;
                bullet.Position.X += fx - 55;
                bullet.Position.Y += fy - 45;
                Game1.instance.bullets.Add(bullet);
                lastShot = 0;

            }
            
            if (KeyState.IsKeyDown(Keys.Space) /*|| pad.Triggers.Right > 0*/ && (lastShot > shotLimitB))
            {
                rec_Y = 99;
                Gun.Play();
                Bullet bullet = new Bullet(1, "chopper");
                bullet.LoadContent();
                bullet.Position = Position;
                bullet.Position.X += fx - 55;
                bullet.Position.Y += fy - 45;
                Game1.instance.bullets.Add(bullet);
                lastShot = 0;

            }
            if (/*KeyState.IsKeyUp(Keys.Space) ||*/  pad.Triggers.Right == 0)
            {
                rec_Y = 0;
            }
            previouspad = pad;
            oldKeyState = KeyState;
            if (EnRockets && lastShot > shotlimitR && (KeyState.IsKeyDown(Keys.R) || pad.Triggers.Left > 0) && rockets > 0)
            {
                Rocket.Play();
                rockets rocket = new rockets(1, "chopper");
                rocket.LoadContent();
                rocket.Position = Position;
                rocket.Position.X += fx - 60;
                rocket.Position.Y += fy - 47;
                Game1.instance.rockets.Add(rocket);
                rockets--;
                lastShot = 0;
            }
            
            lastShot += delta;
            #endregion

        }
        private void bounce()
        {
            if (Position.Y >= (Game1.instance.scrHeight - (chHeight - 18)))
            {
                //if sitting against the top move the sprite down 1
                Position.Y -= 1;
            }

            if (Position.Y <= 31)
            {
                //if sitting on the bottom move the sprite up 1
                Position.Y += 1;
            }

            if (Position.X >= (Game1.instance.scrWidth - (chWidth - 25)))
            {
                //if touching the right edge move back 1
                Position.X -= 1;
            }

            if (Position.X <= 0)
            {
                //if touching the left edge move forward 1
                Position.X += 1;
            }
        }

        public void Animate(GameTime gametime)
        {
            timer += (float)gametime.ElapsedGameTime.TotalMilliseconds / 4;
            if (timer > interval)
            {
                currentframe++;
                timer = 0;
                if (currentframe >= 4)
                {
                    currentframe = 0;
                }

            }
        }
        #region collisionDection
        public void collideBullet()
        {
            for (int i = 0; i < Game1.instance.bullets.Count(); i++)
            {
                if (bounds.Intersects(Game1.instance.bullets[i].bounds) && Game1.instance.bullets[i].creator == "enemy")
                {
                   
                    if (shield)
                    {
                        shieldH -= 5;
                        Sprite = Game1.instance.Content.Load<Texture2D>("Chopper Sprites\\choppershield");
                        if (shieldH <= 0)
                        {
                            shield = false;
                        }

                    }
                    else
                    {
                        health -= 5;
                        Sprite = Game1.instance.Content.Load<Texture2D>("Chopper Sprites\\chopper");
                    }
                    if (health <= 0)
                    {
                        _noLives--;
                        if (_noLives < 0)
                        {
                            alive = false;
                        }
                        else
                        {
                            health = 100;
                        }
                    }
                    Game1.instance.bullets.Remove(Game1.instance.bullets[i]);
                }
            }
        }

        public void collidePowerup()
        {
            for(int i =0;i < Game1.instance.powerUp.Count();i++)
            {
                if (bounds.Intersects(Game1.instance.powerUp[i].bounds) && Game1.instance.powerUp[i].type == "Powerups\\PowerupBulletLv1")
                {
                    PowerB.Play();
                    shotLimitB = 0.4f;
                    Game1.instance.powerUp[i].alive = false;
                }
                if (bounds.Intersects(Game1.instance.powerUp[i].bounds) && Game1.instance.powerUp[i].type == "Powerups\\PowerupBulletLv2")
                {
                    PowerB.Play();
                    shotLimitB = 0.2f;
                    Game1.instance.powerUp[i].alive = false;
                }
                if (bounds.Intersects(Game1.instance.powerUp[i].bounds) && Game1.instance.powerUp[i].type == "Powerups\\PowerupBulletLv3")
                {
                    PowerB.Play();
                    shotLimitB = 0.1f;
                    Game1.instance.powerUp[i].alive = false;
                }
                if (bounds.Intersects(Game1.instance.powerUp[i].bounds) && Game1.instance.powerUp[i].type == "Powerups\\PowerupRockets")
                {
                    PowerR.Play();
                    EnRockets = true;
                    rockets += 10;
                    Game1.instance.powerUp[i].alive = false;
                }
                if (bounds.Intersects(Game1.instance.powerUp[i].bounds) && Game1.instance.powerUp[i].type == "Powerups\\PowerupShield")
                {
                    PowerSh.Play();
                    shield = true;
                    shieldH = 30;
                    if (shield == true)
                    {
                        Sprite = Game1.instance.Content.Load<Texture2D>("Chopper Sprites\\choppershield");
                    }
                    Game1.instance.powerUp[i].alive = false;
                }

            }
        }



        public void collideEnemy()
        {
            for (int i = 0; i < Game1.instance.enemy.Count(); i++)
            {
                if (bounds.Intersects(Game1.instance.enemy[i].bounds) && !Game1.instance.enemy[i].scoreApplied)
                {
                    if (shield)
                    {
                        shieldH -= 5;
                        Sprite = Game1.instance.Content.Load<Texture2D>("Chopper Sprites\\choppershield");
                        if (shieldH <= 0)
                        {
                            shield = false;
                        }

                    }
                    else
                    {
                        health -= 10;
                        Sprite = Game1.instance.Content.Load<Texture2D>("Chopper Sprites\\chopper");
                    }
                    if (health <= 0)
                    {
                        _noLives--;
                        if (_noLives < 0)
                        {
                            alive = false;
                        }
                        else
                        {
                            health = 100;
                        }
                    }
                }
            }
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {
            Game1.instance.spriteBatch.Draw(Sprite, Position, rectangle, Color.White);
            if (rockets > 0)
            {
                Game1.instance.spriteBatch.DrawString(rocketnumber, "R:" + rockets, new Vector2(150, 2.5f), Color.Black);
            }
            
            base.Draw(gameTime);
        }
    }
}
