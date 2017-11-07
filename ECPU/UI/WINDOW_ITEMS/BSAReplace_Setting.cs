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
   public class BSAReplace_Setting : INI_Setting
    {
        private static List<string> currentBSAFilesInINI;
        private string[] _optionOnBSA;
        private string[] _optionOffBSA;

        public BSAReplace_Setting(string name, string text, string desc, int position, string iniFilepath, string iniSection, string iniKey, string[] optionOnBSA, string[] optionOffBSA) : base(name, text, desc, position, null, iniFilepath, iniSection, iniKey)
        {
            _optionOnBSA = optionOnBSA;
            _optionOffBSA = optionOffBSA;

            //_bsa = bsa;
            currentBSAFilesInINI = new IniManager(_iniFilepath).getValueByKey(_iniKey, _iniSection).Split(',').ToList<string>();
            for (int i = 0; i < currentBSAFilesInINI.ToArray().Length; i++)
            {
                currentBSAFilesInINI[i] = currentBSAFilesInINI[i].Trim();
            }
            applyToINI();

            foreach (string bsa in _optionOnBSA)
            {
                if (currentBSAFilesInINI.Contains(bsa))
                {
                    _CurrentState = true;
                }else
                {
                    _CurrentState = false;
                    break;

                }
            }

            if (!_CurrentState)
            {
                foreach (string bsa in _optionOffBSA)
                {
                    if (currentBSAFilesInINI.Contains(bsa))
                    {
                        _CurrentState = false;
                    }
                    else
                    {
                        
                        Logger.addLine(false, "ini файл поврежден. Сделайте сброс настроек.");
                        return;
                    }
                }
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
            
          

            (c as CheckBox).Click += act;
            return view;
        }


        public void act(object sender, RoutedEventArgs e)
        {
            if (_CurrentState)
            {
                for (int i =0; i<_optionOnBSA.Length; i++ )
                {
                    if (_optionOnBSA.Length >= _optionOffBSA.Length)
                    {
                        if (currentBSAFilesInINI.Contains(_optionOnBSA[i]))
                        {
                           int index = currentBSAFilesInINI.IndexOf(_optionOnBSA[i]);
                            
                            currentBSAFilesInINI[index] = _optionOffBSA[i];
                        }
                        else
                        {
                            Logger.addLine(false, "ini файл поврежден. Сделайте сброс настроек.");
                            return;
                        }
                    }else
                    {
                        if (_optionOnBSA.Length < _optionOffBSA.Length)
                        {
                            int index = currentBSAFilesInINI.IndexOf(_optionOnBSA[i]);
                            currentBSAFilesInINI[index] = _optionOffBSA[i];
                            int lastbsa = _optionOffBSA.Length - _optionOnBSA.Length;
                            for(int j=0; j<lastbsa; j++){
                                currentBSAFilesInINI.Add(_optionOffBSA[_optionOffBSA.Length - 1-j]);
                            }
                        }
                       
                    }

                 
                }
                _CurrentState = false;
            }
            else
            {
                for (int i = 0; i < _optionOffBSA.Length; i++)
                {
                    if (_optionOffBSA.Length >= _optionOnBSA.Length)
                    {
                        if (currentBSAFilesInINI.Contains(_optionOffBSA[i]))
                        {
                            int index = currentBSAFilesInINI.IndexOf(_optionOffBSA[i]);
                            MessageBox.Show(_optionOffBSA[i]);
                            currentBSAFilesInINI[index] = _optionOnBSA[i];
                        }
                        else
                        {
                            Logger.addLine(false, "ini файл поврежден. Сделайте сброс настроек.");
                            return;
                        }
                    }
                    else
                    {
                        if (_optionOffBSA.Length < _optionOnBSA.Length)
                        {
                            int index = currentBSAFilesInINI.IndexOf(_optionOffBSA[i]);
                            currentBSAFilesInINI[index] = _optionOnBSA[i];
                            int lastbsa = _optionOnBSA.Length - _optionOffBSA.Length;
                            for (int j = 0; j < lastbsa; j++)
                            {
                                currentBSAFilesInINI.Add(_optionOnBSA[_optionOnBSA.Length - 1 - j]);
                            }
                        }

                    }


                }
                _CurrentState = true;
            }
            applyToINI();
        }


    }
}

