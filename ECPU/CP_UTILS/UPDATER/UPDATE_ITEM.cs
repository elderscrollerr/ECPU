using ECPU.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using ECPU.UI;
using System.Threading;

namespace ECPU.WINDOW_ITEMS
{
    public class UPDATE_ITEM : CPWindowItem
    {
        private bool isInstalled;
     //   private string date;
        private string httpLink;
        private List<string> filesToremove;

      
        private Label lblnumber;
        private Label lbldate;
        private Label lblname;
        private Label instButton;

       


        public UPDATE_ITEM(int _id, string _name, string _desc, string _date, bool _isInstalled, string _httpLink, string _filesToremove) : base(_name, _date, _desc, _id)
        {
         //   date = _date;
            isInstalled = _isInstalled;
            httpLink = _httpLink;

            if ((!string.IsNullOrEmpty(_filesToremove) && !string.IsNullOrWhiteSpace(_filesToremove)))
            {
                filesToremove = new List<string>();
                List<string> withoutRoot = _filesToremove.Split(',').ToList<string>();

                foreach (string file in withoutRoot)
                {
                    filesToremove.Add(INIT.GAME_ROOT + file);
                }

            }

        }

   



        private void installbtn_Click(object sender, MouseButtonEventArgs e)
        {
            lblnumber.Foreground = Brushes.Blue;
            lbldate.Foreground = Brushes.Blue;
            lblname.Foreground = Brushes.Blue;
            instButton.Foreground = Brushes.Blue;
            instButton.Content = "УСТАНОВКА...";
            install();
            GC.Collect();
            Thread.Sleep(500);
           // if (File.Exists(INIT.CTRL_PANEL_DIR + "Update" + _position.ToString() + ".exe"))
          //  {
         //      FileManager.remove(INIT.CTRL_PANEL_DIR + "Update" + _position.ToString() + ".exe");
        //    }

        }
      
   

        private void removeFiles()
        {
            if (filesToremove!=null && filesToremove.Count>0)
            {

                FileManager.remove(filesToremove.ToArray());
            }


        }

  
        private void install()
        {
            try
            {
            
            removeFiles();

                FileManager.downloadBig(httpLink, INIT.CTRL_PANEL_DIR + "Update"+ _position.ToString() + ".exe");
                unpackArchive(INIT.CTRL_PANEL_DIR + "Update" + _position.ToString() + ".exe");
                SQLiteManager checkAsInstalled = new SQLiteManager();
                checkAsInstalled.setUpdateInstalled("WINDOW_UPDATES", _position);          

                checkAsInstalled.ConnectionClose();
                instButton.MouseDown -= installbtn_Click;
            }
            catch (downloadArchiveUpdateException)
            {
               
                MessageBox.Show("Не удалось скачать обновление номер " + _position.ToString());
                lblnumber.Foreground = Brushes.Red;
                lbldate.Foreground = Brushes.Red;
                lblname.Foreground = Brushes.Red;
                instButton.Foreground = Brushes.Red;
                instButton.Content = "НЕ УСТАНОВЛЕНО";
            }
            catch (unpackArchiveUpdateException)
            {
                
                MessageBox.Show("Не удалось распаковать архив с обновлением номер " + _position.ToString());
                lblnumber.Foreground = Brushes.Red;
                lbldate.Foreground = Brushes.Red;
                lblname.Foreground = Brushes.Red;
                instButton.Foreground = Brushes.Red;
                instButton.Content = "НЕ УСТАНОВЛЕНО";
            }
            lblnumber.Foreground = Brushes.Green;
            lbldate.Foreground = Brushes.Green;
            lblname.Foreground = Brushes.Green;
            instButton.Foreground = Brushes.Green;
            instButton.Content = "УСТАНОВЛЕНО";

            

        }

     
 

        private static void unpackArchive(string zipPath)
        {


           
            try
            {
           

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = zipPath;
                proc.StartInfo.WorkingDirectory = INIT.GAME_ROOT;
                proc.Start();


            }
            catch (System.Exception )
            {
              
            }
        

    }

       

