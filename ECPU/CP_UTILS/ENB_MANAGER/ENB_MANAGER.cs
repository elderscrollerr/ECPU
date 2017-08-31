using ECPU.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.IO;
using ECPU.Exceptions;
using System.Collections;
using ECPU.Settings;
using ECPU.UI.WINDOW_ITEMS;

namespace ECPU
{
    public class ENB_MANAGER : CPWindow
    {
       

        private static List<string> enbcore;
        private static ENB_PRESET activePreset;
        // private string currentpreset;
        //  private  presets;

        public ENB_MANAGER() : base(null)
        {
            restoreEnbCoreFiles();
        }


        public static void restoreEnbCoreFiles()
        {
          
            if (enbcore ==null || enbcore.Count <= 0)
            {
                enbcore = new List<string>();
                enbcore.Add("d3d11.dll");
                enbcore.Add("d3dcompiler_46e.dll");
                enbcore.Add("enblocal.ini");
            }
         
            foreach (string file in enbcore)
            {
                if (!File.Exists(INIT.GAME_ROOT + file))
                {
                    FileManager.copyFiles(INIT.RES_DIR + @"ENB core\" + file, INIT.GAME_ROOT + file);
                }
            }
        }

        protected override Grid buildContent()
        {
            string[] presetsFolders = Directory.GetDirectories(INIT.ENB_DIR);

            List<ENB_PRESET> presets = new List<ENB_PRESET>();
            ENB_PRESET noPreset = new ENB_PRESET();
            if (noPreset.getActivity())
            {
                activePreset = noPreset;
            }
            noPreset.Checked += new RoutedEventHandler(act);
            presets.Add(noPreset);

            foreach (string presetDir in presetsFolders)
            {
                if (Directory.GetFileSystemEntries(presetDir).Length != 0)
                {
                    ENB_PRESET preset = new ENB_PRESET(presetDir);
                    preset.Checked += new RoutedEventHandler(act);
                    if (preset.getActivity())
                    {
                        activePreset = preset;
                    }
                    presets.Add(preset);
                }
            }


            Grid enbGrid = new Grid();         
            enbGrid.ColumnDefinitions.Add(new ColumnDefinition());
           
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                enbGrid.Background = new ImageBrush(STYLE.BUTTON);
               // myScrollViewer.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                enbGrid.Background = Brushes.WhiteSmoke;
               // myScrollViewer.Foreground = Brushes.Black;
            }           
           
            enbGrid.Margin = new Thickness(15);

            for (int i = 0; i < presets.Count; i++)
            {
               
                enbGrid.RowDefinitions.Add(new RowDefinition());
                StackPanel sp = new StackPanel();
                
                sp.Children.Add(presets[i]);
                enbGrid.Children.Add(sp);
                Grid.SetRow(sp, i);
                Grid.SetColumn(sp, 0);
            }
           
           // Grid enbGrid2 = new Grid();
         //   myScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
          //  myScrollViewer.Height = 400;
           // enbGrid2.Children.Add(myScrollViewer);
           // Grid.SetRow(myScrollViewer, 0);
           // Grid.SetColumn(myScrollViewer, 0);
            if (activePreset==null)
            {
                noPreset.enable();
                activePreset = noPreset;
            }
            return enbGrid;

        }

        public static void act(object sender, RoutedEventArgs e)
        {
            if (activePreset!=null)
            {
              //  MessageBox.Show(activePreset.Content.ToString());
                activePreset.disable();
               
           //   MessageBox.Show((sender as ENB_PRESET).Content.ToString());
                (sender as ENB_PRESET).enable();
                activePreset = (sender as ENB_PRESET);
                //  (sender as ENB_PRESET).setActive();
            }


          


        }
    }
}
