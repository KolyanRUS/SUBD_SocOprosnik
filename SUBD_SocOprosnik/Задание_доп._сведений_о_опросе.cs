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
    public partial class Добавление_доп : Form
    {
        public Добавление_доп()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.op.DOPSVED = textBox2.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Добавление_доп_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            textBox2.Text = Program.op.DOPSVED;
        }
    }
}
