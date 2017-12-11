using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
/*
 * Caleb Vaccaro
 * GDAPS2
 * Eternal Hazardz
 * Last Entry: 
 * 12.06.2017
 */

namespace GDAPS2
{
    /// <summary>
    /// This is the main class for your game.
    /// </summary>
    public class MainGame : Game
    {

    #region Attributes

        // Given objects by Monogame
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // ---- Player/Sprite Attributes  ---- //

        // Creating Sprite List
        private List<Sprite> _sprites;  

        // int for counting score(placeholder)
        private int score = 0;

        // how many players in game
        int playerCount;

        // whatever the lastamount of players it was reload it
        int lastplayerCount;

        // ----- Game Manager Attributes ---- //

        // enum GameStates to handle each game state {Menu, Game, GameOver}
        enum GameState { Menu, Game, GameOver };

        // gamestate object to acces gamestates
        GameState state = GameState.Menu;

        // float timer for general time check
        public float timer;
        
        // countdown timer for when to start enemy shooting
        public int countdownTimer;

        // ScreenHeight and Width Attributes
        public int ScreenWidth;
        public int ScreenHeight;

        // font for printing text on screen
        private SpriteFont font;

        // Rectangle for all Background Vectors
        private Rectangle mainFrame;

        //Additional Game Screen Attributes
        private Texture2D pause;
        private Texture2D pauseKey;
        private Texture2D gameover;
        private Texture2D gameoverKey;
        private Vector2 gameoverCoords;
        public Texture2D bulletTexture;       
        
        //Intro Animation and Main Menu Attributes
        private Texture2D menuPad;
        private Texture2D menuKey;
        private Texture2D splash;
        private Texture2D tree;
        private Texture2D eternal;
        private Texture2D hazard;
        private Vector2 splashBgCoords;
        private Vector2 treeCoords;
        private Vector2 eternalCoords;
        private Vector2 hazardCoords;
        private Vector2 menuCoords;
        private int bgTarget;
        private int treeTarget;
        private float menuOpacity;

        //Player Textures
        private Texture2D playerOneTexture;
        private Texture2D playerTwoTexture;
        private Texture2D playerThreeTexture;
        private Texture2D playerFourTexture;
        private Texture2D enemyOneTexture;
        private Texture2D enemyTwoTexture;
        private Texture2D enemyThreeTexture;

        //Map Textures
        private Texture2D ruins;
        private Texture2D dojo;
        private Texture2D astral;

        #endregion

        #region Capabilities

        //Controller Attributes
        private List<GamePadCapabilities> capabilities = new List<GamePadCapabilities>(); //creates a list of capabilities

        private KeyboardState previousState;
        private KeyboardState keyState;

        //number of gamepads in current game
        private int gamepads;

        // gamepad property
        public int Gamepads
        {
            get { return gamepads; }
            set { gamepads = value; }
        }

        //capbilities for each controller connected
        private GamePadCapabilities capability1;
        private GamePadCapabilities capability2;
        private GamePadCapabilities capability3;
        private GamePadCapabilities capability4;

        //player indexes for each player
        private PlayerIndex playerone = PlayerIndex.One;
        private PlayerIndex playertwo = PlayerIndex.Two;
        private PlayerIndex playerthree = PlayerIndex.Three;
        private PlayerIndex playerfour = PlayerIndex.Four;

        //GamePadState stateC will take in all the states
        //used later for input control
        private GamePadState state1;
        private GamePadState state2;
        private GamePadState state3;
        private GamePadState state4;

        // Previous States for each GamePad
        private GamePadState previousState1;
        private GamePadState previousState2;
        private GamePadState previousState3;
        private GamePadState previousState4;

    #endregion

    #region Game
        // Main Game Constuctor
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Preferred Dimensions of the Screen
            ScreenWidth = graphics.PreferredBackBufferWidth = 1920;
            ScreenHeight = graphics.PreferredBackBufferHeight = 1080;
            
            graphics.IsFullScreen = false;   // make window fullscreen
            graphics.ApplyChanges();         // apply changes

