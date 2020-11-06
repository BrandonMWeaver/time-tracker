using System;
using System.Collections.Generic;
using System.Text;
using TimeTrackerUI.Models.Base;
using TimeTrackerUI.ViewModels.Commands;

namespace TimeTrackerUI.ViewModels.Controls
{
    class StartEndToggleControl : NotificationBase
    {
        private readonly Action _action1;
        private readonly Action _action2;

        private bool _canStart;

        public bool CanStart
        {
            get { return this._canStart; }
            set
            {
                this._canStart = value;
                this.OnPropertyChanged(nameof(this.CanStart));

                this.Content = value ? "Start Task" : "End Task";
                this.Command = value ? new ParameterlessCommand(this._action1) : new ParameterlessCommand(this._action2);
            }
        }

        private string _content;

        public string Content
        {
            get { return this._content; }
            set
            {
                this._content = value;
                this.OnPropertyChanged(nameof(this.Content));
            }
        }

        private ParameterlessCommand _command;

        public ParameterlessCommand Command
        {
            get { return this._command; }
            set
            {
                this._command = value;
                this.OnPropertyChanged(nameof(this.Command));
            }
        }

        public StartEndToggleControl(Action action1, Action action2)
        {
            this._action1 = action1;
            this._action2 = action2;
        }
    }
}
