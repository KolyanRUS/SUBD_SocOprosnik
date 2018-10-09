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
    public partial class Прохождение_опроса : Form
    {
            /*
             Здесь опишу задумку, как пройтись по всем чекам/радикам и считать их показания
             Суть в том, что при создании их надо пихать не только в панель.контролs, но и в спецlistы
             list0ch - лист для чеков
             list0rad -  лист для радиобаттонов
             Если надо пройтись по всем штючкам (чекам OR радиобаттонам) что в панели, то проходим по всем элементам
             list0ch или list0rad
             */
                                //list0_check'овский
                                List<CheckBox> list0_ch = new List<CheckBox>();
                                //list0_radiob'овский
                                List<RadioButton> list0_rad = new List<RadioButton>();

        bool date_exist = false;  DateTime test_date = new DateTime();  //булевская переменная, true если есть дата, false если нет
                                                                        //тест-дата для проверки корректности данных, введённых в поле даты
        /*
        //данные о опрашиваемом
            string fio;//ФИО
            bool pol_nal = false; bool pol;//наличие пола-пол   /женский - отрицалово/
            bool date_nal = false; DateTime date_birth = new DateTime();//наличие даты рождения-дата рождения
            string tel_nomer;//номер телефона
        */
        //массив номеров выбранных вариантов ответа
            List<byte> numbers = new List<byte>();//массив ДЕЙСТВИТЕЛЬНЫХ вариантов ответа (номеров выбранных вариантов)
            //нули разделяют вопросы

        public Прохождение_опроса()
        {
            InitializeComponent();
        }
        int number_vop = 0;//это номер показываемого вопроса

        private void button2_Click(object sender, EventArgs e)//регистрация ответов,
                                                         //переход к следующему вопросу (если есть)
                                                         //если нет, то сообщение "Опрос пройден. Сохранить результаты?"
                                                      //и сообщение, что результат сохранён в файл, если загружался опрос из файла
                                                      //либо результат сохранён в виртуальный образ опроса
        {
            //заносим выбранные варианты ответа в numbers и нуль в конце ставим
            if (list0_ch.Count > 0)//если чеки
            {
                for (int i = 0; i < panel1.Controls.Count; i++)
                {
                    if (list0_ch[i].Checked == true)
                    {
                        byte add = Convert.ToByte(list0_ch[i].Text);// add -= 1;
                        numbers.Add(add);
                    }
                }
                //numbers.Add(0);//добавляем нуль-разделитель в конец
            }
            else if (list0_rad.Count > 0)//если радики
            {
                for (int i = 0; i < panel1.Controls.Count; i++)
                {
                    if(list0_rad[i].Checked == true)
                    {
                        byte add = Convert.ToByte(list0_rad[i].Text);// add -= 1;
                        numbers.Add(add);
                    }
                }
                //numbers.Add(0);//добавляем нуль-разделитель в конец
            }
            numbers.Add(0);//добавляем нуль-разделитель в конец
            //очищаем list0's и панель.контролs, а также textBox1
            list0_ch.Clear(); list0_rad.Clear(); panel1.Controls.Clear(); textBox1.Clear();

            //number_vop++; [увеличиваем переменную, обозначающую номер текущего вопроса на единицу]
            number_vop++;
            if (Program.op.voprs.Count < (number_vop + 1))//если больше вопросов нет
            {
                //--------------------------------------------------------------------------------------------
                DialogResult vibor2 = MessageBox.Show("Анкетирование завершено. Сохранить результаты данного анкетирования?", "", MessageBoxButtons.YesNo);
                if (vibor2 == DialogResult.Yes)//если да, сохранить
                {
                  //сохраняем в РАМ-образ опроса
                    //добавляем пустой элемент в массив данных о тех кого опросили
                    Program.opros.kogo_opr yy = new Program.opros.kogo_opr(); Program.op.numbers.Add(yy);
                    yy.fio = textBox2.Text;//записываем ФИО
                    if (comboBox1.SelectedIndex != -1)//если пол выбран
                    {
                        yy.pol_nal = true;//подтверждаем существование данных о поле
                        if (comboBox1.SelectedIndex == 0)//если мужской
                        {
                            yy.pol = true;
                        }
                        else//если женский
                        {
                            yy.pol = false;
                        }
                    }
                    else//если пол не выбран
                    {
                        yy.pol_nal = false;//подтверждаем НЕсуществование данных о поле
                    }
                    if (date_exist == true)//если дата рождения введена
                    {
                        yy.date_nal = true;
                        yy.date_birth = test_date;
                    }
                    else//если дата р. не введена
                    {
                        yy.date_nal = false;
                    }
                    foreach (byte bytee in numbers) { yy.numbers.Add(bytee); }
                    numbers.Clear();//очищаем numbers сей формы - он свою функцию выполнил
                  //а теперь сохраняем в файл
                    //MessageBox.Show("");
                }
                if (vibor2 == DialogResult.No)//если нет, не сохранить
                {
                    Close();//просто закрываем окно
                }
                //--------------------------------------------------------------------------------------------
            }
            else//если есть следующий вопрос
            {
                //сначала добавляем в вопросно-вариантный текстбокс "ВОПРОС" и энтер
                textBox1.Text += "ВОПРОС:" + "\r\n";
                //теперь добавляем текст первого вопроса
                textBox1.Text += Program.op.voprs[number_vop] + "\r\n";
                //теперь добавляем "ВАРИАНТЫ ОТВЕТА:" и энтер
                textBox1.Text += "ВАРИАНТЫ ОТВЕТА:";

                List<string> otviis = Program.op.otviti[number_vop].otvs;//в otviis запихиваем варианты ответа на number_vop'ый вопрос__//otviis

                for (int i = 0; i < otviis.Count; i++)
                {
                    textBox1.Text += "\r\n" + Convert.ToString(1+i) + ") " + otviis[i];
                }
                //теперь займёмся чеками/радиобаттонами
                if (Program.op.otviti[number_vop].mog_nesk)//если чекбоксы
                {
                    ////очищаем list0_ch и панель.контролs
                    //list0_ch.Clear(); panel1.Controls.Clear();
                    for (int i = 0; i < otviis.Count; i++)//с первого варианта ответа по последний 
                    {
                        CheckBox cheki = new CheckBox(); cheki.Text = Convert.ToString(i + 1);//создаём чек, в его текст записываем номер варианта ответа
                        list0_ch.Add(cheki);//засовываем его в list0_ch
                        list0_ch[list0_ch.Count - 1].Location = new Point(0, 20 * i);//Location вот это всё
                        panel1.Controls.Add(cheki);//засовываем его в панель.контролs
                    }
                }
                else//иначе, то бишь если радиобаттоны
                {
                    ////очищаем list0_ch и панель.контролs
                    //list0_rad.Clear(); panel1.Controls.Clear();
                    for (int i = 0; i < otviis.Count; i++)//с первого варианта ответа по последний 
                    {
                        RadioButton radi = new RadioButton(); radi.Text = Convert.ToString(i + 1);//создаём радик, в его текст записываем номер варианта ответа
                        list0_rad.Add(radi);//засовываем его в list0_ch
                        list0_rad[list0_rad.Count - 1].Location = new Point(0, 20 * i);//Location вот это всё
                        panel1.Controls.Add(radi);//засовываем его в панель.контролs
                    }
                }
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)//проверка даты
        {
            //проверяем не ввёл ли пользователь чепуху в поле даты
            try
            {
                if (textBox3.Text != "")
                {
                    date_exist = true;
                    test_date = DateTime.Parse(textBox3.Text);
                }
            }
            catch (FormatException)
            {
                date_exist = false;
                MessageBox.Show("Введённые в поле даты данные некорректны! Либо очистите это поле, либо введите корректные данные!");
                textBox3.Select();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)//начать собсна опрос_(причём начать с первого вопроса с его вариантами ответа)
        {
            number_vop = 0;//обнуляем number_vop
            //сначала добавляем в вопросно-вариантный текстбокс "ВОПРОС" и энтер
            textBox1.Text += "ВОПРОС:" + "\r\n";
            //теперь добавляем текст первого вопроса
            textBox1.Text += Program.op.voprs[number_vop] + "\r\n";
            //теперь добавляем "ВАРИАНТЫ ОТВЕТА:" и энтер
            textBox1.Text += "ВАРИАНТЫ ОТВЕТА:";

            List<string> otviis = Program.op.otviti[number_vop].otvs;//в otviis запихиваем варианты ответа на первый вопрос__//otviis

            for (int i = 0; i < otviis.Count; i++)
            {
                textBox1.Text += "\r\n" + Convert.ToString(1 + i) + ") " + otviis[i];
            }
            //теперь займёмся чеками/радиобаттонами
            if (Program.op.otviti[number_vop].mog_nesk)//если чекбоксы
            {
                //очищаем list0_ch и панель.контролs
                list0_ch.Clear(); panel1.Controls.Clear();
                for (int i = 0; i < otviis.Count; i++)//с первого варианта ответа по последний 
                {
                    list0_ch.Add(new CheckBox());//создаём чек
                    list0_ch[list0_ch.Count - 1].Text = Convert.ToString(i + 1);//в его текст записываем номер варианта ответа
                    list0_ch[list0_ch.Count - 1].Location = new Point(0, 20 * i);//Location вот это всё
                    panel1.Controls.Add(list0_ch[list0_ch.Count - 1]);//засовываем его в панель.контролs
                }
            }
            else//иначе, то бишь если радиобаттоны
            {
                //очищаем list0_ch и панель.контролs
                list0_rad.Clear(); panel1.Controls.Clear();
                for (int i = 0; i < otviis.Count; i++)//с первого варианта ответа по последний 
                {
                    list0_rad.Add(new RadioButton());//создаём радик
                    list0_rad[list0_rad.Count-1].Text = Convert.ToString(i + 1);//в его текст записываем номер варианта ответа
                    list0_rad[list0_rad.Count - 1].Location = new Point(0, 20 * i);//Location вот это всё
                    panel1.Controls.Add(list0_rad[list0_rad.Count - 1]);//засовываем его в панель.контролs
                }
            }
            button2.Enabled = true;
        }

        private void Прохождение_опроса_Load(object sender, EventArgs e)
        {
            ShowIcon = false;
            //заполнение metatextbox`а доп. сведениями о опросе и опрашивающем лице
            metatextbox.Text = Program.op.DOPSVED;
        }
    }
}