            //Initialize Intro Animation Attributes
            splashBgCoords = new Vector2(0, 0);
            bgTarget = -100;
            treeCoords = new Vector2(2750, 0);
            treeTarget = -200;
            eternalCoords = new Vector2(550, 350);
            hazardCoords = new Vector2(475, 525);
            menuCoords = new Vector2(840, 765);
            menuOpacity = 0.0f;

            // Game OVer Screen Text Coordinates
            gameoverCoords = new Vector2(500, 200);

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

            //set players game pads to -1
            gamepads = -1;

            //one controller has to be connected at the start of the game
            //once the game has loaded you can add more 
            capability1 = GamePad.GetCapabilities(playerone);
            capability2 = GamePad.GetCapabilities(playertwo);
            capability3 = GamePad.GetCapabilities(playerthree);
            capability4 = GamePad.GetCapabilities(playerfour);

            // CountDown Timer 
            countdownTimer = 200;

            //check controllers
            CheckControllers();

            //previous key holds current key state
            previousState = Keyboard.GetState();

            //Get Old gamepad state
            previousState1 = GamePad.GetState(playerone);
            previousState2 = GamePad.GetState(playertwo);
            previousState3 = GamePad.GetState(playerthree);
            previousState4 = GamePad.GetState(playerfour);


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


            //creating each var Texture for each asset needed to be put into the game         
            playerOneTexture = Content.Load<Texture2D>("player1");         // monk texture blue
            playerTwoTexture = Content.Load<Texture2D>("player2");         // monk texture red
            playerThreeTexture = Content.Load<Texture2D>("player3");       // monk texture green
            playerFourTexture = Content.Load<Texture2D>("player4");        // monk texture yellow
            enemyOneTexture = Content.Load<Texture2D>("enemy1");           // enemy texture level 1
            enemyTwoTexture = Content.Load<Texture2D>("enemy2");           // enemy texture level 2
            enemyThreeTexture = Content.Load<Texture2D>("enemy3");         // enemy texture level 3
            bulletTexture = Content.Load<Texture2D>("cyclonebullet");      // bullet texture
            splash = Content.Load<Texture2D>("splashBackground");          // menu texture 
            tree = Content.Load<Texture2D>("tree");                        // tree texture
            eternal = Content.Load<Texture2D>("eternal");                  // Eternal Hazard Title texture
            hazard = Content.Load<Texture2D>("hazard");                    // Hazard Title Texture
            menuPad = Content.Load<Texture2D>("menu");                     // Controller Menu
            menuKey = Content.Load<Texture2D>("menuKey");                  // Keyboard Menu
            ruins = Content.Load<Texture2D>("ruinsBG");                    // level 1 background texture
            dojo = Content.Load<Texture2D>("dojoBG");                      // level 2 background texture
            astral = Content.Load<Texture2D>("astralBG");                  // level 3 background texture
            pause = Content.Load<Texture2D>("pause");                      // pause screen texture
            pauseKey = Content.Load<Texture2D>("pauseKey");                // pause keyboard Texture
            gameover = Content.Load<Texture2D>("gameOver");                // gameover texture
            gameoverKey = Content.Load<Texture2D>("gameOverKey");          // Game Over Keyboard Texture
            font = Content.Load<SpriteFont>("Score");                      // Font Score texture

            //Background Rectangle
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);   
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
            //Get KeyState
            keyState = Keyboard.GetState();

            //check controllers
            CheckControllers();

            //SWTICH STATEMENT
            switch (state)
            {

                // --------- MENU STATE ----------- //
                case GameState.Menu:

                    //Load Content
                    LoadContent();

                    //ease background image
                    splashBgCoords.X -= (splashBgCoords.X - bgTarget) * 0.007f;

                    //ease tree image
                    treeCoords.X -= (treeCoords.X - treeTarget) * 0.015f;

                    //menu fades in
                    if(treeCoords.X < -75)
                    {
                        menuOpacity += 0.03f;
                    }

                    // GAMEPAD STATE: KEY(0)
                    if (Gamepads == 0)
                    {
                        // You can also check the controllers "type"
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !previousState.IsKeyDown(Keys.Enter))
                        {
                            // the button has just been pressed
                            // do something here
                            NewGame();
                        }

                        // You can also check the controllers "type"
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
                        {
                            // the button has just been pressed
                            // do something here
                            this.Exit();
                        }

                        // save keystate to previous keystate
                        previousState = keyState;
                    }

