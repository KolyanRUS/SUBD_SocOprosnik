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
    public partial class Анализ_статистики : Form
    {
        List<byte> variants = new List<byte>();//список индексов вариантов ответа, по которым проводится анализ_по умолчанию пуст
        //и соответственно анализ проводится по всем вариантам ответа
        bool vpv = true;//анализ делать по тем, чья дата рождения неизвестна
        /*
         * по умолчанию словно 4___(-1 OR 4)
        0 - не определён
        1 - м
        2 - ж
        3 - м & ж
        4 - м & ж & н. о.
        5 - ж & н. о.
        6 - м & н. о.
        */
                       
        List<byte> indexs = new List<byte>();//список индексов лиц, по которым проводится анализ (по умолчанию должен по всем проводиться)
        DateTime min = new DateTime();//минимальное значение даты рождения
        DateTime max = new DateTime();//максимальное значение даты рождения
        public Анализ_статистики()
        {
            InitializeComponent();
        }
        public bool uchet = false;//true если учитывать в анализе установленные ограничения, false если не учитывать
        public bool tixt = true;//true если выводить результат анализа текстом, false если выводить результат анализа отдельным окном
        public int ui = 0;//индекс текущего вопроса
        List<int> schottics = new List<int>();  //каждый элемент этого List`а соответствует варианту ответа на текущий вопрос_
                                                //_кажд. элемент - счётчик ответов именно этим вариантом
        List<Program.opros.kogo_opr> nnnumbers = new List<Program.opros.kogo_opr>();//выборка, по которым надо провести анализ
        public bool variants_have(byte bi)//имеет ли variants число bi_true если имеет_false если не имеет
        {
            bool vhod = false;
            for (int i = 0; i < variants.Count; i++)
            {
                if (variants[i] == bi)
                {
                    vhod = true; break;
                }
            }
            return vhod;
        }
        public bool indexs_have(int bi)
        {
            bool vhod = false;
            for (int i = 0; i < indexs.Count; i++)
            {
                if (indexs[i] == bi)
                {
                    vhod = true; break;
                }
            }
            return vhod;
        }
        public void pervo_analiz()
        {
            int nnnumber_analiz_Count = 0;

            if (uchet == true)//первичный ANALIZZ c учётом установленных ограничений:
            {
                for (int j = 0; j < nnnumbers.Count; j++)//по каждому отвечающему
                {//--
                    // __проверка соответствия отвечающего условиям__
                    if (indexs.Count != 0) if (!indexs_have(j))
                        {
                            continue;
                        }
                    if (vpv == false)
                    {
                        if (nnnumbers[j].date_nal == false)
                        {
                            continue;
                        }
                    }
                    else//если выводить тех, чья дата неизвестна
                    {
                        if (nnnumbers[j].date_nal == true)//если дата известна, то проводим проверку вхождения в диапозон
                        {
                            DateTime pustodate = new DateTime();
                            if ((nnnumbers[j].date_birth < min) && (min != pustodate))
                            {
                                continue;
                            }
                            if ((nnnumbers[j].date_birth > max) && (max != pustodate))
                            {
                                continue;
                            }
                        }
                    }
                    if (comboBox1.SelectedIndex == 0)//если 0 - неопределён
                    {
                        if (!(nnnumbers[j].pol_nal == false))//if(отвечающий.пол НЕ неопределён)
                        {
                            continue;
                        }
                    }
                    if (comboBox1.SelectedIndex == 1)//если 1 - м
                    {
                        if (!(nnnumbers[j].pol == true))//if(отвечающий.пол НЕ м{true})
                        {
                            continue;
                        }
                    }
                    if (comboBox1.SelectedIndex == 2)//если 2 - ж
                    {
                        if (!(nnnumbers[j].pol == false))//if(отвечающий.пол НЕ ж{false})
                        {
                            continue;
                        }
                    }
                    if (comboBox1.SelectedIndex == 3)//если 3 - м & ж
                    {
                        if (!((nnnumbers[j].pol == false) || (nnnumbers[j].pol == true)))//if(отвечающий.пол НЕ [м{true} OR ж{false}])
                        {
                            continue;
                        }
                    }
                    if (comboBox1.SelectedIndex == 5)//если 5 - ж & н. о.
                    {
                        if (nnnumbers[j].pol == true)//if(отвечающий.пол НЕ [м{true} OR н.о.{false}])
                        {
                            continue;
                        }
                    }
                    if (comboBox1.SelectedIndex == 6)//если 6 - м & н. о.
                    {
                        if (nnnumbers[j].pol == false)//if(отвечающий.пол НЕ [м{true} OR н.о.{false}])
                        {
                            continue;
                        }
                    }
                    // __проверка соответствия отвечающего условиям__
                    //сейчас надо вычленить те ответы, которые относятся к текущему вопросу |получить набор ответов "ныбор"|
                    //и по всем элементам этого "ныбора" пройтись и сделать {schottics[%элемент этого набора%] += 1;}
                    int ss = -1;//спецсчётчик_по итогам следующего for`а должен быть равен индексу последнего содержащего
                    //разделительный нуль элемента numbers[j] перед непосредственно "ныбором"
                    byte buf;
                    for (int u = 0; u < ui; u++)//проходим предвычленённые ответы
                    {
                        buf = 1;
                        while (buf != 0)
                        {
                            ss++;
                            buf = nnnumbers[j].numbers[ss];
                        }
                    }
                    ss++;
                    buf = nnnumbers[j].numbers[ss];
                    if (variants.Count == 0)
                    {
                        while (buf != 0)//по всем варианто-ответам
                        {
                            schottics[buf - 1] += 1;
                            ss++;
                            buf = nnnumbers[j].numbers[ss];
                        }
                    }
                    else
                    {
                        while (buf != 0)//лишь по тем варианто-ответам, которые входят в variants
                        {
                            int bbi = buf - 1;
                            if (variants_have((byte)bbi)) { schottics[bbi] += 1; }
                            ss++;
                            buf = nnnumbers[j].numbers[ss];
                        }
                    }
                    nnnumber_analiz_Count += 1;
                }//--

                if (tixt == true)//если вывести результаты анализа в textBox1.Text
                {
                    //итак. мы имеем заполненный schottics
                    //нужно сделать его анализ и его результаты запихнуть в textBox1
                    textBox1.Text = "";//очищаем textBox1.Text
                    if (variants.Count == 0)
                    {
                        for (int i = 0; i < schottics.Count; i++)
                        {
                            try
                            {
                                textBox1.Text += "Вариант ответа №" + Convert.ToString(i + 1) + " выбрало " +
                                Convert.ToString(schottics[i]) + " опрошенных лиц.\r\nЭто " +
                                Convert.ToString(100 * schottics[i] / nnnumber_analiz_Count)
                                + "% от всех опрошенных.\r\n";
                            }
                            catch (DivideByZeroException)
                            {
                                textBox1.Text += "Вариант ответа №" + Convert.ToString(i + 1) + " выбрало " +
                                Convert.ToString(schottics[i]) + " опрошенных лиц.\r\nЭто " +
                                Convert.ToString(0)
                                + "% от всех опрошенных.\r\n";
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < schottics.Count; i++)
                        {
                            if (variants_have(Convert.ToByte(i)))
                            {
                                try
                                {
                                    textBox1.Text += "Вариант ответа №" + Convert.ToString(i + 1) + " выбрало " +
                                    Convert.ToString(schottics[i]) + " опрошенных лиц.\r\nЭто " +
                                    Convert.ToString(100 * schottics[i] / nnnumber_analiz_Count)
                                    + "% от всех опрошенных.\r\n";
                                }
                                catch (DivideByZeroException)
                                {
                                    textBox1.Text += "Вариант ответа №" + Convert.ToString(i + 1) + " выбрало " +
                                    Convert.ToString(schottics[i]) + " опрошенных лиц.\r\nЭто " +
                                    Convert.ToString(0)
                                    + "% от всех опрошенных.\r\n";
                                }
                            }
                        }
                    }
                }
                else//если вывести результаты анализа в Chart
                {
                    Program.metatextbox_Text = metatextbox.Text;
                    //)))))))))))))))))))))))))))))))))))))))
                    if (variants.Count == 0)
                    {
                        Program.maxX = new int[schottics.Count];
                        Program.maxY = new int[schottics.Count];
                        for (int i = 0; i < schottics.Count; i++)
                        {
                            Program.maxX[i] = i + 1;//|-номер варианта ответа
                            Program.maxY[i] = schottics[i];//количество людей, которые выбрали |-номер
                        }
                    }
                    else
                    {
                        List<int> maxY = new List<int>(); List<int> maxX = new List<int>();
                        for (int i = 0; i < schottics.Count; i++)
                        {
                            if (variants_have(Convert.ToByte(i)))
                            {
                                maxX.Add(i + 1);//|-номер варианта ответа
                                maxY.Add(schottics[i]);//количество людей, которые выбрали |-номер
                            }
                        }
                        Program.maxX = new int[maxX.Count]; for (int ini = 0; ini < maxX.Count; ini++) { Program.maxX[ini] = maxX[ini]; }
                        Program.maxY = new int[maxY.Count()]; for (int ini = 0; ini < maxY.Count; ini++) { Program.maxY[ini] = maxY[ini]; }
                    }
                    //)))))))))))))))))))))))))))))))))))))))
                    Гистограмма_результатов_анализа_статистики newform = new Гистограмма_результатов_анализа_статистики();
                    newform.ShowDialog();
                }
            }
            else//провести ANALIZZ без учёта установленных ограничений
            {
                for (int j = 0; j < nnnumbers.Count; j++)//по абсолютно каждому отвечающему
                {//--
                    //сейчас надо вычленить те ответы, которые относятся к текущему вопросу |получить набор ответов "ныбор"|
                    //и по всем элементам этого "ныбора" пройтись и сделать {schottics[%элемент этого набора%] += 1;}
                    int ss = -1;//спецсчётчик_по итогам следующего for`а должен быть равен индексу последнего содержащего
                    //разделительный нуль элемента numbers[j] перед непосредственно "ныбором"
                    byte buf;
                    for (int u = 0; u < ui; u++)//проходим предвычленённые ответы
                    {
                        buf = 1;
                        while (buf != 0)
                        {
                            ss++;
                            buf = nnnumbers[j].numbers[ss];
                        }
                    }
                    ss++;
                    buf = nnnumbers[j].numbers[ss];
                    while (buf != 0)//по всем варианто-ответам
                    {
                        schottics[buf - 1] += 1;
                        ss++;
                        buf = nnnumbers[j].numbers[ss];
                    }
                    nnnumber_analiz_Count += 1;
                }//--

                if (tixt == true)//если вывести результаты анализа в textBox1.Text
                {
                    //итак. мы имеем заполненный schottics
                    //нужно сделать его анализ и его результаты запихнуть в textBox1
                    textBox1.Text = "";//очищаем textBox1.Text
                    for (int i = 0; i < schottics.Count; i++)
                    {
                        try
                        {
                            textBox1.Text += "Вариант ответа №" + Convert.ToString(i + 1) + " выбрало " +
                            Convert.ToString(schottics[i]) + " опрошенных лиц.\r\nЭто " +
                            Convert.ToString(100 * schottics[i] / nnnumber_analiz_Count)
                            + "% от всех опрошенных.\r\n";
                        }
                        catch (DivideByZeroException)
                        {
                            textBox1.Text += "Вариант ответа №" + Convert.ToString(i + 1) + " выбрало " +
                            Convert.ToString(schottics[i]) + " опрошенных лиц.\r\nЭто " +
                            Convert.ToString(0)
                            + "% от всех опрошенных.\r\n";
                        }
                    }
                }
                else//если вывести результаты анализа в Chart
                {
                    Program.metatextbox_Text = metatextbox.Text;
                    Program.maxX = new int[schottics.Count];
                    Program.maxY = new int[schottics.Count];
                    for (int i = 0; i < schottics.Count; i++)
                    {
                        Program.maxX[i] = i + 1;//|-номер варианта ответа
                        Program.maxY[i] = schottics[i];//количество людей, которые выбрали |-номер
                    }
                    Гистограмма_результатов_анализа_статистики newform = new Гистограмма_результатов_анализа_статистики();
                    newform.ShowDialog();
                }
            }
        }
        public void analiz()
        {
            if (ui == 0) { button1.Enabled = false; } else { button1.Enabled = true; }
            if (ui == Program.op.voprs.Count - 1) { button2.Enabled = false; } else { button2.Enabled = true; }
            metatextbox.Text = "ВОПРОС:\r\n";
            metatextbox.Text += Program.op.voprs[ui] + "\r\n";
            metatextbox.Text += "ВАРИАНТЫ ОТВЕТА:\r\n";
            schottics.Clear();//обнуляем счётчики для их последующего использования для анализа статистики
            list0_ch.Clear(); panel1.Controls.Clear();//обнуляем list0_ch и панель.контролs
            for(int i = 0; i < Program.op.otviti[ui].otvs.Count; i++)//по всем вариантам ответа
            {
                metatextbox.Text += Convert.ToString(i + 1) + ") " + Program.op.otviti[ui].otvs[i] + "\r\n";

                CheckBox cheki = new CheckBox(); cheki.Text = Convert.ToString(i + 1);//создаём чек, в его текст записываем номер варианта ответа
                cheki.Checked = true;
                list0_ch.Add(cheki);//засовываем его в list0_ch
                list0_ch[list0_ch.Count - 1].Location = new Point(0, 20 * i);//Location вот это всё
                panel1.Controls.Add(cheki);//засовываем его в панель.контролs
                schottics.Add(0);
            }
            pervo_analiz();
        }
        //list0_check'овский
        List<CheckBox> list0_ch = new List<CheckBox>();
        private void Анализ_статистики_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            radioButton1.Checked = true;
            for (int i = 0; i < Program.op.numbers.Count; i++)
            {
                nnnumbers.Add(new Program.opros.kogo_opr());
                string fio = Program.op.numbers[i].fio; nnnumbers[nnnumbers.Count-1].fio = fio;
                string nomer = Program.op.numbers[i].nomer; nnnumbers[nnnumbers.Count-1].nomer = nomer;
                bool pol_nal = Program.op.numbers[i].pol_nal; nnnumbers[nnnumbers.Count-1].pol_nal = pol_nal;
                bool pol = Program.op.numbers[i].pol; nnnumbers[nnnumbers.Count-1].pol = pol;
                bool date_nal = Program.op.numbers[i].date_nal; nnnumbers[nnnumbers.Count-1].date_nal = date_nal;
                DateTime date_birth = DateTime.Parse(Convert.ToString(Program.op.numbers[i].date_birth)); nnnumbers[nnnumbers.Count-1].date_birth = date_birth;
                foreach (byte bi in Program.op.numbers[i].numbers)
                {
                    nnnumbers[nnnumbers.Count-1].numbers.Add(bi);
                }
            }
            label8.Text = "Номер текущего вопроса - "+Convert.ToString(ui+1);
            analiz();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)//не выводить статистику по тем, чья дата рождения неизвестна
        {
            if(checkBox1.Checked == true)
            {
                vpv = false;
            }
            else
            {
                vpv = true;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            //проверяем не ввёл ли пользователь чепуху в поле минимальной даты рождения
            try
            {
                if (textBox2.Text != "")
                {
                    min = DateTime.Parse(textBox2.Text);
                }
                else
                {
                    min = new DateTime();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Введённые в поле даты данные некорректны! Либо очистите это поле, либо введите корректные данные!");
                textBox2.Select();
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            //проверяем не ввёл ли пользователь чепуху в поле максимальной даты рождения
            try
            {
                if (textBox3.Text != "")
                {
                    max = DateTime.Parse(textBox3.Text);
                }
                else
                {
                    max = new DateTime();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Введённые в поле даты данные некорректны! Либо очистите это поле, либо введите корректные данные!");
                textBox3.Select();
            }
        }

        private void button5_Click(object sender, EventArgs e)//Провести анализ статистики опроса с учётом установленных ограничений и вывести результат текстом
        {
            uchet = true;//учитывать в анализе установленные ограничения
            tixt = true;//передаём результаты анализа в textBox1.Text
            schottics.Clear();//обнуляем счётчики для их последующего использования для анализа статистики
            for (int i = 0; i < Program.op.otviti[ui].otvs.Count; i++)//по всем вариантам ответа
            {
                schottics.Add(0);
            }
            variants.Clear();//очищаем variants
            for (int i = 0; i < list0_ch.Count; i++)
            {
                if (list0_ch[i].Checked == false)//если хоть один чекбокс==false _ то заполняем variants
                {
                    for (int j = 0; j < list0_ch.Count; j++)
                    {
                        if (list0_ch[j].Checked == true)
                        {
                            int u = Convert.ToInt32(list0_ch[j].Text);
                            variants.Add(Convert.ToByte(u - 1));
                            //vichist.Add(Convert.ToByte(list0_ch[i].Text));
                        }
                    }
                    break;
                }
            }
            pervo_analiz();
        }

        private void button1_Click(object sender, EventArgs e)//перейти к пред. вопросу
        {
            radioButton1.Checked = true;
            ui -= 1;
            label8.Text = "Номер текущего вопроса - " + Convert.ToString(ui+1);
            analiz();
        }

        private void button2_Click(object sender, EventArgs e)//перейти к следующему вопросу
        {
            radioButton1.Checked = true;
            ui += 1;
            label8.Text = "Номер текущего вопроса - " + Convert.ToString(ui+1);
            analiz();
        }

        private void button6_Click(object sender, EventArgs e)//Вывести элементы variants
        {
            string sss = "";
            foreach(byte bi in variants)
            {
                sss+="_"+Convert.ToString(bi);
            }
            MessageBox.Show(sss);
        }

        private void button7_Click(object sender, EventArgs e)//Провести анализ статистики опроса с учётом установленных ограничений и вывести результат в Chart
        {
            uchet = true;//учитывать в анализе установленные ограничения
            tixt = false;//передаём результаты анализа в Chart
            
        }

        private void button4_Click(object sender, EventArgs e)//Провести анализ статистики опроса без учёта установленных ограничений и вывести результат текстом
        {
            uchet = false;//учитывать в анализе установленные ограничения
            tixt = true;//передаём результаты анализа в текстбокс
            schottics.Clear();//обнуляем счётчики для их последующего использования для анализа статистики
            for (int i = 0; i < Program.op.otviti[ui].otvs.Count; i++)//по всем вариантам ответа
            {
                schottics.Add(0);
            }
            variants.Clear();//очищаем variants
            for (int i = 0; i < list0_ch.Count; i++)
            {
                if (list0_ch[i].Checked == false)//если хоть один чекбокс==false _ то заполняем variants
                {
                    for (int j = 0; j < list0_ch.Count; j++)
                    {
                        if (list0_ch[j].Checked == true)
                        {
                            int u = Convert.ToInt32(list0_ch[j].Text);
                            variants.Add(Convert.ToByte(u - 1));
                            //vichist.Add(Convert.ToByte(list0_ch[i].Text));
                        }
                    }
                    break;
                }
            }
            pervo_analiz();
        }

        private void button8_Click(object sender, EventArgs e)//Провести анализ статистики опроса без учёта установленных ограничений и вывести результат в Chart
        {
            uchet = false;//учитывать в анализе установленные ограничения
            tixt = false;//передаём результаты анализа в Chart
            schottics.Clear();//обнуляем счётчики для их последующего использования для анализа статистики
            for (int i = 0; i < Program.op.otviti[ui].otvs.Count; i++)//по всем вариантам ответа
            {
                schottics.Add(0);
            }
            variants.Clear();//очищаем variants
            for (int i = 0; i < list0_ch.Count; i++)
            {
                if (list0_ch[i].Checked == false)//если хоть один чекбокс==false _ то заполняем variants
                {
                    for (int j = 0; j < list0_ch.Count; j++)
                    {
                        if (list0_ch[j].Checked == true)
                        {
                            int u = Convert.ToInt32(list0_ch[j].Text);
                            variants.Add(Convert.ToByte(u - 1));
                            //vichist.Add(Convert.ToByte(list0_ch[i].Text));
                        }
                    }
                    break;
                }
            }
            pervo_analiz();
        }

        private void button9_Click(object sender, EventArgs e)//Провести анализ статистики и вывести результат
        {
            if (checkBox2.Checked == false)//если учитывать установленные ограничения
            {
                uchet = true;
            }
            else//если не учитывать установленные ограничения
            {
                uchet = false;
            }
            if (radioButton1.Checked)//если выводить в текстбокс
            {
                tixt = true;
            }
            else//если выводить в отдельное окно в гистограмму
            {
                tixt = false;
            }
            schottics.Clear();//обнуляем счётчики для их последующего использования для анализа статистики
            for (int i = 0; i < Program.op.otviti[ui].otvs.Count; i++)//по всем вариантам ответа
            {
                schottics.Add(0);
            }
            variants.Clear();//очищаем variants
            for (int i = 0; i < list0_ch.Count; i++)
            {
                if (list0_ch[i].Checked == false)//если хоть один чекбокс==false _ то заполняем variants
                {
                    for (int j = 0; j < list0_ch.Count; j++)
                    {
                        if (list0_ch[j].Checked == true)
                        {
                            int u = Convert.ToInt32(list0_ch[j].Text);
                            variants.Add(Convert.ToByte(u - 1));
                        }
                    }
                    break;
                }
            }
            pervo_analiz();
        }
    }
}