using System;
using System.Collections.Generic;
using System.Text;
using TimeTrackerUI.Models.Base;

namespace TimeTrackerUI.Models
{
    class TaskModel : NotificationBase
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
                this.OnPropertyChanged(nameof(this.StartTime));
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
                this.OnPropertyChanged(nameof(this.EndTime));
                if (this.StartTime != null)
                    this.SetDuration();
            }
        }

        public TimeSpan Duration { get; set; }

        public void Start()
        {
            this.StartTime = DateTime.Now;
        }

        public void End()
        {
            this.EndTime = DateTime.Now;
        }

        public void SetDuration()
        {
            this.Duration = this.EndTime - this.StartTime;
        }

        public override bool Equals(object obj)
        {
            TaskModel other = obj as TaskModel;

            if (other == null)
                return false;

            return this.Type == other.Type
                && this.StartTime == other.StartTime
                && this.EndTime == other.EndTime;
        }
    }
}
