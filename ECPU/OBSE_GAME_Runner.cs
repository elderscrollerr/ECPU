using ECPU.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ECPU
{
   

    public class OBSE_GAME_Runner
    {

       
       private static string apppath;  
       private static bool explorerisDisableNow;
       private string argument;
       private IniManager im;

        public OBSE_GAME_Runner(string _apppath)
        {
            apppath = _apppath;       
       
            explorerisDisableNow = false;
       
        }

     



        public void runApp()
        {
                beforeRun();
               
                Process _process = new Process();
               
               
                _process.StartInfo.FileName = apppath;
              
                _process.EnableRaisingEvents = true;
                           
                _process.Exited += new EventHandler(scriptExtenderExit);
            
                Logger.addLine(true, "Запущено приложение " + Path.GetFileNameWithoutExtension(apppath));
          
                 _process.Start();
            
           
        }






        private void beforeRun()
        {

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Application.Current.MainWindow.IsEnabled = false;

            }));



            if (File.Exists(INIT.GAME_ROOT + @"d3d9.bak"))
            {
                File.Move(INIT.GAME_ROOT + @"d3d9.bak", INIT.GAME_ROOT + @"d3d9.dll");

            }
            if (File.Exists(INIT.GAME_ROOT + @"Data\OBSE\Plugins\Construction Set Extender.dll"))
            {
                File.Move(INIT.GAME_ROOT + @"Data\OBSE\Plugins\Construction Set Extender.dll", INIT.GAME_ROOT + @"Data\OBSE\Plugins\Const.bak");
            }

            if (File.Exists(INIT.GAME_ROOT + @"ObivionPerfect.ini"))
            {
                IniManager im = new IniManager(INIT.GAME_ROOT + @"ObivionPerfect.ini");

                if (Convert.ToBoolean(im.getValueByKey("bDisableExplorer", "[GBR]")))
                {
                    explorerisDisableNow = true;
                }
            }

            Directory.SetCurrentDirectory(Path.GetDirectoryName(apppath));
          
            if (explorerisDisableNow)
            {
                    foreach (Process process in Process.GetProcessesByName("explorer"))
                    {
                        process.KillExplorer();
                    }
                    explorerisDisableNow = true;
            }

           
            if (MusicPlayer.isPlaying())
            {
                MusicPlayer.Stop();
            }
        }

        private void scriptExtenderExit(object sender, System.EventArgs e)
        {
                foreach (Process line in Process.GetProcessesByName(INIT.GAME_PROCESS_NAME))
                {
                    line.EnableRaisingEvents = true;
                    line.Exited += processAPPExited;
                   
                }
        }


        private void processAPPExited(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Application.Current.MainWindow.IsEnabled = true;

            }));
            if (explorerisDisableNow)
            {
                ExplorerHandler.StartExplorer();
            }
            
            if (!MusicPlayer.isPlaying() && !MusicPlayer.STOPPED_MANUALLY)
            {
                MusicPlayer.Play();
            }
            Directory.SetCurrentDirectory(INIT.CTRL_PANEL_DIR);
            
        }

    }
}
