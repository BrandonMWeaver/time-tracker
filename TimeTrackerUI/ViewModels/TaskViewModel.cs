using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TimeTrackerUI.Models;
using TimeTrackerUI.Models.Base;
using TimeTrackerUI.ViewModels.Commands;
using TimeTrackerUI.ViewModels.Controls;

namespace TimeTrackerUI.ViewModels
{
    class TaskViewModel : NotificationBase
    {
        public ObservableCollection<TaskModel> TaskModels { get; set; }

        private TaskModel _currentTaskModel;

        public TaskModel CurrentTaskModel
        {
            get { return this._currentTaskModel; }
            set
            {
                this._currentTaskModel = value;
                this.OnPropertyChanged(nameof(this.CurrentTaskModel));
            }
        }

        public StartEndToggleControl StartEndToggleControl { get; set; }

        public TaskViewModel()
        {
            this.TaskModels = new ObservableCollection<TaskModel>();
            this.CurrentTaskModel = new TaskModel
            {
                Type = "Example"
            };

            this.StartEndToggleControl = new StartEndToggleControl(this.Start, this.End)
            {
                CanStart = true
            };
        }

        private void Start()
        {
            this.CurrentTaskModel.Start();
            this.StartEndToggleControl.CanStart = false;
        }

        private void End()
        {
            this.CurrentTaskModel.End();
            this.TaskModels.Add(CurrentTaskModel);
            this.CurrentTaskModel = new TaskModel()
            {
                Type = "Example"
            };
            this.StartEndToggleControl.CanStart = true;
        }
    }
}
