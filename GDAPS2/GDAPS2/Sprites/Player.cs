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

        private bool hasDied = false;
        private bool gameover = false;
        public Color playerColor;
        public bool facingSwitch;

        // int to determine when player died
        private int deathTime;

        // int to count player score

        
        MainGame mG = new MainGame();   //Main Game Object

        public Bullet bullet;   //Sprite Class Object

        //Attributes to get the controllers:
        private GamePadCapabilities capability; //capability 
        private PlayerIndex playerindex;        //playerIndex
        private GamePadState state;             //controllerState

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

        public int DeathTime
        {
            get { return deathTime; }
            set { deathTime = value; }
        }

        //enumeration for image flipping
        private enum PlayerDirection { MovingLeft, MovingRight };
        PlayerDirection playerFacing = PlayerDirection.MovingRight;

        //create a new constructor for Player from Sprite
        //Instantiate Movement and First position
        //since we only need to do this to get our Update method and attributes from Sprite
        public Player(Texture2D texture, GamePadCapabilities capability, PlayerIndex pi, GamePadState gpstate, int frameWidth, int frames) : base(texture, frameWidth, frames)
        {
            // Check the device for Player One
            this.capability = capability;
            playerindex = pi;
            state = gpstate;

            scale = .75f;

            
        }

        //Keyboard Overwriiten Player Constructor
        public Player(Texture2D texture, int frameWidth, int frames) : base(texture, frameWidth, frames)
        {
            scale = .75f;
        }

        //override void 
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

                //Intersetc Method
                //HasDied  = true
                if (sprite._Rectangle.Intersects(_Rectangle))
                {
                    this.hasDied = true;
                    gameover = true;
                }
            }

            //Clamp the Player's position so they don't go off screen
            position.X = MathHelper.Clamp(position.X, 0, (mG.ScreenWidth + 25) - _Rectangle.Width);
            position.Y = MathHelper.Clamp(position.Y, 0, (mG.ScreenHeight + 25) - _Rectangle.Height);

            int deathtime = (int)mG.timer;

            // check death time
            if (hasDied == true)
            {
                this.deathTime = deathtime;
            }
        }

        //Overwrite sprite draw method to utilize player state sprite effects
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

        //KeyBoard State Move
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


    }
}




