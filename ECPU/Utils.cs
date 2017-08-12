using ECPU.Exceptions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Windows;

namespace ECPU
{
    public static class Utils
    {
       // private static Dictionary<string, string> realPaths;


        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static void CheckGameFileBeforeStart() //проверяет наличие необходимых для игры файлов
        {
           
            foreach (string file in INIT.FILES_TO_CHECK.ToArray())
            {
              
                try
                {
                    if (!File.Exists(file))
                    {
                        throw new CriticalFileNotFoundException(file);
                    }
                }
                catch (CriticalFileNotFoundException ex)
                {
                    throw ex;
                }

            }
        }
   

        public static DataTable getENBPresets() 
        {
            SQLiteManager mngr = new SQLiteManager();
           DataTable enb =  mngr.getListItems("ENB");
            mngr.ConnectionClose();
            return enb;           
        }
      
        public static void registryFix(string registryHiveName, string value) //Пишет в реестр путь, исправдяя ошибки
        {
            RegistryKey key = null;
            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    RegistryKey reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\\Bethesda Softworks\");
                    key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\" + registryHiveName);
                    key.SetValue("Installed Path", value);
                    key.Close();

                }
                else
                {
                    RegistryKey reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\\Bethesda Softworks\");
                    key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\" + registryHiveName);
                    key.SetValue("Installed Path", value);
                    key.Close();
                }
            }
            catch (Exception)
            {
                RegistryException ex = new RegistryException(key.ToString());
                throw ex;
            }

        }


     
        
    


        public static void ResetSettings()
        {

           try
            {
                MessageBox.Show("настройки будут сброшены");
                Logger.addLine(true, "Начат сброс настроек" + " : " + DateTime.Now.ToString());
                registryFix(INIT.REGISTRY_HIVE, INIT.GAME_ROOT);


                string table = "RESET";
             
                List<Tuple<string, string>> sourceToTargetList = new List<Tuple<string, string>>();

                List<string> removeTargets = new List<string>();
                List<string> dirsToCreate = new List<string>();

                SQLiteManager mngr = new SQLiteManager();
                DataTable data = mngr.getListItems(table);
                foreach (DataRow dtRow in data.Rows)
                {

                    if ((dtRow[0].ToString().Equals("copy")))
                    {
                        string source = INIT.getpath(dtRow[1].ToString());
                        string target = INIT.getpath(dtRow[2].ToString());
                        sourceToTargetList.Add(new Tuple<string, string>(source, target));

                    }
                    else
                    {
                        if ((dtRow[0].ToString().Equals("remove")))
                        {
                            string fileToRemove = INIT.getpath(dtRow[1].ToString());
                            removeTargets.Add(fileToRemove);
                        }
                        else
                        {
                            if ((dtRow[0].ToString().Equals("create_dir")))
                            {
                                string dirToCreate = INIT.getpath(dtRow[1].ToString());
                                dirsToCreate.Add(dirToCreate);
                            }
                        }
                    }

                }
                mngr.ConnectionClose();




                FileManager.remove(removeTargets.ToArray());


                foreach (string dirs in dirsToCreate)
                {
                    FileManager.createDirectory(dirs);
                }

                foreach (Tuple<string, string> pair in sourceToTargetList)
                {
                    FileManager.copyFiles(pair.Item1, pair.Item2);
                }
                MessageBox.Show("Сброс настроек произведен");
                Logger.addLine(true, "Сброс настроек произведен" + " : " + DateTime.Now.ToString());
            }
            catch(Exception)
            {
                Logger.addLine(false, "Сброс настроек произвеcти не удалось!" + " : " + DateTime.Now.ToString());
                Logger.addLine(true, "Завершение приложения: " + DateTime.Now.ToString());
                Environment.Exit(1);
               
            }

        }



       


        public static void checkNETFrameworkVersion()
        {
            int necessaryVersion = 393273; // представление в реестре при версии 4.6.1

            int releaseKey = 0;
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
              
                releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));                
            }

            if (releaseKey < necessaryVersion)
            {
                throw new oldNETFrameworkVersionException();
            }

            /*
            if (releaseKey >= 393273)
            {
                return "4.6 RC or later";
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2 or later";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1 or later";
            }
            if ((releaseKey >= 378389))
            {
                return "4.5 or later";
            }
            // This line should never execute. A non-null release key should mean 
            // that 4.5 or later is installed. 
            return "No 4.5 or later version detected";
            */
        }

   
    }
}

