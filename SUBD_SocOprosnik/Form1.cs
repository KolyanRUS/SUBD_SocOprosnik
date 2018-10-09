using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SUBD_SocOprosnik
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string ttext = "Для прохождения опроса следует загрузить опрос из файла или создать новый.";

        private void button1_Click(object sender, EventArgs e)//создание опроса
        {
            Окно_создания_опроса newform = new Окно_создания_опроса();
            newform.ShowDialog();
            if (Program.op.voprs.Count > 0)
            {
                label1.Text = "Создан новый опрос в оперативной памяти. Условно он называется текущим опросом.";
                //3_6_7_2_5
                button1.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = false;
                button6.Enabled = true;
                button7.Enabled = true;
                button2.Enabled = true;
                button5.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)//сохранение опроса в файл
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            //получаем выбранный файл
            string filename = saveFileDialog1.FileName;
          //сохраняем всю дрисню в этот файл
            using (StreamWriter wr = new StreamWriter(filename, false))
            {
                wr.WriteLine(Program.op.voprs.Count);//халва voprs
                for (int voprs_ind = 0; voprs_ind < Program.op.voprs.Count; voprs_ind++)
                {
                    boxbufer.Text = Program.op.voprs[voprs_ind];
                    wr.WriteLine(Convert.ToString(boxbufer.Lines.Count()));//халва voprs[voprs_ind]
                    for (int boxbufer_ind = 0; boxbufer_ind < boxbufer.Lines.Count(); boxbufer_ind++)
                    {
                        wr.WriteLine(Convert.ToString(boxbufer.Lines[boxbufer_ind]));//строка voprs[voprs_ind]
                    }
                }
                wr.WriteLine(Convert.ToString(Program.op.otviti.Count));//халва otviti
                for (int otviti_ind = 0; otviti_ind < Program.op.otviti.Count; otviti_ind++)
                {
                    wr.WriteLine(Convert.ToString(Program.op.otviti[otviti_ind].mog_nesk));//otviti[otviti_ind].mog_nesk
                    wr.WriteLine(Convert.ToString(Program.op.otviti[otviti_ind].otvs.Count));//халва otvs
                    for (int otvs_ind = 0; otvs_ind < Program.op.otviti[otviti_ind].otvs.Count; otvs_ind++)
                    {
                        boxbufer.Text = Program.op.otviti[otviti_ind].otvs[otvs_ind];
                        wr.WriteLine(Convert.ToString(boxbufer.Lines.Count()));//халва otvs[otvs_ind]
                        for (int boxbufer_ind = 0; boxbufer_ind < boxbufer.Lines.Count(); boxbufer_ind++)
                        {
                            wr.WriteLine(Convert.ToString(boxbufer.Lines[boxbufer_ind]));//строка otvs[otvs_ind]
                        }
                    }

                }
                wr.WriteLine(Convert.ToString(Program.op.numbers.Count));//халва numbers
                for (int numbers_ind = 0; numbers_ind < Program.op.numbers.Count; numbers_ind++)
                {
                    wr.WriteLine(Convert.ToString(Program.op.numbers[numbers_ind].fio));//numbers[numbers_ind].fio
                    wr.WriteLine(Convert.ToString(Program.op.numbers[numbers_ind].nomer));//numbers[numbers_ind].nomer
                    wr.WriteLine(Convert.ToString(Program.op.numbers[numbers_ind].pol_nal));//numbers[numbers_ind].pol_nal
                    if (Program.op.numbers[numbers_ind].pol_nal == true)
                    {
                        wr.WriteLine(Convert.ToString(Program.op.numbers[numbers_ind].pol));//numbers[numbers_ind].pol
                    }
                    wr.WriteLine(Convert.ToString(Program.op.numbers[numbers_ind].date_nal));//numbers[numbers_ind].date_nal
                    if (Program.op.numbers[numbers_ind].date_nal == true)
                    {
                        wr.WriteLine(Convert.ToString(Program.op.numbers[numbers_ind].date_birth));//numbers[numbers_ind].date_birth
                    }
                    wr.WriteLine(Convert.ToString(Program.op.numbers[numbers_ind].numbers.Count));//халва byte`вского numbers
                    for (int bytenumber_ind = 0; bytenumber_ind < Program.op.numbers[numbers_ind].numbers.Count; bytenumber_ind++)
                    {
                        wr.WriteLine(Convert.ToString(Program.op.numbers[numbers_ind].numbers[bytenumber_ind]));//элемент byte`вского numbers
                    }
                }
                wr.WriteLine(Convert.ToString(Program.op.DOPSVED));
            }
        }

        private void button3_Click(object sender, EventArgs e)//пройти анкетирование
        {
            if (Program.op.voprs.Count > 0)
            {
                Прохождение_опроса newform = new Прохождение_опроса();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("Невозможно пройти анкетирование ввиду отсутствия в текущем опросе вопросов!");
            }
        }

        private void button4_Click(object sender, EventArgs e)//Загрузить опрос из файла
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            //получаем выбранный файл
            string filename = openFileDialog1.FileName;
            Program.op.Clear();
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {//подразумевается что op пуст
                    int voprs_Count = Convert.ToInt32(sr.ReadLine());//ХалваЪ voprs
                    for (int voprs_ind = 0; voprs_ind < voprs_Count; voprs_ind++)//по каждому voprs[элемент]
                    {
                        Program.op.voprs.Add(null);
                        int halva_voprs__voprs_ind = Convert.ToInt32(sr.ReadLine());//микрохалва текущего voprs[voprs_ind]
                        for (int i = 0; i < halva_voprs__voprs_ind; i++)
                        {
                            Program.op.voprs[voprs_ind] += sr.ReadLine() + "\r\n";
                        }
                        Program.op.voprs[voprs_ind] = Program.op.voprs[voprs_ind].Remove(Program.op.voprs[voprs_ind].Count() - 2, 2).Insert(Program.op.voprs[voprs_ind].Count() - 2, "");//Program.op.voprs[voprs_ind] -="\r\n";
                    }
                    int otviti_Count = Convert.ToInt32(sr.ReadLine());//ХалваЪ otviti
                    for (int otviti_ind = 0; otviti_ind < otviti_Count; otviti_ind++)//по каждому otviti[элемент]
                    {
                        Program.op.otviti.Add(new Program.opros.otvets());
                        Program.op.otviti[otviti_ind].mog_nesk = Convert.ToBoolean(sr.ReadLine());
                        int otvs_Count = Convert.ToInt32(sr.ReadLine());//считываем и запоминаем халву otviti[otviti_ind].otvs`а
                        for (int otvs_ind = 0; otvs_ind < otvs_Count; otvs_ind++)//по каждому элементу otviti[otviti_ind].otvs`а
                        {
                            Program.op.otviti[otviti_ind].otvs.Add(null);
                            int halva_otviti_otviti_ind_otvs__otvs_ind = Convert.ToInt32(sr.ReadLine());//микрохалва otviti[otviti_ind].otvs[otvs_ind]
                            for (int i = 0; i < halva_otviti_otviti_ind_otvs__otvs_ind; i++)
                            {
                                Program.op.otviti[otviti_ind].otvs[otvs_ind] += sr.ReadLine() + "\r\n";
                            }
                            Program.op.otviti[otviti_ind].otvs[otvs_ind] = Program.op.otviti[otviti_ind].otvs[otvs_ind].Remove(Program.op.otviti[otviti_ind].otvs[otvs_ind].Count() - 2, 2).Insert(Program.op.otviti[otviti_ind].otvs[otvs_ind].Count() - 2, "");//Program.op.otviti[otviti_ind].otvs[otvs_ind] -="\r\n";
                        }
                    }
                    int NumberS_Count = Convert.ToInt32(sr.ReadLine());//ХалваЪ NumberS
                    for (int NumberS_ind = 0; NumberS_ind < NumberS_Count; NumberS_ind++)//по каждому NumberS[элемент]
                    {
                        Program.op.numbers.Add(new Program.opros.kogo_opr());
                        Program.op.numbers[NumberS_ind].fio = sr.ReadLine();
                        Program.op.numbers[NumberS_ind].nomer = sr.ReadLine();
                        Program.op.numbers[NumberS_ind].pol_nal = Convert.ToBoolean(sr.ReadLine());
                        if (Program.op.numbers[NumberS_ind].pol_nal == true)
                        {
                            Program.op.numbers[NumberS_ind].pol = Convert.ToBoolean(sr.ReadLine());
                        }
                        Program.op.numbers[NumberS_ind].date_nal = Convert.ToBoolean(sr.ReadLine());
                        if (Program.op.numbers[NumberS_ind].date_nal == true)
                        {
                            Program.op.numbers[NumberS_ind].date_birth = DateTime.Parse(sr.ReadLine());
                        }
                        int numbers_Count = Convert.ToInt32(sr.ReadLine());//считываем и запоминаем халву NumberS[NumberS_ind].numbers`а
                        for (int numbers_ind = 0; numbers_ind < numbers_Count; numbers_ind++)//по каждому элементу NumberS[NumberS_ind].numbers`а
                        {
                            Program.op.numbers[NumberS_ind].numbers.Add(Convert.ToByte(sr.ReadLine()));
                        }
                    }
                    while (!sr.EndOfStream)
                    {
                        //теперь считываем DOPSVED:
                        Program.op.DOPSVED += sr.ReadLine() + "\r\n";
                    }
                    Program.op.DOPSVED = Program.op.DOPSVED.Remove(Program.op.DOPSVED.Count() - 2, 2).Insert(Program.op.DOPSVED.Count() - 2, "");//Program.op.DOPSVED -="\r\n";
                }
                if (Program.op.voprs.Count > 0)
                {
                    label1.Text = "Из файла загружен опрос. Образовавшаяся в оперативной памяти структура данных\r\nусловно называется текущим опросом.";
                    //3_6_7_2_5
                    button1.Enabled = false;
                    button3.Enabled = true;
                    button4.Enabled = false;
                    button6.Enabled = true;
                    button7.Enabled = true;
                    button2.Enabled = true;
                    button5.Enabled = true;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Недопустимый формат файла!");
                Program.op.Clear();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Редактирование_текущего_опроса newform = new Редактирование_текущего_опроса();
            newform.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Program.op.numbers.Count > 0)
            {
                Анализ_статистики newform = new Анализ_статистики();
                newform.ShowDialog();
            }
            else
            {
                MessageBox.Show("Нельзя провести анализ статистики ввиду того, что отсутствует статистика ответов на вопросы.");
            }
        }

        private void button5_Click(object sender, EventArgs e)//Закрыть текущий опрос
        {
            Program.op.Clear();
            button1.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = true;
            button6.Enabled = false;
            button7.Enabled = false;
            button2.Enabled = false;
            button5.Enabled = false;
            label1.Text = ttext;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
        }
    }
}