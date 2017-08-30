using ECPU.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ECPU
{
  public class SQLiteManager
    {
       private static SQLiteConnection connection;
       
       
        public SQLiteDataReader sqlite_datareader;
        List<String> ListItems;

        public SQLiteManager()
        {
            connection = new SQLiteConnection(INIT.DB_CONNECTION_STRING);
            connection.Open();
        }
        public SQLiteManager(string connectionString)
        {
            connection = new SQLiteConnection(connectionString);
            connection.Open();
        }
        public void ConnectionClose()
        {
            
            if (connection != null && connection.State != ConnectionState.Closed)
            {
              
                connection.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
          
        }

       
        public DataTable getListItems(string table)
        {

           
           try
            {
                ListItems = new List<string>();
               string query = "SELECT * FROM " + table;
              
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                DataTable data = new DataTable();
               
                data.Load(cmd.ExecuteReader());
                return data;
            }
            catch (SQLiteException)
            {
                
                throw new LoadTableException(table);

            }

        }


   



        public void setENBInDB(string presetname)
        {
            try
            {
               
                string query = "UPDATE " + "USER_DATA" + " SET ENB_OPTION =  '" + presetname + "'; ";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();


            }
            catch (SQLiteException)
            {
               
                throw new LoadTableException("USER_DATA");

            }

        }

        public void setShaderInDB(string shadername)
        {
            try
            {

                string query = "UPDATE " + "USER_DATA" + " SET SHADER_OPTION =  '" + shadername + "'; ";               
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                
            }
            catch (SQLiteException)
            {

                throw new LoadTableException("USER_DATA");

            }

        }

        public void setUpdateInstalled(string table, int id)
        {
            try
            {
               
                string query = "UPDATE " + table + " SET is_installed  = 'true'  WHERE id = '" + id + "'; ";

                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();


            }
            catch (SQLiteException)
            {
               
                throw new LoadTableException(table);

            }
            finally
            {
                ConnectionClose();
            }

        }


  

        public DataRowCollection getStyle(string table, int styleNumber)
        {
            try
            {
                ListItems = new List<string>();
                string query = "SELECT * FROM " + table + " WHERE styleNumber = " + styleNumber;
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                DataTable data = new DataTable();
               
                data.Load(cmd.ExecuteReader());
                return data.Rows;
            }
            catch (SQLiteException)
            {
                
                throw new LoadTableException(table);

            }
            finally
            {
                ConnectionClose();
            }

        }


    }
}
