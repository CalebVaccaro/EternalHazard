using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDAPS2
{
    public partial class ControllerException : Form
    {

        //bool to check if content can load
        public bool clicked = false;

        public ControllerException()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Button Click will Reload the Instance of Checking if there is a controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            
            //Close Form
            Close();


        }

        /// <summary>
        /// Will Load Form when game checks if there is no controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControllerException_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            //make click true
            clicked = true;

            //Close Form
            Close();

        }
    }
}
