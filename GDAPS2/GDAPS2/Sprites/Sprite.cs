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
    public class Sprite : ICloneable
    {
        
        //sprite attributes
        protected Texture2D _texture;
        public Vector2 position;
        public float speed = 5f;
        public Rectangle rectangle;

        //attributes for sprite sheet animation
        protected int _frameWidth;
        protected int _frames;
        public int framesElapsed;
        public double timePerFrame = 100;
        public int currentFrame;

        //inhereited bullet attributes
        protected float _rotation;
        public float scale = 1;
        public Vector2 origin;
        public Sprite Parent;

        public bool isRemoved = false;  //check to see if sprite needs to be removed

        //Movment Attibutes for Bullet
        public Vector2 Direction;
        public float rotationVelocity = 3f;
        public float linearVelocity = 10f;

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

        public Sprite()
        {
            // no parameter constructor
        }

        //constructor with taking in texture
        /// <summary>
        /// This will be overrided to load in the constructor in the class you call it in
        /// </summary>
        /// <param name="texture"></param>
        public Sprite(Texture2D texture, int frameWidth, int frames)
        {
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

        //clone will reference the bullet before it
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
