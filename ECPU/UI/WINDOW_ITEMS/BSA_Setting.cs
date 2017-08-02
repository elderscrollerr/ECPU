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
    public class BSA_Setting : INI_Setting
    {
        //    private string _checkboxOnString, _checkboxOffString;
        private static List<string> currentBSAFilesInINI;
        private string _bsa;

        public BSA_Setting(string name, string text, string desc, int position, string iniFilepath, string iniSection, string iniKey, string bsa) : base(name, text, desc, position, null, iniFilepath, iniSection, iniKey)
        {
            _bsa = bsa;
            currentBSAFilesInINI = new IniManager(_iniFilepath).getValueByKey(_iniKey).Split(',').ToList<string>();
            for (int i = 0; i < currentBSAFilesInINI.ToArray().Length; i++)
            {
                currentBSAFilesInINI[i] = currentBSAFilesInINI[i].Trim();
            }
            applyToINI();

            if (currentBSAFilesInINI.Contains(bsa))
            {
                _CurrentState = true;
            }
            else
            {
                _CurrentState = false;
            }
        }
        private void applyToINI()
        {
            if (currentBSAFilesInINI.Count > 0)
            {
                new IniManager(_iniFilepath).setParameter(_iniKey, string.Join(",", currentBSAFilesInINI.ToArray()), _iniSection);
            }
        }

        public override StackPanel getView()
        {
            StackPanel view = new StackPanel();

            TextBlock tb = new TextBlock();
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            tb.FontSize = fontSize;
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                tb.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                tb.Foreground = Brushes.Black;
            }
            
            tb.TextWrapping = TextWrapping.WrapWithOverflow;
            tb.HorizontalAlignment = HorizontalAlignment.Center;
            CheckBox c = new CheckBox();
            c.IsChecked = _CurrentState;
            tb.Text = _text;
            c.HorizontalAlignment = HorizontalAlignment.Center;
            c.VerticalAlignment = VerticalAlignment.Center;
            c.Content = tb;
            c.Margin = new Thickness(10, 10, 10, 10);

            view.HorizontalAlignment = HorizontalAlignment.Center;
            view.VerticalAlignment = VerticalAlignment.Center;
            view.Children.Add(c);
            view.Width = width;
            view.Height = height;
           
         

            (c as CheckBox).Click += act;
            return view;
        }


        public void act(object sender, RoutedEventArgs e)
        {
            if (_CurrentState)
            {
                if (currentBSAFilesInINI.Contains(_bsa))
                {
                    currentBSAFilesInINI.Remove(_bsa);
                    _CurrentState = false;
                }

            }
            else
            {
                if (!currentBSAFilesInINI.Contains(_bsa))
                {
                    currentBSAFilesInINI.Add(_bsa);
                    _CurrentState = true;
                }
            }
            applyToINI();
        }


    }
}
