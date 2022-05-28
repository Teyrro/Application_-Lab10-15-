using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data;
using ReactiveUI;
using RGR.Models;
using System.Reactive;

namespace RGR.ViewModels
{
    public class RequestManagerViewModel : ViewModelBase
    {
        private string[] symbols = { "none", "=", "<>", "<", ">", "<=", ">=", "!<", "!>"};
        private string[] agregateFunc = { "none", "AVG", "SUM", "MIN", "MAX", "COUNT" };
        public string[] Symbols 
        { 
            get => symbols;
        }
        public string[] AgregateFunc
        {
            get => agregateFunc;
        }

        private int selectedJoinTableIndex;
        public int SelectedJoinTableIndex
        {
            get => selectedJoinTableIndex;
            set => this.RaiseAndSetIfChanged(ref selectedJoinTableIndex, value);
        }

        private int selectedRequestIndex;
        public int SelectedRequestIndex
        {
            get => selectedRequestIndex;
            set => this.RaiseAndSetIfChanged(ref selectedRequestIndex, value);
        }

        private DataSet recievedTables;
        public DataSet RecievedTables
        {
            get => recievedTables;
            set => this.RaiseAndSetIfChanged(ref recievedTables, value);
        }

        private string whereCondition, havingCondition;

        public string WhereCondition
        {
            get => whereCondition;
            set => this.RaiseAndSetIfChanged(ref whereCondition, value);
        }

        public string HavingCondition
        {
            get => havingCondition;
            set => this.RaiseAndSetIfChanged(ref havingCondition, value);
        }

        private int selectedWhereIndex;
        public int SelectedWhereIndex
        {
            get => selectedWhereIndex;
            set => this.RaiseAndSetIfChanged(ref selectedWhereIndex, value);
        }

        private int selectedHavingIndex;
        public int SelectedHavingIndex
        {
            get => selectedHavingIndex;
            set => this.RaiseAndSetIfChanged(ref selectedHavingIndex, value);
        }

        private int selectedJoinColumn1Index;
        public int SelectedJoinColumn1Index
        {
            get => selectedJoinColumn1Index;
            set => this.RaiseAndSetIfChanged(ref selectedJoinColumn1Index, value);
        }

        private int selectedJoinColumn2Index;
        public int SelectedJoinColumn2Index
        {
            get => selectedJoinColumn2Index;
            set => this.RaiseAndSetIfChanged(ref selectedJoinColumn2Index, value);
        }

        private int selectedWhereSymbolIndex;
        public int SelectedWhereSymbolIndex
        {
            get => selectedWhereSymbolIndex;
            set => this.RaiseAndSetIfChanged(ref selectedWhereSymbolIndex, value);
        }

        private int selectedHavingSymbolIndex;
        public int SelectedHavingSymbolIndex
        {
            get => selectedHavingSymbolIndex;
            set => this.RaiseAndSetIfChanged(ref selectedHavingSymbolIndex, value);
        }

        public ObservableCollection<DataBaseItem> TableNameCollection { get; set; }
        public ObservableCollection<DataBaseItem> ColumnNameCollection { get; set; }
        public ObservableCollection<DataBaseItem> GroupByColumnNameCollection { get; set; }
        public ObservableCollection<DataBaseItem> SelectedColumnNameCollection { get; set; }
        public ObservableCollection<DataBaseItem> JoinTableNameCollection { get; set; }
        public ObservableCollection<DataBaseItem> JoinColumnNameCollection { get; set; }

        public string request;

        public string Request
        {
            get => request;
            set => this.RaiseAndSetIfChanged(ref request, value);
        }

        List<RequestData> requests;

        public List<RequestData> Requests
        {
            get => requests;
            set => this.RaiseAndSetIfChanged(ref requests, value);
        }

        string[] subrequest;
        public string[] SubRequest
        {
            get => subrequest;
            set => this.RaiseAndSetIfChanged(ref subrequest, value);
        }

        public ReactiveCommand<Unit, List<RequestData>> Send { get; set; }
        public RequestManagerViewModel(DataSet tablesValue, List<RequestData> requests)
        {
            Send = ReactiveCommand.Create(() => Requests);

            RecievedTables = tablesValue;
            TableNameCollection = new ObservableCollection<DataBaseItem>();
            ColumnNameCollection = new ObservableCollection<DataBaseItem>();
            GroupByColumnNameCollection = new ObservableCollection<DataBaseItem>();
            SelectedColumnNameCollection = new ObservableCollection<DataBaseItem>();
            JoinTableNameCollection = new ObservableCollection<DataBaseItem>();
            JoinColumnNameCollection = new ObservableCollection<DataBaseItem>();

            if (requests != null)
                Requests = requests;
            else
                Requests = new List<RequestData>(1);
            Requests.Add(new RequestData("Request " + Convert.ToString(Requests.Count), ""));
            SubRequest = new string[6];
            
            SelectedJoinTableIndex = 0;
            SelectedRequestIndex = 0;
            SelectedHavingIndex = 0;
            SelectedWhereIndex = 0;
            SelectedWhereSymbolIndex = 0;
            SelectedHavingSymbolIndex = 0;
            FillTablesView();
            FillColumnsView();
        }

