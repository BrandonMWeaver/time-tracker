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
            this.TaskModels = new ObservableCollection<TaskModel>()
            {
                new TaskModel("Example")
            };
            this.TaskModels[0].Start();
            this.TaskModels[0].End();
        }
    }
}
