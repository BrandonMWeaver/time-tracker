using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimeTrackerUI.Models.Base
{
    class NotificationBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
