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
            get { return $"{(int)this.Duration.TotalHours}:{this.Duration.Minutes:00}"; }
        }

        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return this._startTime; }
            set
            {
                this._startTime = value;
                if (this.EndTime != null)
                    this.SetDuration();
            }
        }

        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return this._endTime; }
            set
            {
                this._endTime = value;
                if (this.StartTime != null)
                    this.SetDuration();
            }
        }

        public TimeSpan Duration { get; set; }

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

        private void SetDuration()
        {
            this.Duration = this.EndTime - this.StartTime;
        }
    }
}
