using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using TimeTrackerUI.Models;
using TimeTrackerUI.Models.Base;

namespace TimeTrackerUI.ViewModels.Controls
{
    class SelectedTaskModelControl : NotificationBase
    {
        private TaskModel _selectedTaskModel;

        public TaskModel SelectedTaskModel
        {
            get { return this._selectedTaskModel; }
            set
            {
                this._selectedTaskModel = value;
                this.OnPropertyChanged(nameof(this.SelectedTaskModel));
            }
        }

        private bool _isSelectedTaskModelTracked;

        public bool IsSelectedTaskModelTracked
        {
            get { return this._isSelectedTaskModelTracked; }
            set
            {
                this._isSelectedTaskModelTracked = value;
                this.AddButtonVisibility = value ? Visibility.Hidden : Visibility.Visible;
                this.ManipulatorButtonVisibility = value ? Visibility.Visible : Visibility.Hidden;
                this.BackgroundColor = value ? "#CDCDFF" : "#CDCDCD";
                this.OnPropertyChanged(nameof(this.IsSelectedTaskModelTracked));
            }
        }

        private Visibility _addButtonVisibility;

        public Visibility AddButtonVisibility
        {
            get { return this._addButtonVisibility; }
            set
            {
                this._addButtonVisibility = value;
                this.OnPropertyChanged(nameof(this.AddButtonVisibility));
            }
        }

        private Visibility _manipulatorButtonVisibility;

        public Visibility ManipulatorButtonVisibility
        {
            get { return this._manipulatorButtonVisibility; }
            set
            {
                this._manipulatorButtonVisibility = value;
                this.OnPropertyChanged(nameof(this.ManipulatorButtonVisibility));
            }
        }

        private string _backgroundColor;

        public string BackgroundColor
        {
            get { return this._backgroundColor; }
            set
            {
                this._backgroundColor = value;
                this.OnPropertyChanged(nameof(this.BackgroundColor));
            }
        }

        public SelectedTaskModelControl()
        {
            this.IsSelectedTaskModelTracked = false;
        }
    }
}
