using ECPU.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using static ECPU.Utils;

namespace ECPU.Settings
{
    public class TextItem : CPWindowItem
    {

      
        public TextItem(string name, string text, string desc, int position) : base(name, text, desc, position){

           
        }

       


 

        public override StackPanel getView()
        {
            TextBlock tb = new TextBlock();
            tb.Text = _text;
            tb.FontSize = fontSize;
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                tb.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                tb.Foreground = Brushes.Black;
            }
            
            tb.TextAlignment = TextAlignment.Justify;
            tb.TextWrapping = TextWrapping.WrapWithOverflow;
            tb.Padding = new Thickness(20);
            ScrollViewer myScrollViewer = new ScrollViewer();
            myScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                myScrollViewer.Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                myScrollViewer.Foreground = Brushes.Black;
            }
           
            myScrollViewer.Content = tb;
            
            myScrollViewer.Height = 400;
          
            StackPanel view = new StackPanel();
              
                view.Children.Add(myScrollViewer);

            return view;



     




    }
      

        }
    }


