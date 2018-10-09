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
    public partial class Редактирование_варианта_ответа : Form
    {
        public Редактирование_варианта_ответа()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.varik = textBox2.Text;
            Close();
        }

        private void Редактирование_варианта_ответа_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            textBox2.Text = Program.varik;
        }
    }
}
