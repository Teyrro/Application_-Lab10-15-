using Avalonia.Controls;
using Avalonia.Data;
using System.Data;
using RGR.ViewModels;
using Avalonia.Interactivity;
using System;

namespace RGR.Views
{
    public partial class TableView : UserControl
    {
        public TableView()
        {
            InitializeComponent();
            dGrid.CellEditEnding += OnCellEditEnd;
        }

        private void OnCellEditEnd(object sender, DataGridCellEditEndingEventArgs args)
        {
            if (args.EditAction != DataGridEditAction.Commit) return;

            DataGrid dataGrid = sender as DataGrid;
            string index = args.Column.Header as string;
            string? a = (args.EditingElement as TextBox).Text;

            DataRowCollection rows = dataGrid.Items as DataRowCollection;
            try
            {
                rows[dataGrid.SelectedIndex].BeginEdit();
                rows[dataGrid.SelectedIndex][index] = a;
                rows[dataGrid.SelectedIndex].EndEdit();
            }
            catch (Exception)
            {

            }
        }
        public void OnSelect(object sender, SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count == 0) return;
            DataTable table = args.AddedItems[0] as DataTable;
            if (table != null)
            {
                var context = DataContext as TableViewModel;
                context.currentTableIndex = context.Tables.Tables.IndexOf(table);
                ChangeTable(table);
            }
        }

        public void OnDelTapControl(object sender, RoutedEventArgs args)
        {
            var context = DataContext as TableViewModel;
            if (context != null)
                context.DelTable(context.Tables.Tables[TCont.SelectedIndex]);
        }

        void ChangeTable(DataTable table)
        {
            int i = 0;
            var context = DataContext as TableViewModel;
            int Index = i;
            for (int ind = dGrid.Columns.Count - 1; ind >= 0; ind--)
            {
                dGrid.Columns.RemoveAt(ind);
            }
            /*dGrid.Columns.Clear();*/
            dGrid.Items = table.Rows;
            foreach (DataColumn col in table.Columns)
            {
                dGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = col.ColumnName,
                    Binding = new Binding($"ItemArray[{i++}]"),
                    IsReadOnly = false
                });
            }

        }

        public void OnAddRowButtonClick(object sender, RoutedEventArgs args)
        {
            var context = DataContext as TableViewModel;

            context.AddRow();

            ChangeTable(context.Tables.Tables[context.currentTableIndex]);

        }
        public void OnDelRowButtonClick(object sender, RoutedEventArgs args)
        {
            var context = DataContext as TableViewModel;
            context.DeleteRow();
            ChangeTable(context.Tables.Tables[context.currentTableIndex]);
        }

        public void OnExButtonClick(object sender, RoutedEventArgs args)
        {
            var context = DataContext as TableViewModel;
            context.Execute();
            TCont.Items = context.dataTables;




            /*ChangeTable(context.Tables.Tables[context.currentTableIndex]);*/
        }
        public void OnSaveButtonClick(object sender, RoutedEventArgs args)
        {
            var context = DataContext as TableViewModel;
            context.OnClick();
            ChangeTable(context.Tables.Tables[context.currentTableIndex]);
        }

    }
}
