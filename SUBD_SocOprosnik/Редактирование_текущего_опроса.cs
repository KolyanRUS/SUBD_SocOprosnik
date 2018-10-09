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
    public partial class Редактирование_текущего_опроса : Form
    {
        public Редактирование_текущего_опроса()
        {
            InitializeComponent();
        }

        private void Редактирование_текущего_опроса_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            for (int i = 0; i < Program.op.voprs.Count; i++)//заполняем comboBox1
            {
                comboBox1.Items.Add(Program.op.voprs[i]);
            }
            label2.Text = "Количество вопросов = " + Convert.ToString(comboBox1.Items.Count);
        }

        private void button1_Click(object sender, EventArgs e)//Добавить вопрос
        {
            int kol_vopr = Program.op.voprs.Count;
            Окно_создания_вопроса newform = new Окно_создания_вопроса();
            newform.ShowDialog();
            if (Program.op.voprs.Count > kol_vopr)//если какой-то вопрос таки удалось создать
            {
                Program.op.numbers.Clear();
                comboBox1.Items.Add(Program.op.voprs[Program.op.voprs.Count - 1]);//добавление его в выпадающий список
                comboBox1.Enabled = true;//делаем выпадающий список активным
                label4.Enabled = true;//делаем label4 (с призывом к редакт./удал. вопроса) активным
                button3.Enabled = true; button4.Enabled = true;//делаем button3&4 активными (удалялка-редкакта)
                label2.Text = "Количество вопросов = " + Convert.ToString(Program.op.voprs.Count);
            }
        }

        private void button3_Click(object sender, EventArgs e)//Редактирование вопроса
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Program.op.numbers.Clear();
                Program.vopros_ind = comboBox1.SelectedIndex;
                Окно_редактирования_вопроса newform = new Окно_редактирования_вопроса();
                newform.ShowDialog();
            }
            else MessageBox.Show("Не выбран вопрос! Нечего редактировать!");
        }

        private void button4_Click(object sender, EventArgs e)//удалить вопрос
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Program.op.numbers.Clear();
                Program.op.voprs.RemoveAt(Program.op.voprs.Count - 1);//удаление из op.voprs-массива
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);//удаление из выпадающего списка
                if (comboBox1.Items.Count == 0)//если вопросов больше нет
                {
                    comboBox1.Enabled = false;//делаем выпадающий список активным
                    label4.Enabled = false;//делаем label3 (с призывом к редакт./удал. вопроса) активным
                    button3.Enabled = false; button4.Enabled = false;//делаем button3&4 активным (удалялка-редкакта)
                }
            }
            else
                MessageBox.Show("Не выбран вопрос! Нечего удалять!");
        }

        private void button5_Click(object sender, EventArgs e)//добавление доп. сведений о опросе
        {
            Добавление_доп newform = new Добавление_доп();
            newform.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Program.op.voprs.Count > 0)
            {
                //все данные уже добавлены в op, так что закрываем сиё окошко
                Close();
            }
            else
            {
                MessageBox.Show("В результате (этого либо одного из предыдущих) редактирования текущий опрос не содержит ни одного вопроса! Соответственно, вы не можете пройти текущий опрос.");
                Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex != -1)
            {
                label4.Enabled = true;
                button4.Enabled = true;
                button3.Enabled = true;
            }
        }
    }
}
