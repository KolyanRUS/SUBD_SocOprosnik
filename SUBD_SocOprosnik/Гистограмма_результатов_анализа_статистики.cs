using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SUBD_SocOprosnik
{
    public partial class Гистограмма_результатов_анализа_статистики : Form
    {
        public Гистограмма_результатов_анализа_статистики()
        {
            InitializeComponent();
        }

        private void Гистограмма_результатов_анализа_статистики_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            metatextbox.Text = Program.metatextbox_Text;
            string maxy = null, maxx = null; //foreach (int h in Program.maxY) { maxy += Convert.ToString(h) + "_"; } foreach (int h in Program.maxX) { maxx += Convert.ToString(h) + "_"; }
            //MessageBox.Show(maxy+"\r\n"+maxx);
            chart1.Series["Series1"].Points.DataBindXY(Program.maxY, Program.maxX);
        }
    }
}
