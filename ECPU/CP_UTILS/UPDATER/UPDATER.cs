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
        public static string UPDATE_DB_DOWNLOAD_LINK;

        public static float ACTUAL_GAME_VERSION;
        public static int ACTUAL_DB_VERSION;
       


        public UPDATER() : base("WINDOW_UPDATES")
        {


            if (INIT.CURRENT_GAME.Equals(INIT.GAMES.ESSE))
                {
                    UPDATE_DB_DOWNLOAD_LINK = @"https://www.dropbox.com/s/295t2xi7mx3652v/ESSE.db?dl=1";
                }else
                {
                    if (INIT.CURRENT_GAME.Equals(INIT.GAMES.OP))
                    {
                  UPDATE_DB_DOWNLOAD_LINK = @"https://www.dropbox.com/s/k6lsypai1kwztcz/CP.db?dl=1";
                
                }
                    else
                    {
                        if (INIT.CURRENT_GAME.Equals(INIT.GAMES.MASS))
                        {
                            UPDATE_DB_DOWNLOAD_LINK = @"";
                        }else
                        {
                            return;
                        }
                    }
                }
             
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

      //  protected override void getContent()
      //  {







            //   LO_MANAGER lom = new LO_MANAGER();

        //    buildLO();
         //   getList();
        //    getControls();


       //     StackPanel view = new StackPanel();
     //       view.Orientation = Orientation.Horizontal;
      //      view.Children.Add(plugins);
      //      view.Children.Add(controls);
      //      Grid content = new Grid();
    //        content.Children.Add(view);
      //      Grid.SetRow(view, 0);
     //       Grid.SetColumn(view, 0);
     //       return content;

   //     }



        public void getnewBD()
        {
            try
            {
                new WebClient().DownloadFile(UPDATE_DB_DOWNLOAD_LINK, TEMP_DB_SQLITE_FILE);
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
