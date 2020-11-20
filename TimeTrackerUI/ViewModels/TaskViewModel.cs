using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
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
        public ParameterlessCommand CopyReportCommand { get; set; }
        public ParameterlessCommand GenerateReportCommand { get; set; }

        public TaskViewModel()
        {
            string applicationJson;
            if (!File.Exists("data.json"))
                File.Create("data.json");
            using (StreamReader sr = new StreamReader("data.json"))
            {
                applicationJson = sr.ReadToEnd();
            }
            ApplicationState applicationState;
            if (applicationJson != string.Empty)
                applicationState = JsonSerializer.Deserialize<ApplicationState>(applicationJson);
            else
                applicationState = new ApplicationState()
                {
                    TaskModels = new TaskModel[0],
                    CurrentTaskModel = new TaskModel(),
                    IsSelectedTaskModelTracked = false,
                    CanStartEndToggleControlStart = true
                };
            this.TaskModels = new ObservableCollection<TaskModel>(applicationState.TaskModels);

            TaskModel selectedTaskModel = new TaskModel();
            foreach (TaskModel taskModel in this.TaskModels)
            {
                taskModel.SetDuration();
                if (taskModel.Equals(applicationState.CurrentTaskModel))
                    selectedTaskModel = taskModel;
            }
            this.TaskModelTypes = new string[] { "Example A", "Example B", "Example C", "Example D", "Example E" };

            this.SelectedTaskModelControl = new SelectedTaskModelControl
            {
                SelectedTaskModel = selectedTaskModel.Type == null ? applicationState.CurrentTaskModel : selectedTaskModel,
                IsSelectedTaskModelTracked = applicationState.IsSelectedTaskModelTracked
            };
            this.CurrentTaskModelType = this.SelectedTaskModelControl.SelectedTaskModel.Type == null ? this.TaskModelTypes[0] : this.SelectedTaskModelControl.SelectedTaskModel.Type;

            this.TimeTotalString = "Time Total: 0:00:00";
            this.TimeTotalStrings = new ObservableCollection<string>();
            this.GetTimeTotalStrings();

            this.StartEndToggleControl = new StartEndToggleControl(this.Start, this.End)
            {
                CanStart = applicationState.CanStartEndToggleControlStart,
                IsEnabled = !this.SelectedTaskModelControl.IsSelectedTaskModelTracked
            };

            this.SetCurrentTaskModelCommand = new Command<TaskModel>(this.SetCurrentTaskModel);

            this.SetNewCurrentTaskModelCommand = new ParameterlessCommand(this.SetNewCurrentTaskModel);
            this.AddCommand = new ParameterlessCommand(this.Add);
            this.UpdateCommand = new ParameterlessCommand(this.Update);
            this.RemoveCommand = new ParameterlessCommand(this.Remove);
            this.CopyReportCommand = new ParameterlessCommand(this.CopyReport);
            this.GenerateReportCommand = new ParameterlessCommand(this.GenerateReport);
        }

        private void SetNewCurrentTaskModel()
        {
            this.SetCurrentTaskModel(new TaskModel());
        }

        private void SetCurrentTaskModel(TaskModel taskModel)
        {
            this.SelectedTaskModelControl.SelectedTaskModel = taskModel;
            this.StartEndToggleControl.CanStart = true;

            this.SelectedTaskModelControl.IsSelectedTaskModelTracked = this.TaskModels.Contains(taskModel);
            this.CurrentTaskModelType = this.SelectedTaskModelControl.IsSelectedTaskModelTracked ? this.SelectedTaskModelControl.SelectedTaskModel.Type : this.TaskModelTypes[0];
            this.StartEndToggleControl.IsEnabled = !this.SelectedTaskModelControl.IsSelectedTaskModelTracked;

            this.Save();
        }

        private void Start()
        {
            this.SelectedTaskModelControl.SelectedTaskModel.Start();
            this.StartEndToggleControl.CanStart = false;
            this.Save();
        }

        private void End()
        {
            this.SelectedTaskModelControl.SelectedTaskModel.End();
            this.StartEndToggleControl.CanStart = true;
            this.Save();
        }

        private void Add()
        {
            this.SelectedTaskModelControl.SelectedTaskModel.Type = this.CurrentTaskModelType;
            this.TaskModels.Add(this.SelectedTaskModelControl.SelectedTaskModel);
            this.SetCurrentTaskModel(new TaskModel());
            this.GetTimeTotalStrings();
            this.Save();
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
            this.Save();
        }

        private void Remove()
        {
            TaskModel temp = this.SelectedTaskModelControl.SelectedTaskModel;
            this.TaskModels.Remove(temp);
            this.SetCurrentTaskModel(new TaskModel());
            this.GetTimeTotalStrings();
            this.Save();
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

        private void Save()
        {
            string json = JsonSerializer.Serialize<ApplicationState>(new ApplicationState()
            {
                TaskModels = this.TaskModels.ToArray(),
                CurrentTaskModel = this.SelectedTaskModelControl.SelectedTaskModel,
                IsSelectedTaskModelTracked = this.SelectedTaskModelControl.IsSelectedTaskModelTracked,
                CanStartEndToggleControlStart = this.StartEndToggleControl.CanStart
            });
            File.WriteAllText("data.json", json);
        }

        private string ReportString()
        {
            string reportString = string.Empty;
            foreach (TaskModel taskModel in this.TaskModels)
                reportString = $"{reportString}{taskModel}\n";
            return $"{reportString}{this.TimeTotalString}";
        }

        private void CopyReport()
        {
            Clipboard.SetDataObject(this.ReportString(), true);
        }

        private void GenerateReport()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text Files|*.txt"
            };
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, this.ReportString());
        }
    }
}
