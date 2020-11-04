using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TimeTrackerUI.Models;

namespace TimeTrackerUI.ViewModels
{
    class TaskViewModel
    {
        public ObservableCollection<TaskModel> TaskModels { get; set; }

        public TaskViewModel()
        {
            this.TaskModels = new ObservableCollection<TaskModel>();
            for (int i = 0; i < 50; i++)
            {
                this.TaskModels.Add(new TaskModel($"Example {i + 1:00}"));
            }
            foreach (TaskModel taskModel in this.TaskModels)
            {
                taskModel.Start();
                taskModel.End();
            }
        }
    }
}
