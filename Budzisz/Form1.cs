using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Budzisz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            potencjometr1.MyClickEvent += Potencjometr1_MyClickEvent;
        }

        private void Potencjometr1_MyClickEvent(object sender, EventArgs e)
        {
            MessageBox.Show("kliknieto mnie");
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            progressBar1.Value = (int)potencjometr2.Value;
        }


    }
}
