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
   

    public class CSE_Runner
    {

       
        private static string apppath;      
        private string argument;
        private IniManager im;

        public CSE_Runner(string _apppath)
        {
            if (_apppath.Contains('?'))
            {              
                argument = _apppath.Split('?')[1];
                _apppath = _apppath.Split('?')[0];

            }else
            {               
                argument = "";
            }

            apppath = _apppath;
       
            }

     



        public void runApp()
        {   
                beforeRun();
               
                Process _process = new Process();
               
               
                _process.StartInfo.FileName = apppath;
                if (!string.IsNullOrEmpty(argument))
                {
                    _process.StartInfo.Arguments = argument;
                }
                
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


            if (File.Exists(INIT.GAME_ROOT + @"d3d9.dll"))
            {
                File.Move(INIT.GAME_ROOT + @"d3d9.dll", INIT.GAME_ROOT + @"d3d9.bak");
            }

            if (File.Exists(INIT.GAME_ROOT + @"Data\OBSE\Plugins\Const.bak"))
            {
                File.Move(INIT.GAME_ROOT + @"Data\OBSE\Plugins\Const.bak", INIT.GAME_ROOT + @"Data\OBSE\Plugins\Construction Set Extender.dll");
            }


            Directory.SetCurrentDirectory(Path.GetDirectoryName(apppath));
            if (MusicPlayer.isPlaying())
            {
                MusicPlayer.Stop();
            }
        }

        private void scriptExtenderExit(object sender, System.EventArgs e)
        {
                foreach (Process line in Process.GetProcessesByName("TESConstructionSet"))
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
          
           
            if (!MusicPlayer.isPlaying() && !MusicPlayer.STOPPED_MANUALLY)
            {
                MusicPlayer.Play();
            }
            Directory.SetCurrentDirectory(INIT.CTRL_PANEL_DIR);
            
        }

    }
}
