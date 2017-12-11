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

namespace GDAPS2
{
    /// <summary>
    /// Enemy Class will inherit Sprite Class
    /// Enemy Object Class
    /// Enemy Class Takes Bullets and Creates Patterns
    /// </summary>
    public class Enemy : Sprite
    {
        //int timer that will count down the timer before the game starts
        private int countdownTimer;

        //int timer check to shoot another bullet
        private int bulletcount;

        //int timer to check bullet patterns
        private int bulletPattern;

        //bool to check if enemy can shoot
        private bool canShoot;

        // int to check which wave the enemy is on
        private int wave;

        //bool property to check if bullets are allowed to shoot
        public bool CanShoot
        {
            get { return canShoot; }
            set { canShoot = value; }
        }

        // Get Main Game Object ref
        MainGame mG;

        // Get Bullet Game Object ref
        Bullet bullet;

        //holds a list of bullets so they will continue to exist past their initial draw
        //List<Bullet> hell;

        //overriding spriteclass method
        public Enemy(MainGame mg,Texture2D texture, int frameWidth, int frames) : base(texture, frameWidth, frames)
        {           
            // bullet count = 1
            bulletcount = 1;

            // set bullet pattern
            bulletPattern = 0;

            // initialize main game object
            mG = mg;

            // instantiate wave to 0
            wave = 0;

            // bullet object referenced
            bullet = new Bullet(mG, mG.bulletTexture, 0, 1);

        }

        //overrides the update method
        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            //denying the shooting of bullets
            countdownTimer = mG.countdownTimer;

            if (CanShoot == true)
            {
                ShootPatterns(sprites);
            }
            
        }

        /// <summary>
        /// Resetting the Timer For Shooting the Bullets
        /// </summary>
        public void ResetTimer()
        {
            // reset bullet pattern
            bulletPattern = 0;

            // reset bullet count
            bulletcount = 0;

            // bullet count set back to 1
            bulletcount = 1;
        }

        /// <summary>
        /// This will let enemy shoot the patterns created from bullet class
        /// </summary>
        /// <param name="sprites"></param>
        public void ShootPatterns(List<Sprite> sprites)
        {

            // increment bullet pattern +1
            bulletPattern++;

            // Random Object for randomizing enemy object
            Random rgen = new Random();

            // temporary int to count for wave variation
            int temp = (int)mG.timer;

            // check if wave is 0
            if (wave == 0)
            {
                // randomize wave value
                wave = rgen.Next(1, 6);

                // HARD CODE WAVE for checkign
                // wave = 2;
            }

            // if temp = 99
            // reset wave and set a new random
            if (temp == 99)
            {
                wave = 0;
                wave = rgen.Next(1, 5);
            }

            // wave states
            switch (wave)
            {
                case 1:

                    if (bulletPattern <= 5)
                    {

                        bullet.SpiralPattern(sprites, this.position);

                        
                    }
                    //if bulletPattern is greater than 0
                    if (bulletPattern >= 25 && bulletPattern <= 30)
                    {
                        bullet.LoopDLoopPattern(sprites, this.position);

                    }



                    break;

                case 2:

                    //if bulletPattern is greater than 0
                    if (bulletPattern == 7 || bulletPattern == 35)
                    {

                        bullet.SpiralPattern(sprites, this.position);

                    }

                    if (bulletPattern == 15 || bulletPattern >= 35)
                    {

                        bullet.VortexPattern(sprites, this.position);
                        //bullet.LoopDLoopPattern(sprites, this.position);

                    }

                    break;

                case 3:

                    //if bulletPattern is greater than 0
                    if (bulletPattern >= 20 && bulletPattern <= 29)
                    {

                        bullet.VortexPattern(sprites, this.position);

                    }

                    // shoot at 10
                    if (bulletPattern == 10)
                    {


                        bullet.LoopDLoopPattern(sprites, this.position);

                    }



                    break;

                case 4:

                    if (bulletPattern >= 2 && bulletPattern <= 7)
                    {

                        bullet.SpiralPattern(sprites, this.position);


                    }

                    if (bulletPattern >= 1 && bulletPattern <= 5)
                    {
                        bullet.LoopDLoopPattern(sprites, this.position);
                    }

                    break;

                case 5:

                    if (bulletPattern >= 2 && bulletPattern <= 7)
                    {

                        bullet.LoopDLoopPattern(sprites, this.position);
                    }

                    if (bulletPattern >= 1 && bulletPattern <= 5)
                    {
                        
                        bullet.SpiralFastPattern(sprites, this.position);
                    }

                    break;

                default:
                    // null not shooting
                    break;


            }

            //turn off shooting
            if (bulletPattern >= 40)
            {
                ResetTimer();
            }
        }




    }
}
