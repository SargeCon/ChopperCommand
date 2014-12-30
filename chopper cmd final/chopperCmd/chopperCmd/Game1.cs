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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace chopperCmd
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static Game1 instance;

        
        public Chopper chopper = new Chopper();

        //List for GameEntity
        public List<Bullet> bullets = new List<Bullet>();
        public List<rockets> rockets = new List<rockets>();
        public List<enemies> enemy = new List<enemies>();
        public List<Powerups> powerUp = new List<Powerups>();

        //Enemy spawn variables
        TimeSpan EnemySpawn;
        TimeSpan previousEnemySpawn;

        //Enemy speed and shooting speed
        int EnemySpeed = 60;
        float Enshotlimit = 1.0f;

        //To ckeck to see if a boss is on the screen
        private bool _boss;
        public bool boss
        {
            get { return _boss; }
            set { _boss = value; }
        }

        #region MenuSystem textures and bools
        private Texture2D Main_Menu;
        private Texture2D Tutorial;
        private Texture2D Tutorial_Menu2;
        private Texture2D Tutorial_Menu3;
        private Texture2D Credits;
        private Texture2D Pause_Menu;
        private Texture2D Death_Screen;


        private bool M_Menu;
        private bool Tut;
        private bool Tut2;
        private bool Tut3;
        private bool Cred;
        private bool GameState;
        private bool Pause;
        private bool death;
        #endregion

        #region Spritefonts
        public SpriteFont font;
        public SpriteFont text;
        public SpriteFont score;
        #endregion

        //counter variables
        float count_speed = 0.0f;
        float counter = 0;
        int count = 0;

        public int scrHeight =500, scrWidth =700;

        //Main song and soundeffects
        public Song song;
        SoundEffect Freedom;
        SoundEffect chopperroter;
        SoundEffectInstance Heli;

        

        #region Background
        int BackgroundSpeed = 100;

        Background mBackgroundOne;
        Background mBackgroundTwo;
        Background mBackgroundThree;
        Background mBackgroundFour;
        Background mBackgroundFive;
        #endregion

        KeyboardState oldKeyState;
        GamePadState oldPad;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //initalise instance with Game1
            instance = this;
        }

        protected override void Initialize()
        {
            //set the window height and width
            graphics.PreferredBackBufferWidth = scrWidth;
            graphics.PreferredBackBufferHeight = scrHeight;
            boss = false;
            //disable full screen
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "ChopperCmd";

            //Timing enemy spawn 
            previousEnemySpawn= TimeSpan.Zero;
            EnemySpawn = TimeSpan.FromSeconds(2.5f);

            #region New Background
            mBackgroundOne = new Background();

            mBackgroundTwo = new Background();

            mBackgroundThree = new Background();

            mBackgroundFour = new Background();
          
            mBackgroundFive = new Background();
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            #region MenuSystem textures
            Main_Menu = Content.Load<Texture2D>("Menus\\MM");
            Tutorial = Content.Load<Texture2D>("Menus\\Tutorial");
            Credits = Content.Load<Texture2D>("Menus\\Credits");
            Pause_Menu = Content.Load<Texture2D>("Menus\\Pause");
            Tutorial_Menu2 = Content.Load<Texture2D>("Menus\\Tutorial2");
            Tutorial_Menu3 = Content.Load<Texture2D>("Menus\\Tutorial3");
            Death_Screen = Content.Load<Texture2D>("Menus\\Game Over");
            #endregion

            #region Menu bool values
            M_Menu = true;
            Tut = false;
            Tut2 = false;
            Tut3 = false;
            Cred = false;
            GameState = false;
            Pause = false;
            death = false;
            #endregion

            //Load counter spritefont
            font = Content.Load<SpriteFont>("counter");


            //Load Health spritefont
            text = Content.Load<SpriteFont>("text");

            score = Content.Load<SpriteFont>("Score");

            #region Background Textures
            mBackgroundOne.LoadContent(this.Content, "Background\\Background01");
            mBackgroundOne.Pos = new Vector2(0, 0);

            mBackgroundTwo.LoadContent(this.Content, "Background\\Background02");
            mBackgroundTwo.Pos = new Vector2(mBackgroundOne.Pos.X + mBackgroundOne.size.Width, 0);

            mBackgroundThree.LoadContent(this.Content, "Background\\Background03");
            mBackgroundThree.Pos = new Vector2(mBackgroundTwo.Pos.X + mBackgroundTwo.size.Width, 0);

            mBackgroundFour.LoadContent(this.Content, "Background\\Background04");
            mBackgroundFour.Pos = new Vector2(mBackgroundThree.Pos.X + mBackgroundThree.size.Width, 0);

            mBackgroundFive.LoadContent(this.Content, "Background\\Background05");
            mBackgroundFive.Pos = new Vector2(mBackgroundFour.Pos.X + mBackgroundFour.size.Width, 0);
            #endregion
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load powerup textures
            for (int i = 0; i < powerUp.Count(); i++)
            {
                    powerUp[i].LoadContent();
            }

            //load the chopper
            chopper.LoadContent();

            #region songs and sound effects
            song = Content.Load<Song>("ChopperCmd");
            Freedom = Content.Load<SoundEffect>("Soundeffects\\ForFreedom");
            chopperroter = Content.Load<SoundEffect>("Soundeffects\\Helicopter");
            Heli = chopperroter.CreateInstance();
            Heli.IsLooped = true;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
            #endregion
            
            //Load enemies
            for (int i = 0; i < enemy.Count(); i++)
            {
                enemy[i].LoadContent();
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
                
            KeyboardState keyState =Keyboard.GetState();

            //Allows player to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
            {
                Exit();

            }
            

            #region Load main game
            if (GameState)
            {
                if (!chopper.alive)
                    this.Exit();
                if (!boss)
                {
                    genEnemies(gameTime);
                }
                #region Background

                if (mBackgroundOne.Pos.X < -mBackgroundOne.size.Width)
                {
                    mBackgroundOne.Pos.X = mBackgroundFive.Pos.X + mBackgroundFive.size.Width;
                }

                if (mBackgroundTwo.Pos.X < -mBackgroundTwo.size.Width)
                {
                    mBackgroundTwo.Pos.X = mBackgroundOne.Pos.X + mBackgroundOne.size.Width;
                }

                if (mBackgroundThree.Pos.X < -mBackgroundThree.size.Width)
                {
                    mBackgroundThree.Pos.X = mBackgroundTwo.Pos.X + mBackgroundTwo.size.Width;
                }

                if (mBackgroundFour.Pos.X < -mBackgroundFour.size.Width)
                {
                    mBackgroundFour.Pos.X = mBackgroundThree.Pos.X + mBackgroundThree.size.Width;
                }

                if (mBackgroundFive.Pos.X < -mBackgroundFive.size.Width)
                {
                    mBackgroundFive.Pos.X = mBackgroundFour.Pos.X + mBackgroundFour.size.Width;
                }

                Vector2 bDirection = new Vector2(-1, 0);
                Vector2 bSpeed = new Vector2(BackgroundSpeed, 0);

                mBackgroundOne.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                mBackgroundTwo.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                mBackgroundThree.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                mBackgroundFour.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                mBackgroundFive.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                #endregion 
                
                counter += count_speed;

                #region Enemy and boss spawn logic
                if (boss)
                {
                    count_speed = 0.0f; 
                }
                if(boss == false)
                {
                    count_speed = 1.0f;
                }
                if(boss == false && (counter >= 1002 && counter <=2502))
                {
                    count_speed = 1.5f;
                    BackgroundSpeed = 200;
                    EnemySpawn = TimeSpan.FromSeconds(2.2f);
                    EnemySpeed = 80;
                    Enshotlimit = 0.9f;
                }
                if (boss == false && (counter >= 2502 && counter <=6002))
                {
                    count_speed = 2.0f;
                    BackgroundSpeed = 350;
                    EnemySpeed = 100;
                    EnemySpawn = TimeSpan.FromSeconds(1.7f);
                    Enshotlimit = 0.7f;
                }
                if (boss == false && (counter >= 6002 && counter <=8002))
                {
                    count_speed = 2.5f;
                    BackgroundSpeed = 500;
                    EnemySpawn = TimeSpan.FromSeconds(1.5f);
                    EnemySpeed = 120;
                    Enshotlimit = 0.6f;
                }
                if (boss == false && (counter >= 8002 && counter <= 12002))
                {
                    EnemySpawn = TimeSpan.FromSeconds(1.2f);
                    EnemySpeed = 160;
                    Enshotlimit = 0.5f;
                }
                if (boss == false && (counter >= 12002 && counter <= 16002))
                {
                    count_speed = 3.0f;
                    BackgroundSpeed = 600;
                    EnemySpawn = TimeSpan.FromSeconds(1.0f);
                    EnemySpeed = 170;
                    Enshotlimit = 0.45f;
                }
                if (boss == false && (counter >= 16002 && counter <= 20002))
                {
                    EnemySpawn = TimeSpan.FromSeconds(0.7f);
                    BackgroundSpeed = 700;
                    EnemySpeed = 190;
                    Enshotlimit = 0.4f;
                }
                if (boss == false && (counter >= 20002 && counter <= 24002))
                {
                    EnemySpawn = TimeSpan.FromSeconds(0.5f);
                    EnemySpeed = 200;
                }
                count = Convert.ToInt32(counter);

                if (count == 1000 && !boss)
                {
                    BossEnemy temp = new BossEnemy(new Vector2(800, 150), "boss", 300, 100, 1.2f);
                    temp.LoadContent();
                    enemy.Add(temp);
                    
                    boss = true;
                    
                }
                if (count == 2500  && !boss)
                {
                    BossEnemy temp = new BossEnemy(new Vector2(800, 150), "boss", 900, 200, 0.9f);
                    temp.LoadContent();
                    enemy.Add(temp);
                    boss = true;
                    
                }
                if (count == 6000 && !boss)
                {
                    BossEnemy temp = new BossEnemy(new Vector2(800, 150), "boss", 1400, 300, 0.7f);
                    temp.LoadContent();
                    enemy.Add(temp);
                    boss = true;

                }
                if (count == 12000 && !boss)
                {
                    BossEnemy temp = new BossEnemy(new Vector2(800, 150), "boss", 1900, 400, 0.5f);
                    temp.LoadContent();
                    enemy.Add(temp);
                    boss = true;
                }
                if (count == 24000 && !boss)
                {
                    BossEnemy temp = new BossEnemy(new Vector2(800, 150), "boss", 2200, 400, 0.3f);
                    temp.LoadContent();
                    enemy.Add(temp);
                    boss = true;
                }
                #endregion
                #region spawn powerups
                if (count == 800)
                {
                    Powerups temp = new Powerups("Powerups\\PowerupBulletLv1");
                    temp.LoadContent();
                    powerUp.Add(temp);
                }
                if (count == 4700)
                {
                    Powerups temp = new Powerups("Powerups\\PowerupBulletLv2");
                    temp.LoadContent();
                    powerUp.Add(temp);
                }
                if (count == 10500)
                {
                    Powerups temp = new Powerups("Powerups\\PowerupBulletLv3");
                    temp.LoadContent();
                    powerUp.Add(temp);
                }
                if (count == 1500 || count == 4200 || count == 7200 || count == 12200)
                {
                    Powerups temp = new Powerups("Powerups\\PowerupRockets");
                    temp.LoadContent();
                    powerUp.Add(temp);
                }
                if (count == 1100 || count == 5700 || count == 10500 || count == 18000)
                {
                    Powerups temp = new Powerups("Powerups\\PowerupShield");
                    temp.LoadContent();
                    powerUp.Add(temp);
                }
                #endregion
                #region Load bullets
                for (int i = 0; i < bullets.Count(); i++)
                {
                    if (bullets[i].alive)
                    {
                        bullets[i].Update(gameTime);
                    }
                    else
                    {
                        bullets.Remove(bullets[i]);
                    }
                }
                #endregion
                #region Load Rockets
                for (int i = 0; i < rockets.Count(); i++)
                {
                    if (rockets[i].alive)
                    {
                        rockets[i].Update(gameTime);
                    }
                    else
                    {
                        rockets.Remove(rockets[i]);
                    }
                }
                #endregion
                #region Load powerups
                for (int i = 0; i < powerUp.Count(); i++)
                {
                    if (powerUp[i].alive)
                    {
                        powerUp[i].Update(gameTime);
                    }
                    else
                    {
                        powerUp.Remove(powerUp[i]);
                    }

                }
                #endregion
                chopper.Update(gameTime);

                //loads and removes enemies
                for (int i = 0; i < enemy.Count(); i++)
                {
                    if (enemy[i].alive)
                    {
                        enemy[i].Update(gameTime);
                    }
                    else
                    {
                        enemy.Remove(enemy[i]);
                    }
                }
               
                base.Update(gameTime);
            }
            #endregion

            #region menusystem
            if (M_Menu)
            {
                KeyboardState KeyState = Keyboard.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                if (KeyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space) || 
                    (pad.Buttons.A == ButtonState.Pressed && oldPad.Buttons.A == ButtonState.Released))
                {
                    M_Menu = false;
                    Tut = true;
                }
                if (KeyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter) || 
                    (pad.Buttons.Start == ButtonState.Pressed && oldPad.Buttons.Start == ButtonState.Released))
                {
                    Freedom.Play();
                    Heli.Play();
                    
                    M_Menu = false;
                    GameState = true;
                }
                if ((KeyState.IsKeyDown(Keys.A) && oldKeyState.IsKeyUp(Keys.A)) ||
                    (pad.Buttons.B == ButtonState.Pressed && oldPad.Buttons.B == ButtonState.Released))
                {
                    M_Menu = false;
                    Cred = true;
                }
                oldKeyState = KeyState;
                oldPad = pad;
                
            }
            if (Tut)
            {
                KeyboardState KeyState = Keyboard.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                if (KeyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space) ||
                    (pad.IsConnected && pad.Buttons.B == ButtonState.Pressed && oldPad.Buttons.B == ButtonState.Released))
                {
                    M_Menu = true;
                    Tut = false;
                }
                if ((KeyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter)) ||
                    ((pad.IsConnected && pad.Buttons.A == ButtonState.Pressed && oldPad.Buttons.A == ButtonState.Released)))
                {
                    Tut = false;
                    Tut2 = true;
                }
                oldPad = pad;
                oldKeyState = KeyState;
            }
            if (Tut2)
            {
                KeyboardState KeyState = Keyboard.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                if (KeyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space) ||
                    (pad.IsConnected && pad.Buttons.B == ButtonState.Pressed && oldPad.Buttons.B == ButtonState.Released))
                {
                    Tut = true;
                    Tut2 = false;
                }
                if ((KeyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter)) ||
                    ((pad.IsConnected && pad.Buttons.A == ButtonState.Pressed && oldPad.Buttons.A == ButtonState.Released)))
                {
                    Tut2 = false;
                    Tut3 = true;
                }
                oldPad = pad;
                oldKeyState = KeyState;
            }
            if (Tut3)
            {
                KeyboardState KeyState = Keyboard.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                if (KeyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space) ||
                    (pad.IsConnected && pad.Buttons.B == ButtonState.Pressed && oldPad.Buttons.B == ButtonState.Released))
                {
                    Tut2 = true;
                    Tut3 = false;
                }
                if ((KeyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter)) ||
                    ((pad.IsConnected && pad.Buttons.A == ButtonState.Pressed && oldPad.Buttons.A == ButtonState.Released)))
                {
                    Tut3 = false;
                    GameState = true;
                }
                oldPad = pad;
                oldKeyState = KeyState;
            }
            if (Cred)
            {
                KeyboardState KeyState = Keyboard.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                if (KeyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyUp(Keys.Space) || 
                    (pad.IsConnected && pad.Buttons.B == ButtonState.Pressed && oldPad.Buttons.B == ButtonState.Released))
                {
                    M_Menu = true;
                    Cred = false;

                }
                oldKeyState = KeyState;
                oldPad = pad;
            }
            if (GameState)
            {
                KeyboardState KeyState = Keyboard.GetState();
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                if ((KeyState.IsKeyDown(Keys.P) && oldKeyState.IsKeyUp(Keys.P)) || 
                    (pad.Buttons.Start == ButtonState.Pressed && oldPad.Buttons.Start == ButtonState.Released))
                {
                   GameState = false;
                   MediaPlayer.Pause();
                   Heli.Pause();
                   Pause = true;
                }
                oldKeyState = KeyState;
                oldPad = pad;

            }
            if (instance.chopper.alive == false)
            {
                death = true;
                GameState = false;
                MediaPlayer.Stop();
                Heli.Stop();
            }
            else if (Pause)
            {
                GamePadState pad = GamePad.GetState(PlayerIndex.One);
                KeyboardState KeyState = Keyboard.GetState();
                if (KeyState.IsKeyDown(Keys.P) && oldKeyState.IsKeyUp(Keys.P) ||
                    (pad.Buttons.Start == ButtonState.Pressed && oldPad.Buttons.Start == ButtonState.Released))
                {
                    GameState = true;
                    MediaPlayer.Resume();
                    Heli.Resume();
                }
                oldKeyState = KeyState;
                oldPad = pad;
            }
            #endregion

        }
       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            #region Draw for main game
            if (GameState)
            {
                #region Draw Background
                mBackgroundOne.Draw(this.spriteBatch);
                mBackgroundTwo.Draw(this.spriteBatch);
                mBackgroundThree.Draw(this.spriteBatch);
                mBackgroundFour.Draw(this.spriteBatch);
                mBackgroundFive.Draw(this.spriteBatch);
                #endregion
                spriteBatch.DrawString(font, "" +count +"M" , new Vector2(10, 2.5f), Color.Black);
                spriteBatch.DrawString(text, "health: " + chopper.health.ToString() + "       lives: " + chopper.noLives.ToString() + "                 score: " + chopper.score.ToString(), new Vector2(210, 2.5f), Color.Black);
               
                chopper.Draw(gameTime);
                for (int i = 0; i < rockets.Count(); i++)
                {
                    rockets[i].Draw(gameTime);
                }
                for (int i = 0; i < bullets.Count(); i++)
                {
                    bullets[i].Draw(gameTime);

                }
                for (int i = 0; i < powerUp.Count(); i++)
                {
                    powerUp[i].Draw(gameTime);
                }
                for (int i = 0; i < enemy.Count(); i++)
                {
                    enemy[i].Draw(gameTime);
                }

               
              

            }
            #endregion 
            #region Draw Menusystem
            else
            {
                if (M_Menu)
                {
                    spriteBatch.Draw(Main_Menu, Vector2.Zero, Color.White);
                }
                if (Tut)
                {
                    spriteBatch.Draw(Tutorial, Vector2.Zero, Color.White);
                }
                if (Tut2)
                {
                    spriteBatch.Draw(Tutorial_Menu2, Vector2.Zero, Color.White);
                }
                if (Tut3)
                {
                    spriteBatch.Draw(Tutorial_Menu3, Vector2.Zero, Color.White);
                }
                if (Cred)
                {
                    spriteBatch.Draw(Credits, Vector2.Zero, Color.White);
                }
                if (Pause)
                {
                    spriteBatch.Draw(Pause_Menu, Vector2.Zero, Color.White);
                }
                if (death)
                {
                    spriteBatch.Draw(Death_Screen, Vector2.Zero, Color.White);
                    spriteBatch.DrawString(score, "You traveled: " + count.ToString() + " Score was:" + chopper.score.ToString() +" Your final score is:" + ((count/10) * chopper.score) +"", new Vector2(50, 350), Color.White);
                }
            }
            #endregion
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void genEnemies(GameTime gameTime)
        {
            //Randomly spawn enemies
            Random rand = new Random();
            if (gameTime.TotalGameTime - previousEnemySpawn > EnemySpawn)
            {
                enemies temp = new enemies(new Vector2(rand.Next(scrWidth + 50, scrWidth + 100), rand.Next(62, scrHeight - 97)), EnemySpeed, Enshotlimit);
                temp.LoadContent();
                enemy.Add(temp);

                previousEnemySpawn = gameTime.TotalGameTime;
            }
        }
    }
}
