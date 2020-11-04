using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTrackerUI.Models
{
    class TaskModel
    {
        public string Type { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TaskModel(string type)
        {
            this.Type = type;
        }

        public void Start()
        {
            this.StartTime = DateTime.Now;
        }

        public void End()
        {
            this.EndTime = DateTime.Now;
        }
    }
}
