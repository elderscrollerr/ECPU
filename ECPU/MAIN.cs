
using ECPU.Exceptions;
using ECPU.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static ECPU.INIT;

namespace ECPU
{
    public static class MAIN
    {
        static Mutex mutex = new Mutex(false, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
      
        [STAThread]
        public static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                MessageBox.Show("Панель уже запущена!");
                return;
            }

            try
            {

                initUserBDGame();
                Logger.init();
                Logger.addLine(true, "Запуск приложения. Начало проверок");
                Utils.checkNETFrameworkVersion();



                try
                {

                    Utils.CheckGameFileBeforeStart();
                    Logger.addLine(true, "Проверка необходимых файлов");
                }
                catch (CriticalFileNotFoundException ex)
                {
                    Logger.addLine(false, "Проверка необходимых файлов: " + ex.Message);
                    MessageBox.Show(ex.Message);
                    MessageBox.Show("Будет произведен сброс настроек!");
                    Utils.ResetSettings();
                    Thread.Sleep(1000);
                    Logger.addLine(true, "Завершение приложения");
                    Environment.Exit(1);
                }

                try
                {
                    Utils.registryFix(INIT.REGISTRY_HIVE, INIT.GAME_ROOT);
                    Logger.addLine(true, "Проверка и исправления записи в реестре");
                }
                catch (RegistryException ex)
                {
                    Logger.addLine(false, "Проверка и исправления записи в реестре: " + ex.Message);
                    Logger.addLine(true, "Завершение приложения");
                    Environment.Exit(1);
                }
                Application app = new Application();
                app.Run(new CPWindow("WINDOW_MAINMENU"));
            }
            finally { mutex.ReleaseMutex(); }


      
           
         
          

        }
    }
}