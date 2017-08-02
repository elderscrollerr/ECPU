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
using System.Globalization;

namespace ECPU.Settings
{
    public class INI_TRACKBAR_Setting : INI_Setting
    {
        private double[] trackbarrange;
        private string isReverse;
        private Label l;
        public INI_TRACKBAR_Setting(string name, string text, string desc, int position, string defaultValue, string iniFilepath, string iniSection, string iniKey, string min, string max, string freq, string reverse) : base(name, text, desc, position, defaultValue, iniFilepath, iniSection, iniKey)
        {

            trackbarrange = new double[3];
                  trackbarrange[0] = double.Parse(min, CultureInfo.InvariantCulture);
            trackbarrange[1] = double.Parse(max, CultureInfo.InvariantCulture); 
            trackbarrange[2] = double.Parse(freq, CultureInfo.InvariantCulture);
            isReverse = reverse;
        }

     
        private void slider_Move(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _currentValue = ((sender as Slider).Value).ToString();

            if (_currentValue.Contains(','))
            {
                _currentValue = _currentValue.Replace(',', '.');
            }
                    change(_currentValue);
               
           
            

            l.Content = _text + ": " + _currentValue;
        }

   

        public override StackPanel getView()
        {
            StackPanel view = new StackPanel();
            Slider c = new Slider();
            if (isReverse.Equals("reverse"))
            {
                c.IsDirectionReversed = true;
            }
            
            c.Margin = new Thickness(10, 10, 10, 0);
            
            c.IsSnapToTickEnabled = true;
            c.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.TopLeft;
            c.TickFrequency = trackbarrange[2];
            c.Maximum = trackbarrange[1];
            c.Minimum = trackbarrange[0];
            c.Value = double.Parse(_currentValue, CultureInfo.InvariantCulture);
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                c.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                c.Foreground = Brushes.Black;
            }
           
            view.Children.Add(c);

            l = new Label();
         //   VISUAL_STYLES.STYLE_SETTING_LABEL(l);
            l.Content = _text + ": " + _currentValue;

            view.Children.Add(l);
            view.Width = width;
            view.Height = height;
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

           
          
           

            (c as Slider).ValueChanged += slider_Move;
            return view;
        }

   
    }
}
