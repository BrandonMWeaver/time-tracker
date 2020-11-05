using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTrackerUI.Models
{
    class TaskModel
    {
        public string Type { get; set; }

        public string StartTimeString
        {
            get { return this.StartTime.ToString("h:mm tt"); }
        }

        public string EndTimeString
        {
            get { return this.EndTime.ToString("h:mm tt"); }
        }

        public string DurationString
        {
            get { return this.Duration.ToString(@"h\:mm"); }
        }

        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }

        private TimeSpan Duration { get; set; }

        public TaskModel(string type)
        {
            this.Type = type;
        }

        public void Start()
        {
            this.StartTime = DateTime.Now;
            if (this.EndTime != null)
                this.SetDuration();
        }

        public void End()
        {
            this.EndTime = DateTime.Now;
            if (this.StartTime != null)
                this.SetDuration();
        }

        private void SetDuration()
        {
            this.Duration = this.EndTime - this.StartTime;
        }
    }
}
