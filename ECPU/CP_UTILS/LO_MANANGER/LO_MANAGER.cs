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


namespace ECPU.LoadOrderUtility
{
    public class LO_MANAGER : CPWindow
    {
        public enum CONTROLS_ACTIONS { UP, DOWN, RESET };
   
        private static List<PluginInList> LO;
      public static int ACTIVE_PLUGINS_LIMIT = 255;
        public static int currentActivePlugins;
        public static PluginInList currentShifted;

        public LO_MANAGER()
        {                     
        }
       
        protected override Grid buildContent()
        {
            currentActivePlugins = INIT.MASTER_FILES_ESM.Length;
            buildLO();
            StackPanel list = getList();
            StackPanel controls = getControls();
            Grid content = new ContentArea();
            content.RowDefinitions.Add(new RowDefinition());
            content.ColumnDefinitions.Add(new ColumnDefinition());
            content.ColumnDefinitions.Add(new ColumnDefinition());
            content.Children.Add(list);
            Grid.SetRow(list, 0);
            Grid.SetColumn(list, 0);
           content.Children.Add(controls);
           Grid.SetRow(controls, 0);
           Grid.SetColumn(controls, 1);
          
            return content;
          
        }
        public static void buildLO()
        {
            PluginInList esp;
            LO = new List<PluginInList>();

            for (int number = 0; number < INIT.MASTER_FILES_ESM.Length; number++)
            {
                if (!File.Exists(INIT.DATA_DIR + INIT.MASTER_FILES_ESM[number]))
                {
                    throw new CriticalFileNotFoundException(INIT.MASTER_FILES_ESM[number]);
                }
                else
                {
                    LO.Add(new PluginInList(number, INIT.MASTER_FILES_ESM[number], true));
                }
            }
            List<string> linesInTextFile = FileManager.GetContentAsLines(INIT.PLUGINS_TXT_PATH).Cast<string>().ToList();


            foreach (string vanillamaster in INIT.MASTER_FILES_ESM)
            {
                linesInTextFile.RemoveAll(item => item.Equals(vanillamaster) || item.Equals("*" + vanillamaster));
            }

            //  linesInTextFile.RemoveAll(item => !item.Substring(item.Length - 4).Equals(".esp") || !item.Substring(item.Length - 4).Equals(".esm"));

            int lastAddedNumber = INIT.MASTER_FILES_ESM.Length - 1;


            foreach (string line in linesInTextFile)
            {
                line.Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if (line.Substring(line.Length - 4).Equals(".esp") || line.Substring(line.Length - 4).Equals(".esm"))
                {
                    //  MessageBox.Show(line);
                    ++lastAddedNumber;
                    if (File.Exists(INIT.GAME_ROOT + @"Data\" + line) || line[0].CompareTo('*') == 0 && File.Exists(INIT.GAME_ROOT + @"Data\" + line.Substring(1, line.Length - 1)))
                    {

                        if (line[0].CompareTo('*') == 0)
                        {
                            esp = new PluginInList(lastAddedNumber, line.Substring(1, line.Length - 1), false);
                        }
                        else
                        {
                            esp = new PluginInList(lastAddedNumber, line, false);
                        }



                        if (INIT.CURRENT_GAME.Equals(INIT.GAMES.OP) || (INIT.CURRENT_GAME.Equals(INIT.GAMES.ESSE) && line[0].CompareTo('*') == 0))
                        {
                            esp.activate();
                        }

                        LO.Add(esp);
                    }

                }

            }
            var pluginsExtensions = new[] { ".esm", ".esp" };
            List<string> filesFromDataDirectory = Directory
                .GetFiles(INIT.DATA_DIR)
                .Where(file => pluginsExtensions.Any(file.ToLower().EndsWith))
                .ToList();
            foreach (string vanillamaster in INIT.MASTER_FILES_ESM)
            {
                filesFromDataDirectory.RemoveAll(item => Path.GetFileName(item).Equals(vanillamaster));
            }
            foreach (string file in filesFromDataDirectory)
            {
                string filename = Path.GetFileName(file);
                //  MessageBox.Show(filename);
                if (!linesInTextFile.Contains(filename) && !linesInTextFile.Contains("*" + filename))
                {
                    ++lastAddedNumber;
                    esp = new PluginInList(lastAddedNumber, filename, false);
                    LO.Add(esp);
                }
            }

            // pluginsAmount = lastAddedNumber;

            //  LO = LO.OrderBy(o => o.getNumber()).ToList();
           
        }

        public static void writeLOInFile()
        {
            ArrayList LOForWriteinFile = new ArrayList();

            if (INIT.CURRENT_GAME.Equals(INIT.GAMES.OP))
            {
                LOForWriteinFile.Add("# This file is used to tell Oblivion which data files to load.");
                LOForWriteinFile.Add(Environment.NewLine);

                foreach (PluginInList plugin in LO)
                {
                    if (plugin.getActivity())
                    {
                        LOForWriteinFile.Add(plugin.getTitle());
                    }

                }

            }
            else
            {
                if (INIT.CURRENT_GAME.Equals(INIT.GAMES.ESSE))
                {
                    LOForWriteinFile.Add("# This file is used by Skyrim to keep track of your downloaded content.");
                    LOForWriteinFile.Add("# Please do not modify this file.");

                    foreach (PluginInList plugin in LO)
                    {
                        if (!plugin.ismaster())
                        {
                            if (plugin.getActivity())
                            {
                                LOForWriteinFile.Add("*" + plugin.getTitle());
                            }
                            else
                            {
                                LOForWriteinFile.Add(plugin.getTitle());
                            }

                        }

                    }
                }
            }


            FileManager.WriteAllLines(INIT.getpath("plugins_txt"), LOForWriteinFile);

        }


        public static void changeLO(CONTROLS_ACTIONS action)
        {

            if (action.Equals(CONTROLS_ACTIONS.RESET))
            {

                try
                {
                    FileManager.remove(INIT.PLUGINS_TXT_PATH);
                    FileManager.copyFiles(INIT.getpath("plugins_txt_backup"), INIT.PLUGINS_TXT_PATH);
                   
                }
                catch
                {
                    MessageBox.Show("Не удалось провести сброс");
                }


            }else
            {
                if (currentShifted != null)
                {

                    PluginInList current = LO.Find(o => o.getTitle().Equals(currentShifted.getTitle()));
                    int pos = LO.IndexOf(current);

                    if (action.Equals(CONTROLS_ACTIONS.UP) && (pos == INIT.MASTER_FILES_ESM.Length))
                    {
                        return;
                    }

                    PluginInList tmp;

                    switch (action)
                    {
                        case CONTROLS_ACTIONS.UP:

                            tmp = LO[pos];

                            LO[pos] = LO[pos - 1];

                            LO[pos - 1] = tmp;

                            break;
                        case CONTROLS_ACTIONS.DOWN:
                            tmp = LO[pos];
                            LO[pos] = LO[pos + 1];
                            LO[pos + 1] = tmp;
                            break;

                        default:
                            break;
                    }
                    writeLOInFile();

                }
            }
           
        }


        private StackPanel getList()
        {
            StackPanel plugins = new StackPanel();

            Grid grid = new Grid();       
            ColumnDefinition c1 = new ColumnDefinition();   
            grid.ColumnDefinitions.Add(c1);


            foreach (PluginInList plugin in LO)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }


            for (int i = 0; i < LO.Count; i++)
            {

                DockPanel ch = LO[i].getView();
                grid.Children.Add(ch);
                Grid.SetRow(ch, i);
            }
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                plugins.Background = new ImageBrush(STYLE.BUTTON);
              
            }
            else
            {
                plugins.Background = Brushes.White;
            }


            plugins.Margin = new Thickness(10);       
            plugins.Children.Add(grid);
            return plugins;

        }

