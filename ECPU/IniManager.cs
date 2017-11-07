using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ECPU
{
    public class IniManager
    {

      private ArrayList lines;
        private List<IniLineEntity> allLines;
        private string path;

        public IniManager(string path)
        {

            //if (FileManager.IsExist(path) && FileManager.IsNotEmpty(path))
         //   {
                this.path = path;
                allLines = new List<IniLineEntity>();
                lines = FileManager.GetContentAsLines(path);
                read();

         //   }
        }



        public void read()
        {

            foreach (string line in lines)
            {
                if (line == null)
                {
                    break;
                }

                if (line == string.Empty)
                {
                    continue;
                }

                line.Trim();

                if (line.Contains("[") && line.Contains("]") || line.Contains("=") || line.Trim().Equals("}"))
                {
                    allLines.Add(new IniLineEntity(line));
                }

            }

        }

        public bool isSectionExist(string sectionname)
        {
            bool existence = false;



            foreach (IniLineEntity element in allLines)
            {
                if (element.isSection())
                {

                    if (element.getNameOfSection().Equals(sectionname))
                    {
                        existence = true;
                        break;
                    }
                    else
                    {

                        continue;
                    }
                }

                continue;

            }


            return existence;
        }

        public string getValueByKey(string key, string section)
        {

            if (allLines==null)
            {
                MessageBox.Show("dasdas");
            }
          
     //       MessageBox
            foreach (IniLineEntity element in allLines)
            {

             //  MessageBox.Show(element.print());
                if (element.isSection())
                {
                    continue;
                }
                else
                {
                    if (element.getKey().Equals(key) && element.getNameOfSection().Equals(section))
                    {
                        return element.getValue();
                    }
                    else
                    {
                        continue;
                    }
                }

            }
            return "";
        }

        public bool isKeyExist(string key)
        {
            bool existence = false;



            foreach (IniLineEntity element in allLines)
            {
                if (!element.isSection())
                {
                    //   MessageBox.Show(key);
                    //   MessageBox.Show(element.getKey());

                    if (element.getKey().Equals(key))
                    {
                        existence = true;
                        break;
                    }
                    else
                    {

                        continue;
                    }
                }

                continue;

            }


            return existence;
        }






        public void setParameter(string key, string value, string section)
        {


            ArrayList linesToWrite = new ArrayList();

            if (key.Equals("}"))
            {
                allLines.Insert(allLines.Count, new IniLineEntity(key));
            }



            if (isSectionExist(section))
            {
                if (isKeyExist(key))
                {
                  //  x => (x.Body.Scopes.Count > 5) && (x.Foo == "test")
                    allLines.Find( x => x.getKey() == key && x.getNameOfSection() == section).setNewValue(value);
                }
                else
                {
                    int ind = allLines.IndexOf(allLines.Find(x => x.getNameOfSection() == section));
                    allLines.Insert(ind + 1, new IniLineEntity(key + '=' + value));
                }
            }
            else
            {

                if (section.Equals("[NULL]"))
                {
                    if (isKeyExist(key))
                    {
                        allLines.Find(x => x.getKey() == key).setNewValue(value);
                    }
                    else
                    {
                        MessageBox.Show("Запись в ini без секции: Ключ " + key + "не найден");
                    }
                }
                else
                {
                    if (isKeyExist(key))
                    {
                        allLines.Remove(allLines.Find(x => x.getKey() == key));
                        allLines.Insert(allLines.Count, new IniLineEntity(section));
                        allLines.Insert(allLines.Count + 1, new IniLineEntity(key + '=' + value));
                    }
                    else
                    {
                        allLines.Insert(allLines.Count, new IniLineEntity(section));
                        allLines.Insert(allLines.Count, new IniLineEntity(key + '=' + value));
                    }
                }

            }
            foreach (IniLineEntity line in allLines)
            {
                linesToWrite.Add(line.print());
            }


            FileManager.WriteAllLines(path, linesToWrite);
        }
    }
}
