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
    /// class sprite takes in a vector2,and spriteTexture
    /// Parent Class of Sprite Classes
    /// </summary>
    public class Sprite
    {
        // ------ Sprite Attributes ----- //
        
        // parent texture2D
        protected Texture2D _texture;

        // parent vector2D position 
        public Vector2 position;

        // parent movement speed
        public float speed;

        // parent rectangle
        public Rectangle rectangle;

        // ----- Sprite Animation ----- //

        // frame width
        protected int _frameWidth;

        // count of frames
        protected int _frames;

        // frames counted over time
        public int framesElapsed;

        // time during each frame
        public double timePerFrame;

        // current frame the sprite is on
        public int currentFrame;

        // ----- Bullet Related Attributes ----- //

        // rotation velocity
        protected float _rotation;

        // scale of the sprite object
        public float scale;

        // origin point of object
        public Vector2 origin;

        // check to see if sprite needs to be removed
        public bool isRemoved;

        //angle float
        public double angle = 0;

        //get Rectangle Object
        public Rectangle _Rectangle
        {
            get
            {
                return rectangle = new Rectangle((int)position.X, (int)position.Y, _frameWidth / 2, _texture.Height / 2);
            }
        }

        //constructor with taking in texture
        /// <summary>
        /// This will be overrided to load in the constructor in the class you call it in
        /// </summary>
        /// <param name="texture"></param>
        public Sprite(Texture2D texture, int frameWidth, int frames)
        { 
            speed = 5f;

            timePerFrame = 100;

            scale = 1;

            isRemoved = false;

            _texture = texture;

            origin = new Vector2(_frameWidth / 2, _texture.Height / 2);

            _frameWidth = frameWidth;

            _frames = frames;
        }

        //Parent Method for Update Method
        //takes in gametime and the lists of sprites 
        public virtual void Update(GameTime gametime, List<Sprite> sprites)
        {
            
        }


        //Parent Method of drawing spirtes, will be use by bullet, enemy, player
        public virtual void Draw(SpriteBatch spirtebatch)
        {
            spirtebatch.Draw(_texture, position, new Rectangle(currentFrame * _frameWidth, 0, _frameWidth, _texture.Height), Color.White, _rotation, origin, scale, SpriteEffects.None, 0);
        }

    }
}
