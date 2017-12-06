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
 * 11.21.2017
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

        private List<Sprite> _sprites;  //Creating Sprite List

        // misc.
        // int for counting score(placeholder)
        private int score = 0;
        KeyboardState previousState;
        KeyboardState keyState;

        // Font
        private SpriteFont font;

        // float 
        public float timer;
        public int bulletPattern;
        public int countdownTimer;

        // ScreenHeight and Width Attributes
        public int ScreenWidth;
        public int ScreenHeight;

        // enum GameStates to handle each game state {Menu, Game, GameOver}
        enum GameState { Menu, Game, GameOver };
        GameState state = GameState.Menu;        // gamestate object to acces gamestates
        int playerCount;                         // how many players in game
        int lastplayerCount;                     // whatever the lastamount of players it was reload it

        Rectangle mainFrame;                     // Rectangle for all Background Vectors

        //Texture2Ds for Loading Backgroun/Menu Images
        Texture2D menu;
        Texture2D dojo;
        Texture2D gameover;
        Texture2D playerTexture;
        Texture2D enemyTexture;
        Texture2D bulletTexture;
        Texture2D splash;
        Texture2D tree;
        Texture2D eternal;
        Texture2D hazard;
        Texture2D splashScreen;
        Texture2D gameoverKey;

        //coordinates for animated states
        Vector2 splashBgCoords;
        Vector2 treeCoords;
        Vector2 eternalCoords;
        Vector2 hazardCoords;
        Vector2 menuCoords;
        Vector2 gameoverCoords;

        //Controller Exception
        ControllerException cE = new ControllerException();

        #endregion

    #region Capabilities
        //Controller Attributes
        List<GamePadCapabilities> capabilities = new List<GamePadCapabilities>(); //creates a list of capabilities

        //capbilities for each controller connected
        GamePadCapabilities capability1;
        GamePadCapabilities capability2;
        GamePadCapabilities capability3;
        GamePadCapabilities capability4;

        //player indexes
        PlayerIndex player1 = PlayerIndex.One;
        PlayerIndex player2 = PlayerIndex.Two;
        PlayerIndex player3 = PlayerIndex.Three;
        PlayerIndex player4 = PlayerIndex.Four;

        //GamePadState stateC will take in all the states
        //used later for input control
        GamePadState state1;
        GamePadState state2;
        GamePadState state3;

        GamePadState previousState1;
        GamePadState previousState2;
        GamePadState previousState3;

        //number of gamepads in current game
        public int gamepads;

