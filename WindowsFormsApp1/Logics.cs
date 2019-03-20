using System;
using System.IO;
using System.Text;


namespace WindowsFormsApp1
{
    class Logics
    {
        const string path = "PathAndCommand.txt";

        public string[] Search_Command()//записывает все команды из файла в массив
        {
            StreamReader sr = new StreamReader(path, Encoding.GetEncoding(1251));
            string line = sr.ReadToEnd();
            string[] commandArray = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] command = new string[commandArray.Length / 2];
            for (int i = 0; i < commandArray.Length / 2; i++)
            {
                command[i] = commandArray[2 * i + 1];
            }
            sr.Close();
            return command;
        }
    }
}
