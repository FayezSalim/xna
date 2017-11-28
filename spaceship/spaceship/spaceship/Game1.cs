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

namespace spaceship
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        LevelLibrary.Level level;
        SoundEffect missilefire,shuttlesound,gunsound,gunstart,gunstop;
        GraphicsDeviceManager graphics;
        Rectangle heatmeasure;
        SpriteBatch spriteBatch;
        background bg;
        earth earths;
        List<Texture2D> erths;
        shuttle ship;
        ParticleEngine pe;
        missileengine me;
        SpriteFont velocity,pause,missilestatus;
        Color missilestatcolor;
        string vel;
        int missileinterval = 100,cannonheatinterval=40,cannonheat=0;
        bool cannonsound=false,cannonstopped=true,paused=false,pauseKeyDown = false,overheated=false;
        int cannondelay=10, cannoncurrentdelay=0;
        missileengine cannonengine;
        Texture2D heatbar,heatpointer,overheat;
        bool reset = false;
        debris asteroids;

        enum missilestate
        {
            Reloading,Ready
        }
        missilestate stateofmissile=missilestate.Ready;
        int missilelimit = 9;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //graphics.ToggleFullScreen();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bg = new background(Content.Load<Texture2D>("stars"), Content.Load<Texture2D>("stars"), new Vector2(0, 0));
            missilefire = Content.Load<SoundEffect>("Missile Fire");
            shuttlesound = Content.Load<SoundEffect>("spaceship");
            gunstart = Content.Load<SoundEffect>("gunstart");
            gunstop = Content.Load<SoundEffect>("gunstop");
            level = Content.Load<LevelLibrary.Level>("level1");
            //load start and stop sounds
            gunsound = Content.Load<SoundEffect>("gunsound");
            heatbar = Content.Load<Texture2D>("progress");
            overheat = Content.Load<Texture2D>("overheated");
            heatmeasure = new Rectangle(640, 430, 80, 20);
            heatpointer = Content.Load<Texture2D>("bar");
            erths = new List<Texture2D>();
            string h;
            string img = "earth/Capture";
            for (int i = 120; i >=0; i--)
            {
                h = img + i;
                erths.Add(Content.Load<Texture2D>(h));
            }
            earths = new earth(erths);
            List<Texture2D> asts = new List<Texture2D>();
            img = "asteroid/Asteroidframe";
            for (int i = 0; i <=24; i++)
            {
                h = img + i;
                asts.Add(Content.Load<Texture2D>(h));
            }
            asteroids = new debris(asts,level);
            ship = new shuttle(Content.Load<Texture2D>("shuttle"),shuttlesound);
            List<Texture2D> particles = new List<Texture2D>();
            particles.Add(Content.Load<Texture2D>("circle"));
            particles.Add(Content.Load<Texture2D>("circle"));
            particles.Add(Content.Load<Texture2D>("circle"));
            pe = new ParticleEngine(particles, new Vector2(0, 0),10);
            me = new missileengine(Content.Load<Texture2D>("missile"), ship.position, ship.rotation,particles,missilefire);
            //create engine load texture gatling cannon
            //gatling cannon sound
            cannonengine = new missileengine(Content.Load<Texture2D>("20mm"), ship.position, ship.rotation, gunsound);
            velocity = Content.Load<SpriteFont>("velocity");
            missilestatus = Content.Load<SpriteFont>("missilestats");
            pause = Content.Load<SpriteFont>("pause");
            
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
            KeyboardState keyboardstate=Keyboard.GetState();
            checkPauseKey(keyboardstate);
            checkshpiloc();
            if ((keyboardstate.IsKeyDown(Keys.R) == true)||(reset==true))
            {
                ship.position = new Vector2(420, 420);
                ship.direction = Vector2.Zero;
                ship.speed = Vector2.Zero;
                ship.shuttle_speed = 0;
                reset = false;
            }
            if (!paused)
            {
                earths.update();
                asteroids.update();//asteroids
                ship.update(gameTime);
                me.angle = ship.rotation;
                cannonengine.angle = ship.rotation;
                me.EmitterLocation = ship.position;
                cannonengine.EmitterLocation = ship.position;
                // do both the above for cannon
                if (ship.direction != Vector2.Zero)
                {
                    meengineupdate();
                    cannonengineupdate();
                    //call cannon engine update
                }
                else
                {
                    //call cannon cannon engine delete
                    me.missiledelete();
                    cannonengine.missiledelete();
                }
                peengineupdate();
                bg.update(gameTime);
                vel = "Velocity : " + ship.shuttle_speed;
                if (missileinterval == 0)
                {
                    stateofmissile = missilestate.Ready;
                    missilestatcolor = Color.Green;
                }
                else
                {
                    stateofmissile = missilestate.Reloading;
                    missilestatcolor = Color.Red;
                    missileinterval -= 1;
                }
                //detectcollission();
                // TODO: Add your update logic here

               
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            bg.draw(this.spriteBatch);
            earths.draw(spriteBatch);
           asteroids.draw(this.spriteBatch);//asteroids
            spriteBatch.Draw(heatbar, new Rectangle(630, 430, 90, 20), Color.Gray);
            spriteBatch.Draw(heatpointer, heatmeasure, Color.Black);
            spriteBatch.DrawString(velocity, vel, new Vector2(630, 450), Color.White);
            spriteBatch.DrawString(missilestatus, "  "+(missilelimit-me.usedmissiles+1).ToString(), new Vector2(740, 420),missilestatcolor);
            ship.draw(this.spriteBatch);
            if (stateofmissile.ToString() == "Ready")
            {
                spriteBatch.Draw(Content.Load<Texture2D>("missile"), new Rectangle(710, 400, 70, 50), Color.Green);
            }
            else
            {
                spriteBatch.Draw(Content.Load<Texture2D>("missile"), new Rectangle(710, 400, 70, 50), Color.Red);
            }
            if (overheated)
            {
                spriteBatch.Draw(overheat, new Rectangle(630, 430, 90, 20), Color.White);
            }
            if (paused == true)
            {
                spriteBatch.DrawString(pause, "PAUSED", new Vector2(700, 20), Color.Red);
            }
            spriteBatch.End();
            me.Draw(spriteBatch);
            cannonengine.Draw(spriteBatch);
            pe.Draw(spriteBatch);
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        private void peengineupdate()
        {
            bool keydown = false;
            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            if (CurrentKeyboardState.IsKeyDown(Keys.Left) == true)
            {
                pe.EmitterLocation = new Vector2(ship.position.X+35 , ship.position.Y - 4);
                keydown = true;
            }
            else if (CurrentKeyboardState.IsKeyDown(Keys.Right) == true)
            {
                pe.EmitterLocation = new Vector2(ship.position.X - 39, ship.position.Y + 4);
                keydown = true;
            }
            if (CurrentKeyboardState.IsKeyDown(Keys.Up) == true)
            {
                pe.EmitterLocation = new Vector2(ship.position.X + 2, ship.position.Y + 39);
                keydown = true;
                if (CurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    pe.EmitterLocation = new Vector2(ship.position.X + 30, ship.position.Y + 25);
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    pe.EmitterLocation = new Vector2(ship.position.X - 30, ship.position.Y + 25);
                }
            }
            else if (CurrentKeyboardState.IsKeyDown(Keys.Down) == true)
            {
                keydown = true;
                pe.EmitterLocation = new Vector2(ship.position.X - 3, ship.position.Y - 38);
                if (CurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    pe.EmitterLocation = new Vector2(ship.position.X + 21, ship.position.Y - 31);
                }
                else if (CurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    pe.EmitterLocation = new Vector2(ship.position.X - 25, ship.position.Y-27);
                }
            }
            if (keydown == true)
            {
                if (ship.shuttle_speed < 110)
                {
                    pe.tt = (int)(((ship.shuttle_speed - 110) * -1) / 10);
                }
                else
                {
                    pe.tt = (int)((ship.shuttle_speed - 110) / 10);
                }
                pe.Update();
            }
            else
            {
                pe.particledelete();
            }
        }

        private void meengineupdate()
        {
            KeyboardState curr=Keyboard.GetState();
            if ((curr.IsKeyDown(Keys.Space) == true)&&(me.usedmissiles<=missilelimit)&&(stateofmissile==missilestate.Ready))
            {
                Vector2 velocity = ship.direction*3;
                me.Update(velocity);
                missileinterval = 100;
                
            }
            else
            {
                me.missiledelete();
            }
        }

        private void cannonengineupdate()
        {
            KeyboardState curr = Keyboard.GetState();
            if (curr.IsKeyDown(Keys.LeftControl) == true)
            {
                if (cannonsound == false)
                {
                    gunstart.Play();
                    cannonstopped = false;
                    cannoncurrentdelay += 1;
                    cannonsound = true;
                    coolcannon();
                }
                else if (cannoncurrentdelay != cannondelay)
                {
                    cannoncurrentdelay += 1;
                    coolcannon();
                    cannonengine.missiledelete();
                }
                else if ((cannoncurrentdelay == cannondelay) && (!overheated))
                {
                    Vector2 velocity = ship.direction * 5;
                    cannonengine.Update(velocity);
                    heatcannon();
                }
                else
                {
                    coolcannon();
                    cannonengine.missiledelete();
                }
            }
            else
            {
                coolcannon();
                cannonengine.missiledelete();
                cannonsound = false;
                cannoncurrentdelay = 0;
                if (cannonstopped == false)
                {
                    gunstop.Play();
                    cannonstopped = true;
                }
            }
            if (cannonheat % 2 == 0)
            {
                heatmeasure.X=640 + cannonheat;
                heatmeasure.Width=80 - cannonheat;
            }
        }

        private void heatcannon()
        {
            if (cannonheat <= 95)
            {
                cannonheat += 1;
                if (cannonheat == 96)
                {
                    overheated = true;
                    gunstop.Play();
                    cannonstopped = true;
                }
            }
            else if (cannonheat == 0)
            {
                overheated = false;
            }

        }

        private void coolcannon()
        {
            if ((cannonheat >= 1)&&(!overheated))
            {
                cannonheat -= 1;
            }
            else if (cannonheat == 0)
            {
                overheated = false;
                cannonheatinterval = 40;//change cannon heat interval here too
            }
            else if ((overheated) && (cannonheatinterval > 0))
            {
                cannonheatinterval -= 1;
            }
            else
            {
                cannonheat -= 1;
            }
        }
       
        private void BeginPause()
        {
         paused = true;
        }
        
        private void EndPause()
        {
        paused = false;
        }

        private void checkPauseKey(KeyboardState keyboardState)
       {
         bool pauseKeyDownThisFrame = (keyboardState.IsKeyDown(Keys.Escape));
         if (!pauseKeyDown && pauseKeyDownThisFrame)
         {
            if (!paused)
            {
                BeginPause();
            }
            else
            {
                EndPause();
            }
         }
         pauseKeyDown = pauseKeyDownThisFrame;
       }

        private void checkshpiloc()
        {
            if ((ship.position.X < 0) || (ship.position.Y < 0) || (ship.position.X > 800) || (ship.position.Y > 500))
            {
                reset = true;
            }
        }

        private void detectcollission()
        {
           // Rectangle earthrec = new Rectangle((int)earths.position.X,(int)earths.position.Y,earths.earths[earths.index].Width,earths.earths[earths.index].Height);
           // Rectangle shiprec = new Rectangle((int)ship.position.X, (int)ship.position.Y, ship.img.Width, ship.img.Height);
           // if (earthrec.Intersects(shiprec) == true)
           // {
                //reset
           //     ship.position.X = 0.0f;
           //     ship.position.Y = 0.0f;
          //  }

        }
    }
}
