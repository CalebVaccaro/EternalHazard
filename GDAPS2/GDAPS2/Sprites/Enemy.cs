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
        //File Data Object
        BulletData fD = new BulletData();

        //int timer that will count down the timer before the game starts
        private int countdownTimer;

        //int timer check to shoot another bullet
        private int bulletcount;

        //int timer to check bullet patterns
        private int bulletPattern;

        //bool to check if enemy can shoot
        private bool canShoot;

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
        //overriding spriteclass method
        public Enemy(MainGame mg,Texture2D texture, int frameWidth, int frames) : base(texture, frameWidth, frames)
        {           
            bulletcount = 1;
            bulletPattern = 0;
            mG = mg;
            bullet = new Bullet(mG, mG.bulletTexture, 200, 1);
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
            bulletPattern = 0;
            bulletcount = 0;
            bulletcount = 1;
            bulletPattern++;
        }

        /// <summary>
        /// This will let enemy shoot the patterns created from bullet class
        /// </summary>
        /// <param name="sprites"></param>
        public void ShootPatterns(List<Sprite> sprites)
        {
            // increment bullet pattern +1
            bulletPattern++;

            //if bulletPattern is greater than 0
            if (bulletPattern <= 25)
            {

                bullet.CyclonePattern(sprites, this.position);

            }

            if (bulletPattern == 14)
            {

                bullet.VortexPattern(sprites, this.position);
                bullet.SlowPattern(sprites, this.position);
            }

            if (bulletPattern >= 25)
            {
                bullet.ChangedPattern(sprites, this.position);
            }

            //turn off shooting
            if (bulletPattern >= 60)
            {
                ResetTimer();
            }
        }

        


    }
}
