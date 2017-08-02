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
        //   public enum CONTROLS_ACTIONS { UP, DOWN, RESET };
        //  private StackPanel plugins;
        //   private StackPanel controls;

        //  private static List<PluginInList> LO;

        public static List<string> enbcore;

        private string currentpreset;
        private List<CPWindowItem> presets;
        public static bool callFromENBMANAGER;

        public ENB_MANAGER() : base(null)
        {
           

           

           


        }

        protected override Grid getContent()
        {
            enbcore = new List<string>();
            enbcore.Add("d3d11.dll");
            enbcore.Add("d3dcompiler_46e.dll");
            enbcore.Add("enblocal.ini");

            foreach (string file in enbcore)
            {
                if (!File.Exists(INIT.GAME_ROOT + file))
                {
                    FileManager.copyFiles(INIT.RES_DIR + @"ENB core\file", INIT.GAME_ROOT);
                }
            }

            callFromENBMANAGER = true;
            string[] presetsFolders = Directory.GetDirectories(INIT.ENB_DIR);
            CPWindowItem item;
            presets = new List<CPWindowItem>();
            for (int numberOfPreset = 0; numberOfPreset < presetsFolders.Length; numberOfPreset++)
            {
                if (Directory.GetFileSystemEntries(presetsFolders[numberOfPreset]).Length != 0)
                {
                    item = new ENB_PRESET(numberOfPreset, Path.GetFileName(presetsFolders[numberOfPreset]), presetsFolders[numberOfPreset], null, null);
                    presets.Add(item);
                }


            }


            if (string.IsNullOrWhiteSpace(INIT.CURRENT_ENB_OPTION))
            {
                currentpreset = "";
            }
            else
            {
                currentpreset = INIT.CURRENT_ENB_OPTION;
            }




            Grid enbGrid = new Grid();
            StackPanel view = new StackPanel();
            view.Width = 200;
            foreach (CPWindowItem preset in presets)
            {
                enbGrid.RowDefinitions.Add(new RowDefinition());
            }

            enbGrid.ColumnDefinitions.Add(new ColumnDefinition());


            for (int i = 0; i < presets.Count; i++)
            {

                presets[i].setFontSize(14.0);

                StackPanel itemOnForm = presets[i].getView();
                if (!INIT.DEFAULT_VISUAL_STYLE)
                {
               //     itemOnForm.Background = new ImageBrush(STYLE.BUTTON);
                //    itemOnForm.Width = CP_RADIO_BUTTONS_LIST_ELEMENT.maxWidth;
                }
                else
                {
                    itemOnForm.Background = Brushes.WhiteSmoke;
                }
                itemOnForm = presets[i].getView();

                enbGrid.Children.Add(itemOnForm);
            enbGrid.Background = new ImageBrush(STYLE.BUTTON);
                enbGrid.Margin = new Thickness(15);
                Grid.SetRow(itemOnForm, i);
                Grid.SetColumn(itemOnForm, 0);
            }
           // enbGrid.ShowGridLines = true;
            return enbGrid;

        }
    

    


     




     

     

        private void act(object sender, MouseButtonEventArgs e)
        {
         //   changeLO((CONTROLS_ACTIONS)(sender as TextBlock).Tag);
            getContent();
            CPWindow window = (CPWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.allWindow.Children.RemoveAt(1);
            window.allWindow.Children.Add(getContent());
        
        }

    
        /*
        public override StackPanel getView()
        {
            view = new StackPanel();
            view.Orientation = Orientation.Horizontal;
            view.Children.Add(plugins);
            view.Children.Add(controls);
            return view;
        }

      */
    }
}
