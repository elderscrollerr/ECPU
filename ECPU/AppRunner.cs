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
   

    public class AppRunner
    {

       
        private static string apppath;      
        private static bool needToMountVirtualCD;
        private static bool VirtualCDisMountedNow;   
        private static bool explorerisDisableNow;
        private string argument;
        private IniManager im;

        public AppRunner(string _apppath)
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
            needToMountVirtualCD = false;
            VirtualCDisMountedNow = false;
       
            explorerisDisableNow = false;
       
            }

     



        public void runApp()
        {
            
           
            FileAttributes attr = File.GetAttributes(apppath);
            if (attr.HasFlag(FileAttributes.Directory))
            {

                Process.Start(apppath);
            }
            else
            {    
                beforeRun();
               
                Process _process = new Process();
               
               
                _process.StartInfo.FileName = apppath;
                if (!string.IsNullOrEmpty(argument))
                {
                    _process.StartInfo.Arguments = argument;
                }
                
                _process.EnableRaisingEvents = true;
                if (INIT.CURRENT_GAME.Equals(INIT.GAMES.OP) && Path.GetFileNameWithoutExtension(apppath).Equals("obse_loader"))
                {
                    _process.Exited += new EventHandler(scriptExtenderExit);
                }else
                {
                    _process.Exited += new EventHandler(processAPPExited);
                   
                }

            
                Logger.addLine(true, "Запущено приложение " + Path.GetFileNameWithoutExtension(apppath));

          
                    _process.Start();
            
            }
        }






        private void beforeRun()
        {

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Application.Current.MainWindow.IsEnabled = false;

            }));
            if (INIT.CURRENT_GAME.Equals(INIT.GAMES.OP))
            {

                if (string.IsNullOrEmpty(argument))
                {

                    if (File.Exists(INIT.GAME_ROOT + @"d3d9.bak"))
                    {
                        File.Move(INIT.GAME_ROOT + @"d3d9.bak", INIT.GAME_ROOT + @"d3d9.dll");

                    }
                    if (File.Exists(INIT.GAME_ROOT + @"Data\OBSE\Plugins\Construction Set Extender.dll"))
                    {
                        File.Move(INIT.GAME_ROOT + @"Data\OBSE\Plugins\Construction Set Extender.dll", INIT.GAME_ROOT + @"Data\OBSE\Plugins\Const.bak");
                    }
                    

                  
                }
                else
                {
                    if (File.Exists(INIT.GAME_ROOT + @"d3d9.dll"))
                    {
                        File.Move(INIT.GAME_ROOT + @"d3d9.dll", INIT.GAME_ROOT + @"d3d9.bak");
                    }

                    if (File.Exists(INIT.GAME_ROOT + @"Data\OBSE\Plugins\Const.bak"))
                    {
                        File.Move(INIT.GAME_ROOT + @"Data\OBSE\Plugins\Const.bak", INIT.GAME_ROOT + @"Data\OBSE\Plugins\Construction Set Extender.dll");
                    }
                   
                 
                }
            }


            Directory.SetCurrentDirectory(Path.GetDirectoryName(apppath));
            if (needToMountVirtualCD)
            {
                VirtualCDisMountedNow = true;
                //монтируем
            }
            IniManager im;
            if (File.Exists(INIT.GAME_ROOT + @"ObivionPerfect.ini"))
            {
                im = new IniManager(INIT.GAME_ROOT + @"ObivionPerfect.ini");
            }else
            {
                im = null;
            }
          


           if (INIT.CURRENT_GAME.Equals(INIT.GAMES.OP) && Convert.ToBoolean(im.getValueByKey("bDisableExplorer")) && string.IsNullOrEmpty(argument) && Path.GetFileNameWithoutExtension(apppath).Equals("obse_loader") )
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
            if (string.IsNullOrEmpty(argument))
            {
                foreach (Process line in Process.GetProcessesByName(INIT.GAME_PROCESS_NAME))
                {
                    line.EnableRaisingEvents = true;
                    line.Exited += processAPPExited;
                   
                }
            }
            else
            {
                foreach (Process line in Process.GetProcessesByName("TESConstructionSet"))
                {
                    line.EnableRaisingEvents = true;
                    line.Exited += processAPPExited;

                }
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
            if (VirtualCDisMountedNow)
            {
                //размонтируем
            }
            if (!MusicPlayer.isPlaying() && !MusicPlayer.STOPPED_MANUALLY)
            {
                MusicPlayer.Play();
            }
            Directory.SetCurrentDirectory(INIT.CTRL_PANEL_DIR);
            
        }

    }
}
