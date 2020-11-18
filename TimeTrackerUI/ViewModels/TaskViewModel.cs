using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public SelectedTaskModelControl SelectedTaskModelControl { get; set; }

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

        private string _timeTotalString;

        public string TimeTotalString
        {
            get { return this._timeTotalString; }
            set
            {
                this._timeTotalString = value;
                this.OnPropertyChanged(nameof(this.TimeTotalString));
            }
        }

        private ObservableCollection<string> _timeTotalStrings;

        public ObservableCollection<string> TimeTotalStrings
        {
            get { return this._timeTotalStrings; }
            set
            {
                this._timeTotalStrings = value;
                this.OnPropertyChanged(nameof(this.TimeTotalStrings));
            }
        }

        public string[] TaskModelTypes { get; set; }

        public StartEndToggleControl StartEndToggleControl { get; set; }

        public Command<TaskModel> SetCurrentTaskModelCommand { get; set; }

        public ParameterlessCommand SetNewCurrentTaskModelCommand { get; set; }
        public ParameterlessCommand AddCommand { get; set; }
        public ParameterlessCommand UpdateCommand { get; set; }
        public ParameterlessCommand RemoveCommand { get; set; }

        public TaskViewModel()
        {
            this.TaskModels = new ObservableCollection<TaskModel>();
            this.TaskModelTypes = new string[] { "Example A", "Example B", "Example C", "Example D", "Example E" };

            this.SelectedTaskModelControl = new SelectedTaskModelControl();
            this.SelectedTaskModelControl.SelectedTaskModel = new TaskModel();
            this.CurrentTaskModelType = this.TaskModelTypes[0];

            this.TimeTotalString = "Time Total: 0:00:00";
            this.TimeTotalStrings = new ObservableCollection<string>();

            this.StartEndToggleControl = new StartEndToggleControl(this.Start, this.End)
            {
                CanStart = true
            };

            this.SetCurrentTaskModelCommand = new Command<TaskModel>(this.SetCurrentTaskModel);

            this.SetNewCurrentTaskModelCommand = new ParameterlessCommand(this.SetNewCurrentTaskModel);
            this.AddCommand = new ParameterlessCommand(this.Add);
            this.UpdateCommand = new ParameterlessCommand(this.Update);
            this.RemoveCommand = new ParameterlessCommand(this.Remove);
        }

        private void SetNewCurrentTaskModel()
        {
            this.SetCurrentTaskModel(new TaskModel());
        }

        private void SetCurrentTaskModel(TaskModel taskModel)
        {
            this.SelectedTaskModelControl.SelectedTaskModel = taskModel;
            this.StartEndToggleControl.CanStart = true;

            if (this.TaskModels.Contains(taskModel))
                this.SelectedTaskModelControl.IsSelectedTaskModelTracked = true;
            else
                this.SelectedTaskModelControl.IsSelectedTaskModelTracked = false;
        }

        private void Start()
        {
            this.SelectedTaskModelControl.SelectedTaskModel.Start();
            this.StartEndToggleControl.CanStart = false;
        }

        private void End()
        {
            this.SelectedTaskModelControl.SelectedTaskModel.End();
            this.StartEndToggleControl.CanStart = true;
        }

        private void Add()
        {
            this.SelectedTaskModelControl.SelectedTaskModel.Type = this.CurrentTaskModelType;
            this.TaskModels.Add(this.SelectedTaskModelControl.SelectedTaskModel);
            this.SelectedTaskModelControl.SelectedTaskModel = new TaskModel();
            this.GetTimeTotalStrings();
        }

        private void Update()
        {
            this.SelectedTaskModelControl.SelectedTaskModel.Type = this.CurrentTaskModelType;
            TaskModel temp = this.SelectedTaskModelControl.SelectedTaskModel;
            int index = this.TaskModels.IndexOf(this.SelectedTaskModelControl.SelectedTaskModel);
            this.TaskModels.Remove(this.SelectedTaskModelControl.SelectedTaskModel);
            this.TaskModels.Insert(index, temp);
            this.SetCurrentTaskModel(new TaskModel());
            GetTimeTotalStrings();
        }

        private void Remove()
        {
            TaskModel temp = this.SelectedTaskModelControl.SelectedTaskModel;
            this.TaskModels.Remove(temp);
            this.SetCurrentTaskModel(new TaskModel());
            this.GetTimeTotalStrings();
        }

        private void GetTimeTotalStrings()
        {
            this.TimeTotalStrings = new ObservableCollection<string>();

            TimeSpan timeSpan = new TimeSpan();
            foreach (TaskModel taskModel in this.TaskModels)
                timeSpan = timeSpan.Add(taskModel.Duration);
            this.TimeTotalString = $"Time Total: {(int)timeSpan.TotalHours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";

            foreach (string taskModelType in this.TaskModelTypes)
            {
                timeSpan = new TimeSpan();
                bool isTimed = false;
                foreach (TaskModel taskModel in this.TaskModels.Where(t => t.Type == taskModelType))
                {
                    timeSpan = timeSpan.Add(taskModel.Duration);
                    if (!isTimed)
                        isTimed = true;
                }
                if (isTimed)
                    this.TimeTotalStrings.Add($"{taskModelType} Time: {(int)timeSpan.TotalHours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}");
            }
        }
    }
}
