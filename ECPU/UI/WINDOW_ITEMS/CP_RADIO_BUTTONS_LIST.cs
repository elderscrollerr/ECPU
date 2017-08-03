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

namespace ECPU.UI.WINDOW_ITEMS
{
    public abstract class CP_RADIO_BUTTONS_LIST_ELEMENT : CPWindowItem
    {


        public static double maxWidth;
        private string link;
        protected RadioButton rb;
        protected static CP_RADIO_BUTTONS_LIST_ELEMENT currentpreset;
       

        public CP_RADIO_BUTTONS_LIST_ELEMENT(int position, string name, string path, string _link, string desc) : base(name, path, desc, position)
        {
                link = _link;
        }

   

        public abstract void enablePreset();
        public abstract void disablePreset();

        public override StackPanel getView()
        {
            StackPanel view = new StackPanel();
                         
                Grid rblistgrid = new Grid();

                rblistgrid.ColumnDefinitions.Add(new ColumnDefinition());
                rblistgrid.ColumnDefinitions.Add(new ColumnDefinition());
                rblistgrid.RowDefinitions.Add(new RowDefinition());

                rb = new RadioButton();
                rb.Content = _name;
                rb.ToolTip = _description;



                if (!INIT.DEFAULT_VISUAL_STYLE)
                {
                    rb.Foreground = STYLE.MAIN_MENU_FOREGROUND;
                }
                else
                {
                    rb.Foreground = Brushes.Black;
                }
                if (_name.Equals(INIT.CURRENT_ENB_OPTION) || _name.Equals(INIT.CURRENT_SHADER_OPTION))
                {
                    rb.IsChecked = true;
                }
                TextBlock textBlock = new TextBlock();
                if (!INIT.DEFAULT_VISUAL_STYLE)
                {
                    textBlock.Foreground = STYLE.MAIN_MENU_FOREGROUND;
                }
                else
                {
                    textBlock.Foreground = Brushes.Black;
                }

                textBlock.FontSize = fontSize;
                Hyperlink link = new Hyperlink();
                link.Click += link_Click;
                link.Inlines.Add("Видео");
                textBlock.Inlines.Add(link);
                rb.Checked += new RoutedEventHandler(act);
                rb.Margin = new Thickness(10, 10, 10, 10);
                textBlock.Margin = new Thickness(10, 10, 10, 10);

                Grid.SetRow(rb, 0);
                Grid.SetColumn(rb, 0);
                Grid.SetRow(textBlock, 0);
                Grid.SetColumn(textBlock, 1);
                rblistgrid.Children.Add(rb);
                rblistgrid.Children.Add(textBlock);


                view.Children.Add(rblistgrid);



                view.Width = width;
                view.Height = height;
                view.HorizontalAlignment = HorizontalAlignment.Center;
                view.VerticalAlignment = VerticalAlignment.Center;
                return view;
        }

        private void act(object sender, RoutedEventArgs e)
        {

            if (currentpreset!=null)
            {
                currentpreset.disablePreset();
                currentpreset = this;

                currentpreset.enablePreset();

            }
           

          

        }

        private void link_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(link))
            {
                System.Diagnostics.Process.Start(link);
            }


        }

    }
}
