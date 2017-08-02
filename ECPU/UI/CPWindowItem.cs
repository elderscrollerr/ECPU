using ECPU.Exceptions;
using ECPU.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace ECPU.UI
{
   public abstract class CPWindowItem : Control
    {
      //  private static string DBTable = "WINDOW_ITEMS";
        protected string _name;
        protected string _text;
        protected int _position;
        protected string _description;
        public bool isTopPanelButton = false;
        protected double width;
        protected double height;
        protected double fontSize;
        public bool autoConuntAndSize;
        protected StackPanel view;

        List<CPWindowItem>  items;
        public CPWindowItem(string name, string text, string desc, int position)
        {
            _name = name;
            _text = text;
            _description = desc;
            _position = position;
            autoConuntAndSize = false;



        }
       public abstract StackPanel getView();

 

        public string getDescription()
        {
            return _description;
        }

        public int getPosition()
        {
            return _position;
        }

        public void setWidth(double _width)
        {
            width = _width;
        }
        public void setHeight(double _height)
        {
            height = _height;
        }

        public void setFontSize(double _fontSize)
        {
            fontSize = _fontSize;
        }




    }
}
