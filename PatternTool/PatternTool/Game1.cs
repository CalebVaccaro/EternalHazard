/*  Author: Michael Khang
 *  Program: PatternTool
 *  Purpose: An editor which provides visual aid of the bullet patterns for bullet hell games 
 *           that the user can create as well as edit to their needs and wants.
 *           
 *  Instructions: Under the Initialization() section of the code, there will be box out section
 *                that has the header comment "List of variables that can be edited".
 *                
 *                There, it will list out the variables that has a direct impact on the resulting pattern
 *                and a comment explaining what each variable affects as well as their default value.
 *                
 *                The tool works so that the user will run the tool to see what the default pattern is, 
 *                then make changes to the variable then preview it and repeat the process to create a pattern to better suit their needs.
 *                
 *                There is also the option of using the in-tool options to edit the pattern live.
 *                
 *                After booting up, the left and right arrow keys can be used to decide which pattern the user wants to preview and edit.
 *                
 *                Once a pattern has been decided to preview, the pattern is editable by using the top two rows of the alphabets keys on the keyboard starting from the left.
 *                
 *                The first variable editable will use the combination of Q and A, and the second using the combination W and S, followed in that pattern.
 *                
 *                Then after the user has decided on a pattern to save, the enter key can be pressed to save those settings.
 */ 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.IO;

