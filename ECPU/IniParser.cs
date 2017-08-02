using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECPU
{
    public class IniParser
    {



        public static string getNameOfSection(string currentLine) // возвращает имя секции, если это секция
        {


            if (string.Compare(currentLine.Substring(0, 1), "[") == 0 && string.Compare(currentLine.Substring(currentLine.Length - 1, 1), "]") == 0)
            {
                return currentLine;
            }
            return "";

        }
    }
}
