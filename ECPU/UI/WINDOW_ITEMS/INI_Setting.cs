using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ECPU.UI;
using ECPU.Exceptions;
using System.Threading;


namespace ECPU
{
    public abstract class INI_Setting : CP_Setting
    {
        protected string _iniFilepath;
        //    private IniManager _iniManager;
        protected string _iniSection;
        protected string _iniKey;
       
        
       
        public INI_Setting(string name, string text, string desc, int position, string defaultValue,
            string iniFilepath, string iniSection, string iniKey) : base(name, text, desc, defaultValue, position)
        {
            
            _iniFilepath = INIT.getpath(iniFilepath);
            
            _iniKey = iniKey;
            _iniSection = iniSection;
            if (!string.IsNullOrEmpty(_iniFilepath))
            {
                IniManager _iniManager = new IniManager(_iniFilepath);

                if (!_iniSection.Equals("[NULL]"))
                {
                if (!_iniManager.isKeyExist(_iniKey) || !_iniManager.isSectionExist(_iniSection))
                {
                    if (!_iniKey.Contains(','))
                    {
                        throw new INIDataNotFoundException(_iniSection, _iniKey, _iniFilepath);
                    }
                    else
                    {
                        List<string> substrings = iniKey.Split(',').ToList<string>();
                        foreach (string s in substrings)
                        {
                            if (!_iniManager.isKeyExist(s))
                            {
                                throw new INIDataNotFoundException(_iniSection, _iniKey, _iniFilepath);
                            }
                        }
                    }

                }
            }
                _currentValue = "";
                _currentValue = _iniManager.getValueByKey(iniKey, iniSection);
            }
           
        }

        public override void fix()
        {
            if (string.IsNullOrEmpty(_currentValue) || string.IsNullOrWhiteSpace(_currentValue))
            {
                resetTodefault();
            }
        }

        public override void resetTodefault()
        {
            IniManager _iniManager = new IniManager(_iniFilepath);
            _iniManager.setParameter(_iniKey, _default_value, _iniSection);
        }

        public override void change(string newValue)
        {
            try {
                IniManager _iniManager = new IniManager(_iniFilepath);
                _iniManager.setParameter(_iniKey, newValue, _iniSection);
                Logger.addLine(true, "Изменение настроек");
            } catch(Exception)
            {
                Logger.addLine(false, "Изменение настроек");
                throw new INIDataNotFoundException(_iniSection, _iniKey, _iniFilepath);
                
            }

                
        }

 

        public abstract override StackPanel getView();
        
    }
}