        public override StackPanel getView()
        {
            StackPanel view = new StackPanel();

          //  Grid grid = new Grid();

        //    ColumnDefinition c1 = new ColumnDefinition();
        ////    c1.Width = new GridLength(20);
        //    grid.ColumnDefinitions.Add(c1); //номер

         //   ColumnDefinition c2 = new ColumnDefinition();
        //    c2.Width = new GridLength(60);
        //    grid.ColumnDefinitions.Add(c2); //дата

        //    ColumnDefinition c3 = new ColumnDefinition();
        //    c3.Width = new GridLength(160);
        //    grid.ColumnDefinitions.Add(c3); //название


        //    ColumnDefinition c4 = new ColumnDefinition();
       //     c4.Width = new GridLength(110);
       //     grid.ColumnDefinitions.Add(c4); //установлено-неустановлено

       //     grid.RowDefinitions.Add(new RowDefinition());

           
            lblnumber = new Label();
            lblnumber.FontSize = fontSize;
            lblnumber.Content = _position;
            lblnumber.ToolTip = _description;
            lblnumber.FontWeight = FontWeights.Bold;
            lblnumber.FontFamily = new FontFamily("Helvetica");
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                lblnumber.BorderBrush = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                lblnumber.BorderBrush = Brushes.Black;
            }
            lblnumber.BorderThickness = new Thickness(1);
          //  grid.Children.Add(lblnumber);
        //    Grid.SetRow(lblnumber, 0);
        //    Grid.SetColumn(lblnumber, 0);


            lbldate = new Label();
            lbldate.Content = _text;
            lbldate.ToolTip = _description;
            lbldate.FontSize = fontSize;
            lbldate.FontWeight = FontWeights.Bold;
            lbldate.FontFamily = new FontFamily("Helvetica");
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                lbldate.BorderBrush = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                lbldate.BorderBrush = Brushes.Black;
            }
           
            lbldate.BorderThickness = new Thickness(1);
         //   grid.Children.Add(lbldate);
         //   Grid.SetRow(lbldate, 0);
         //   Grid.SetColumn(lbldate, 1);

            lblname = new Label();
            lblname.Content = _name;
            lblname.ToolTip = _description;
            lblname.FontSize = fontSize;
            lblname.FontWeight = FontWeights.Bold;
            lblname.FontFamily = new FontFamily("Helvetica");
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                lblname.BorderBrush = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                lblname.BorderBrush = Brushes.Black;
            }
           
            lblname.BorderThickness = new Thickness(1);
         //   grid.Children.Add(lblname);
        //    Grid.SetRow(lblname, 0);
       //     Grid.SetColumn(lblname, 2);

            instButton = new Label();
            instButton.ToolTip = _description;
            instButton.FontWeight = FontWeights.Bold;
            instButton.FontSize = fontSize;
            instButton.FontFamily = new FontFamily("Helvetica");
            if (!INIT.DEFAULT_VISUAL_STYLE)
            {
                instButton.BorderBrush = STYLE.MAIN_MENU_FOREGROUND;
            }
            else
            {
                instButton.BorderBrush = Brushes.Black;
            }
           
            instButton.BorderThickness = new Thickness(1);

            if (isInstalled)
            {
                lblnumber.Foreground = Brushes.Green;
                lbldate.Foreground = Brushes.Green;
                lblname.Foreground = Brushes.Green;
                instButton.Foreground = Brushes.Green;
                instButton.Content = "УСТАНОВЛЕНО";
            }
            else
            {
                lblnumber.Foreground = Brushes.Red;
                lbldate.Foreground = Brushes.Red;
                lblname.Foreground = Brushes.Red;
                instButton.Foreground = Brushes.Red;
                instButton.Content = "УСТАНОВИТЬ";
                instButton.Tag = _position;
                instButton.MouseDown += installbtn_Click;

            }
            //   grid.Children.Add(instButton);
            //    Grid.SetRow(instButton, 0);
            //    Grid.SetColumn(instButton, 3);


            //view.Background = new ImageBrush(STYLE.BUTTON);
            //    view.Margin = new Thickness(20, 20, 20, 20);

            view.Orientation = Orientation.Horizontal;
            view.Children.Add(lblnumber);
            view.Children.Add(lbldate);
            view.Children.Add(lblname);
            view.Children.Add(instButton);


            return view;
        }
    }
}