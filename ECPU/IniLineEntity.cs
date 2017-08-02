using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECPU
{
    public class IniLineEntity
    {
        private bool section;
        private int sectionNumber;
        private string sectionName;
        private string key;
        private string value;


        public IniLineEntity(string line)
        {
            string nameOfSection = IniParser.getNameOfSection(line);



            if (String.IsNullOrEmpty(nameOfSection)) //создаем строку с параметром
            {

                int count = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i].Equals('='))
                    {
                        count++;
                    }
                }
                if ((line.Trim()).Equals("}"))
                {
                    key = line.Trim();
                    value = "";
                }

                if (count == 1)
                {
                    key = (line.Split('=')[0]).Trim();
                    value = line.Split('=')[1];

                }
                else
                {
                    if (count > 1)
                    {
                        key = (line.Split('=')[0]).Trim();
                        for (int i = 1; i <= count; i++)
                        {
                            if (i == count)
                            {
                                value = value + line.Split('=')[i];
                            }
                            else
                            {
                                value = value + line.Split('=')[i] + "=";
                            }

                        }
                    }
                }


                //    MessageBox.Show(value);  



            }
            else //создаем секцию
            {

                sectionNumber++;
                this.sectionName = nameOfSection;
                this.section = true;


            }

        }

        public bool isSection()
        {
            return section;
        }

        public string getKey()
        {
            return this.key;
        }

        public void setNewValue(string newValue)
        {
            this.value = newValue;
        }


        public string getValue()
        {
            return this.value;
        }

        public override string ToString()
        {
            if (section)
            {
                return sectionName;
            }
            else
            {
                return key + '=' + Convert.ToString(value);
            }

        }

        public int getSectionNumber()
        {
            return sectionNumber;
        }

        public string getNameOfSection()
        {
            return sectionName;
        }

        public string print()
        {
            if (isSection())
            {
                return sectionName;
            }
            else
            {
                if (key.Equals("}"))
                {
                    return key;
                }
                else
                {
                    return key + '=' + value;
                }

            }
        }

    }
}