        public void FillPartRequest(out string request, string nameCommand, ObservableCollection<DataBaseItem> data)
        {
            request = "";
            request += nameCommand;
            foreach (DataBaseItem item in data)
            {
                if (item.IsUsed == 1)
                    request += " " + item.Text;
            }
        }

        public void FillPartWhereHaving(out string request, string nameCommand)
        {
            request = "";
            request += nameCommand;
            if (nameCommand == "WHERE")
            {
                request += " " + ColumnNameCollection[SelectedWhereIndex].Text;
                request += " " + symbols[SelectedWhereSymbolIndex];
                request += " " + WhereCondition;
            }
            else if (nameCommand == "HAVING")
            {
                request += " " + ColumnNameCollection[SelectedHavingIndex].Text;
                request += " " + symbols[SelectedHavingSymbolIndex];
                request += " " + HavingCondition;
            }            
        }

        public void FillPartRequestSelect(out string request, string nameCommand, ObservableCollection<DataBaseItem> data)
        {
            request = "";
            request += nameCommand;
            foreach (DataBaseItem item in data)
            {
                if (item.IsUsed != 0)
                {
                    request += " " + agregateFunc[item.IsUsed] + "(" + item.Text + "),";
                }
                else
                {
                    request += " " + item.Text + ",";
                }
            }
            request = request.Remove(request.Length - 1);
        }

        public void FillRequestJoinClick(out string request)
        {
            request = "";
            request += "JOIN " + JoinTableNameCollection[selectedJoinTableIndex].Text + " ON " + ColumnNameCollection[selectedJoinColumn1Index].Text + " " + symbols[1] + " " + JoinColumnNameCollection[selectedJoinColumn2Index].Text;
            
        }

        public void FillTablesView()
        {
            for (int i = 0; i < RecievedTables.Tables.Count; i++)
            {
                TableNameCollection.Add(new DataBaseItem(RecievedTables.Tables[i].TableName, 0));
            }
        }

        public void FillColumnsView()
        {
            ColumnNameCollection.Clear();
            GroupByColumnNameCollection.Clear();
            JoinTableNameCollection.Clear();
            for (int i = 0; i < RecievedTables.Tables.Count; i++)
            {
                if (TableNameCollection[i].IsUsed == 1)
                {
                    for (int j = 0; j < RecievedTables.Tables[i].Columns.Count; j++)
                    {
                        ColumnNameCollection.Add(new DataBaseItem(RecievedTables.Tables[i].Columns[j].ColumnName, 0));
                        GroupByColumnNameCollection.Add(new DataBaseItem(RecievedTables.Tables[i].Columns[j].ColumnName, 0));
                    }
                }
                else if (TableNameCollection[i].IsUsed == 0)
                {
                    JoinTableNameCollection.Add(TableNameCollection[i]);
                }
            }
        }

        public void FillJoinColumnsView()
        {
            JoinColumnNameCollection.Clear();
            for(int i = 0; i < RecievedTables.Tables[JoinTableNameCollection[SelectedJoinTableIndex].Text].Columns.Count; i++)
            {
                JoinColumnNameCollection.Add(new DataBaseItem(RecievedTables.Tables[JoinTableNameCollection[SelectedJoinTableIndex].Text].Columns[i].ColumnName, 0));
            }
        }

        public void CreateRequest()
        {
            Requests[SelectedRequestIndex].Text = "";
            for (int i = 0; i < SubRequest.Length; i++)
            {
                if (SubRequest[i] != "" || SubRequest[i] != null)
                {
                    Requests[SelectedRequestIndex].Text += SubRequest[i];
                    Requests[SelectedRequestIndex].Text += "\n";
                }
            }
            Request = Requests[SelectedRequestIndex].Text;
        }

        public void FillSelectedColumnsView()
        {
            SelectedColumnNameCollection.Clear();
            for (int i = 0; i < ColumnNameCollection.Count; i++)
            {
                if (ColumnNameCollection[i].IsUsed == 1)
                {
                    SelectedColumnNameCollection.Add(new DataBaseItem(ColumnNameCollection[i].Text, 0));
                }
            }
        }

        public void ClearRequest()
        {
            Request = "";
            WhereCondition = "";
            HavingCondition = "";
            for (int i = 0; i < TableNameCollection.Count; i++)
                TableNameCollection[i].IsUsed = 0;
            ColumnNameCollection.Clear();
            GroupByColumnNameCollection.Clear();
            SelectedColumnNameCollection.Clear();
            JoinTableNameCollection.Clear();
            JoinColumnNameCollection.Clear();
        }
    }
}
