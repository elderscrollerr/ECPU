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
    public class INI_CHECKBOX_Setting : INI_Setting
    {
        private string _checkboxOnString, _checkboxOffString;
        public INI_CHECKBOX_Setting(string name, string text, string desc, int position, string defaultValue, string iniFilepath, string iniSection, string iniKey, string checkboxOffString, string checkboxOnString) : base(name, text, desc, position, defaultValue, iniFilepath, iniSection, iniKey)
        {

            _checkboxOnString = checkboxOnString;
            _checkboxOffString = checkboxOffString;
            if (_currentValue.Equals(_checkboxOnString))
            {
                _CurrentState = true;
            }
            else
            {
                if (_currentValue.Equals(_checkboxOffString))
                {
                    _CurrentState = false;
                }
            }
        }


        public override StackPanel getView()
        {
            StackPanel view = new StackPanel();

            TextBlock tb = new TextBlock();
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
    
            tb.TextWrapping = TextWrapping.WrapWithOverflow;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            CheckBox c = new CheckBox();
            c.IsChecked = _CurrentState;
            tb.Text = _text;
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                tb.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                tb.Foreground = Brushes.Black;
            }
           
            tb.FontSize = fontSize;
            c.HorizontalAlignment = HorizontalAlignment.Center;
            c.VerticalAlignment = VerticalAlignment.Center;
            c.Content = tb;
           c.Margin = new Thickness(10, 10, 10, 10);

            view.HorizontalAlignment = HorizontalAlignment.Center;
            view.VerticalAlignment = VerticalAlignment.Center;
            view.Children.Add(c);
                view.Width = width;
                view.Height = height;
           
            
               // view.Children.Add(description);
        //    }

            (c as CheckBox).Click += act;
            return view;
        }


        public void act(object sender, RoutedEventArgs e)
        {
            if (_CurrentState)
            {
                change(_checkboxOffString);
                _CurrentState = false;
            }
            else
            {
                change(_checkboxOnString);
                _CurrentState = true;
            }
        }

      
    }
}