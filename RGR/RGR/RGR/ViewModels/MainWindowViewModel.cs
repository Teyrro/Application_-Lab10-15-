using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace RGR.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase currentView;

        public ViewModelBase CurrentView
        {
            set => this.RaiseAndSetIfChanged(ref currentView, value);
            get => currentView;
        }

        private float minWidth, maxWidth, width;
        public float MinWidth { get => minWidth; set => this.RaiseAndSetIfChanged(ref minWidth, value); }
        public float MaxWidth { get => maxWidth; set => this.RaiseAndSetIfChanged(ref maxWidth, value); }
        public float Width { get => width; set => this.RaiseAndSetIfChanged(ref width, value); }

        public TableViewModel mainView
        {
            get;
        }

        public void Change()
        {
            if (CurrentView is TableViewModel)
            {
                var vm = new RequestManagerViewModel(mainView.Tables, mainView.Requests);
                Observable.Merge(vm.Send).Take(1).Subscribe(msg =>
                {
                    if (msg != null)
                    {
                        mainView.Requests = msg;
                    }
                    CurrentView = mainView;
                });
                CurrentView = vm;
                Width = 600;
                MinWidth = 600;
            }
            else if (currentView is RequestManagerViewModel) {
                CurrentView = new TableViewModel();
                Width = 800;
                MinWidth = 450;
            } 
        }

        public MainWindowViewModel()
        {
            CurrentView = mainView = new TableViewModel();
            Width = 800;
            MinWidth = 450;
            MaxWidth = 800;
        }
    }
}
