using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace RGR.Models
{
    public class RequestData : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        private string text;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                NotifyPropertyChanged();
            }
        }
        public RequestData(string nameValue, string textValue)
        {
            Name = nameValue;
            Text = textValue;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
