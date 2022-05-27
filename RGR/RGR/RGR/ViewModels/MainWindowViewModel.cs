using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;

namespace RGR.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private SQLiteConnection sql_con;
        private DataSet tables;
        int selectRow;

        public int currentTableIndex;


        
        public int SelectRow
        {
            get => selectRow;
            private set => this.RaiseAndSetIfChanged(ref selectRow, value);
        }

        public DataSet Tables
        {
            get => tables;
            private set => this.RaiseAndSetIfChanged(ref tables, value);
        }
        public MainWindowViewModel()
        {
            string sql = "SELECT name FROM sqlite_master WHERE type=\"table\" ORDER BY 1";
            string connectionStr = "Data Source=firstAttempt_NewV.db;Mode=ReadWrite";
            
            using (sql_con = new SQLiteConnection(connectionStr))
            {
                sql_con.Open();
                SQLiteCommand command = new SQLiteCommand(sql, sql_con);
                DataTable tablesNames = new DataTable();
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                
                    tablesNames.Load(reader);
                    tables = new DataSet();
                    string subsql = "SELECT * FROM ";
                    foreach (DataRow row in tablesNames.Rows)
                    {
                        string name = row.ItemArray[0].ToString();
                        if (name == "sqlite_sequence") continue;
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(subsql + name, sql_con);
                        adapter.FillSchema(tables, SchemaType.Source, name);
                        adapter.Fill(tables, name);
                        /*SQLiteCommand sqlTab = new SQLiteCommand(subsql + name, sql_con);
                        DataTable table = new DataTable();
                        table.Load(sqlTab.ExecuteReader());
                        tables.Tables.Add(table);*/
                    }
                }
            }
        }


        public void AddRow()
        {
            DataRow row = tables.Tables[currentTableIndex].NewRow();

            for (int i = 1; i < row.ItemArray.Length ; i++)
            {
                row.ItemArray[i] = '1';
            }
            tables.Tables[currentTableIndex].Rows.Add(row);
        }

        public void DeleteRow()
        {
            tables.Tables[currentTableIndex].Rows[selectRow].Delete();
          /*  tables.Tables[currentTableIndex].Rows.RemoveAt(selectRow);*/

        }

        public void OnClick()
        {
            string connectionStr = "Data Source=firstAttempt_NewV.db;Mode=ReadWrite";

            using (sql_con = new SQLiteConnection(connectionStr))
            {
                for (int i = 0; i < tables.Tables.Count; i++){
                
                    try
                    {
                        SQLiteDataAdapter adapter = new SQLiteDataAdapter("SELECT * FROM " + tables.Tables[i].TableName, sql_con);

                        SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(adapter);
                        adapter.Update(tables.Tables[i]);

                    }
                    catch (SqlException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }
                        tables.AcceptChanges();
            }
        }
        ~MainWindowViewModel()
        {
            sql_con.Close();
        }

    }
}
