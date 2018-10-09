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
    public partial class Окно_создания_вопроса : Form//завершено (без заглушек)
    {
        List<string> variants = new List<string>();//список вариантов ответа
        public Окно_создания_вопроса()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)//добавление варианта ответа
        {
            variants.Add(textBox2.Text);//добавление варианта ответа в список
            comboBox1.Items.Add(textBox2.Text);//добавление его в выпадающий список

            comboBox1.Enabled = true;//делаем выпадающий список активным
            label3.Enabled = true;//делаем label3 (с призывом к редакт./удал. вар. ответа) активным
            button3.Enabled = true; button4.Enabled = true;//делаем button3&4 активными (удалялка-редкакта)
        }

        private void button3_Click(object sender, EventArgs e)//удаление варианта ответа
        {
            if (comboBox1.SelectedIndex != -1)
            {
                variants.RemoveAt(comboBox1.SelectedIndex);//удаление из массива
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

        private void button4_Click(object sender, EventArgs e)//редактирование варианта ответа
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Program.varik = variants[comboBox1.SelectedIndex];
                Редактирование_варианта_ответа newform = new Редактирование_варианта_ответа();
                //newform.Owner = this;
                newform.ShowDialog();
                variants[comboBox1.SelectedIndex] = Program.varik;
                comboBox1.Items[comboBox1.SelectedIndex] = Program.varik;
            }
            else MessageBox.Show("Не выбран вариант ответа! Нечего редактировать!");
        }

        private void button5_Click(object sender, EventArgs e)//собсна создание вопроса в op'е
        {
            if (variants.Count > 0)
            {
                //добавляем текст вопроса
                Program.op.voprs.Add(textBox1.Text);//+1 раз
                //Program.op.voprs.RemoveAt(0);//Add("");
                //добавляем набор вариантов ответа
                Program.opros.otvets ty = new Program.opros.otvets(); ty.otvs = variants; ty.mog_nesk = checkBox1.Checked;
                Program.op.otviti.Add(ty);//+1 два
                Close();
            }
            else
            {
                MessageBox.Show("Нельзя создать вопрос, ибо не добавлено ни одного варианта варианта ответа!");
            }
        }
        private void Окно_создания_вопроса_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
        }
    }
}