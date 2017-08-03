using ECPU.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ECPU.LoadOrderUtility
{
   public class PluginInList
    {

      //  private int numberInOrder;
       private bool isMaster;
        private string filename;
        private bool isActive;
        public Label pluginname;
        public DockPanel view;
      private bool isShifted;
     //   public static PluginInList currentShifted;
      
        public PluginInList(int _numberInOrder, string _filename, bool _ismaster)
        {
            LO_MANAGER.currentShifted = null;
            
            //   numberInOrder = _numberInOrder;
            filename = _filename;
            isMaster = _ismaster;
            if (isMaster)
            {
                isActive = true;
            }
           
        }

        public void activate()
        {
            if (LO_MANAGER.currentActivePlugins<LO_MANAGER.ACTIVE_PLUGINS_LIMIT)
            {
                isActive = true;
                LO_MANAGER.currentActivePlugins = LO_MANAGER.currentActivePlugins + 1;
            }
           
        
        }
        public void deactivate()
        {
            isActive = false;
            LO_MANAGER.currentActivePlugins = LO_MANAGER.currentActivePlugins - 1;
        }
        public string getTitle()
        {
            return filename;
        }

        public bool getActivity()
        {
            return isActive;
        }
        public bool ismaster()
        {
            return isMaster;
        }

        public DockPanel getView()
        {


            view = new DockPanel();
           // view.Orientation = Orientation.Horizontal;
           
            CheckBox c = new CheckBox();
            c.VerticalAlignment = VerticalAlignment.Center;
            c.Margin = new Thickness(10, 0, 5, 0);
            c.IsChecked = isActive;
           // view.Foreground = STYLE.MAIN_MENU_BORDER_COLOR;
            if (isMaster)
            {
                c.IsEnabled = false;
            }else
            {
                (c as CheckBox).Click += act;
            }
       


            pluginname = new Label();
            pluginname.Margin = new Thickness(0, 0, 5, 0);
            pluginname.Content = filename;
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                pluginname.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                pluginname.Foreground = Brushes.Black;
            }
           
            pluginname.HorizontalAlignment = HorizontalAlignment.Stretch;
            pluginname.MouseDown += focusOnPluginLine;
            view.HorizontalAlignment = HorizontalAlignment.Stretch;
            DockPanel.SetDock(c, Dock.Left);
            DockPanel.SetDock(pluginname, Dock.Right);
           
                if (INIT.CURRENT_SHIFTED_PLUGIN_IN_LO.Equals(filename))
                {
                LO_MANAGER.currentShifted = this;

                if (!INIT.DEFAULT_VISUAL_STYLE)
                {
                    view.Background = new ImageBrush(STYLE.BG);
                    pluginname.Foreground = STYLE.MAIN_MENU_BG_CONTRAST_COLOR;


                }
                else
                {
                    view.Background = Brushes.Black;
                    pluginname.Foreground = Brushes.White;
                }


               
                }
           
            view.Children.Add(c);
            view.Children.Add(pluginname);
           
            return view;
        }

        private void act(object sender, RoutedEventArgs e)
        {
            if (isActive)
            {
                deactivate();
              
            }else
            {
                activate();
               
            }
            LO_MANAGER.writeLOInFile();
           CPWindow window = (CPWindow)Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
           // window.allWindow.Children.RemoveAt(1);
        //    window.allWindow.Children.Add(new LO_MANAGER().getcontent());
        }

        private void focusOnPluginLine(object sender, RoutedEventArgs e)
        {
            if (!isMaster)
            {
                if (LO_MANAGER.currentShifted != null)
                {
                    if (!INIT.DEFAULT_VISUAL_STYLE)
                    {
                        LO_MANAGER.currentShifted.view.ClearValue(StackPanel.BackgroundProperty);
                        //  LoadOrderManager.currentShifted.pluginname.ClearValue(Label.ForegroundProperty);
                        LO_MANAGER.currentShifted.pluginname.Foreground = STYLE.MAIN_MENU_FOREGROUND;

                    }
                    else
                    {
                        LO_MANAGER.currentShifted.view.ClearValue(StackPanel.BackgroundProperty);
                        //  LoadOrderManager.currentShifted.pluginname.ClearValue(Label.ForegroundProperty);
                        LO_MANAGER.currentShifted.pluginname.Foreground = Brushes.Black;
                    }


                    LO_MANAGER.currentShifted.isShifted = false;
                }

                if (!INIT.DEFAULT_VISUAL_STYLE)
                {
                    view.Background = new ImageBrush(STYLE.BG);
                    pluginname.Foreground = STYLE.MAIN_MENU_BG_CONTRAST_COLOR;
                 

                }
                else
                {
                    view.Background = Brushes.Black;
                    pluginname.Foreground = Brushes.White;
                }
                


                isShifted = true;
                LO_MANAGER.currentShifted = this;
                INIT.CURRENT_SHIFTED_PLUGIN_IN_LO = LO_MANAGER.currentShifted.filename;
            }
           
        }

     

    }
}
