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

        // --- Object Called Attributes ---- //

        // Map Data Object
        MapData data = new MapData();

        // ---- Player/Sprite Attributes  ---- //

        // Creating Sprite List
        private List<Sprite> _sprites;

        // Add Player list
        private List<Player> _players;

        // Add Player list
        private List<Enemy> _enemies;

        // Add Player list
        private List<Bullet> _bullets;

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

        // count the seconds 
        public int seconds;

        // count minutes
        public int minutes;

        // ScreenHeight and Width Attributes
        public int ScreenWidth;
        public int ScreenHeight;

        // font for printing text on screen
        private SpriteFont font;

        // Rectangle for all Background Vectors
        private Rectangle mainFrame;

        //Texture2Ds for Loading Backgroun/Menu Images      
        Texture2D dojo;
        Texture2D gameover;
        Texture2D playerTexture;
        Texture2D enemyTexture;
        Texture2D resultsrect;
        public Texture2D bulletTexture;       
        Texture2D splashScreen;
        Texture2D gameoverKey;
        Vector2 gameoverCoords;

        //Intro Animation Attributes
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


            _players = new List<Player>();

            // instantiate players list
            _enemies = new List<Enemy>();

            // instantiate players list
            _bullets = new List<Bullet>();
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
            countdownTimer = 5;

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
            playerTexture = Content.Load<Texture2D>("player1");            // monk texture
            enemyTexture = Content.Load<Texture2D>("enemy");               // enemy texture
            bulletTexture = Content.Load<Texture2D>("cyclonebullet");      // bullet texture
            splash = Content.Load<Texture2D>("splashBackground");          // menu texture 
            tree = Content.Load<Texture2D>("tree");                        // tree texture
            eternal = Content.Load<Texture2D>("eternal");                  // Eternal Hazard Title texture
            hazard = Content.Load<Texture2D>("hazard");                    // Hazard Title Texture
            menuPad = Content.Load<Texture2D>("menu");                     // Controller Menu
            menuKey = Content.Load<Texture2D>("menuKey");                  // Keyboard Menu
            dojo = Content.Load<Texture2D>("dojoBG");                      // dojo background texture
            gameover = Content.Load<Texture2D>("gameOver");                // gameover text texture
            font = Content.Load<SpriteFont>("Score");                      // Font Score texture
            splashScreen = Content.Load<Texture2D>("splashscreen");        // Spalsh Screen Texture
            gameoverKey = Content.Load<Texture2D>("gameOverKey");          // Game Over Keyboard Texture
            resultsrect = Content.Load<Texture2D>("resultsrect");          // results rectangle texture

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

            // Read in Map Text
            data.TextReader();

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

                        // check if player won
                        Won();
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

                        // check if player won
                        Won();
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
                    spriteBatch.Draw(dojo, mainFrame, Color.White);

                    //foreach sprite in the list _sprites
                    //Draw the sprites in the spritebatch
                    foreach (var sprite in _sprites)
                    {
                        sprite.Draw(spriteBatch);
                    }



                    //Text On Screen
                    spriteBatch.DrawString(font, "CountDown: " + countdownTimer.ToString(), new Vector2(ScreenWidth / 2 - 52, 25), Color.Red);
                    // spriteBatch.DrawString(font, "Time: " + timer.ToString(), new Vector2(ScreenWidth / 2, 50), Color.Black);
                    spriteBatch.DrawString(font, "Time: " + minutes.ToString() + " : " + seconds.ToString(), new Vector2(ScreenWidth / 2 - 45, 60), Color.Black);


                    // GAMEPADS: 1 or KEYSTATE(SinglePlayer)
                    if (Gamepads < 2)
                    {
                        spriteBatch.DrawString(font, "Score: " + _players[0].Score.ToString(), new Vector2(60, 20), Color.Blue);
                    }
                    // GAMEPADS: 2
                    if (Gamepads == 2)
                    {
                        spriteBatch.DrawString(font, "Player 1 Score: " + _players[0].Score.ToString(), new Vector2(60, 20), Color.Blue);
                        spriteBatch.DrawString(font, "Player 2 Score: " + _players[1].Score.ToString(), new Vector2(ScreenWidth - 200, 20), Color.Green);
                    }
                    // GAMEPADS: 3
                    if (Gamepads == 3)
                    {
                        spriteBatch.DrawString(font, "Player 1 Score: " + _players[0].Score.ToString(), new Vector2(60, 20), Color.Blue);
                        spriteBatch.DrawString(font, "Player 2 Score: " + _players[1].Score.ToString(), new Vector2(ScreenWidth - 200, 20), Color.Green);
                        spriteBatch.DrawString(font, "Player 3 Score: " + _players[2].Score.ToString(), new Vector2(60, ScreenHeight - 50), Color.Purple);
                    }
                    // GAMEPADS: 4
                    if (Gamepads == 4)
                    {
                        spriteBatch.DrawString(font, "Player 1 Score: " + _players[0].Score.ToString(), new Vector2(60, 20), Color.Blue);
                        spriteBatch.DrawString(font, "Player 2 Score: " + _players[1].Score.ToString(), new Vector2(ScreenWidth - 200, 20), Color.Green);
                        spriteBatch.DrawString(font, "Player 3 Score: " + _players[2].Score.ToString(), new Vector2(60, ScreenHeight - 50), Color.Purple);
                        spriteBatch.DrawString(font, "Player 4 Score: " + _players[3].Score.ToString(), new Vector2(ScreenWidth - 200, ScreenHeight - 50), Color.Orange);
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
                        player.DeathTimeSec = (int)seconds;

                        player.DeathTimeMin = (int)minutes;
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
        /// Check all players if they won by surviving for a time limit
        /// Add Score too
        /// </summary>
        public void Won()
        {
            // FORLOOP for allowing Enemies to Shoot
            //cycle through list of sprites
            foreach (var player in _players)
            {
 
                if (player.HasDied == false && minutes == 2)
                {
                    // add 100 to player score
                    player.Score = player.Score + 100;

                    // go to next state
                    state = GameState.GameOver;
                }
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
                        new Player(this,playerTexture, "Player 1", capability1, playerone, state1, 50, 8){
                            position = new Vector2(200,200)
                        },

                        // Add Enemy to the Sprite List
                        new Enemy(this, enemyTexture, 400, 1){
                            //Set position to the Middle of Screen
                            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 200,graphics.GraphicsDevice.Viewport.Height / 2)
                        },

                        // Add Bulet Object to the Sprite List
                        new Bullet(this, bulletTexture, 200, 1)
                    };

                    break;

                // GamePadeState: 1
                case 1:
                    // Instantiate Sprite List
                    _sprites = new List<Sprite>
                    {
                        // Add Players to Sprite List

                        // Player 1
                        new Player(this,playerTexture,"Player 1", capability1, playerone, state1, 50, 8){
                            position = new Vector2(200,200),
                            playerColor = Color.Blue,
                        },

                        // Add Enemy to the Sprite List
                        new Enemy(this, enemyTexture, 400, 1){
                            //Set position to the Middle of Screen
                            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 200,graphics.GraphicsDevice.Viewport.Height / 2)
                        },

                        // Add Bulet Object to the Sprite List
                        new Bullet(this, bulletTexture, 200, 1)
                    };
                    break;


                // GamePadeState: 2
                case 2:

                    // Instantiate Sprite List
                    _sprites = new List<Sprite>
                    {
                        // Add Players to Sprite List

                        // Player 1
                        new Player(this, playerTexture, "Player 1",capability1, playerone, state1, 50, 8){
                            position = new Vector2(200,200),
                            playerColor = Color.Blue,
                        },
                        // Player 2
                        new Player(this, playerTexture, "Player 2",capability2, playertwo, state2, 50, 8){
                            position = new Vector2(ScreenWidth - 200,200),
                            playerColor = Color.Green,
                        },

                        // Add Enemy to the Sprite List
                        new Enemy(this, enemyTexture, 400, 1){
                            //Set position to the Middle of Screen
                            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 200,graphics.GraphicsDevice.Viewport.Height / 2)
                        },

                        // Add Bulet Object to the Sprite List
                        new Bullet(this, bulletTexture, 200, 1)
                    };

                    break;

                // GamePadeState: 3
                case 3:

                    // Instantiate Sprite List
                    _sprites = new List<Sprite>
                    {
                        // Add Players to Sprite List

                        // Player 1
                        new Player(this, playerTexture,"Player 1", capability1, playerone, state1, 50, 8){
                            position = new Vector2(200,200),
                            playerColor = Color.Blue,
                        },
                        // Player 2
                        new Player(this, playerTexture, "Player 2",capability2, playertwo, state2, 50, 8){
                            position = new Vector2(ScreenWidth - 200,200),
                            playerColor = Color.Green,
                        },
                        // Player 3
                        new Player(this, playerTexture,"Player 3", capability3, playerthree, state3, 50, 8){
                            position = new Vector2(200,ScreenHeight - 200),
                            playerColor = Color.Purple,
                        },

                        // Add Enemy to the Sprite List
                        new Enemy(this, enemyTexture, 400, 1){
                            //Set position to the Middle of Screen
                            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 200,graphics.GraphicsDevice.Viewport.Height / 2)
                        },

                        // Add Bulet Object to the Sprite List
                        new Bullet(this, bulletTexture, 200, 1)
                    };

                    break;

                // GamePadeState: 4
                case 4:

                    // Instantiate Sprite List
                    _sprites = new List<Sprite>
                    {
                        // Add Players to Sprite List

                        // Player 1
                        new Player(this, playerTexture,"Player 1", capability1, playerone, state1, 20, 1){
                            position = new Vector2(200,200),
                            playerColor = Color.Blue,
                        },
                        // Player 2
                        new Player(this, playerTexture,"Player 2", capability2, playertwo, state2, 20, 1){
                            position = new Vector2(ScreenWidth - 200,200),
                            playerColor = Color.Green,
                        },
                        // Player 3
                        new Player(this, playerTexture,"Player 3", capability3, playerthree, state3, 20, 1){
                            position = new Vector2(200,ScreenHeight - 200),
                            playerColor = Color.Purple,
                        },
                        // Player 3
                        new Player(this, playerTexture,"Player 4", capability4, playerfour, state4, 20, 1){
                            position = new Vector2(ScreenWidth - 200,ScreenHeight - 200),
                            playerColor = Color.Orange,
                        },

                        // Add Enemy to the Sprite List
                        new Enemy(this, enemyTexture, 400, 1){
                            //Set position to the Middle of Screen
                            position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 ,graphics.GraphicsDevice.Viewport.Height / 2)
                        },

                        // Add Bulet Object to the Sprite List
                        new Bullet(this, bulletTexture,20, 1)
                    };

                    break;
            }

            // check all the sprites in the list
            for (int i = 0; i < 1; i++)
            {
                //store currently indexed sprite
                Sprite sprite = _sprites[i];

                //if current sprite is enemy
                if (sprite is Enemy)
                {
                    //create enemy object, if sprite is an Enemy
                    Enemy enemy = sprite as Enemy;

                    // add player to the list
                    _enemies.Add(enemy);

                    return;
                }
            }

            // check all the sprites in the list
            for (int i = 0; i < 5; i++)
            {
                //store currently indexed sprite
                Sprite sprite = _sprites[i];

                //if current sprite is enemy
                if (sprite is Player)
                {
                    //create enemy object, if sprite is an Enemy
                    Player player = sprite as Player;

                    // add player to the list
                    _players.Add(player);

                    return;
                }
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

                // check if timer hits a second
                // 100 miliseconds =  1 second
                int secondcounter = 100;

                // if timer in miliseconds = second counter
                if (timer == secondcounter)
                {
                    // increment a second + 1
                    seconds++;

                    //reset timer
                    timer = 0;

                }

                // check if time hit a minute
                if (seconds >= 60)
                {
                    // increment minutes +1
                    minutes++;

                    //reset seconds
                    seconds = 0;
                }

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

            else if (countdownTimer > 0)
            {
                // increment timer
                timer++;
                // count up will count up to a certain number and then increment the countdowntimer - 1
                float countupcounter = timer;

                // check if timer hits a second
                // 100 miliseconds =  1 second
                if (countupcounter == 100)
                {
                    // reset timer
                    timer = 0;

                    //normal condition, decrement counter -1
                    countdownTimer--;
                }


            }

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
            // draw rect around results
            spriteBatch.Draw(resultsrect, new Vector2(ScreenWidth / 2 - 160, ScreenHeight / 2 + 25), Color.White);
            // Print Text: Results
            spriteBatch.DrawString(font, "Results ", new Vector2(ScreenWidth / 2 - 37, ScreenHeight / 2 + 37), Color.White);

            // int position x for printing players stats
            int playerpoxX = ScreenWidth / 2 - 145;

            // int position y for printing players stats
            int playerpoxY = ScreenHeight / 2 + 89;

            // int position x for printing players stats
            int resultboxX = ScreenWidth / 2 - 160;

            // int position y for printing players stats
            int resultboxY = ScreenHeight / 2 + 25;

            // check through all sprites
            foreach (Sprite sprite in _sprites)
            {
                // check through to see if a sprite is a player
                if (sprite is Player)
                {
                    Player player = sprite as Player;

                    // temp list for players
                    List<Player> _players = new List<Player>();

                    // add players to list
                    _players.Add(player);

                    // check if player has not died yet
                    foreach (var players in _players)
                    {
                        // if player has not died
                        if (player.HasDied == false)
                        {
                            spriteBatch.Draw(resultsrect, new Vector2(resultboxX, resultboxY + 50), Color.White);
                            spriteBatch.DrawString(font, player.Name + " has Survived! " + " Score: " + player.Score, new Vector2(playerpoxX + 10, playerpoxY), Color.LightGreen);

                        }
                        // else print their death time
                        else
                        {
                            spriteBatch.Draw(resultsrect, new Vector2(resultboxX, resultboxY + 50), Color.White);
                            spriteBatch.DrawString(font, player.Name + " Time Lasted: " + player.DeathTimeMin + " : " + player.DeathTimeSec + " Score: " + player.Score, new Vector2(playerpoxX, playerpoxY), Color.Red);
                        }
                    }

                    // increment player text positions foreach player
                    resultboxY = resultboxY + 75;
                    playerpoxY = playerpoxY + 50;
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
            countdownTimer = 3;
        }

        /// <summary>
        /// Set Position Data for Players if File Says So
        /// Set Boss Position
        /// </summary>
        public void SetData()
        {
            // Mapa Data Object
            MapData mD = new MapData();

            // check to make sure its no 0,0 
            // its hard to do when placing coordinates
            if (mD.P1.X != 0 && mD.P1.Y != 0)
            {
                // first player in list
                _players[0].position.X = mD.P1.X;
                _players[0].position.Y = mD.P1.Y;
            }

            if (mD.P2.X != 0 && mD.P2.Y != 0)
            {
                // first player in list
                _players[1].position.X = mD.P2.X;
                _players[1].position.Y = mD.P2.Y;
            }

            if (mD.P3.X != 0 && mD.P3.Y != 0)
            {
                // first player in list
                _players[2].position.X = mD.P3.X;
                _players[2].position.Y = mD.P3.Y;
            }

            if (mD.P4.X != 0 && mD.P4.Y != 0)
            {
                // first player in list
                _players[3].position.X = mD.P4.X;
                _players[3].position.Y = mD.P4.Y;
            }


            // first map
            if (mD.DomainName != null)
            {
                // check if image changed
                if (mD.BkImage != null)
                {
                    // set dojo to new image name
                    dojo = Content.Load<Texture2D>(mD.DomainName);
                }
            }
            
        }

        #endregion

    }

}
