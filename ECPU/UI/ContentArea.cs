using ECPU.Exceptions;
using ECPU.LoadOrderUtility;
using ECPU.Settings;
using ECPU.WINDOW_ITEMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ECPU.UI
{
   

   public class ContentArea : Grid
    {

       private string windowtitle;
        private string tableBD;

        private int itemsInWidth;
        private int itemsInHeight;
        private int itemsCount;
        private double itemSpace;
        private double itemWidth;
        private double itemHeight;
        private double fontSize;
        public bool autoAllSettings;
        private List<CPWindowItem> items;

        

        public ContentArea(string _windowtitle)
        {
            windowtitle = _windowtitle;

                tableBD = windowtitle;
                try
                {
                    getItemsData();
                }
                catch
                {
                    autoAllSettings = true;

                }
            createItems();
            build();
            if (!autoAllSettings)
            {
                Width = (itemWidth + itemSpace) * itemsInWidth;
                Height = (itemHeight + itemSpace) * itemsInHeight;
            }
            Margin = new Thickness(15);
        }

        public ContentArea()
        {            
        }

        private void createItems()
        {
            if (autoAllSettings)
            {
                items = new List<CPWindowItem>();
            }else
            {
                items = new List<CPWindowItem>(itemsCount);
            }
           
            SQLiteManager mngr = new SQLiteManager();
            DataTable dataItems = mngr.getListItems(tableBD);
            foreach (DataRow row in dataItems.Rows)
            {
                CPWindowItem item = null;
       
                switch (row[3].ToString())
                { 
                    case "INI":
                        switch (row[8].ToString())
                        {
                            case "checkbox":
                               item = new INI_CHECKBOX_Setting(
                               row[0].ToString(),
                               row[1].ToString(),
                               row[2].ToString(),
                               Convert.ToInt32(row[17]),
                               row[7].ToString(),
                               row[4].ToString(),
                               row[5].ToString(),
                               row[6].ToString(),
                               row[14].ToString(),
                               row[15].ToString()
                              );
                                items.Add(item);
                                break;
                            case "trackbar":
                                item = new INI_TRACKBAR_Setting(
                            row[0].ToString(),
                               row[1].ToString(),
                               row[2].ToString(),
                               Convert.ToInt32(row[17]),
                               row[7].ToString(),
                               row[4].ToString(),
                               row[5].ToString(),
                               row[6].ToString(),
                            row[9].ToString(),
                             row[10].ToString(),
                             row[11].ToString(),
                              row[12].ToString()
                              );
                                items.Add(item);
                                break;
                            case "combobox":
                                item = new INI_COMBOBOX_Setting(
                            row[0].ToString(),
                               row[1].ToString(),
                               row[2].ToString(),
                               Convert.ToInt32(row[17]),
                               row[7].ToString(),
                               row[4].ToString(),
                               row[5].ToString(),
                               row[6].ToString(),
                               row[12].ToString());
                                items.Add(item);
                                break;
                            default:
                                MessageBox.Show("Тип " + row[8].ToString() + "не предусмотрен в базе данных");
                                break;
                        }
                        break;
                    case "APP":
                        item = new MainMenuButton(
                           row[0].ToString(),
                           row[1].ToString(),
                           row[2].ToString(),
                           Convert.ToInt32(row[6]),
                          row[3].ToString(),// "APP",
                           row[4].ToString());
                        items.Add(item);
                        break;
                    case "WINDOW":
                        item = new MainMenuButton(
                           row[0].ToString(),
                           row[1].ToString(),
                           row[2].ToString(),
                           Convert.ToInt32(row[6]),
                          row[3].ToString(),//  "WINDOW",
                           row[5].ToString());
                        items.Add(item);
                        break;
                    case "CP_UTIL":
                        item = new MainMenuButton(
                           row[0].ToString(),
                           row[1].ToString(),
                           row[2].ToString(),
                           Convert.ToInt32(row[6]),
                          row[3].ToString(),
                          row[7].ToString());
                        items.Add(item);
                        break;                  
                    case "RESOLUTION":
                        item = new Resolution_Item(
                          row[0].ToString(),
                          row[1].ToString(),
                          row[2].ToString(),
                          Convert.ToInt32(row[7]),
                          row[7].ToString(),
                          row[4].ToString(),
                          row[5].ToString(),
                          row[6].ToString(),
                          row[12].ToString());
                        items.Add(item);
                        break;
                    case "BSA":
                        item = new BSA_Setting(
                          row[0].ToString(),
                          row[1].ToString(),
                          row[2].ToString(),
                          Convert.ToInt32(row[17]),
                          row[4].ToString(),
                          row[5].ToString(),
                          row[6].ToString(),
                          row[13].ToString());
                        items.Add(item);
                        break;
                    case "BSA_REPLACE":
                       
                        item = new BSAReplace_Setting(
                          row[0].ToString(),
                          row[1].ToString(),
                          row[2].ToString(),
                          Convert.ToInt32(row[17]),
                           row[4].ToString(),
                          row[5].ToString(),
                          row[6].ToString(),
                          row[13].ToString().Split(','),
                          row[16].ToString().Split(','));
                        items.Add(item);
                        break;
                    case "ENB_OPTION":
                          item = new ENB_PRESET_ITEM(
                                Convert.ToInt32(row[4]),
                                row[0].ToString(),
                               INIT.ENB_DIR + row[1].ToString(),
                                row[2].ToString(),
                                row[5].ToString()
                                );
                        items.Add(item);
                        break;
                    case "SHADER_OPTION":
                        item = new SHADER_OPTION(
                              Convert.ToInt32(row[4]),
                              row[0].ToString(),
                              row[1].ToString(),
                              row[2].ToString(),
                              row[5].ToString()
                              );
                        items.Add(item);
                        break;
                    case "TEXT":
                        item = new TextItem(
                          row[0].ToString(),
                          row[1].ToString(),
                          row[2].ToString(),
                          Convert.ToInt32(row[4]));
                        items.Add(item);
                        break;
                    case "HTTP":
                        item = new MainMenuButton(
                           row[0].ToString(),
                           row[1].ToString(),
                           row[2].ToString(),
                           Convert.ToInt32(row[6]),
                            row[2].ToString(), //"HTTP",
                          row[4].ToString());
                        items.Add(item);
                        break;
                    case "UPDATE_ITEM":
                        item = new UPDATE_ITEM(
                            Convert.ToInt32(row[0]),
                            row[1].ToString(),
                            row[4].ToString(),
                            row[2].ToString(),
                            Convert.ToBoolean(row[5]),
                            row[6].ToString(),
                            row[7].ToString());
                        items.Add(item);
                        break;
                    default:                    
                        throw new TypeOfWindowItemNotFoundException(row[3].ToString());
                }

            }
            items = items.OrderBy(x => x.getPosition()).ToList();
            if (autoAllSettings)
            {
                itemsCount = items.Count;
                itemsInHeight = 1;
                itemsInWidth = itemsCount;
            }
           
        }

        private void build()
        {

          
            for (int j = 0; j < itemsInHeight; j++)
            {
                RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < itemsInWidth; j++)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < itemsInHeight * itemsInWidth; i++)
            {
                for (int j = 0; j < itemsInHeight; j++)
                {
                   
                   
                    for (int k = 0; k < itemsInWidth; k++)
                    {
                        if (!autoAllSettings)
                        {
                            if (fontSize<=0)
                            {
                                fontSize = 14.0;                            }
                            items[i].setFontSize(fontSize);
                            items[i].setWidth(itemWidth);
                            items[i].setHeight(itemHeight);
                        }else
                        {
                            items[i].setFontSize(14.0);
                            items[i].autoConuntAndSize = true;
                        }



                     
                        StackPanel itemOnForm = items[i].getView();
                        if (!INIT.DEFAULT_VISUAL_STYLE)
                        {
                            itemOnForm.Background = new ImageBrush(STYLE.BUTTON);

                        }else
                        {
                            itemOnForm.Background = Brushes.WhiteSmoke;
                        }
                        if (!windowtitle.Equals("WINDOW_MAINMENU"))
                        {
                            itemOnForm.ToolTip = items[i].getDescription();
                        }
                        Children.Add(itemOnForm);
                        SetRow(itemOnForm, j);
                        SetColumn(itemOnForm, k);
                                         
                        i++;
                       
                    }

                }
            }
        }

        private void getItemsData()
        {
            DataTable itemsTable = new DataTable();
            SQLiteManager mngr = new SQLiteManager();
            itemsTable = mngr.getListItems("WINDOWS");
            mngr.ConnectionClose();
            foreach (DataRow row in itemsTable.Rows)
            {
                if (row[0].ToString().Equals(tableBD))
                {
                    itemsInWidth = Convert.ToInt32(row[1]);
                    itemsInHeight = Convert.ToInt32(row[2]);
                    itemSpace = Convert.ToDouble(row[3]);
                    itemWidth = Convert.ToDouble(row[5]);
                    itemHeight = Convert.ToDouble(row[4]);
                    try
                    {
                        fontSize = Convert.ToDouble(row[6]);
                    }
                    catch
                    {
                        fontSize = 14.0;
                    }
                    
                    break;
                }
            }
            itemsCount = itemsInWidth * itemsInHeight;

        }
    }
}
