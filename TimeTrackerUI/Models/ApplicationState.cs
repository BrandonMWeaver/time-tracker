using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTrackerUI.Models
{
    class ApplicationState
    {
        public TaskModel[] TaskModels { get; set; }
        public TaskModel CurrentTaskModel { get; set; }
        public bool IsSelectedTaskModelTracked { get; set; }
        public bool CanStartEndToggleControlStart { get; set; }
    }
}
