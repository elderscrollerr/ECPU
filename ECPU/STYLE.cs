using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ECPU
{
  public static class STYLE
    {
        private static int styleNumber;
        public static string BG_IMAGE_PATH, ICON_IMAGE_PATH, BUTTON_IMAGE_PATH, MUSIC_FILE_PATH;
        private enum IMAGE_TYPES { BG, BUTTON, ICON };



               
        public static BitmapImage BG;
        public static BitmapImage BUTTON;
        public static SolidColorBrush MAIN_MENU_FOREGROUND;
        public static SolidColorBrush MAIN_MENU_BORDER_COLOR;
        public static SolidColorBrush MAIN_MENU_BG_CONTRAST_COLOR;

        public static void init(int _styleNumber)
        {
        
            styleNumber = _styleNumber;
            string table = "STYLES";
            SQLiteManager mngr = new SQLiteManager();
            DataRowCollection styleRows = mngr.getStyle(table, styleNumber);
            BG_IMAGE_PATH = INIT.RES_DIR + @"Styles\" + styleNumber + @"\mainbg.png";
            BUTTON_IMAGE_PATH = INIT.RES_DIR + @"Styles\" + styleNumber + @"\button.png";
            ICON_IMAGE_PATH = INIT.RES_DIR + @"Styles\" + styleNumber + @"\favicon.ico"; 
            MUSIC_FILE_PATH = INIT.RES_DIR + @"Styles\" + styleNumber + @"\music.wav";
         


          
            BG = getImage(IMAGE_TYPES.BG);
            BUTTON = getImage(IMAGE_TYPES.BUTTON);
            MAIN_MENU_FOREGROUND = (SolidColorBrush)new BrushConverter().ConvertFromString(styleRows[0][2].ToString());
            MAIN_MENU_BORDER_COLOR = (SolidColorBrush)new BrushConverter().ConvertFromString(styleRows[0][4].ToString());
            MAIN_MENU_BG_CONTRAST_COLOR = (SolidColorBrush)new BrushConverter().ConvertFromString(styleRows[0][5].ToString());

        }




        private static BitmapImage getImage(IMAGE_TYPES it)
        {
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            if (it.Equals(IMAGE_TYPES.BG))
            {
                bitimg.UriSource = new Uri(BG_IMAGE_PATH, UriKind.RelativeOrAbsolute);
                bitimg.EndInit();
                
            }
            else
            {
                if (it.Equals(IMAGE_TYPES.BUTTON))
                {
                    bitimg.UriSource = new Uri(BUTTON_IMAGE_PATH, UriKind.RelativeOrAbsolute);
              //      bitimg.UriSource = new Uri(BUTTON_IMAGE_PATH, UriKind.RelativeOrAbsolute);
                    bitimg.EndInit();
                }
                else
                {
                    
                        MessageBox.Show("Тип изображения не определен");
                   
                }
            }


            System.Windows.Controls.Image img = new Image();


            img.Source = bitimg;

            return bitimg;
        }
    }
}
