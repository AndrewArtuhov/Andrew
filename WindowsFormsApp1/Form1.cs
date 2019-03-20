using System;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Globalization;
using System.IO;
using System.Text;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static Label enterOnMonitor;
        static File_Work fw = new File_Work();
        static Logics logics = new Logics();

        static string pathForOpen;
        const string path = "PathAndCommand.txt";

        public Form1()
        {
            InitializeComponent();
        }

        public void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)//сравнивает полученную голосовую команду по степени соответсвия с имеющимися командами 
        {
            if (e.Result.Confidence > 0.7)
            {
                enterOnMonitor.Text = "Запускается " + e.Result.Text;
                pathForOpen = fw.File_Path(e.Result.Text);
                fw.Open_File(pathForOpen);
                enterOnMonitor.Text = null;
            }
            else
                enterOnMonitor.Text = "Текст не распознан";
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            enterOnMonitor = label1;

            bool dataInFile = fw.File_Create();
            if (dataInFile)
            {
                CultureInfo ci = new CultureInfo("ru-ru");//язык использования

                SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);//объект для распознования речи
                sre.SetInputToDefaultAudioDevice();//от куда идет распознания речи(микрофон)
                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Sre_SpeechRecognized);//если текст распознан вызываем функцию Sre_SpeechRecognized()

                Choices command = new Choices();
                command.Add(logics.Search_Command());//какие команды должны быть расознаны

                GrammarBuilder gb = new GrammarBuilder();
                gb.Append(command);

                Grammar grammar = new Grammar(gb);

                sre.LoadGrammar(grammar);
                sre.RecognizeAsync(RecognizeMode.Multiple);//распознание множественное повторение речи 
            }
            else
                enterOnMonitor.Text = "Добавьте команды";
        }

        private void button1_Click_1(object sender, EventArgs e)//проверка на существование файла(каталога) и добавление их в файл при их существовании
        {
            enterOnMonitor.Text = null;
            label4.Text = null;
            label5.Text = null;

            if (!File.Exists(path))
                File.Create(path).Close();
            int check = fw.CheckInput(textBox2.Text);

            if (File.Exists(textBox1.Text) || (textBox2.Text != null || check != 0) || Directory.Exists(textBox1.Text))
            {               
                using (var f = new StreamWriter(path, true, Encoding.Default))
                {
                    f.WriteLine(textBox1.Text + " " + textBox2.Text + " ");
                }
                textBox2.Text = null;
                textBox1.Text = null;
            }
            else
                label4.Text = "Вы ввели неверный путь к файлу,каталогу или указали некорректное имя команды";

            Form1_Shown(null, null);
        }

        private void button2_Click(object sender, EventArgs e)//просмотр внесеных команд
        {
            string textInFile;
            StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1251));
            textInFile = sr.ReadToEnd();
            if (textInFile == "")
                textInFile = "Команд нет";
            sr.Close();
            MessageBox.Show(textInFile);
        }

        private void Form1_Load(object sender, EventArgs e)//показывает в свернутом режими эти данные
        {
            notifyIcon1.BalloonTipTitle = "VoiceManagement";
            notifyIcon1.BalloonTipText = "Приложение свернуто";
            notifyIcon1.Text = "VoiceManagement";
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)//не будем видеть иконку пока приложение не свернулось
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
            else if (FormWindowState.Normal == this.WindowState)
            { notifyIcon1.Visible = false; }
        }
    }
}
