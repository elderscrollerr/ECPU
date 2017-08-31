using ECPU.Exceptions;
using ECPU.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ECPU
{
  public static class INIT
    {
        public struct APP { public string PATH; public string ARGS;}

       
      
       
       

        //Данные пользователя и пользовательской БД
       
        public static string CTRL_PANEL_DIR, RES_DIR, ENB_DIR, SHADER_DIR, ESP_SETTINGS_DIR; //корневая директория панели управления, ресурсов панели, esp и пресетов ENB
        public static string GAME_ROOT;//корневая папка игры
        public static string DATA_DIR;//папка Data игры
        public static string USER_DOCS, APPDATA_LOCAL_DIR; //пути к игровому профилю
        public static string LOG_FILE;
        public static string USER_DB_FILE, DB_CONNECTION_STRING; // путь к базе данных пользователя, строка подключения к этой базе
     //   public static string DB_TABLE_PREFIX; //префикс таблиц БД (зависит от игры)
        public static string GAME_ISO_MOUNT_PATH; //путь к образу диска для монтирования (если нужно сонтировать для запуска)
        public static Dictionary<string, string> PATHS; //пути к различным файлам
        public static List<string> FILES_TO_CHECK;  // Файлы, проверяемые перед запуском панели
        public static string GAME_TITLE; //название игры (сборки)
        public static string LOAD_ORDER_FILE; //Файл по которому игра читает порядок загрузки плагинов
        public static float USER_GAME_VERSION; //Версия игры (из базы пользователя)
        public static string REGISTRY_HIVE; //ветка реестра (зависит от игры)
        public static string GAME_PROCESS_NAME; //имя процесс запущенной игры
        public static string CURRENT_ENB_OPTION; // имя пресета ENB, включенного у пользователя в данный момент
        public static string CURRENT_SHADER_OPTION;
        public static bool RUN_THROW_SCRIPT_EXTENDER;// запускать ли игру через Script Extender
        public static int USER_BD_VERSION; // Версия базы у данных пользователя
        public static List<int> USER_UPDATES_INSTALLED; //Список номеров установленных обновлений (из базы пользователя)
        public static bool DEFAULT_VISUAL_STYLE;
        public static string[] MASTER_FILES_ESM; // необходимые для игры мастер-файлы
        public static string CURRENT_SHIFTED_PLUGIN_IN_LO;
        public static string PLUGINS_TXT_PATH;





        public static void initUserBDGame()
        {
            // CTRL_PANEL_DIR = @"J:\Games\TES 5 Skyrim - ESSE 0.6\Control Panel\";
            //  CTRL_PANEL_DIR = @"D:\TES_DEV\!!!!!!!!!!!!!!!!!!MASS\Control Panel\"; // bumagi
            //  CTRL_PANEL_DIR = @"D:\OblivionPR\Control Panel\"; //корневая директория панели управления - временно
            //   CTRL_PANEL_DIR = AppDomain.CurrentDomain.BaseDirectory;
           CTRL_PANEL_DIR = Directory.GetParent(Assembly.GetExecutingAssembly().Location) + @"\";
         


            RES_DIR = CTRL_PANEL_DIR + @"Resources\"; //корневая директория ресурсов панели
            ENB_DIR = CTRL_PANEL_DIR + @"ENB\"; //корневая директория ENB-пресетов 
            SHADER_DIR = CTRL_PANEL_DIR + @"SHADER\"; //корневая директория вариантов шейдеров
            GAME_ROOT = Path.GetFullPath(Path.Combine(CTRL_PANEL_DIR, @"..\"));
            DATA_DIR = GAME_ROOT + @"Data\";
            ESP_SETTINGS_DIR = RES_DIR + @"esp settings\"; //корневая директория esp файлов для настроеек 
            
            USER_DOCS = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\";
            APPDATA_LOCAL_DIR = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\";
            INIT.CURRENT_SHIFTED_PLUGIN_IN_LO = "";
            LOG_FILE = CTRL_PANEL_DIR + "cp_log.txt";

            GAME_TYPE.init(); 

            SQLiteManager userBDSQLManager = new SQLiteManager();
            setPathsAndFilesForCheck(userBDSQLManager);
            setUserGlobals(userBDSQLManager);
            setUserSettings(userBDSQLManager);
            setUserInstalledUpdates(userBDSQLManager);
            int stylesAmount = userBDSQLManager.getListItems("STYLES").Rows.Count;
            userBDSQLManager.ConnectionClose();
           
            Random rnd = new Random();
            try
            {
                STYLE.init(rnd.Next(1, stylesAmount + 1));
                DEFAULT_VISUAL_STYLE = false;
            }
            catch
            {
                DEFAULT_VISUAL_STYLE = true;
                Logger.addLine(false, "Не удалось загрузить стили оформления. Будет использован стиль по умолчанию");
            }



        }

     
   

        private static void setPathsAndFilesForCheck(SQLiteManager mngrs)
        {
            PATHS = new Dictionary<string, string>();
            FILES_TO_CHECK = new List<string>();
            DataTable data = new DataTable();
            data = mngrs.getListItems("FILES");
            foreach (DataRow dtRow in data.Rows)
            {
                bool fileNeedForCheck = false;
                string path = "";

                if (Convert.ToBoolean(dtRow[3].ToString()))
                {
                    fileNeedForCheck = true;
                }

                if ((dtRow[1].ToString().Equals("gameroot")))
                {

                    path = GAME_ROOT + dtRow[2];
                }
                else
                {
                    if ((dtRow[1].ToString().Equals("userdocs")))
                    {
                        path = USER_DOCS + dtRow[2];
                    }
                    else
                    {
                        if ((dtRow[1].ToString().Equals("appdata")))
                        {
                            path = APPDATA_LOCAL_DIR + dtRow[2];
                        }
                        else
                        {
                            if ((dtRow[1].ToString().Equals("link")))
                            {
                                path = dtRow[2].ToString();
                            }
                            else
                            {

                            }
                        }
                    }

                }
                PATHS.Add(dtRow[0].ToString(), path);

                if (fileNeedForCheck)
                {
                    FILES_TO_CHECK.Add(path);
                }

            }
        }

        private static void setUserGlobals(SQLiteManager mngrs)
        {
            DataTable globals = mngrs.getListItems("GLOBALS");
            GAME_TITLE = globals.Rows[0][0].ToString();
            USER_GAME_VERSION = float.Parse(globals.Rows[0][2].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            USER_BD_VERSION = Convert.ToInt32(globals.Rows[0][3]);
            REGISTRY_HIVE = globals.Rows[0][1].ToString();
            LOAD_ORDER_FILE = globals.Rows[0][2].ToString();
            PLUGINS_TXT_PATH = getpath("plugins_txt");

        }
        private static void setUserSettings(SQLiteManager mngrs)
        {
            DataTable userData = mngrs.getListItems("USER_DATA");
          
            CURRENT_ENB_OPTION = userData.Rows[0][0].ToString();
            CURRENT_SHADER_OPTION = userData.Rows[0][1].ToString();
         

        }

        private static void setUserInstalledUpdates(SQLiteManager mngrs)
        {
            DataTable updateslist = mngrs.getListItems("WINDOW_UPDATES");
           
            USER_UPDATES_INSTALLED = new List<int>();
            foreach (DataRow dr in updateslist.Rows)
            {
                if (Convert.ToBoolean(dr[5]))
                {
                    USER_UPDATES_INSTALLED.Add(Convert.ToInt32(dr[0]));
                }
            }
        }


        public static string getpath(string name)
        {
            string path;
            if (!PATHS.TryGetValue(name, out path))
            {
                return null;
            }
            else
            {
                return path;
            }
        }


     
        }

      
    }

