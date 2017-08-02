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

namespace ECPU.Settings
{
   public class ENB_PRESET : CP_RADIO_BUTTONS_LIST_ELEMENT
    {
        
       
        public ENB_PRESET(int _number, string _name, string _path, string _link, string _description) : base(_number, _name, _path, _link, _description)
        {
            if (_name.Equals(INIT.CURRENT_ENB_OPTION))
            {
                currentpreset = this;
            }
        }
    

        public override void disablePreset()
        {
          

            foreach (string file in FileManager.getOnlyFileNamesFromDirectory(_text))
            {
               
               FileManager.remove(INIT.GAME_ROOT + file);
            }
            rb.IsChecked = false;
            
            if (File.Exists(INIT.GAME_ROOT + @"d3d9.bak"))
            {
                FileManager.remove(INIT.GAME_ROOT + @"d3d9.bak");
            }

          




        }

        public override void enablePreset()
        {
           //
            string[] files = FileManager.getOnlyFileNamesFromDirectory(_text);
           
            foreach (string s in files)
            {
                DirectoryInfo source = new DirectoryInfo(_text);
                DirectoryInfo target = new DirectoryInfo(INIT.GAME_ROOT);
                FileManager.CopyAllDirectoryContent(source, target);

            }
            if (ENB_MANAGER.callFromENBMANAGER)
            {
                foreach (string file in ENB_MANAGER.enbcore)
                {
                    FileManager.copyFiles(INIT.RES_DIR + @"ENB core\file", INIT.GAME_ROOT);
                }
            }

            SQLiteManager mngr = new SQLiteManager();
            mngr.setENBInDB(_name);

            mngr.ConnectionClose();
            rb.IsChecked = true;
            INIT.CURRENT_ENB_OPTION = _name;
        }

     
    }
}
