using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class File_Work : Form1
    {
        static string pathForOpen;
        const string path = "PathAndCommand.txt";

        public void Open_File(string pathForOpen)//открывает файл(каталог), если не получилось, выводит сообщение
        {
            try
            {
                Process.Start("explorer",pathForOpen);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("Файл не найден");
            }
        }

        public string File_Path(string command)//возвращает путь файла(каталога), который должен быть открыт по переданной команде
        {
            StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1251));
            string line = sr.ReadToEnd();
            string[] commandArray = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < commandArray.Length / 2; i++)
            {
                if (command == commandArray[2 * i + 1])
                {
                    pathForOpen = commandArray[2 * i];
                    break;
                }
            }
            sr.Close();
            return pathForOpen;
        }

        public bool File_Create()//проверяет существование файла для хранения данных и возвращает false/true взависимости от его наполнения
        {
            if (!File.Exists(path))
                File.Create(path).Close();

            StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1251));
            string line = sr.ReadToEnd();
            sr.Close();

            if (line == "")
                return false;

            return true;
        }
      
        public int CheckInput(string str)//проверяет имя команды на недопустимые символы
        {
            char[] strChar = str.ToCharArray();

            for (int i = 0; i < strChar.Length; i++)
            {
                strChar[i] = str[i];
            }

            int[] strInt = new int[strChar.Length];

            for (int i = 0; i < strChar.Length; i++)
            {
                strInt[i] = Convert.ToInt32(strChar[i]);
            }

            for (int i = 0; i < strChar.Length; i++)
            {
                if (strInt[i] >= 'a' && strInt[i] <= 'z' || strInt[i] == ' ' || strInt[i] >= 'A' && strInt[i] <= 'Z')
                {
                    return 0;
                }
            }

            return 1;
        }

    }
}
