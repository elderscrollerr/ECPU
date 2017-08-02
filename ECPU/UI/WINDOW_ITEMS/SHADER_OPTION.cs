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
   public class SHADER_OPTION : CP_RADIO_BUTTONS_LIST_ELEMENT
    {

      
       


   
        public SHADER_OPTION(int _number, string _name, string _path, string _link, string _description) : base(_number, _name, INIT.SHADER_DIR + _path + @"Data\Shaders", _link, _description)
        {
            if (_name.Equals(INIT.CURRENT_SHADER_OPTION))
            {
                currentpreset = this;
            }

        }


        public override void disablePreset()
        {
          

            foreach (string file in FileManager.getOnlyFileNamesFromDirectory(_text))
            {
             
             FileManager.remove(INIT.GAME_ROOT + @"Data\Shaders\" + file);
            }
            rb.IsChecked = false;
            

        }

        public override void enablePreset()
        {
            string[] files = FileManager.getOnlyFileNamesFromDirectory(_text);
           
            foreach (string s in files)
            {

               
              DirectoryInfo source = new DirectoryInfo(_text);
              
                DirectoryInfo target = new DirectoryInfo(INIT.GAME_ROOT + @"Data\Shaders\");
                FileManager.CopyAllDirectoryContent(source, target);

            }

            SQLiteManager mngr = new SQLiteManager();
            mngr.setShaderInDB(_name);

            mngr.ConnectionClose();
            rb.IsChecked = true;
            INIT.CURRENT_SHADER_OPTION = _name;
        }

          

      

    

      

    }
}
