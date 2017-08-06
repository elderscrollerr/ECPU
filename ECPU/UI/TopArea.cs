using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Threading;
using System.Data;
using ECPU.Exceptions;
using System;
using ECPU.Settings;
using static ECPU.INIT;

namespace ECPU.UI
{
  public  class TopArea : Grid
    {
        private enum TOP_PANEL_BUTTONS { CLOSE_BUTTON, MUSIC_BUTTON, LOG_BUTTON }
      //  private string windowTitle;
        public TopArea()
        {
         //   windowTitle = _windowTitle;
            HorizontalAlignment = HorizontalAlignment.Left;
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                Background = new ImageBrush(STYLE.BUTTON);
            }else
            {
                Background = Brushes.WhiteSmoke;
            }
            

            int i = -1;
            // Grid grid = new Grid();
            foreach (TOP_PANEL_BUTTONS type in Enum.GetValues(typeof(TOP_PANEL_BUTTONS)))
            {
                i++;
                ColumnDefinitions.Add(new ColumnDefinition());
                TextBlock topButton = new TextBlock();
                topButton.Text = getButton(type).ToString();
                topButton.Tag = getButton(type);
                topButton.FontSize = 20.0;
                if (!INIT.DEFAULT_VISUAL_STYLE)
                {
                    topButton.Foreground = STYLE.MAIN_MENU_FOREGROUND;
                }else
                {
                    topButton.Foreground = Brushes.Black;
                }
               
                topButton.FontWeight = FontWeights.Bold;                
                topButton.Width = 30.0;
                topButton.Height = 30.0;
                topButton.TextAlignment = TextAlignment.Center;
                //  topButton.VerticalAlignment = VerticalAlignment.Center;
                (topButton as TextBlock).MouseDown += act;
                Children.Add(topButton);
                Grid.SetRow(topButton, 0);
                Grid.SetColumn(topButton, i);
            }

        }

        private void act(object sender, MouseButtonEventArgs e)
        {
           // char c = getButton(TOP_PANEL_BUTTONS.CLOSE_BUTTON);
            char senderButton = (char)(sender as TextBlock).Tag;
            if (!Convert.ToBoolean(senderButton.CompareTo(getButton(TOP_PANEL_BUTTONS.CLOSE_BUTTON))))
            {
                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive).Close();
               // CPWindow windowmain = (CPWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
             //   if (windowmain.windowTitle.Equals();)
            }
            else
            {
                if (!Convert.ToBoolean(senderButton.CompareTo(getButton(TOP_PANEL_BUTTONS.MUSIC_BUTTON))))
                {
                    if (MusicPlayer.isPlaying())
                    {
                        MusicPlayer.Stop();
                        MusicPlayer.STOPPED_MANUALLY = true;
                    }
                    else
                    {
                        MusicPlayer.Play();
                        MusicPlayer.STOPPED_MANUALLY = false;
                    }
                }
                else
                {
                    if (!Convert.ToBoolean(senderButton.CompareTo(getButton(TOP_PANEL_BUTTONS.LOG_BUTTON))))
                    {
                        if (File.Exists(INIT.LOG_FILE))
                        {
                            Process.Start("notepad.exe", INIT.LOG_FILE);
                        }
                    }
                }
            }
        }

        private static char getButton(TOP_PANEL_BUTTONS button)
        {
          
            switch (button)
            {
                case TOP_PANEL_BUTTONS.CLOSE_BUTTON:
                    return 'X';
                case TOP_PANEL_BUTTONS.MUSIC_BUTTON:
                    return '\u266B';
                case TOP_PANEL_BUTTONS.LOG_BUTTON:
                    return '\u00A7';
                default:
                    return '0';
                 
            }
        }


    }
}
