using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GDAPS2
{
    /// <summary>
    /// Bullet Class Inherits from Sprite Class
    /// This is where Bullet Collision is Stored and Shooting the Bullet with no Pattern
    /// </summary>
    public class Bullet : Sprite
    {
        //attributes
        private float _timer;           //timer will be calcualted as gametime in total seconds

        //counts frames as it's updated
        public int frameCounter;

        //counts how many bullets each farame
        public int rateOfFire;

        //int timer check to shoot another bullet
        public int bulletcount;

        // direction of where the bullet is traveling
        public Vector2 Direction;

        // rotational velocity
        public float rotationVelocity = 3f;

        // linear velocity
        public float linearVelocity = 10f;

        // Main Object Reference
        MainGame mG;

        //overriding spriteclass method
        public Bullet(MainGame mg, Texture2D texture, int frameWidth, int frames) : base(texture, frameWidth, frames)
        {
            mG = mg;
            bulletcount = 1;
        }

        //if the lifespan is greater(lasts longer) than bullet dies
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            //Getting the Game Time in Seconds
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //adds a default angle of 1 degree per update multiplied by the rotationalVelocity variable
            angle += 1 * rotationVelocity;

            //if the angle is greater than 360, set it back to 0
            if (angle > 360)
            {
                angle = 0;
            }

            //if x many frames has elapsed
            if (frameCounter > rateOfFire)
            {
                position += Direction * linearVelocity;
                frameCounter = 0;
            }

            //increment count +1
            frameCounter++;

            //Loop to check if any bullet in the list is greater than screen boundaries
            //Also Calls CheckScreenCollision()
            for (int i = 0 ; i < sprites.Count ; i++)
            {
                //Call Check Screen Collision
                CheckScreenCollision();

                //if sprites isRemoved == true then remove element
                if (sprites[i].isRemoved)
                {
                    //Remove Sprite From List
                    sprites.RemoveAt(i);

                    //Decrement the Value at i
                    i--;
                }
            }

        }

        //CheckScreenCollision
        /// <summary>
        /// Check Screen Collision will check if it collides with the screen boundaries
        /// If Position is Greater than Screen Boundaries
        /// Is Removed = True;
        /// </summary>
        public void CheckScreenCollision()
        {
            if ( position.Y >= mG.ScreenHeight)
                isRemoved = true;

            if (position.Y <= 0)
                isRemoved = true;

            if (position.X >= mG.ScreenWidth)
                isRemoved = true;

            if (position.X <= 0)
                isRemoved = true;
        }

        /// <summary>
        /// Load Data from bullet ini file
        /// </summary>
        public void LoadBulletData()
        {
            //File Data Object
            BulletData fD = new BulletData();

            fD.TextReader();

            //Parse Data
            angle += double.Parse(fD.angle);

            rotationVelocity = int.Parse(fD.rotationalVelocity);

            linearVelocity = int.Parse(fD.velocity);

        }

        #region Bullet Patterns

        /// <summary>
        /// Straight Pattern shoots the bullets (however many are in the newbullet) in a radial 360 shot
        /// This takes a list sprite parameter from the bullet class
        /// </summary>
        /// <param name="sprites"></param>
        public void CyclonePattern(List<Sprite> sprites, Vector2 position)
        {
            //loop foreach bullet in BulletCount add these values
            for (int i = 0; i < bulletcount; i++)
            {
                //bulletCount[i] = this.Clone() as Bullet;
                Bullet b = new Bullet(mG, _texture,20,1);
                b.Direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                b.position = new Vector2(position.X + 200, position.Y);
                b.linearVelocity = 5f;
                b.rotationVelocity = 2;
                b.scale = .8f;
                b.rateOfFire = 1;

                //adds bullet to sprite list to be drawn
                sprites.Add(b);

                angle += 360 * rotationVelocity;
            }
        }

        //each instrance adds a bullet at the parent position
        /// <summary>
        /// Vortex Shot is the bullets shoot in a spiral vortex outwards
        /// Takes in a parameter of the bullet sprite list
        /// </summary>
        /// <param name="sprites"></param>
        public void VortexPattern(List<Sprite> sprites, Vector2 position)
        {
            //loop foreach bullet in BulletCount add these values
            for (int i = 0; i < bulletcount; i++)
            {
                Bullet b = new Bullet(mG, _texture, 30, 1);
                Direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                b.Direction = Direction;
                b.position = new Vector2(position.X + 200, position.Y);
                b.linearVelocity = 4f;
                b.rotationVelocity = this.rotationVelocity;
                b.scale = .9f;
                b.rateOfFire = 1;

                //adds bullet to sprite list to be drawn
                sprites.Add(b);

                angle += 20 * rotationVelocity;
            }
        }

        public void SlowPattern(List<Sprite> sprites, Vector2 position)
        {
            //loop foreach bullet in BulletCount add these values
            for (int i = 0; i < bulletcount; i++)
            {
                Bullet b = new Bullet(mG, _texture, 200, 1);
                Direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                b.Direction = Direction;
                b.position = new Vector2(position.X + 200, position.Y);
                b.linearVelocity = 3f;
                b.rotationVelocity = this.rotationVelocity;
                b.scale = .7f;

                //adds bullet to sprite list to be drawn
                sprites.Add(b);

                angle += 20;
            }
        }

        public void ChangedPattern(List<Sprite> sprites, Vector2 position)
        {
            //loop foreach bullet in BulletCount add these values
            for (int i = 0; i < bulletcount; i++)
            {
                Bullet b = new Bullet(mG, _texture, 200, 1);
                Direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                b.position = new Vector2(position.X + 200, position.Y);
                LoadBulletData();
                
                //adds bullet to sprite list to be drawn
                sprites.Add(b);
            }
        }


        #endregion

    }
}
