using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GDAPS2
{
    class BulletData
    {
        //rotates the shooting vector
        public string angle;

        //rotates the shooting vector
        public string loopangle;

        //conditional to make sure fire rate of projectiles can be controled
        public string rateOfFire;

        //manipulates rotating velocity
        public string rotationalVelocity;

        //manipulates velocity of the bullet
        public string linearvelocity;

        //manipulates rotating velocity
        //one copy for each of the pattern
        public string loopRotationalVelocity;

        // loop velocity for certain bullet patterns
        public string loopVelocity;

        //timer used to keep track of how long a projectile travels backwards for the LoopDLoop pattern
        public string travelTimer;

        //conditional to make sure fire rate of projectiles can be controled
        public string loopRateOfFire;

        //Need a Dictionary for Game Values
        public List<string> _sections = new List<string>();

        //Attributes for Comments
        public List<string> _comments = new List<string>();

        //attributes for Game Values
        public List<string> _gameValues = new List<string>();

        public BulletData()
        {
            angle = null;
            rateOfFire = null;
            rotationalVelocity = null;
            linearvelocity = null;
        }

        //.ini filereader
        /// <summary>
        /// Reads in .Ini File for Bullet Data
        /// </summary>
        public void TextReader()
        {
            //try catch
            try
            {
                using (StreamReader reader = new StreamReader("settings.ini"))
                {
                    foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory()))
                    {
                        while (!reader.EndOfStream)
                        {
                            // loop to read in and display each line
                            string line = "";
                            while ((line = reader.ReadLine()) != null)
                            {

                                if (line.StartsWith("["))
                                {
                                    _sections.Add(line);
                                }
                                else if (line.StartsWith(";"))
                                {
                                    _comments.Add(line);
                                }

                                else if (line == null || line == "")
                                {
                                    //do nothing
                                    //excess lines
                                }
                                else
                                {
                                    //add game values
                                    string[] gameValuestring = line.Split(':');
                                    _gameValues.Add(gameValuestring[1].Trim());
                                }
                            }
                        }

                    }

                }

            }
            catch (Exception ioe)
            {
                Console.WriteLine("Reading: " + ioe.Message);
                Console.WriteLine(ioe.StackTrace);
            }

            // foreach section
            foreach (var section in _sections)
            {
                if (section.Contains("Spiral"))
                {
                    // Angle at which the projectile will travel in
                    angle = _gameValues[0];

                    // Increase in angle at x degrees per second
                    rotationalVelocity = _gameValues[1];

                    // Will fire 1 projectile every x amounts of frames
                    rateOfFire = _gameValues[2];

                    // Speed of the projectile
                    linearvelocity = _gameValues[3];
                }

                if (section.Contains("LoopDLoop"))
                {
                    // Angle at which the projectile will travel in
                    loopangle = _gameValues[4];

                    // Increase in angle at x degrees per second
                    loopRotationalVelocity = _gameValues[5];

                    // Will fire 1 projectile every x amounts of frames
                    loopRateOfFire = _gameValues[6];

                    // Speed of the projectile
                    loopVelocity = _gameValues[7];

                    // travel time
                    travelTimer = _gameValues[8];

                }
            }
        }
    }
}