        private StackPanel getControls()
        {
            StackPanel controls = new StackPanel();
            controls.Orientation = Orientation.Horizontal;
            controls.VerticalAlignment = VerticalAlignment.Top;


            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());


            TextBlock up = new TextBlock();
            up.Text = '\u25b2'.ToString();
            up.Tag = CONTROLS_ACTIONS.UP;
            up.FontSize = 20.0;
            (up as TextBlock).MouseDown += act;

            TextBlock down = new TextBlock();
            down.Text = '\u25bc'.ToString();
            down.Tag = CONTROLS_ACTIONS.DOWN;
            down.FontSize = 20.0;
            (down as TextBlock).MouseDown += act;

            TextBlock reset = new TextBlock();
            reset.Text = "СБРОС"; reset.Tag = CONTROLS_ACTIONS.RESET;
            reset.FontSize = 12.0;
            (reset as TextBlock).MouseDown += act;

            controls.Margin = new Thickness(10);
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                controls.Background = new ImageBrush(STYLE.BUTTON);
                up.Foreground = STYLE.MAIN_MENU_FOREGROUND;
                down.Foreground = STYLE.MAIN_MENU_FOREGROUND;
                reset.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                up.Foreground = Brushes.Black;
                down.Foreground = Brushes.Black;
                reset.Foreground = Brushes.Black;
                controls.Background = Brushes.White;
            }

            up.FontWeight = FontWeights.Bold;
            up.Width = 30.0;
            up.Height = 30.0;
            up.TextAlignment = TextAlignment.Center;

            down.FontWeight = FontWeights.Bold;
            down.Width = 30.0;
            down.Height = 30.0;
            down.TextAlignment = TextAlignment.Center;

            reset.Margin = new Thickness(0, 15, 0, 0);
            reset.FontWeight = FontWeights.Bold;
            reset.Width = 50.0;
            reset.Height = 30.0;
            reset.TextAlignment = TextAlignment.Center;
            //  topButton.VerticalAlignment = VerticalAlignment.Center;
            //  (topButton as TextBlock).MouseDown += act;


            Grid.SetRow(up, 0);
            Grid.SetColumn(up, 0);

            Grid.SetRow(down, 0);
            Grid.SetColumn(down, 1);

            Grid.SetRow(reset, 1);
            Grid.SetColumnSpan(reset, 2);

            grid.Children.Add(up);
            grid.Children.Add(down);
            grid.Children.Add(reset);
            controls.Children.Add(grid);

            return controls;
           
        }

        private void act(object sender, MouseButtonEventArgs e)
        {
            changeLO((CONTROLS_ACTIONS)(sender as TextBlock).Tag);

            //   CPWindow window = (CPWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            allWindow.Children.RemoveAt(1);
            allWindow.Children.Add(buildContent());
           // top = new TopArea();
          //  content = new LO_MANAGER().buildContent();

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
