using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace GDAPS2
{
    /// <summary>
    /// Map Data Take In MapSettings values for Map Tool
    /// </summary>
    class MapData
    {
        // -- Attributes -- //

        // point player 1
        private Point p1;

        // point player 2
        private Point p2;

        // point player 3
        private Point p3;

        // point player 4
        private Point p4;

        // background immage text
        private Image bkgImage;

        // current map Domain
        private string currentMapDomain;

        // player values list
        private List<int> playervalues;
        

        #region properties

        // -- Properties -- //

        public Point P1
        {
            get { return p1; }
            set { p1 = value; }
        }
        public Point P2
        {
            get { return p1; }
            set { p1 = value; }
        }
        public Point P3
        {
            get { return p1; }
            set { p1 = value; }
        }
        public Point P4
        {
            get { return p1; }
            set { p1 = value; }
        }
        public Image BkImage
        {
            get { return bkgImage; }
            set { bkgImage = value; }
        }
        public string DomainName
        {
            get { return currentMapDomain; }
            set { currentMapDomain = value; }
        }
        #endregion

        // Map Data Constructor
        public MapData()
        {
            // initialize players value list
            playervalues = new List<int>();

            //set image null
            bkgImage = null;
        }

        /// <summary>
        /// Reads in the Values of Text from the Map File
        /// </summary>
        public void TextReader()
        {
            try
            {
                using (StreamReader reader = new StreamReader("map.txt"))
                {
                    // save the player locations
                    for (int i = 0; i < 8; i++)
                    {
                        string input = reader.ReadLine().ToString();
                        string[] inputArray = input.Split(':');

                        if (i == 0)
                        {
                            p1.X = Convert.ToInt16(inputArray[1]);
                        }
                        else if (i == 1)
                        {
                            p1.Y = Convert.ToInt16(inputArray[1]);
                        }
                        else if (i == 2)
                        {
                            p2.X = Convert.ToInt16(inputArray[1]);
                        }
                        else if (i == 3)
                        {
                            p3.Y = Convert.ToInt16(inputArray[1]);
                        }
                        else if (i == 4)
                        {
                            p3.X = Convert.ToInt16(inputArray[1]);
                        }
                        else if (i == 5)
                        {
                            p3.Y = Convert.ToInt16(inputArray[1]);
                        }
                        else if (i == 6)
                        {
                            p4.X = Convert.ToInt16(inputArray[1]);
                        }
                        else if (i == 7)
                        {
                            p4.Y = Convert.ToInt16(inputArray[1]);
                        }
                    }

                    // Read in the background image
                    string input2 = reader.ReadLine().ToString();
                    string[] input2Array = input2.Split(' ');
                    bkgImage = Image.FromFile(input2Array[2]);

                    // Read in the map name
                    string input3 = reader.ReadLine().ToString();
                    string[] input3Array = input3.Split(' ');
                    currentMapDomain = input3Array[2].ToString();

                    // Read in Opponent locations

                }
            }
            catch (Exception ioe)
            {
                Console.WriteLine("Reading: " + ioe.Message);
                Console.WriteLine(ioe.StackTrace);
            }
        }
    }
}