namespace PatternTool
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D tonberry;

        //holds texture for projectile
        Texture2D projectile;

        //used to print variables in the window
        SpriteFont text;

        //holds initial position vector
        Vector2 position;

        //holds vector to be traveled upon
        Vector2 direction;

        //holds a list of bullets so they will continue to exist past their initial draw
        List<bullet> hell;

        //rotates the shooting vector
        double angle;

        //manipulates rotating velocity
        //one copy for each of the pattern
        float rotationalVelocity;
        float loopRotationalVelocity;

        //counts frames as it's updated
        int frameCounter;

        //conditional to make sure fire rate of projectiles can be controled
        int rateOfFire;
        int loopRateOfFire;

        //timer used to keep track of how long a projectile travels backwards for the LoopDLoop pattern
        int travelTimer;

        //holds coordinate for when initializing new vectors for update
        float x;
        float y;

        //manipulates velocity of the bullet
        //one copy for each of the pattern
        float velocity;
        float loopVelocity;

        //checks to see if file has been saved or not
        bool saved;

        //used to check user input for the process of saving the file
        KeyboardState kb;
        KeyboardState prevKb;

        enum previewState
        {
            Spiral,
            LoopDLoop
        }

        previewState preview;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;

            graphics.ApplyChanges();
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
            

            //gives x and y coordinates based off of the angle
            x = (float)Math.Cos(DegToRad(angle));
            y = (float)Math.Sin(DegToRad(angle));

            //sets initial position in the center of the screen

            //initial coordinates the bullets are created at
            position = new Vector2((GraphicsDevice.Viewport.Width / 2), (GraphicsDevice.Viewport.Height / 2));

            //directional vector the bullets will travel upon after creation
            direction = new Vector2(x, y);

            //hold all bullets created
            hell = new List<bullet>();

            //used to keep track of to see if file has been saved or not
            saved = false;

            //used to keep track of frames processed
            frameCounter = 0;

            //sets initial previewState to Spiral
            preview = previewState.Spiral;

            //List of variables that can be edited

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

            //initial angle is 0 degrees
            //dictates at what angle the bullets will be created first
            /*not editable live due to the nature of its usage
            and limited impact which is only visable at the beginning*/
            angle = 0;

            //initial rotationalVelocity is 1
            //rotates the angle at x degrees per second
            /*setting it to a negative value will rotate it 
              counter clockwise instead of the default clockwise direction*/
            //higher = rotates faster, lower = rotates slower
            rotationalVelocity = (float)1;
            loopRotationalVelocity = (float)1;
            
            //Speed of the projectile
            //initial value is 1
            //higher = projectile travels faster, lower = travels slower
            velocity = 1;
            loopVelocity = 1;

            //Will fire 1 projectile every x amounts of frames
            //initial value is 1 projectile per frame
            //higher = will fire less often, lower = fires more often
            rateOfFire = 1;
            loopRateOfFire = 1;

            //timer used to keep track of how long a projectile travels backwards for the LoopDLoop pattern
            //initial value is 50
            //higher = projectile will travel backwards further, lower = will travel less further
            travelTimer = 50;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

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

            // TODO: use this.Content to load your game content here
            projectile = Content.Load<Texture2D>("yellowArrow");
            text = Content.Load<SpriteFont>("Normal");
            tonberry = Content.Load<Texture2D>("custom_tonberry");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //adds a default angle of 1 degree per update multiplied by a rotationalVelocity variable
            //will use the respective variable depending on which preview the user is on
            if(preview == previewState.Spiral)
                angle += 1 * rotationalVelocity;
            else
            if(preview == previewState.LoopDLoop)
                angle += 1 * loopRotationalVelocity;

            //if the angle is greater than 360, set it back to 0
            if (angle > 360)
            {
                angle = 0;
            }

            //holds x and y magnitudes for the vector at a given angle
            x = (float)Math.Cos(DegToRad(angle));
            y = (float)Math.Sin(DegToRad(angle));

            //holds the vector at a given angle
            direction = new Vector2(x, y);

            //if x many frames has elapsed
            if (preview == previewState.Spiral)
            {
                if (frameCounter > rateOfFire)
                {
                    //shoot
                    Shoot(hell, position, direction);

                    //reset frameCounter
                    frameCounter = 0;
                }
            }
            else
            if (preview == previewState.LoopDLoop)
                if (frameCounter > loopRateOfFire)
                {
                    //shoot
                    Shoot(hell, position, direction);

                    //reset frameCounter
                    frameCounter = 0;
                }

            //shoot depending on the previewState and reset frameCounter
            switch (preview)
            {
                //if state is spiral, use spiral pattern
                case previewState.Spiral:
                    Spiral(hell);
                    break;

                //if state is LoopDLoop, use LoopDLoop pattern
                case previewState.LoopDLoop:
                    LoopDLoop(hell);
                    break;
            }

            //increments frameCounter per update (per frame)
            frameCounter++;

            //gets keyboardstate
            kb = Keyboard.GetState();

            //if current preview state is Spiral and the right arrow key is pressed, switch to the LoopDLoop previewState
            if (preview == previewState.Spiral && kb.IsKeyDown(Keys.Right))
            {
                preview = previewState.LoopDLoop;

                //clears list to make sure the remainder of bullets from the previous pattern won't hinder the previewing of the new pattern
                hell.Clear();
            }

            //if current preview state is LoopDLoop and the left arrow key is pressed, switch to the Spiral previewState
            if (preview == previewState.LoopDLoop && kb.IsKeyDown(Keys.Left))
            {
                preview = previewState.Spiral;

                //same reason as above
                hell.Clear();
            }

            //if enter is pressed
            if (kb.IsKeyDown(Keys.Enter))
            {
                //save settings
                Save();

                //set bool to true
                saved = true;

                //save the state in another variable for record keeping
                prevKb = kb;
            }

            //checks to see if a key is pressed to update the prevKb variable
            if(checkInput())
                prevKb = kb;

            //if some other key has been pressed other than "Enter", set saved to false
            if (!prevKb.IsKeyDown(Keys.Enter))
                saved = false;

            //Edits variable to edit patterns live
            Edit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //central "enemy"
            spriteBatch.Draw(tonberry, position, null, Color.White, 0, new Vector2(tonberry.Width / 2, tonberry.Height / 2), 0.1f, SpriteEffects.None, 0);

            //draws each projectile
            foreach (bullet b in hell)
            {
                spriteBatch.Draw(projectile, b.BulletPosition, null, Color.White, b.Rotation, Vector2.Zero, 0.08f, SpriteEffects.None, 0);
            }

            //draws the approriate info regarding the variables for the respective patterns
            switch (preview)
            {
                case previewState.Spiral:
                    { 
                    spriteBatch.DrawString(text, "Preview: Spiral Pattern", new Vector2(20, 20), Color.White);
                    spriteBatch.DrawString(text, "Angle: " + angle, new Vector2(20, 60), Color.White);
                    spriteBatch.DrawString(text, "Rotational Velocity (Degrees per second): " + rotationalVelocity * 60, new Vector2(20, 100), Color.White);
                    spriteBatch.DrawString(text, "Rate of Fire: 1 Projectile per " + rateOfFire + " frames", new Vector2(20, 140), Color.White);
                    spriteBatch.DrawString(text, "Bullet Velocity: " + velocity, new Vector2(20, 180), Color.White);

                    //if file has yet to be saved, print instructions
                    if (saved == false)
                        spriteBatch.DrawString(text, "Hit the 'Enter' key to save the current settings", new Vector2(20, 220), Color.White);
                    else//print a notification that the file has been saved
                        spriteBatch.DrawString(text, "Settings have been saved!", new Vector2(20, 220), Color.White);
                    }
                    break;

                case previewState.LoopDLoop:
                    {
                        spriteBatch.DrawString(text, "Preview: LoopDLoop Pattern", new Vector2(20, 20), Color.White);
                        spriteBatch.DrawString(text, "Angle: " + angle, new Vector2(20, 60), Color.White);
                        spriteBatch.DrawString(text, "Rotational Velocity (Degrees per second): " + loopRotationalVelocity * 60, new Vector2(20, 100), Color.White);
                        spriteBatch.DrawString(text, "Rate of Fire: 1 Projectile per " + loopRateOfFire + " frames", new Vector2(20, 140), Color.White);
                        spriteBatch.DrawString(text, "Bullet Velocity: " + loopVelocity, new Vector2(20, 180), Color.White);
                        spriteBatch.DrawString(text, "Reverse Travel Time: " + travelTimer, new Vector2(20, 220), Color.White);
                        
                        //if file has yet to be saved, print instructions
                        if (saved == false)
                            spriteBatch.DrawString(text, "Hit the 'Enter' key to save the current settings", new Vector2(20, 260), Color.White);
                        else//print a notification that the file has been saved
                            spriteBatch.DrawString(text, "Settings have been saved!", new Vector2(20, 260), Color.White);
                    }
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //converts degrees to radians for usage of Cos() and Sin()
        double DegToRad(double angleInDeg)
        {
            return 2 * Math.PI * angleInDeg / 360.0;
        }

        //method to create new bullets whenever called upon
        public void Shoot(List<bullet> bullets, Vector2 pos, Vector2 dir)
        {
            //instantiates a new bullet at the default coordinate facing the direction it will travel in
            bullet temp = new bullet(pos, dir);

            //rotate bullet's sprite so that it will appear to be traveling parallell to the vector
            temp.Rotation = (float)angle / 60;

            //add bullet to the list
            bullets.Add(temp);
        }

        //updates the bullets to create a Spiral pattern
        public void Spiral(List<bullet> bullets)
        {
            //vector to hold position outside of window
            Vector2 origin = new Vector2(-100, -100);

            //for every bullet in the list
            foreach (bullet b in bullets)
            {
                //if the bullet is within the boundary of the "box" displaying the statistical information
                if (b.BulletPosition.X < 600 && b.BulletPosition.Y < 300)
                    b.BulletPosition = origin; //move them outside the window for readability
                else
                    b.BulletPosition += b.BulletDirection * velocity; //keep them traveling upon the vector they are on
            }
        }

        //updates bullets to create a LoopDLoop pattern
        public void LoopDLoop(List<bullet> bullets)
        {
            //vector to hold position outside of window
            Vector2 origin = new Vector2(-100, -100);

            //for every bullet in the list
            foreach (bullet b in bullets)
            {
                //if the bullet is within the boundary of the "box" displaying the statistical information
                if (b.BulletPosition.X < 600 && b.BulletPosition.Y < 300)
                    b.BulletPosition = origin; //move them outside the window for readability
                else
                    if (b.Timer < travelTimer) //the bullet hasn't traveled the x amount of frames in the opposite direction
                    {
                        //travel in the opposite direction
                        b.BulletPosition += b.BulletDirection * -loopVelocity;

                        b.Timer++;
                    }
                else
                    b.BulletPosition += b.BulletDirection * loopVelocity; //keep them traveling upon the vector they are on
            }
        }

        //used to check for user input and edit variables live
        public void Edit()
        {
            //the first and second row of the alphabets of the keyboard is used for editing
            //the first row is to increment the variable, the second to decrement
            //the first variable editable will use the combination of Q and A, the second using W and S and so on and so forth
            switch(preview)
            {
                case previewState.Spiral:
                    {
                        if (kb.IsKeyDown(Keys.Q))
                        {
                            rotationalVelocity++;
                        }

                        if (kb.IsKeyDown(Keys.A))
                        {
                            rotationalVelocity--;
                        }

                        if (kb.IsKeyDown(Keys.W))
                        {
                            rateOfFire++;
                        }

                        if (kb.IsKeyDown(Keys.S))
                        {
                            rateOfFire--;
                        }

                        if (kb.IsKeyDown(Keys.E))
                        {
                            velocity++;
                        }

                        if (kb.IsKeyDown(Keys.D))
                        {
                            velocity--;
                        }
                    }
                    break;

                case previewState.LoopDLoop:
                    {
                        if (kb.IsKeyDown(Keys.Q))
                        {
                            loopRotationalVelocity++;
                        }

                        if (kb.IsKeyDown(Keys.A))
                        {
                            loopRotationalVelocity--;
                        }

                        if (kb.IsKeyDown(Keys.W))
                        {
                            rateOfFire++;
                        }

                        if (kb.IsKeyDown(Keys.S))
                        {
                            rateOfFire--;
                        }

                        if (kb.IsKeyDown(Keys.E))
                        {
                            loopVelocity++;
                        }

                        if (kb.IsKeyDown(Keys.D))
                        {
                            loopVelocity--;
                        }

                        if (kb.IsKeyDown(Keys.R))
                        {
                            travelTimer++;
                        }

                        if (kb.IsKeyDown(Keys.F))
                        {
                            travelTimer--;
                        }
                    }
                    break;
            }
        }

        //check for user input
        public bool checkInput()
        {
            //boolean to be returned
            bool pressed = false;
            
            //for every key available
            foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                //if a key has been pressed
                if (kb.IsKeyDown(k))
                    pressed = true; //return true
            }

            return pressed;
        }

        //saves files with the settings of user choice
        public void Save()
        {
            //new streamwriter
            StreamWriter writer = new StreamWriter("BulletSettings.ini");

            //heading to differentiate between different categories of settings
            writer.WriteLine("[Spiral]");
            writer.WriteLine("");

            //actual settings for the category with comments on each setting
            writer.WriteLine("//Angle at which the projectile will travel in");
            writer.WriteLine("Angle: " + angle);
            writer.WriteLine("");

            writer.WriteLine("//Increase in angle at x degrees per second");
            writer.WriteLine("Rotational Velocity: " + rotationalVelocity);
            writer.WriteLine("");

            writer.WriteLine("//Will fire 1 projectile every x amounts of frames");
            writer.WriteLine("Rate of Fire: " + rateOfFire);
            writer.WriteLine("");

            writer.WriteLine("//Speed of the projectile");
            writer.WriteLine("Bullet Velocity: " + velocity);

            //heading to differentiate between different categories of settings
            writer.WriteLine("[LoopDLoop]");
            writer.WriteLine("");

            //actual settings for the category with comments on each setting
            writer.WriteLine("//Angle at which the projectile will travel in");
            writer.WriteLine("Angle: " + angle);
            writer.WriteLine("");

            writer.WriteLine("//Increase in angle at x degrees per second");
            writer.WriteLine("Rotational Velocity: " + loopRotationalVelocity);
            writer.WriteLine("");

            writer.WriteLine("//Will fire 1 projectile every x amounts of frames");
            writer.WriteLine("Rate of Fire: " + loopRateOfFire);
            writer.WriteLine("");

            writer.WriteLine("//Speed of the projectile");
            writer.WriteLine("Bullet Velocity: " + loopVelocity);
            writer.WriteLine("");

            writer.WriteLine("//Timer used to keep track of how long a projectile travels backwards for the LoopDLoop pattern");
            writer.WriteLine("Reverse Travel Time: " + travelTimer);

            writer.Close();
        }
    }
}
