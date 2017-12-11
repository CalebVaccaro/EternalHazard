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

        // loop angle
        private double loopanlge;

        //timer will be calcualted as gametime in total seconds
        private float _timer;

        //counts frames as it's updated
        public int frameCounter;

        //counts how many bullets each farame
        public int rateOfFire;

        //int timer check to shoot another bullet
        public int bulletcount;

        // direction of where the bullet is traveling
        public Vector2 direction;

        // rotational velocity
        public float rotationVelocity;

        // linear velocity
        public float linearVelocity;

        // linear velocity for looping patterns
        public float loopvelocity;

        // internal int timer for bullet class
        public int timer;

        //timer used to keep track of how long a projectile travels backwards for the LoopDLoop pattern
        int travelTimer;

        // bullet list
        List<Bullet> hell = new List<Bullet>();


        //holds coordinate for when initializing new vectors for update
        float x;
        float y;

        //manipulates rotating velocity
        //one copy for each of the pattern
        float rotationalVelocity;
        float loopRotationalVelocity;

        //conditional to make sure fire rate of projectiles can be controled
        int loopRateOfFire;

        // Main Object Reference
        MainGame mG;

        //overriding spriteclass method
        public Bullet(MainGame mg, Texture2D texture, int frameWidth, int frames) : base(texture, frameWidth, frames)
        {
            // load bullet data
            LoadBulletData();

            //direction.X = 0;
            //direction.Y = 0;

            // Instantiate Main Game Object
            mG = mg;

            // bullet count = 1
            bulletcount = 1;

            //load bullet data
            LoadBulletData();

            //rate of fire starts at 1
            rateOfFire = 1;
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
                //Shoot(hell, position, direction);
                position += direction * linearVelocity;

                frameCounter = 0;
            }

            //increment count +1
            frameCounter++;

            // Call Read Bullet data
            LoadBulletData();

            //Loop to check if any bullet in the list is greater than screen boundaries
            //Also Calls CheckScreenCollision()
            for (int i = sprites.Count - 1 ; i > 0 ; i--)
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

        //converts degrees to radians for usage of Cos() and Sin()
        double DegToRad(double angleInDeg)
        {
            return 2 * Math.PI * angleInDeg / 360.0;
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

            // check sections
            //Parse Data
            angle = double.Parse(fD.angle);

            rotationVelocity = int.Parse(fD.rotationalVelocity);

            linearVelocity = int.Parse(fD.linearvelocity);

            //Parse Data
            loopanlge = double.Parse(fD.loopangle);

            loopRotationalVelocity = int.Parse(fD.loopRotationalVelocity);

            loopRateOfFire = int.Parse(fD.loopRateOfFire);

            loopvelocity = int.Parse(fD.loopVelocity);

            travelTimer = int.Parse(fD.travelTimer);
        }

        #region Bullet Patterns

        //updates the bullets to create a Spiral pattern
        public void SpiralPattern(List<Sprite> bullets, Vector2 position)
        {


            //loop foreach bullet in BulletCount add these values
            for (int i = 0; i < bulletcount; i++)
            {
                Bullet b = new Bullet(mG, _texture, 20, 1);
                b.direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                b.position = new Vector2(position.X + 200, position.Y);
                b.linearVelocity = 5f;
                b.rotationVelocity = 2;
                b.scale = .8f;
                b.rateOfFire = loopRateOfFire;



                //adds bullet to sprite list to be drawn
                bullets.Add(b);

                // add to angle
                angle += 360 * rotationVelocity;
            }
        }

        //updates bullets to create a LoopDLoop pattern
        public void LoopDLoopPattern(List<Sprite> bullets, Vector2 position)
        {
            //loop foreach bullet in BulletCount add these values
            for (int i = 0; i < bulletcount; i++)
            {
                Bullet b = new Bullet(mG, _texture, 20, 1);

                b.direction = new Vector2((float)Math.Cos(loopanlge), (float)Math.Sin(loopanlge));
                b.position = new Vector2(position.X + 200, position.Y);
                b.loopvelocity = loopvelocity;
                b.rotationVelocity = loopRotationalVelocity;
                b.scale = .5f;
                b.rateOfFire = loopRateOfFire;

                // add bullet to list
                bullets.Add(b);

                loopanlge += 360 * loopRotationalVelocity;

                
                if (b.timer < travelTimer) //the bullet hasn't traveled the x amount of frames in the opposite direction
                {
                    //travel in the opposite direction
                    b.position += b.direction * -b.linearVelocity;

                    b.timer++;
                }
                else
                    b.position += b.direction * b.linearVelocity; //keep them traveling upon the vector they are on
                    
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
                Bullet b = new Bullet(mG, mG.bulletTexture, 30, 1);

                b.direction = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
                b.position = new Vector2(position.X + 200, position.Y);
                b.linearVelocity = 6f;
                b.rotationVelocity = this.rotationVelocity;
                b.scale = .7f;
                b.rateOfFire = 1;

                //adds bullet to sprite list to be drawn
                sprites.Add(b);

                angle += 20 * rotationVelocity;
            }
        }

        //updates the bullets to create a Spiral pattern
        public void SpiralFastPattern(List<Sprite> bullets, Vector2 position)
        {


            //loop foreach bullet in BulletCount add these values
            for (int i = 0; i < bulletcount; i++)
            {
                Bullet b = new Bullet(mG, _texture, 20, 1);
                b.direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                b.position = new Vector2(position.X + 200, position.Y);
                b.linearVelocity = 8f;
                b.rotationVelocity = 2;
                b.scale = .8f;
                b.rateOfFire = loopRateOfFire;



                //adds bullet to sprite list to be drawn
                bullets.Add(b);

                // add to angle
                angle += 360 * rotationVelocity;
            }
        }

        #endregion

    }
}
