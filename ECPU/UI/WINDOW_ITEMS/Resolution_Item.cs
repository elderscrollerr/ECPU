using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ECPU.Settings
{
    public class Resolution_Item : INI_COMBOBOX_Setting
    {
        private List<string> comboboxOptions;
        private int[] currentWindowsResolution;
        private int[] currentResolutionInINIFile;
        private bool currentResolHasInList;
        private Label l;
        private bool desktopCurrentNowInIni;
        private string keyWidth;
        private string keyHeight;
        private CheckBox sysRes;
        ComboBox c;
        public Resolution_Item(string name, string text, string desc, int position, string defaultValue, string iniFilepath, string iniSection, string iniKey, string comboBoxItems) : base(name, text, desc, position, defaultValue, iniFilepath, iniSection, iniKey, comboBoxItems)
        {

            List<string> keys = iniKey.Split(',').ToList<string>();
           
            keyWidth = keys[0];
            keyHeight = keys[1];

            currentWindowsResolution = new int[2] {
            Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenWidth),
            Convert.ToInt32(System.Windows.SystemParameters.PrimaryScreenHeight)
        };

            
            currentResolutionInINIFile = new int[2] {
             Convert.ToInt32(new IniManager(_iniFilepath).getValueByKey(keyWidth)),
             Convert.ToInt32(new IniManager(_iniFilepath).getValueByKey(keyHeight)),
        };

            if (currentWindowsResolution[0] == currentResolutionInINIFile[0] && currentWindowsResolution[1] == currentResolutionInINIFile[1])
            {
                desktopCurrentNowInIni = true;
            }
            else
            {
                desktopCurrentNowInIni = false;
            }

            List<string> resolutions = comboBoxItems.Split(',').ToList<string>();
            resolutions.Reverse();


            List<int[]> resolutionsList = new List<int[]>();

            foreach (string resolution in resolutions)
            {
                List<string> resolutionEntry = resolution.Split('-').ToList<string>();
              

                int width = Convert.ToInt32(resolutionEntry[0]);
                int height = Convert.ToInt32(resolutionEntry[1]);
                int[] resol = new int[] { width, height };
                resolutionsList.Add(resol);
            }

            currentResolHasInList = false;


            comboboxOptions = new List<string>();
            foreach (int[] element in resolutionsList)
            {
                if (element[0] == currentWindowsResolution[0] && element[1] == currentWindowsResolution[1])
                {
                    currentResolHasInList = true;
                    comboboxOptions.Add(element[0].ToString() + " X " + element[1].ToString());
                }
                else
                {
                    
                    comboboxOptions.Add(element[0].ToString() + " X " + element[1].ToString());
                }
                
            }

            if (!currentResolHasInList)
            {

                comboboxOptions.Add(currentWindowsResolution[0].ToString() + " X " + currentWindowsResolution[1].ToString());
            }
            
        }

        public override StackPanel getView()
        {
            StackPanel view = new StackPanel();
            c = new ComboBox();
            c.ItemsSource = comboboxOptions;
            c.SelectedValue = c.SelectedValue = currentResolutionInINIFile[0].ToString() + " X " + currentResolutionInINIFile[1].ToString();
            c.VerticalAlignment = VerticalAlignment.Center;
            view.Children.Add(c);
            c.Margin = new Thickness(10, 10, 10, 0);
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                c.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                c.Foreground = Brushes.Black;
            }
           
        
         
            l = new Label();
            l.Content = _text;
            view.Children.Add(l);
            l.FontSize = fontSize;
            l.HorizontalAlignment = HorizontalAlignment.Center;
            l.VerticalAlignment = VerticalAlignment.Center;
            
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                l.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                l.Foreground = Brushes.Black;
            }




            sysRes = new CheckBox() ;
            if (desktopCurrentNowInIni)
            {
                sysRes.IsChecked = true;
            }
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                sysRes.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                sysRes.Foreground = Brushes.Black;
            }
            
            sysRes.Click += sysRes_Click;
            sysRes.Content = "Системное";
            sysRes.HorizontalAlignment = HorizontalAlignment.Center;
            sysRes.VerticalAlignment = VerticalAlignment.Center;
            view.Children.Add(sysRes);


            view.Width = width;
            view.Height = height;
          
           
            view.HorizontalAlignment = HorizontalAlignment.Center;
            view.VerticalAlignment = VerticalAlignment.Center;
            
           
            (c as ComboBox).SelectionChanged += comboChange;
            return view;
        }

        private void sysRes_Click(object sender, RoutedEventArgs e)
        {
            if (!desktopCurrentNowInIni)
            {
                new IniManager(_iniFilepath).setParameter(keyWidth, currentWindowsResolution[0].ToString(), _iniSection);
                new IniManager(_iniFilepath).setParameter(keyHeight, currentWindowsResolution[1].ToString(), _iniSection);
                c.SelectedValue = currentWindowsResolution[0].ToString() + " X " + currentWindowsResolution[1].ToString();
            }
        }

        private void comboChange(object sender, SelectionChangedEventArgs e)
        {
            string selected = (sender as ComboBox).SelectedItem.ToString();
            if (!selected.Equals(currentWindowsResolution[0].ToString() + " X " + currentWindowsResolution[1].ToString()))
            {
                sysRes.IsChecked = false;
                new IniManager(_iniFilepath).setParameter(keyWidth, (selected.Split('X')[0]).Trim(), _iniSection);
                new IniManager(_iniFilepath).setParameter(keyHeight, (selected.Split('X')[1]).Trim(), _iniSection);
            }
            else
            {
                sysRes.IsChecked = true;
                new IniManager(_iniFilepath).setParameter(keyWidth, (selected.Split('X')[0]).Trim(), _iniSection);
                new IniManager(_iniFilepath).setParameter(keyHeight, (selected.Split('X')[1]).Trim(), _iniSection);
            }
        }


    }
}
