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
    public partial class Окно_создания_опроса : Form
    {
        public Окно_создания_опроса()
        {
            InitializeComponent();
        }

        private void Окно_создания_опроса_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            label2.Text = "Количество вопросов = 0";
        }

        private void button1_Click(object sender, EventArgs e)//добавить вопрос
        {            
            //Program.op.voprs.Add();
            int kol_vopr = Program.op.voprs.Count;
            Окно_создания_вопроса newform = new Окно_создания_вопроса();
            newform.ShowDialog();
            if (Program.op.voprs.Count>kol_vopr)//если какой-то вопрос таки удалось создать
            {
                comboBox1.Items.Add(Program.op.voprs[Program.op.voprs.Count-1]);//добавление его в выпадающий список
                comboBox1.Enabled = true;//делаем выпадающий список активным
                label4.Enabled = true;//делаем label4 (с призывом к редакт./удал. вопроса) активным
                button3.Enabled = true; button4.Enabled = true;//делаем button3&4 активными (удалялка-редкакта)
                label2.Text = "Количество вопросов = " + Convert.ToString(Program.op.voprs.Count);
            }
            /*if (Program.est_vopr == false)//если создание вопроса не удалось, то удаляем пустой вопрос
            {
                //Program.op.voprs //.RemoveAt[0];//Program.op.voprs.Count - 1
            }*/
        }

        private void button5_Click(object sender, EventArgs e)//добавление доп. сведений о опросе
        {
            Добавление_доп newform = new Добавление_доп();
            newform.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)//создание опроса в op'е
        {
            if (Program.op.voprs.Count > 0)
            {
                //все данные уже добавлены в op, так что закрываем сиё окошко
                Close();
            }
            else
            {
                MessageBox.Show("Вы не добавили ни одного вопроса! Опрос не может быть создан, поскольку для его создания должен быть создан минимум один вопрос.");
            }
        }

        private void button3_Click(object sender, EventArgs e)//редактировать вопрос
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Program.vopros_ind = comboBox1.SelectedIndex;
                Окно_редактирования_вопроса newform = new Окно_редактирования_вопроса();
                newform.ShowDialog();
                comboBox1.SelectedItem = Program.op.voprs[comboBox1.SelectedIndex];
            }
            else MessageBox.Show("Не выбран вопрос! Нечего редактировать!");
        }

        private void button4_Click(object sender, EventArgs e)//удалить вопрос
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Program.op.voprs.RemoveAt(Program.op.voprs.Count - 1);//удаление из op.voprs-массива
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);//удаление из выпадающего списка
                if (comboBox1.Items.Count == 0)//если вопросов больше нет
                {
                    comboBox1.Enabled = false;//делаем выпадающий список активным
                    label4.Enabled = false;//делаем label3 (с призывом к редакт./удал. вопроса) активным
                    button3.Enabled = false; button4.Enabled = false;//делаем button3&4 активным (удалялка-редкакта)
                }
            }
            else MessageBox.Show("Не выбран вопрос! Нечего удалять!");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Program.op.Clear();
            Close();
        }
    }
}