                    // GAMEPAD STATE: 1
                    //if gamepad true, controllers are connected
                    if (Gamepads == 1)
                    {
                        GamePadCapabilities controllerC = GamePad.GetCapabilities(PlayerIndex.One);

                        // Get New Controller State, adding the player index
                        state1 = GamePad.GetState(playerone);

                        if (controllerC.GamePadType == GamePadType.GamePad)
                        {
                            // You can also check the controllers "type"
                            if (state1.IsButtonDown(Buttons.Start) && !previousState1.IsButtonDown(Buttons.Start))
                            {
                                // the button has just been pressed
                                // do something here
                                NewGame();
                            }

                            // You can also check the controllers "type"
                            if (state1.IsButtonDown(Buttons.Y))
                            {
                                // the button has just been pressed
                                // do something here
                                this.Exit();
                            }
                        }

                        //previous gamepad sticks = gamepad states
                        previousState1 = state1;
                    }




                    // GAMEPAD STATE: 2
                    //if gamepad true, controllers are connected
                    if (Gamepads == 2)
                    {
                        foreach (var controller in capabilities)
                        {

                            // Get New Controller State, adding the player index
                            state1 = GamePad.GetState(playerone);
                            state2 = GamePad.GetState(playertwo);


                            if (controller.GamePadType == GamePadType.GamePad)
                            {
                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Start) || state2.IsButtonDown(Buttons.Start) 
                                    && !previousState1.IsButtonDown(Buttons.Start) && !previousState2.IsButtonDown(Buttons.Start))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    NewGame();
                                }

                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Y) || state2.IsButtonDown(Buttons.Y) 
                                    && !previousState1.IsButtonDown(Buttons.Y) && !previousState2.IsButtonDown(Buttons.Y))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    this.Exit();
                                }
                            }
                        }

                        //previous gamepad sticks = gamepad states
                        previousState1 = state1;
                        previousState2 = state2;

                    }

                    // GAMEPAD STATE: 3
                    //if gamepad true, controllers are connected
                    if (Gamepads == 3)
                    {
                        foreach (var controller in capabilities)
                        {

                            // Get New Controller State, adding the player index
                            state1 = GamePad.GetState(playerone);
                            state2 = GamePad.GetState(playertwo);
                            state3 = GamePad.GetState(playerthree);


                            if (controller.GamePadType == GamePadType.GamePad)
                            {
                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Start) || state2.IsButtonDown(Buttons.Start) || state3.IsButtonDown(Buttons.Start) 
                                    && !previousState1.IsButtonDown(Buttons.Start) && !previousState2.IsButtonDown(Buttons.Start) && !previousState3.IsButtonDown(Buttons.Start))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    NewGame();
                                }

                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Y) || state2.IsButtonDown(Buttons.Y) || state3.IsButtonDown(Buttons.Y) 
                                    && !previousState1.IsButtonDown(Buttons.Y) && !previousState2.IsButtonDown(Buttons.Y) && !previousState3.IsButtonDown(Buttons.Y))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    this.Exit();
                                }
                            }
                        }

                        //previous gamepad sticks = gamepad states
                        previousState1 = state1;
                        previousState2 = state2;
                        previousState3 = state3;

                    }

                    // GAMEPAD STATE: 3
                    //if gamepad true, controllers are connected
                    if (Gamepads == 4)
                    {
                        foreach (var controller in capabilities)
                        {

                            // Get New Controller State, adding the player index
                            state1 = GamePad.GetState(playerone);
                            state2 = GamePad.GetState(playertwo);
                            state3 = GamePad.GetState(playerthree);
                            state4 = GamePad.GetState(playerfour);


                            if (controller.GamePadType == GamePadType.GamePad)
                            {
                                // checking for controller Start Button Input
                                if (state1.IsButtonDown(Buttons.Start) || state2.IsButtonDown(Buttons.Start) || state3.IsButtonDown(Buttons.Start) || state4.IsButtonDown(Buttons.Start) 
                                    && !previousState1.IsButtonDown(Buttons.Start) && !previousState2.IsButtonDown(Buttons.Start) && !previousState3.IsButtonDown(Buttons.Start) && !previousState4.IsButtonDown(Buttons.Start))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    NewGame();
                                }

                                // checking for controller Y Button Input
                                if (state1.IsButtonDown(Buttons.Y) || state2.IsButtonDown(Buttons.Y) || state3.IsButtonDown(Buttons.Y) || state4.IsButtonDown(Buttons.Y)
                                    && !previousState1.IsButtonDown(Buttons.Y) && !previousState2.IsButtonDown(Buttons.Y) && !previousState3.IsButtonDown(Buttons.Y) && !previousState4.IsButtonDown(Buttons.Y))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    this.Exit();
                                }
                            }
                        }

                        //previous gamepad sticks = gamepad states
                        previousState1 = state1;
                        previousState2 = state2;
                        previousState3 = state3;
                        previousState4 = state4;

                    }

                    break;

                    // --------- GAME STATE ----------- //
                case GameState.Game:

                    // Gamepads is Greater than 0, Anything
                    // Anything but KeyState
                    if (Gamepads > 0)
                    {

                        StartCountdown();
                        //Foreach Sprite in the List, Call Update
                        //Used to Fire Bullets
                        foreach (var sprite in _sprites)
                        {
                            // call Update methods foreach sprite in the list
                            sprite.Update(gameTime, _sprites);

                        }

                        //Has Died Method is Called
                        HasDied();
                    }

                    // KeyState for Game
                    else if (Gamepads == 0)
                    {
                        //start countdown 
                        StartCountdown();

                        //Foreach Sprite in the List, Call Update
                        //Used to Fire Bullets
                        foreach (var sprite in _sprites.ToArray())
                        {
                            sprite.Update(gameTime, _sprites);

                        }

                        //Has Died Method is Called
                        HasDied();
                    }

                    // save keystate to previous keystated
                    previousState = keyState;

                    break;

                // --------- GAMEOVER STATE ----------- //
                case GameState.GameOver:

                    // No Code For GameOVer Yet
                    // Check the device for Player One
                    foreach (var controller in capabilities)
                    {
                        //Gamepad State
                        if (Gamepads > 0)
                        {
                            // initialize gamepad states, with player indexes
                            state1 = GamePad.GetState(playerone);
                            state2 = GamePad.GetState(playertwo);
                            state3 = GamePad.GetState(playerthree);
                            state4 = GamePad.GetState(playerfour);

                            // if controller gamepad is connected
                            if (controller.GamePadType == GamePadType.GamePad)
                            {
                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Start) || state2.IsButtonDown(Buttons.Start) || state3.IsButtonDown(Buttons.Start) && !previousState1.IsButtonDown(Buttons.Start) && !previousState2.IsButtonDown(Buttons.Start) && !previousState3.IsButtonDown(Buttons.Start))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    state = GameState.Menu;
                                }

                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Y) || state2.IsButtonDown(Buttons.Y) || state3.IsButtonDown(Buttons.Y) && !previousState1.IsButtonDown(Buttons.Y) && !previousState2.IsButtonDown(Buttons.Y) && !previousState3.IsButtonDown(Buttons.Y))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    Exit();
                                }

                            }

                        }

                        //previous gamepad sticks = gamepad states
                        previousState1 = state1;
                        previousState2 = state2;
                        previousState3 = state3;
                        previousState4 = state4;
                    }

                    //Keyboard State
                    if (Gamepads == 0)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !previousState.IsKeyDown(Keys.Enter))
                        {
                            // the button has just been pressed
                            // do something here
                            state = GameState.Menu;
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !previousState.IsKeyDown(Keys.Escape))
                        {
                            //Escape Exit
                            Exit();
                        }
                    }

                    //previous gamepad sticks = gamepad states
                    previousState = keyState;

                    break;

                    
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            // background color
            GraphicsDevice.Clear(Color.DarkGreen);
            
            // Begin Spritebatch
            spriteBatch.Begin();

            // Switch Statement for GAMESTATES
            switch (state)
            {
                // --------- MENU STATE ----------- //
                case GameState.Menu:

                    //display menu image
                    // GAMEPAD STATES
                    if (Gamepads > 0)
                    {
                        // Opening Transition and Title Screen
                        spriteBatch.Draw(splash, splashBgCoords, Color.White);
                        spriteBatch.Draw(tree, treeCoords, Color.White);
                        if (treeCoords.X < 0)
                        {
                            spriteBatch.Draw(eternal, eternalCoords, Color.White);
                        }

                        if (treeCoords.X < -30)
                        {
                            spriteBatch.Draw(hazard, hazardCoords, Color.White);
                        }

                        spriteBatch.Draw(menuPad, menuCoords, Color.White * menuOpacity);
                    }

                    //display menu image
                    // KEYSTATE STATES
                    else
                    {
                        // Opening Transition and Title Screen
                        spriteBatch.Draw(splash, splashBgCoords, Color.White);
                        spriteBatch.Draw(tree, treeCoords, Color.White);
                        if (treeCoords.X < 0)
                        {
                            spriteBatch.Draw(eternal, eternalCoords, Color.White);
                        }
                            
                        if (treeCoords.X < -30)
                        {
                            spriteBatch.Draw(hazard, hazardCoords, Color.White);
                        }
                                                  
                        spriteBatch.Draw(menuKey, menuCoords, Color.White * menuOpacity);

                        //spriteBatch.Draw(splashScreen, mainFrame, Color.White);
                        spriteBatch.DrawString(font, "Controllers Are Recommended for this Game", new Vector2(ScreenWidth - 350, ScreenHeight - 150), Color.White);
                    }
                    
                    break;

                // --------- GAME STATE ----------- //
                case GameState.Game:
                    //drawing the background
                    spriteBatch.Draw(ruins, mainFrame, Color.White);

                    //foreach sprite in the list _sprites
                    //Draw the sprites in the spritebatch
                    foreach (var sprite in _sprites)
                    {
                        sprite.Draw(spriteBatch);
                    }

                    //Text On Screen
                    spriteBatch.DrawString(font, "CountDown: " + countdownTimer.ToString(), new Vector2(ScreenWidth / 2, 10), Color.Red);
                    spriteBatch.DrawString(font, "Time: " + timer.ToString(), new Vector2(ScreenWidth / 2, 50), Color.Black);

                    // GAMEPADS: 3
                    if (Gamepads == 3)
                    {
                        spriteBatch.DrawString(font, "Player 1 Score: " + score.ToString(), new Vector2(60, 20), Color.Blue);
                        spriteBatch.DrawString(font, "Player 2 Score: " + score.ToString(), new Vector2(ScreenWidth - 200, 20), Color.Green);
                        spriteBatch.DrawString(font, "Player 3 Score: " + score.ToString(), new Vector2(60, ScreenHeight - 50), Color.Purple);
                    }
                    // GAMEPADS: 2
                    if (Gamepads == 2)
                    {
                        spriteBatch.DrawString(font, "Player 1 Score: " + score.ToString(), new Vector2(60, 20), Color.Blue);
                        spriteBatch.DrawString(font, "Player 2 Score: " + score.ToString(), new Vector2(ScreenWidth - 200, 20), Color.Green);
                    }
                    // GAMEPADS: 1 or KEYSTATE(SinglePlayer)
                    if (Gamepads < 2)
                    {
                        spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(60, 20), Color.Blue);
                    }

                    

                    break;

                // --------- GAMEOVER STATE ----------- //
                case GameState.GameOver:

                    //GamePads for Controllers
                    if (Gamepads == 1 || Gamepads == 2 || Gamepads == 3)
                    {
                        spriteBatch.Draw(splash, mainFrame, Color.White);
                        spriteBatch.Draw(gameover, gameoverCoords, Color.White);
                        Results();
                    }
                    // KEYSTATE
                    if(Gamepads == 0)
                    {
                        spriteBatch.Draw(gameoverKey, mainFrame, Color.White);
                        Results();
                    }
                    
                    break;
            }


            //End SpriteBatch Drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Has Died takes the sprite list and check to see if its the player
        /// If so then it sees if hasDied = true, if so then Exit Game
        /// If hasDied is true then put sate into GamOver
        /// </summary>
        /// <param name="_sprites"></param>
        public void HasDied()
        {
            // For each sprite in sprite list
            for (int i = 0; i < _sprites.Count; i++)
            {
                // make new sprite object foreach sprite in the list
                Sprite sprite = _sprites[i];

                // if the sprite is a player then true
                if (sprite is Player)
                {
                    // Make Player Object, if sprite is a player
                    Player player = sprite as Player;

                    // if has died is true, kill player
                    if (player.HasDied == true)
                    {
                        // player is removed
                        player.isRemoved = true;

                        // decrement playerCount -1
                        playerCount -= 1;

                        // get death time
                        player.DeathTime = (int)timer;
                    }
                }

            }
            
            // IF NO PLAYERS ARE LEFT GO To Game Over State
            if(playerCount == 0)
            {
                state = GameState.GameOver;
            }

        }

        /// <summary>
        /// Instaitates a New Game
        /// </summary>
        public void NewGame()
        {
            //set state to Game
            state = GameState.Game;

            // Reset Timer
            ResetTimer();

         //takes in playerCount for condition
            switch (Gamepads)
            {
                //KeyBoard Case
                case 0:

                    // Instantiate Sprite List
                    _sprites = new List<Sprite>
                    {
                        // Player 1
                        new Player(this,playerOneTexture, capability1, playerone, state1, 100, 1){
                            position = new Vector2(200,200)
                        },

                        // Add Enemy to the Sprite List
                        new Enemy(this, enemyOneTexture, 100, 1){
                            //Set position to the Middle of Screen
                            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50,graphics.GraphicsDevice.Viewport.Height / 2)
                        },

                        // Add Bulet Object to the Sprite List
                        new Bullet(this, bulletTexture, 50, 1)
                    };

                    break;

                // GamePadeState: 1
                case 1:
                    // Instantiate Sprite List
                    _sprites = new List<Sprite>
                    {
                        // Add Players to Sprite List

                        // Player 1
                        new Player(this,playerOneTexture, capability1, playerone, state1, 100, 1){
                            position = new Vector2(200,200),
                            playerColor = Color.Blue,
                        },

                        // Add Enemy to the Sprite List
                        new Enemy(this, enemyOneTexture, 100, 1){
                            //Set position to the Middle of Screen
                            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50,graphics.GraphicsDevice.Viewport.Height / 2)
                        },

                        // Add Bulet Object to the Sprite List
                        new Bullet(this, bulletTexture, 50, 1)
                    };
                    break;


                // GamePadeState: 2
                case 2:

                    // Instantiate Sprite List
                    _sprites = new List<Sprite>
                    {
                        // Add Players to Sprite List

                        // Player 1
                        new Player(this, playerOneTexture, capability1, playerone, state1, 100, 1){
                            position = new Vector2(200,200),
                            playerColor = Color.White,
                        },
                        // Player 2
                        new Player(this, playerTwoTexture, capability2, playertwo, state2, 100, 1){
                            position = new Vector2(ScreenWidth - 200,200),
                            playerColor = Color.White,
                        },

                        // Add Enemy to the Sprite List
                        new Enemy(this, enemyOneTexture, 100, 1){
                            //Set position to the Middle of Screen
                            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50,graphics.GraphicsDevice.Viewport.Height / 2)
                        },

                        // Add Bulet Object to the Sprite List
                        new Bullet(this, bulletTexture, 50, 1)
                    };

                    break;

                // GamePadeState: 3
                case 3:

                    // Instantiate Sprite List
                    _sprites = new List<Sprite>
                    {
                        // Add Players to Sprite List

                        // Player 1
                        new Player(this, playerOneTexture, capability1, playerone, state1, 100, 1){
                            position = new Vector2(200,200),
                            playerColor = Color.White,
                        },
                        // Player 2
                        new Player(this, playerTwoTexture, capability2, playertwo, state2, 100, 1){
                            position = new Vector2(ScreenWidth - 200,200),
                            playerColor = Color.White,
                        },
                        // Player 3
                        new Player(this, playerThreeTexture, capability3, playerthree, state3, 100, 1){
                            position = new Vector2(200,ScreenHeight - 200),
                            playerColor = Color.White,
                        },

                        // Add Enemy to the Sprite List
                        new Enemy(this, enemyOneTexture, 100, 1){
                            //Set position to the Middle of Screen
                            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50,graphics.GraphicsDevice.Viewport.Height / 2)
                        },

                        // Add Bulet Object to the Sprite List
                        new Bullet(this, bulletTexture, 50, 1)
                    };

                    break;


            }

        }

        /// <summary>
        /// Start Countdown Timer
        /// Set Enemy Can Shoot if Coutdown hits Zero
        /// </summary>
        public void StartCountdown()
        {
            //end condition, countdown expired
            if (countdownTimer <= 0)
            {
                // Increment timer +1
                timer++;

                //cycle through list of sprites
                for (int i = 0; i < _sprites.Count; i++)
                {
                    //store currently indexed sprite
                    Sprite sprite = _sprites[i];

                    //if current sprite is enemy
                    if (sprite is Enemy)
                    {
                        //create enemy object, if sprite is an Enemy
                        Enemy enemy = sprite as Enemy;
                        
                        //set its can shoot property
                        enemy.CanShoot = true;

                        return;
                    }
                }
               
            }

            //normal condition, decrement counter -1
            countdownTimer--;

        }

        /// <summary>
        /// Check Controllers, Checks How Many Controllers are being used
        /// Sets the Player Count and 
        /// </summary>
        public void CheckControllers()
        {
            // Check the device for Player One
            // If there a controller attached, handle it
            // connect controller then add to capabailities
            capability1 = GamePad.GetCapabilities(playerone);
            capability2 = GamePad.GetCapabilities(playertwo);
            capability3 = GamePad.GetCapabilities(playerthree);

            // checking for 3 controllers connected
            if (capability1.IsConnected && capability2.IsConnected && capability3.IsConnected)
            {

                //capabilities.Add(capability2);    //add each capabilityu added to a list
                //if gamepad capability is connected then gamepad bool is true
                capabilities.Add(capability1);
                capabilities.Add(capability2);
                capabilities.Add(capability3);
                Gamepads = 3;
                playerCount = 3;
                lastplayerCount = playerCount;
            }

            // checking for 2 controllers connected
            else if (capability1.IsConnected && capability2.IsConnected)
            {

                //capabilities.Add(capability2);    //add each capabilityu added to a list
                //if gamepad capability is connected then gamepad bool is true
                capabilities.Add(capability1);
                capabilities.Add(capability2);
                Gamepads = 2;
                playerCount = 2;
                lastplayerCount = playerCount;
            }

            // checking for 1 controllers connected
            else if (capability1.IsConnected)
            {
                state1 = GamePad.GetState(playerone);
                capabilities.Add(capability1);      //add each capabilityu added to a list 
                Gamepads = 1;
                playerCount = 1;                    //if gamepad capability is connected then gamepad bool is true
                lastplayerCount = playerCount;
            }

            //Keyboard State
            else
            {
                Gamepads = 0;
                playerCount = 1;
                lastplayerCount = playerCount;
            }
        }

        /// <summary>
        /// Get Keyboard Access
        /// Allowed to Use Keyboard Controls
        /// </summary>
        /// <returns></returns>
        public bool KeyboardAccess()
        {
            //bool to let player use keyboard
            bool keyboardState = false;

            // check if gamepad is 0
            if (Gamepads == 0)
            {
                keyboardState = true;

                return keyboardState;
            }

            // if still false return false
            return keyboardState;
        }
        
        /// <summary>
        /// At the end of the game results for the game will be shown
        /// Results to be Shown: Surviving Players, How Long Each Player Survived
        /// </summary>
        public void Results()
        {
            //loop through list of sprites
            for (int i = 0; i < _sprites.Count; i++)
            {
                //store currently indexed sprite
                Sprite sprite = _sprites[i];

                //if current sprite is enemy
                if (sprite is Player)
                {
                    //create player object, if sprite is an Player
                    Player player = sprite as Player;

                    // check if player has not died yet
                    if (player.HasDied == false)
                    {
                        spriteBatch.DrawString(font, "Player has survived: Player " + player.PlayerIndex, player.position, Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, player.PlayerIndex + ": has not survived  Time Lasted To: " + player.DeathTime, player.position, Color.White);
                    }
                }
            }
        }

        /// <summary>
        /// Reset Timer Main Timer to 0
        /// CountDown Timer is reset
        /// </summary>
        public void ResetTimer()
        {
            //reset timer
            timer = 0;
            // countdown timer
            countdownTimer = 200;
        }

        #endregion

    }

}
