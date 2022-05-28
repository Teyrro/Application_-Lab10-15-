using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using RGR.ViewModels;
using System.Diagnostics;
using System.Threading;
using RGR.Models;

namespace RGR.Views
{
    public partial class RequestManagerView : UserControl
    {
        public RequestManagerView()
        {
            InitializeComponent();
        }

        private void OnCheckBoxTableClick(object sender, RoutedEventArgs args)
        {
            var context = DataContext as RequestManagerViewModel;
            context.FillColumnsView();
            context.FillPartRequest(out context.SubRequest[1], "FROM", context.TableNameCollection);
        }

        private void OnCheckBoxColumnClick(object sender, RoutedEventArgs args)
        {
            var context = DataContext as RequestManagerViewModel;
            context.FillSelectedColumnsView();
        }

        private void OnComboBoxSelectedClick(object sender, SelectionChangedEventArgs args)
        {
            var context = DataContext as RequestManagerViewModel;   
            if (context != null)
                context.FillPartRequestSelect(out context.SubRequest[0], "SELECT", context.SelectedColumnNameCollection);
        }
        

        private void OnJoinClick(object sender, RoutedEventArgs args)
        {
            var context = DataContext as RequestManagerViewModel;
            if (context != null)
                context.FillRequestJoinClick(out context.SubRequest[2]);
        }

        private void OnComboBoxWhereClick(object sender, SelectionChangedEventArgs args)
        {
            var context = DataContext as RequestManagerViewModel;
          
            var box = sender as ComboBox;
            DataBaseItem a = box.SelectedItem as DataBaseItem;
            if (context != null)
                context.FillPartWhereHaving(out context.SubRequest[3], "WHERE");
        }

        private void OnComboBoxHavingClick(object sender, SelectionChangedEventArgs args)
        {
            var context = DataContext as RequestManagerViewModel;

            var box = sender as ComboBox;
            DataBaseItem a = box.SelectedItem as DataBaseItem;
            if (context != null)
                context.FillPartWhereHaving(out context.SubRequest[4], "HAVING");
        }

        private void OnComboBoxTableSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var context = DataContext as RequestManagerViewModel;
            context.FillJoinColumnsView();
        }
        private void OnExecuteClick(object sender, RoutedEventArgs args)
        {
            var context = DataContext as RequestManagerViewModel;
            context.CreateRequest();
            
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
