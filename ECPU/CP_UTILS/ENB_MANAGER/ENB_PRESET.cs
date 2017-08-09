using ECPU;
using ECPU.UI;
using ECPU.UI.WINDOW_ITEMS;
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

namespace ECPU
{
   public class ENB_PRESET : RadioButton
    {
        private string path;
        private string name;
        private bool activeNow;
        private bool noPreset;

        
        public ENB_PRESET(string _path)
        {
            path = _path;
            name = Path.GetFileName(_path);
            Content = name;
            noPreset = false;
            
            if (name.Equals(INIT.CURRENT_ENB_OPTION))
            {
                activeNow = true;
                IsChecked = true;
            }
            else
            {
                activeNow = false;
            }
           

            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                Foreground = Brushes.Black;
            }
           Margin = new Thickness(10, 10, 10, 10);

        }

        public ENB_PRESET()
        {
            noPreset = true;
            name = "Нет пресета";
            Content = name;
            if (name.Equals(INIT.CURRENT_ENB_OPTION))
            {
                activeNow = true;
                IsChecked = true;
            }
            else
            {
                activeNow = false;
            }
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                Foreground = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                Foreground = Brushes.Black;
            }
            Margin = new Thickness(10, 10, 10, 10);


        }

     
        public bool getActivity()
        {
            return activeNow;
        }

        public void disable()
        {
            if (!noPreset)
            {
                foreach (string file in FileManager.getOnlyFileNamesFromDirectory(path))
                {
                    FileManager.remove(INIT.GAME_ROOT + file);
                }
            }
   
            activeNow = false;
            IsChecked = false;
        }


        public void enable()
        {
            if (!noPreset)
            {

                DirectoryInfo source = new DirectoryInfo(path);
                DirectoryInfo target = new DirectoryInfo(INIT.GAME_ROOT);
                FileManager.CopyAllDirectoryContent(source, target);
            }

            ENB_MANAGER.restoreEnbCoreFiles();
            SQLiteManager mngr = new SQLiteManager();
            mngr.setENBInDB(name);

            mngr.ConnectionClose();
            INIT.CURRENT_ENB_OPTION = name;
            IsChecked = true;
            activeNow = true;
        }

    }
}
