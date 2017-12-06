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
    /// Controller Input Class is where the Keys Property's are stored
    /// It Can be where the Controller Propterty's Can go if needed
    /// </summary>
    public class ControllerInput
    {
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Up { get; set; }
        public Keys Down { get; set; }


    }

    
}
