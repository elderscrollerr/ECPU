using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Win32;
using System.Windows.Controls;
using ECPU.Settings;
using System.Windows.Input;
using ECPU.UI;
using System.IO;
using System.Diagnostics;
using System.Threading;
using ECPU.Exceptions;
using System.Windows.Media;
using System.Windows;
using ECPU;
using System.Reflection;
using ECPU.LoadOrderUtility;

namespace ECPU
{
    public class MainMenuButton : CPWindowItem, CP_ActionableItem
    {
        
      
        private string windowsToOpen;
        
        private string type;
        private Label button;
        private INIT.APP application;
        private string target;
        private string typeOfApp;

        private static string musicPath = STYLE.MUSIC_FILE_PATH;
       
         public MainMenuButton(string _name, string _text, string _desc, int _position, string _type, string _target) : base(_name, _text, _desc, _position)
        {
           
            type = _type;
            target = _target;
            
            switch (type)
            {
                case "APP":
                 //   MessageBox.Show(_name);
                    application.PATH = INIT.getpath(target);
                    typeOfApp = _desc;
                    break;             
                default:
                  target = _target;
                    break;
            }
        }


        public override StackPanel getView()
        {
            view = new StackPanel();
            button = new Label();
            button.Content = _text;

            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                button.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                button.Foreground = Brushes.Black;
            }

           
          //  button.Background = new ImageBrush(STYLE.BUTTON);
            button.FontFamily = new FontFamily("Helvetica");
            button.FontSize = fontSize;
            button.Width = width;
            button.Height = height;
           // button.BorderBrush = STYLE.MAIN_MENU_BORDER_COLOR;
            button.BorderThickness = new Thickness(2);
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.VerticalContentAlignment = VerticalAlignment.Center;           
            view.HorizontalAlignment = HorizontalAlignment.Center;
          view.VerticalAlignment = VerticalAlignment.Center;          
            view.Children.Add(button);
           
            (button as Label).MouseDown += act;
            
            return view;
        }


        public void act(object sender, MouseButtonEventArgs e)
        {


            switch (type)
            {
                case "APP":
                    switch (typeOfApp)
                    {
                        case "GAME":

                            try { new AppRunner(application.PATH).runApp(); }
                            catch { throw new ApplicationNotFoundException(Path.GetFileNameWithoutExtension(application.PATH)); }
                            break;
                        case "OBSE_GAME":

                            try { new OBSE_GAME_Runner(application.PATH).runApp(); }
                            catch { throw new ApplicationNotFoundException(Path.GetFileNameWithoutExtension(application.PATH)); }
                            break;
                        case "CSE":

                            try { new CSE_Runner(application.PATH).runApp(); }
                            catch { throw new ApplicationNotFoundException(Path.GetFileNameWithoutExtension(application.PATH)); }
                            break;
                        default:
                            break;
                    }
                            break;
                case "WINDOW":
                    new CPWindow(target).ShowDialog();
                    break;
                case "HTTP":
                    System.Diagnostics.Process.Start(INIT.getpath(target));
                    break;
                case "CP_UTIL":
                    switch (target)
                    {
                        case "RESET":
                            Utils.ResetSettings();
                            break;
                        case "ENB_MANAGER":
                            if (Directory.Exists(INIT.ENB_DIR))
                            {
                                new ENB_MANAGER().ShowDialog();
                            }                            
                            break;
                        case "LO_MANAGER":
                            new LO_MANAGER().ShowDialog();                        
                            break;
                        case "UPDATER":
                            try
                            {
                                UPDATER.TEMP_DB_SQLITE_FILE = INIT.RES_DIR + "TEMP.db";
                                UPDATER.TEMP_DB_CONNECTION_STRING = "Data Source=" + UPDATER.TEMP_DB_SQLITE_FILE;
                                UPDATER.getnewBD();
                            }
                            catch (NoInternetConnectionException)
                            {
                                return;
                            }

                            try
                            {
                               new UPDATER().ShowDialog();
                            }
                            catch (gameBuildInvalidVersionForUpdateException ex)
                            {
                                MessageBox.Show(ex.Message);
                                return;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:                   
                    break;
            }
     
        }

        private void onexit(object sender, EventArgs e)
        {
            if (!MusicPlayer.isPlaying())
            {
                MusicPlayer.Play();
            }
        }

 
     
    }
}
