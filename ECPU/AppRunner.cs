﻿using ECPU.UI;
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

       
        protected static string apppath;      
        private static bool needToMountVirtualCD;
        private static bool VirtualCDisMountedNow;   
        private static bool explorerisDisableNow;
        private string argument;
        private IniManager im;

        public AppRunner(string _apppath)
        {
           

            apppath = _apppath;
          //  needToMountVirtualCD = false;
         //   VirtualCDisMountedNow = false;
       
//explorerisDisableNow = false;
       
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
             
                
                _process.EnableRaisingEvents = true;
               
                    _process.Exited += new EventHandler(processAPPExited);
                   
              

            
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


            Directory.SetCurrentDirectory(Path.GetDirectoryName(apppath));
          //  if (needToMountVirtualCD)
        //    {
       //         VirtualCDisMountedNow = true;
                //монтируем
         //   }
       

           
            if (MusicPlayer.isPlaying())
            {
                MusicPlayer.Stop();
            }
        }

    


        private void processAPPExited(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Application.Current.MainWindow.IsEnabled = true;

            }));
       
//if (VirtualCDisMountedNow)
        //    {
                //размонтируем
        //    }
            if (!MusicPlayer.isPlaying() && !MusicPlayer.STOPPED_MANUALLY)
            {
                MusicPlayer.Play();
            }
            Directory.SetCurrentDirectory(INIT.CTRL_PANEL_DIR);
            
        }

    }
}
