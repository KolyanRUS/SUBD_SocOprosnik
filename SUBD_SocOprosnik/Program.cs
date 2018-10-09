using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SUBD_SocOprosnik
{
    
        
    static class Program
    {
        public class opros
        {
            public class otvets//класс_массива ВОЗМОЖНЫХ вариантов ответа
            {
                public bool mog_nesk = false;//true если можно выбрать несколько вариантов false если только один (false по умолчанию)
                public List<string> otvs = new List<string>();
            }
            public class kogo_opr//класс_набор данных о тех кого опрашивают & ИХ ОТВЕТЫ>
            //[фамилия, имя, отчество, номер телефона, наличие пола-пол, наличие даты рождения-дата рождения, подмассив номеров ответов]
            {
                public string fio;
                public string nomer;
                public bool pol_nal = false;//наличие пола
                    public bool pol;//женский - false, мужской - true
                public bool date_nal = false;//наличие даты рождения
                    public DateTime date_birth = new DateTime();//дата рождения
                public List<byte> numbers = new List<byte>();//массив ДЕЙСТВИТЕЛЬНЫХ вариантов ответа (номеров выбранных вариантов)
                                                             //нули разделяют вопросы
            }
            //VOPROSES:
                public List<string> voprs = new List<string>();//<массив вопросов>//+1 если +1 вопрос
                public List<otvets> otviti = new List<otvets>();//<массив массивов вариантов ответа>//+1 если +1 вопрос
            //<массив-набор данных о тех кого опрашивают> [фамилия, имя, отчество, номер телефона, дата рождения, подмассив номеров ответов]
                public List<kogo_opr> numbers = new List<kogo_opr>();
            //MetatextBox:
                //DOPSVED
                public string DOPSVED;


                public void Clear()
                {
                    voprs.Clear();
                    otviti.Clear();
                    numbers.Clear();
                    DOPSVED = null;
                }
        }
        public static int vopros_ind;//индекс вопроса, передаваемого из окна создания опроса в окно редактирования вопроса
        public static string varik;//вариант ответа (для передачи варианта ответа из окна созд. в. в окно редактирования варианта о. и наоборот)
        public static opros op = new opros();
        public static bool est_vopr = true;//эта булевская переменная для того, что если окно вопроса создаёт вопрос, то делаем false и удаляем

        //вся та дрисня, что нужно передать на форму гистограммы:--//
        public static string metatextbox_Text;//метатекстбоксовый текст
        public static int[] maxX;//массив X-значений
        public static int[] maxY;//массив Y-значений
        //--вся та дрисня, что нужно передать на форму гистограммы://
        //созданный пустой вопрос перед инициализацией формы
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
