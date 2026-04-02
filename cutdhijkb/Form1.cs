using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cutdhijkb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Start start = new Start();
            start.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            History history = new History();
            history.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            History history = new History();
            Start start = new Start();
            start.Close();
            history.Show();
        }
    }
}
