using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;
using static ECPU.INIT;
using System.IO;
using System.Windows.Threading;
using ECPU.Exceptions;
using ECPU.WINDOW_ITEMS;
using ECPU.LoadOrderUtility;

namespace ECPU.UI
{
  public class CPWindow : System.Windows.Window
    {
        public string windowTitle;
        protected TopArea top;
        protected Grid content;
        protected StackPanel allWindow;


        public CPWindow(string _windowTitle)
        {
            windowTitle = _windowTitle;
            Closing += closing;
            setVisualStyle();


            top = new TopArea();
            content = buildContent();


            setContent();


        }

        public CPWindow()
        {
            Closing += closing;
            setVisualStyle();


            top = new TopArea();
            content = buildContent();
            

            setContent();

        }

        protected virtual Grid buildContent()
        {
            if (!string.IsNullOrEmpty(windowTitle))
            {
               return new ContentArea(windowTitle);
            }else
            {
                return null;
            }
           
        }



        protected void setContent()
        {
            BorderThickness = new Thickness(2);
            Width = content.Width;
            Height = content.Height + top.Height;



            allWindow = new StackPanel();
            allWindow.Children.Add(top);
            allWindow.Children.Add(content);
            Content = allWindow;

            SizeToContent = SizeToContent.WidthAndHeight;
            ResizeMode = ResizeMode.NoResize;
            MouseDown += new System.Windows.Input.MouseButtonEventHandler(Window_MouseDown);
            WindowStyle = WindowStyle.None;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            LocationChanged += ParentLocationChanged;
        }


        private void setVisualStyle()
        {
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                Icon = BitmapFrame.Create(new Uri(STYLE.ICON_IMAGE_PATH, UriKind.RelativeOrAbsolute));
                Background = new ImageBrush(STYLE.BG);
                BorderBrush = STYLE.MAIN_MENU_BORDER_COLOR;
                if (!MusicPlayer.STOPPED_MANUALLY)
                {
                    MusicPlayer.Play();
                }

            }
            else
            {
                Background = Brushes.Gray;
            }
        }

     



        private void closing(object sender, CancelEventArgs e)
        {
            if (windowTitle!=null && windowTitle.Equals("WINDOW_MAINMENU"))
            {
                if (File.Exists(UPDATER.TEMP_DB_SQLITE_FILE))
                {
                    FileManager.remove(UPDATER.TEMP_DB_SQLITE_FILE);
                }
                Logger.addLine(true, "Завершение приложения");
                Application.Current.Shutdown();
            }
            

        }

        private void ParentLocationChanged(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

   

    }

  
}
