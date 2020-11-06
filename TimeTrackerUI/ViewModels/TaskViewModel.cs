using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TimeTrackerUI.Models;
using TimeTrackerUI.Models.Base;
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

        private string _currentTaskModelType;

        public string CurrentTaskModelType
        {
            get { return this._currentTaskModelType; }
            set
            {
                this._currentTaskModelType = value;
                this.OnPropertyChanged(nameof(this.CurrentTaskModelType));
            }
        }

        public string[] TaskModelTypes { get; set; }

        public StartEndToggleControl StartEndToggleControl { get; set; }

        public TaskViewModel()
        {
            this.TaskModels = new ObservableCollection<TaskModel>();
            this.TaskModelTypes = new string[] { "Example A", "Example B", "Example C" };

            this.CurrentTaskModel = new TaskModel();
            this.CurrentTaskModelType = this.TaskModelTypes[0];

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
            this.StartEndToggleControl.CanStart = true;
            this.Add();
        }

        private void Add()
        {
            this.CurrentTaskModel.Type = this.CurrentTaskModelType;
            this.TaskModels.Add(this.CurrentTaskModel);
            this.CurrentTaskModel = new TaskModel();
        }
    }
}
