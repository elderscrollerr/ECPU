using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ECPU
{
   public static class Logger
    {
        private static string logFile = INIT.CTRL_PANEL_DIR + "cp_log.txt";
        private static string successString = "Успешно: ";
        private static string failString = "Не успешно: ";
        public static void init()
        {
            if (!File.Exists(logFile))
            {

                StreamWriter sw = new StreamWriter(logFile);
                sw.WriteLine("Панель управления: Сборка " + INIT.GAME_TITLE + " " + INIT.USER_GAME_VERSION + " по пути " + INIT.GAME_ROOT + Environment.NewLine + "Версия базы данных панели: " + INIT.USER_BD_VERSION);
                sw.Close();
            }else
            {
                if (new FileInfo(logFile).Length > 10000000)
                {
                    File.Create(logFile).Close();
                }
            }
        }
        public static void addLine(bool success, string line)
        {
            if (success)
            {
                line = successString + line + " : " + DateTime.Now.ToString() + Environment.NewLine;
            }else
            {
                line = failString + line + " : " + DateTime.Now.ToString() + Environment.NewLine;
            }
            File.AppendAllText(logFile, line);
            
            
        }
    }
}
