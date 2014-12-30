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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static Game1 instance;
        public Chopper chopper = new Chopper();
        public List<Bullet> bullets = new List<Bullet>();
        public int scrHeight =500, scrWidth =800;

        Background mBackgroundOne;
        Background mBackgroundTwo;
        Background mBackgroundThree;
        Background mBackgroundFour;
        Background mBackgroundFive;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //initalise instance with Game1
            instance = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //set the window height and width
            graphics.PreferredBackBufferWidth = scrWidth;
            graphics.PreferredBackBufferHeight = scrHeight;
            //disable full screen
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "ChopperCmd";

            mBackgroundOne = new Background();

            mBackgroundTwo = new Background();

            mBackgroundThree = new Background();

            mBackgroundFour = new Background();
          
            mBackgroundFive = new Background();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            mBackgroundOne.LoadContent(this.Content, "Background01");
            mBackgroundOne.Pos = new Vector2(0, 0);

            mBackgroundTwo.LoadContent(this.Content, "Background02");
            mBackgroundTwo.Pos = new Vector2(mBackgroundOne.Pos.X + mBackgroundOne.size.Width, 0);

            mBackgroundThree.LoadContent(this.Content, "Background03");
            mBackgroundThree.Pos = new Vector2(mBackgroundTwo.Pos.X + mBackgroundTwo.size.Width, 0);

            mBackgroundFour.LoadContent(this.Content, "Background04");
            mBackgroundFour.Pos = new Vector2(mBackgroundThree.Pos.X + mBackgroundThree.size.Width, 0);

            mBackgroundFive.LoadContent(this.Content, "Background05");
            mBackgroundFive.Pos = new Vector2(mBackgroundFour.Pos.X + mBackgroundFour.size.Width, 0);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //load the chopper
            chopper.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            KeyboardState keyState =Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyState.IsKeyDown(Keys.Escape))
                this.Exit();
            //update the chopper
            


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

            Vector2 bDirection = new Vector2(-1,0);
            Vector2 bSpeed = new Vector2(200, 0);

            mBackgroundOne.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mBackgroundTwo.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mBackgroundThree.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mBackgroundFour.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            mBackgroundFive.Pos += bDirection * bSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

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

            chopper.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //draw the chopper
            

            mBackgroundOne.Draw(this.spriteBatch);
            mBackgroundTwo.Draw(this.spriteBatch);
            mBackgroundThree.Draw(this.spriteBatch);
            mBackgroundFour.Draw(this.spriteBatch);
            mBackgroundFive.Draw(this.spriteBatch);

            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Draw(gameTime);
            }

            chopper.Draw(gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}