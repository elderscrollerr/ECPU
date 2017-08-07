using ECPU.Exceptions;
using ECPU.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ECPU
{
   public class UPDATER : CPWindow
    {
        public static string TEMP_DB_SQLITE_FILE;
        public static string TEMP_DB_CONNECTION_STRING;
       

        public static float ACTUAL_GAME_VERSION;
        public static int ACTUAL_DB_VERSION;
       


        public UPDATER() : base("WINDOW_UPDATES")
        {
                TEMP_DB_SQLITE_FILE = INIT.RES_DIR + "TEMP.db";
                TEMP_DB_CONNECTION_STRING = "Data Source=" + TEMP_DB_SQLITE_FILE;
            try
            {
                getnewBD();
            }
            catch (NoInternetConnectionException)
            {
                return;
            }
        }
  

        public void getnewBD()
        {
            try
            {
                new WebClient().DownloadFile(GAME_TYPE.UPDATE_DB_DOWNLOAD_LINK, TEMP_DB_SQLITE_FILE);
            }
            catch
            {
                throw new NoInternetConnectionException();
            }
            if (File.Exists(TEMP_DB_SQLITE_FILE))
            {
                SQLiteManager newBD = new SQLiteManager(TEMP_DB_CONNECTION_STRING);
                DataTable dt = newBD.getListItems("GLOBALS");
                ACTUAL_DB_VERSION = Convert.ToInt32(dt.Rows[0][3].ToString());
                ACTUAL_GAME_VERSION = float.Parse(dt.Rows[0][2].ToString(), CultureInfo.InvariantCulture.NumberFormat); 
                newBD.ConnectionClose();
                if (INIT.USER_GAME_VERSION != ACTUAL_GAME_VERSION)
                {
                    throw new gameBuildInvalidVersionForUpdateException(ACTUAL_GAME_VERSION, INIT.USER_GAME_VERSION);
                    
                }else
                {
                    if (INIT.USER_BD_VERSION < ACTUAL_DB_VERSION)
                    {
                        switchDBForUpdate();
                    }else
                    {
                        return;
                    }
                }
            }else
            {
                return;
            }
        }

        public static void switchDBForUpdate()
        {
            MessageBox.Show("Есть новая версия базы данных панели." + Environment.NewLine + "Будет произведено обновление с версии " + INIT.USER_BD_VERSION + " до версии " + ACTUAL_DB_VERSION);
            Thread.Sleep(500);
            FileManager.MoveWithReplace(TEMP_DB_SQLITE_FILE, INIT.USER_DB_FILE);

            SQLiteManager userDatarestore = new SQLiteManager();
          userDatarestore.setENBInDB(INIT.CURRENT_ENB_OPTION);
            userDatarestore.setShaderInDB(INIT.CURRENT_SHADER_OPTION);
            userDatarestore.ConnectionClose();

            foreach (int i in INIT.USER_UPDATES_INSTALLED)
            {
                SQLiteManager userupd = new SQLiteManager();
                userupd.setUpdateInstalled("WINDOW_UPDATES", i);
                userupd.ConnectionClose();
            }

            MessageBox.Show("Обновление прошло успешно");
            Logger.addLine(true, "Обновление БД панели с версии " + INIT.USER_BD_VERSION + " до версии " + ACTUAL_DB_VERSION);
            INIT.USER_BD_VERSION = ACTUAL_DB_VERSION;
           
           
        }

    }
}
