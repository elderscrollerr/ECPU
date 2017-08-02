using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ECPU.UI;

namespace ECPU.Settings
{
    public class INI_COMBOBOX_Setting : INI_Setting
    {
        private List<string> comboboxOptions;
        private Label l;
        public INI_COMBOBOX_Setting(string name, string text, string desc, int position, string defaultValue, string iniFilepath, string iniSection, string iniKey, string comboBoxItems) : base(name, text, desc, position, defaultValue, iniFilepath, iniSection, iniKey)
        {
            comboboxOptions = new List<string>();
            comboboxOptions = comboBoxItems.Split(',').Reverse().ToList();
        }

    

     

        public override StackPanel getView()
        {
            StackPanel view = new StackPanel();
            ComboBox c = new ComboBox();
          
            c.ItemsSource = comboboxOptions;
            c.SelectedValue = _currentValue;
            c.VerticalAlignment = VerticalAlignment.Center;
            view.Children.Add(c);
            c.Margin = new Thickness(10, 10, 10, 0);
            l = new Label();

            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                c.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                c.Foreground = Brushes.Black;
            }
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
         
            view.Width = width;
                view.Height = height;

            view.HorizontalAlignment = HorizontalAlignment.Center;
            view.VerticalAlignment = VerticalAlignment.Center;
     
            (c as ComboBox).SelectionChanged += comboChange;
            return view;
        }

     

        private void comboChange(object sender, SelectionChangedEventArgs e)
        {
            _currentValue = (sender as ComboBox).SelectedItem.ToString();
            change(_currentValue);
        }

   
    }
}
