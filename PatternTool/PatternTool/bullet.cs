using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PatternTool
{
    public class bullet
    {
        Vector2 bulletPosition;
        Vector2 bulletDirection;

        float rotation;

        int timer;
        
        public Vector2 BulletPosition
        {
            get
            {
                return bulletPosition;
            }

            set
            {
                bulletPosition = value;
            }
        }

        public Vector2 BulletDirection
        {
            get
            {
                return bulletDirection;
            }

            set
            {
                bulletDirection = value;
            }
        }

        public float Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
            }
        }

        public int Timer
        {
            get
            {
                return timer;
            }

            set
            {
                timer = value;
            }
        }

        public bullet(Vector2 pos, Vector2 dir)
        {
            bulletPosition = pos;
            bulletDirection = dir;

            timer = 0;
        }
        
        
    }
}