#endregion

    #region Game
        //Constructor
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Preferred Dimensions of the Screen
            ScreenWidth = graphics.PreferredBackBufferWidth = 1920;
            ScreenHeight = graphics.PreferredBackBufferHeight = 1080;
            
            graphics.IsFullScreen = true;   // make window fullscreen
            graphics.ApplyChanges();        // apply changes

            //initial animated object locations
            splashBgCoords = new Vector2(0, 0);
            treeCoords = new Vector2(1920, 0);
            eternalCoords = new Vector2(550, 350);
            hazardCoords = new Vector2(475, 525);
            menuCoords = new Vector2(840, 765);
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
            //hero initialized rectangle vector variables
            gamepads = -1;

            //one controller has to be connected at the start of the game
            //once the game has loaded you can add more 
            capability1 = GamePad.GetCapabilities(player1);
            capability2 = GamePad.GetCapabilities(player2);
            capability3 = GamePad.GetCapabilities(player3);

            //Game Manager
            countdownTimer = 200;

            //check controllers
            CheckControllers();

            //previous key holds current key state
            previousState = Keyboard.GetState();

            //Get Old gamepad state
            previousState1 = GamePad.GetState(player1);
            previousState2 = GamePad.GetState(player2);
            previousState3 = GamePad.GetState(player3);

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
            playerTexture = Content.Load<Texture2D>("player1");            //monk texture
            enemyTexture = Content.Load<Texture2D>("enemy");               //enemy texture
            bulletTexture = Content.Load<Texture2D>("bulletSheet");        //bullet texture
            splash = Content.Load<Texture2D>("splashBackground");          //menu texture 
            tree = Content.Load<Texture2D>("tree");
            eternal = Content.Load<Texture2D>("eternal");
            hazard = Content.Load<Texture2D>("hazard");
            menu = Content.Load<Texture2D>("menu");
            dojo = Content.Load<Texture2D>("dojoBG");                      //dojo background texture
            gameover = Content.Load<Texture2D>("gameOver");                //gameover texture
            font = Content.Load<SpriteFont>("Score");
            splashScreen = Content.Load<Texture2D>("splashscreen");
            gameoverKey = Content.Load<Texture2D>("gameOverKey");



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
            //keystate
            keyState = Keyboard.GetState();

            //check controllers
            CheckControllers();

            //switch state 
            switch (state)
            {

                //Menu Game State
                //Loads Content lets player access game screen or close
                case GameState.Menu:

                    //Load Content
                    LoadContent();

                    //background image
                    if (splashBgCoords.X > -100)
                    {
                        splashBgCoords = new Vector2(splashBgCoords.X - 1, splashBgCoords.Y);
                    }

                    //forground tree
                    if (treeCoords.X > -200)
                    {
                        treeCoords = new Vector2(treeCoords.X - 20, treeCoords.Y);
                    }


                    //if gamepad true, controllers are connected
                    if (gamepads == 1)
                    {
                        GamePadCapabilities controllerC = GamePad.GetCapabilities(PlayerIndex.One);
                        //state2 = GamePad.GetState(player2);
                        //foreach (var controller in capabilities)
                        //{
                        state1 = GamePad.GetState(PlayerIndex.One);

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





                    //if gamepad true, controllers are connected
                    if (gamepads == 2)
                    {
                        foreach (var controller in capabilities)
                        {
                            state1 = GamePad.GetState(player1);
                            state2 = GamePad.GetState(player2);
                            if (controller.GamePadType == GamePadType.GamePad)
                            {
                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Start) || state2.IsButtonDown(Buttons.Start) && !previousState1.IsButtonDown(Buttons.Start) && !previousState2.IsButtonDown(Buttons.Start))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    NewGame();
                                }

                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Y) || state2.IsButtonDown(Buttons.Y) && !previousState1.IsButtonDown(Buttons.Y) && !previousState2.IsButtonDown(Buttons.Y))
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

                    //if gamepad true, controllers are connected
                    if (gamepads == 3)
                    {
                        foreach (var controller in capabilities)
                        {
                            state1 = GamePad.GetState(player1);
                            state2 = GamePad.GetState(player2);
                            state3 = GamePad.GetState(player3);
                            if (controller.GamePadType == GamePadType.GamePad)
                            {
                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Start) || state2.IsButtonDown(Buttons.Start) || state3.IsButtonDown(Buttons.Start) && !previousState1.IsButtonDown(Buttons.Start) && !previousState2.IsButtonDown(Buttons.Start) && !previousState3.IsButtonDown(Buttons.Start))
                                {
                                    // the button has just been pressed
                                    // do something here
                                    NewGame();
                                }

                                // You can also check the controllers "type"
                                if (state1.IsButtonDown(Buttons.Y) || state2.IsButtonDown(Buttons.Y) || state3.IsButtonDown(Buttons.Y) && !previousState1.IsButtonDown(Buttons.Y) && !previousState2.IsButtonDown(Buttons.Y) && !previousState3.IsButtonDown(Buttons.Y))
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

                    if (gamepads == 0)
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

                        previousState = keyState;
                    }

                    break;

                case GameState.Game:

                    if (gamepads > 0)
                    {

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

                    else if (gamepads == 0)
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

                    previousState = keyState;

                    break;


                case GameState.GameOver:

                    //No Code For GameOVer Yet
                    // Check the device for Player One
                    foreach (var controller in capabilities)
                    {
                        //Gamepad State
                        if (gamepads > 0)
                        {
                            state1 = GamePad.GetState(player1);
                            state2 = GamePad.GetState(player2);
                            state3 = GamePad.GetState(player3);
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
                    }

                    //Keyboard State
                    if (gamepads == 0)
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
            GraphicsDevice.Clear(Color.DarkGreen);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (state)
            {

                case GameState.Menu:

                    //display menu image
                    if (gamepads > 0)
                    {
                        // Opening Transition and Title Screen
                        spriteBatch.Draw(splash, splashBgCoords, Color.White);
                        spriteBatch.Draw(tree, treeCoords, Color.White);
                        if (treeCoords.X < 0)
                            spriteBatch.Draw(eternal, eternalCoords, Color.White);
                        if (treeCoords.X < -100)
                            spriteBatch.Draw(hazard, hazardCoords, Color.White);
                        if (treeCoords.X < -180)
                            spriteBatch.Draw(menu, menuCoords, Color.White);
                    }
                    else
                    {
                        //for key
                        // Opening Transition and Title Screen
                        spriteBatch.Draw(splash, splashBgCoords, Color.White);
                        spriteBatch.Draw(tree, treeCoords, Color.White);
                        if (treeCoords.X < 0)
                            spriteBatch.Draw(eternal, eternalCoords, Color.White);
                        if (treeCoords.X < -100)
                            spriteBatch.Draw(hazard, hazardCoords, Color.White);
                        if (treeCoords.X < -180)
                            spriteBatch.Draw(menu, menuCoords, Color.White);
                        //spriteBatch.Draw(splashScreen, mainFrame, Color.White);
                        spriteBatch.DrawString(font, "Controllers Are Recommended for this Game", new Vector2(ScreenWidth - 350, ScreenHeight - 150), Color.White);
                    }
                    
                    break;

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
                    //start countdown timer
                    //spriteBatch.DrawString(font, "Bullet Pattern Time: " + bulletPattern.ToString(), new Vector2(100, 100), Color.White);
                    spriteBatch.DrawString(font, "CountDown: " + countdownTimer.ToString(), new Vector2(ScreenWidth / 2, 10), Color.Red);
                    spriteBatch.DrawString(font, "Time: " + timer.ToString(), new Vector2(ScreenWidth / 2, 50), Color.Black);

                    if (gamepads == 3)
                    {
                        spriteBatch.DrawString(font, "Player 1 Score: " + score.ToString(), new Vector2(60, 20), Color.Blue);
                        spriteBatch.DrawString(font, "Player 2 Score: " + score.ToString(), new Vector2(ScreenWidth - 200, 20), Color.Green);
                        spriteBatch.DrawString(font, "Player 3 Score: " + score.ToString(), new Vector2(60, ScreenHeight - 50), Color.Purple);
                    }
                    if (gamepads == 2)
                    {
                        spriteBatch.DrawString(font, "Player 1 Score: " + score.ToString(), new Vector2(60, 20), Color.Blue);
                        spriteBatch.DrawString(font, "Player 2 Score: " + score.ToString(), new Vector2(ScreenWidth - 200, 20), Color.Green);
                    }
                    if(gamepads < 2)
                    {
                        spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(60, 20), Color.Blue);
                    }

                    

                    break;

                case GameState.GameOver:

                    //GamePads for Controllers
                    if (gamepads == 1 || gamepads == 2 || gamepads == 3)
                    {
                        spriteBatch.Draw(splash, mainFrame, Color.White);
                        spriteBatch.Draw(gameover, gameoverCoords, Color.White);
                        Results();
                    }
                    if(gamepads == 0)
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
            for (int i = 0; i < _sprites.Count; i++)
            {
                Sprite sprite = _sprites[i];

                // if the sprite is a player then true
                if (sprite is Player)
                {
                    Player player = sprite as Player;
                    if (player.HasDied == true)
                    {
                        player.isRemoved = true;
                        playerCount -= 1;
                        player.DeathTime = (int)timer;
                    }
                }

            }
            

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

            //reset timer
            timer = 0;
            countdownTimer = 200;



            //gets last player count and loads up the amount of players
            if (lastplayerCount == 3)
            {
                playerCount = 3;
            }
            if (lastplayerCount == 2)
            {
                playerCount = 2;
            }

            if(lastplayerCount == 1)
            {
                playerCount = 1;
            }


            //takes in playerCount for condition
            switch (gamepads)
            {
                //KeyBoard Case
                case 0:

                    //list of sprites are created in MainGame
                    _sprites = new List<Sprite>()
            {
                //Have to get this sprite list from player
                 new Player(playerTexture,50,8)
                 {
                     position = new Vector2(200,200),
                     playerColor = Color.White,

                 },


                //Creating a new Enemy Object
                //New position, Buell Object
                new Enemy(enemyTexture, 400, 1)
                {
                    //Set position to the Middle of Screen
                    position = new Vector2(graphics.GraphicsDevice.Viewport.
                       Width / 2 - 200,graphics.GraphicsDevice.Viewport.
                       Height / 2),

                },

                new Bullet(bulletTexture,200,1)

            };

                    break;

                case 1:
                    //list of sprites are created in MainGame
                    _sprites = new List<Sprite>()
            {
                //Have to get this sprite list from player
                 new Player(playerTexture, capability1,player1,state1,50,8)
                 {
                     position = new Vector2(200,200),
                     playerColor = Color.White,
                 },


                //Creating a new Enemy Object
                //New position, Buell Object
                new Enemy(enemyTexture, 400, 1)
                {
                    //Set position to the Middle of Screen
                    position = new Vector2(graphics.GraphicsDevice.Viewport.
                       Width / 2 - 200,graphics.GraphicsDevice.Viewport.
                       Height / 2)
                },

                new Bullet(bulletTexture,200,1)

            };
                    break;



                case 2:

                    //list of sprites are created in MainGame
                    _sprites = new List<Sprite>()
            {


                //Have to get this sprite list from player
                 new Player(playerTexture, capability1,player1,state1,50,8)
                 {
                     position = new Vector2(200,200),
                     playerColor = Color.Blue,
                 },
                 new Player(playerTexture, capability2,player2,state2,50,8)
                 {
                     position = new Vector2(800,800),
                     playerColor = Color.Green,
                 },

                //Creating a new Enemy Object
                //New position, Buell Object
                new Enemy(enemyTexture,400,1)
                {
                    //Set position to the Middle of Screen
                    position = new Vector2(graphics.GraphicsDevice.Viewport.
                       Width / 2 - 200,graphics.GraphicsDevice.Viewport.
                       Height / 2),
                },

            };

                    break;

                case 3:

                    //list of sprites are created in MainGame
                    _sprites = new List<Sprite>()
            {


                //Have to get this sprite list from player
                 new Player(playerTexture, capability1,player1,state1,50,8)
                 {
                     position = new Vector2(200,200),
                     playerColor = Color.Blue,
                 },
                 new Player(playerTexture, capability2,player2,state2,50,8)
                 {
                     position = new Vector2(ScreenWidth - 200,200),
                     playerColor = Color.Green,
                 },
                 new Player(playerTexture, capability3,player3,state3,50,8)
                 {
                     position = new Vector2(200,ScreenHeight - 200),
                     playerColor = Color.Purple,
                 },

                //Creating a new Enemy Object
                //New position, Buell Object
                new Enemy(enemyTexture,400,1)
                {
                    //Set position to the Middle of Screen
                    position = new Vector2(graphics.GraphicsDevice.Viewport.
                       Width / 2 - 200,graphics.GraphicsDevice.Viewport.
                       Height / 2)
                },

            };

                    break;


            }

        }

        //this will start as soon as the game starts
        //this method will recursively count down timer
        //and begin firing bullets
        public void StartCountdown()
        {
            //end condition, countdown expired
            if (countdownTimer <= 0)
            {
                timer++;
                //cycle through list of sprites
                for (int i = 0; i < _sprites.Count; i++)
                {
                    //store currently indexed sprite
                    Sprite sprite = _sprites[i];

                    //if current sprite is enemy
                    if (sprite is Enemy)
                    {
                        //create enemy object
                        Enemy enemy = sprite as Enemy;
                        
                        //set its can shoot property
                        enemy.CanShoot = true;

                        return;
                    }
                }
               
            }

            //normal condition, decrement counter
            countdownTimer--;


        }

        public void CheckControllers()
        {
            // Check the device for Player One
            // If there a controller attached, handle it
            // connect controller then add to capabailities
            capability1 = GamePad.GetCapabilities(player1);
            capability2 = GamePad.GetCapabilities(player2);
            capability3 = GamePad.GetCapabilities(player3);

            if (capability1.IsConnected && capability2.IsConnected && capability3.IsConnected)
            {

                //capabilities.Add(capability2);    //add each capabilityu added to a list
                //if gamepad capability is connected then gamepad bool is true
                capabilities.Add(capability1);
                capabilities.Add(capability2);
                capabilities.Add(capability3);
                gamepads = 3;
                playerCount = 3;
                lastplayerCount = playerCount;
            }

            if (capability1.IsConnected && capability2.IsConnected)
            {

                //capabilities.Add(capability2);    //add each capabilityu added to a list
                //if gamepad capability is connected then gamepad bool is true
                capabilities.Add(capability1);
                capabilities.Add(capability2);
                gamepads = 2;
                playerCount = 2;
                lastplayerCount = playerCount;
            }

            else if (capability1.IsConnected)
            {
                state1 = GamePad.GetState(player1);
                capabilities.Add(capability1);      //add each capabilityu added to a list 
                gamepads = 1;
                playerCount = 1;                    //if gamepad capability is connected then gamepad bool is true
                lastplayerCount = playerCount;
            }

            //Keyboard State
            else
            {
                gamepads = 0;
                playerCount = 1;
                lastplayerCount = playerCount;
            }
        }

        public bool KeyboardAccess()
        {
            //bool to let player use keyboard
            bool keyboardState = false;

            if (gamepads == 0)
            {
                keyboardState = true;

                return keyboardState;
            }

            return keyboardState;
        }
        
        /// <summary>
        /// At the end of the game results for the game will be shown
        /// Results to be Shown: Surviving Players, How Long Each Player Survived
        /// </summary>
        public void Results()
        {
            //cycle through list of sprites
            for (int i = 0; i < _sprites.Count; i++)
            {
                //store currently indexed sprite
                Sprite sprite = _sprites[i];

                //if current sprite is enemy
                if (sprite is Player)
                {
                    Player player = sprite as Player;

                    if (player.HasDied == false)
                    {
                        spriteBatch.DrawString(font, "Player has survived: Player " + player.PlayerIndex, player.position, Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, player.PlayerIndex + ": has not survived  Time Lasted To: " + player.DeathTime, player.position, Color.White);
                    }

                    return;
                }
            }
        }

        #endregion

    }

}
