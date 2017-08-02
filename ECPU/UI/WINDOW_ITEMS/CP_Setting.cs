using ECPU.Settings;
using ECPU.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ECPU
{
    public abstract class CP_Setting : CPWindowItem
    {
       
        protected string _default_value;
        protected string _currentValue;
        protected bool _CurrentState;

       
       
     
        List<string> filesToMove, filesToMoveWithPath;

        public CP_Setting(string name, string text, string desc, string defaultValue, int position) : base(name, text, desc, position)
        {
            _default_value = defaultValue;
        }

    

        public abstract void resetTodefault();

        public abstract void change(string newValue);
        

  

        public string getCurrentValue()
        {

            return _currentValue;
        }
        public abstract void fix();

    




      


    }
}
