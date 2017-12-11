using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GDAPS2
{
    /// <summary>
    /// Player Class where we create a player's attributes, and methods
    /// </summary>
    public class Player : Sprite
    {
        //Attributes

        // main game object
        MainGame mG;

        // name for each character
        private string name;

        // checks when player is dead
        private bool hasDied;

        // bool to check if the player game is over
        private bool gameover = false;

        // each player has a color if in multiplayer mode
        public Color playerColor;

        // check if the player should switch direction (animation)
        public bool facingSwitch;

        // int to determine when player died Minutes
        private int deathTimeMin;

        // int to determine when player died Seconds
        private int deathTimeSec;

        // int to count player score
        private int score;

        //Sprite Class Object
        public Bullet bullet;

        //capability 
        private GamePadCapabilities capability;

        //playerIndex
        private PlayerIndex playerindex;

        //controllerState
        private GamePadState state;

        //enumeration for image flipping
        private enum PlayerDirection { MovingLeft, MovingRight };
        PlayerDirection playerFacing = PlayerDirection.MovingRight;

        #region Properties

        public PlayerIndex PlayerIndex
        {
            get { return playerindex; }
        }

        public bool HasDied
        {
            get { return hasDied; }
        }

        public bool Gameover
        {
            get { return gameover; }
        }

        public int DeathTimeMin
        {
            get { return deathTimeMin; }
            set { deathTimeMin = value; }
        }

        public int DeathTimeSec
        {
            get { return deathTimeSec; }
            set { deathTimeSec = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        //create a new constructor for Player from Sprite
        //Instantiate Movement and First position
        //since we only need to do this to get our Update method and attributes from Sprite
        public Player(MainGame mg, Texture2D texture, string name, GamePadCapabilities capability, PlayerIndex pi, GamePadState gpstate, int frameWidth, int frames) : base(texture, frameWidth, frames)
        {
            // Check the device for Player One
            this.capability = capability;
            playerindex = pi;
            state = gpstate;
            currentFrame = frames;

            // scale reduced
            scale = .1f;

            mG = mg;
            this.name = name;

            _frames = 1;

            hasDied = false;
            gameover = false;

            // set score to 0
            score = 0;

            // set speed
            speed = 3;

            

        }

        /// <summary>
        /// Keyboard Overwriiten Player Constructor
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="frameWidth"></param>
        /// <param name="frames"></param>
        public Player(Texture2D texture, int frameWidth, int frames) : base(texture, frameWidth, frames)
        {
            scale = .75f;
        }

        /// <summary>
        /// override void
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="sprites"></param>
        public override void Update(GameTime gametime, List<Sprite> sprites)
        {
            //Calling the Move Method
            Move();

            //bool that takes in the value from keyboard access
            bool allowKeyboard = mG.KeyboardAccess();

            //if true allow moveing with controller
            KeyboardMove();

            //calculate current frame
            framesElapsed = (int)(gametime.TotalGameTime.TotalMilliseconds / timePerFrame);
            currentFrame = framesElapsed % _frames;

            //foreach sprite in the list
            //if its the sprite continue, if its another rectangle HasDied = true
            foreach (var sprite in sprites)
            {
                var player = sprite as Player;
                if (sprite == this || sprite == player) //if connects with player or itself then do nothing
                    continue;

                // if the another sprites rectangle intersects with players rectangle
                if (sprite._Rectangle.Intersects(_Rectangle))
                {
                    //HasDied  = true
                    this.hasDied = true;
                    gameover = true;
                }
            }

            //Clamp the Player's position so they don't go off screen
            position.X = MathHelper.Clamp(position.X, 0, (mG.ScreenWidth + 25) - _Rectangle.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, (mG.ScreenHeight + 25) - _Rectangle.Height);

            // add points if needed
            AddPoints();

            // death time equals the seconds he was alive
            int deathtime = mG.seconds;

            // track the player if he survived a minute
            int minutes = 0;

            // if the deathtime (seconds) 60 then start adding minutes
            if (deathTimeSec == 60)
            {
                // increment minutes
                minutes++;

                // deathtime now equals minutes
                deathTimeMin = minutes;

            }

            // check death time
            if (hasDied == true)
            {
                // initialize deathtime
                this.deathTimeSec = deathtime;

                this.deathTimeMin = minutes;

            }

            

        }
        /// <summary>
        /// Overwrite sprite draw method to utilize player state sprite effects
        /// </summary>
        /// <param name="spirtebatch"></param>
        public override void Draw(SpriteBatch spirtebatch)
        {
            switch (playerFacing)
            {
                //when the player is moving right
                case PlayerDirection.MovingRight:

                    //draw the source image animation
                    spirtebatch.Draw(_texture, position, new Rectangle(currentFrame * _frameWidth, 0, _frameWidth, _texture.Height), Color.White, _rotation, origin, 1, SpriteEffects.None, 0);
                    break;

                //when the player is moving left
                case PlayerDirection.MovingLeft:

                    //flip the source image
                    spirtebatch.Draw(_texture, position, new Rectangle(currentFrame * _frameWidth, 0, _frameWidth, _texture.Height), Color.White, _rotation, origin, 1, SpriteEffects.FlipHorizontally, 0);
                    break;
            }
        }

        /// <summary>
        /// This is how the player moves around the screen getting the input from the input class
        /// </summary>
        public void Move()
        {          
            // If there a controller attached, handle it
            if (capability.IsConnected)
            {
                // Get the current state of Controller1
                GamePadState state = GamePad.GetState(playerindex);

                // You can check explicitly if a gamepad has support 
                //for a certain feature
                if (capability.HasLeftXThumbStick)
               {
                    // Check teh direction in X axis of left analog stick
                    if(state.ThumbSticks.Left.Length() > 0.1f)
                    {
                        position.X += state.ThumbSticks.Left.X * speed;
                        position.Y -= state.ThumbSticks.Left.Y * speed;
                    }
                    
                }        
            }
        }


        /// <summary>
        /// KeyBoard State Movement Controls
        /// </summary>
        public void KeyboardMove()
        {
            //move left
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position.X -= speed;
                playerFacing = PlayerDirection.MovingLeft;
            }
            //move right
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X += speed;
                playerFacing = PlayerDirection.MovingRight;
            }
            //move up
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position.Y -= speed;
            }
            //move down
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position.Y += speed;
            }
        }

        /// <summary>
        /// Add Points for Lasting Longer
        /// </summary>
        public void AddPoints()
        {
            // temp to hold score until end of game
            int temp = score;

            // check if player has not died yet
            if (hasDied == false)
            {
                // check how long seconds has been running, if player survived long enough
                if (mG.seconds == 30)
                {
                    // add default point to player score
                    score = score + 1;
                }
                if (mG.minutes == 1)
                {
                    // add default point to player score
                    score = score + 1;
                }
            }
            else
            {
                // check score is still temp score when player dies
                score = temp;
            }

        }
    }
}




