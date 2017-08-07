using ECPU.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECPU
{
  public static class GAME_TYPE
    {
        public enum GAMES { MASS, OP, MP, ESSE }
        public static GAMES CURRENT_GAME; //игра, под которой запущена панель
        public static bool needToSwitchDXFiles = false;
        public static bool needToDisableExplorer = false;
        public static bool runThrowOBSE = false;
        public static bool activepluginsMarkAsterisk = false;
        public static string UPDATE_DB_DOWNLOAD_LINK;
        public static bool resolutionItemChangeWithCopyUIFixes = false;

        public static void init()
        {
            if (File.Exists(INIT.GAME_ROOT + "Morrowind.exe"))
            {
                CURRENT_GAME = GAMES.MASS;
                INIT.USER_DB_FILE = INIT.RES_DIR + "MASS.db";
                INIT.DB_CONNECTION_STRING = "Data Source=" + INIT.USER_DB_FILE;
                INIT.GAME_ISO_MOUNT_PATH = INIT.CTRL_PANEL_DIR + "Morrowind.iso";
                INIT.RUN_THROW_SCRIPT_EXTENDER = false;
                INIT.GAME_PROCESS_NAME = "Morrowind";
                INIT.MASTER_FILES_ESM = new string[3];
                UPDATE_DB_DOWNLOAD_LINK = "";
    }
            else
            {
                if (File.Exists(INIT.GAME_ROOT + "Oblivion.exe"))
                {
                    runThrowOBSE = true;
                    if (File.Exists(INIT.GAME_ROOT + @"ObivionPerfect.ini"))
                    {
                        IniManager im = new IniManager(INIT.GAME_ROOT + @"ObivionPerfect.ini");
                      
                        if (Convert.ToBoolean(im.getValueByKey("bDisableExplorer")))
                        {
                            needToDisableExplorer = true;
                        }
                    }

                    if (File.Exists(INIT.GAME_ROOT + @"Data\Morrowind_ob.esm"))
                    {
                        UPDATE_DB_DOWNLOAD_LINK = "";
                        CURRENT_GAME = GAMES.MP;
                        INIT.USER_DB_FILE = INIT.RES_DIR + "MP.db";
                        INIT.DB_CONNECTION_STRING = "Data Source=" + INIT.USER_DB_FILE;
                        INIT.RUN_THROW_SCRIPT_EXTENDER = true;
                        INIT.GAME_PROCESS_NAME = "Oblivion";
                        INIT.MASTER_FILES_ESM = new string[2];
                        INIT.MASTER_FILES_ESM[0] = "Oblivion.esm";
                        INIT.MASTER_FILES_ESM[1] = "Morrowind_ob.esm";
                        needToSwitchDXFiles = true;
                    }
                    else
                    {
                        UPDATE_DB_DOWNLOAD_LINK = @"https://www.dropbox.com/s/k6lsypai1kwztcz/CP.db?dl=1";
                        CURRENT_GAME = GAMES.OP;
                        INIT.USER_DB_FILE = INIT.RES_DIR + "OP.db";
                        INIT.DB_CONNECTION_STRING = "Data Source=" + INIT.USER_DB_FILE;
                        INIT.RUN_THROW_SCRIPT_EXTENDER = true;
                        INIT.GAME_PROCESS_NAME = "Oblivion";
                        INIT.MASTER_FILES_ESM = new string[1];
                        INIT.MASTER_FILES_ESM[0] = "Oblivion.esm";
                        needToSwitchDXFiles = true;
                    }



                }
                else
                {
                    if (File.Exists(INIT.GAME_ROOT + "SkyrimSE.exe"))
                    {
                        resolutionItemChangeWithCopyUIFixes = true;
                        UPDATE_DB_DOWNLOAD_LINK = @"https://www.dropbox.com/s/295t2xi7mx3652v/ESSE.db?dl=1"; 
                        activepluginsMarkAsterisk = true;
                        CURRENT_GAME = GAMES.ESSE;
                        INIT.USER_DB_FILE = INIT.RES_DIR + "ESSE.db";
                        INIT.DB_CONNECTION_STRING = "Data Source=" + INIT.USER_DB_FILE;
                        INIT.RUN_THROW_SCRIPT_EXTENDER = false;
                        INIT.GAME_PROCESS_NAME = "SkyrimSE";
                        INIT.MASTER_FILES_ESM = new string[5];
                        INIT.MASTER_FILES_ESM[0] = "Skyrim.esm";
                        INIT.MASTER_FILES_ESM[1] = "Update.esm";
                        INIT.MASTER_FILES_ESM[2] = "Dawnguard.esm";
                        INIT.MASTER_FILES_ESM[3] = "HearthFires.esm";
                        INIT.MASTER_FILES_ESM[4] = "Dragonborn.esm";

                    }
                    else
                    {
                        throw new GameTypeInitializationException();
                    }
                }
            }
        }
    }
    }

