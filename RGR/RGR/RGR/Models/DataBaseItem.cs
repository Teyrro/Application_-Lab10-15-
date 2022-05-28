using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RGR.Models
{
    public class DataBaseItem : INotifyPropertyChanged
    {
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
        private short isUsed;
        public short IsUsed
        {
            get => isUsed;
            set
            {
                isUsed = value;
                NotifyPropertyChanged();
            }
        }
        public DataBaseItem(string textValue, short isUsedValue)
        {
            Text = textValue;
            IsUsed = isUsedValue;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
