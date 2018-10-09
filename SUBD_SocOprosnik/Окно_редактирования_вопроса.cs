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
    public partial class Окно_редактирования_вопроса : Form
    {
        List<string> variants = new List<string>();//список вариантов ответа
        public Окно_редактирования_вопроса()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            variants.Add(textBox2.Text);//добавление варианта ответа в список
            comboBox1.Items.Add(textBox2.Text);//добавление его в выпадающий список

            comboBox1.Enabled = true;//делаем выпадающий список активным
            label3.Enabled = true;//делаем label3 (с призывом к редакт./удал. вар. ответа) активным
            button3.Enabled = true; button4.Enabled = true;//делаем button3&4 активными (удалялка-редкакта)
        }
        private void Окно_редактирования_вопроса_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            textBox1.Text = Program.op.voprs[Program.vopros_ind];
            checkBox1.Checked = Program.op.otviti[Program.vopros_ind].mog_nesk;
            for (int i = 0; i < Program.op.otviti[Program.vopros_ind].otvs.Count; i++)//варианты ответа
            {
                variants.Add(Program.op.otviti[Program.vopros_ind].otvs[i]);
                comboBox1.Items.Add(Program.op.otviti[Program.vopros_ind].otvs[i]);
            }
        }

        private void button3_Click(object sender, EventArgs e)////удаление варианта ответа
        {
            if (comboBox1.SelectedIndex != -1)
            {
                variants.RemoveAt(comboBox1.SelectedIndex);//удаление из variants-массива
                //Program.op.otviti[Program.vopros_ind].otvs.;
                comboBox1.Items.RemoveAt(comboBox1.SelectedIndex);//удаление из выпадающего списка
                if (comboBox1.Items.Count == 0)//если вариантов ответа больше нет
                {
                    comboBox1.Enabled = false;//делаем выпадающий список активным
                    label3.Enabled = false;//делаем label3 (с призывом к редакт./удал. вар. ответа) активным
                    button3.Enabled = false; button4.Enabled = false;//делаем button3&4 активным (удалялка-редкакта)
                }
            }
            else MessageBox.Show("Не выбран вариант ответа! Нечего удалять!");
        }

        private void button4_Click(object sender, EventArgs e)//редактировать вариант ответа
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Program.varik = variants[comboBox1.SelectedIndex];
                Редактирование_варианта_ответа newform = new Редактирование_варианта_ответа();
                //newform.Owner = this;
                newform.ShowDialog();
                variants[comboBox1.SelectedIndex] = Program.varik;
                comboBox1.SelectedItem = Program.varik;
            }
            else MessageBox.Show("Не выбран вариант ответа! Нечего редактировать!");
        }

        private void button5_Click(object sender, EventArgs e)//Отредактировать вопрос
        {
            Program.op.voprs[Program.vopros_ind] = textBox1.Text;//+1 раз
            Program.opros.otvets ty = new Program.opros.otvets(); ty.otvs = variants; ty.mog_nesk = checkBox1.Checked;
            Program.op.otviti.Add(ty);//+1 два
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex != -1)
            {
                label3.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
            }
        }
    }
}
