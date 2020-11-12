using System;
using System.Collections.Generic;
using System.Text;
using TimeTrackerUI.ViewModels.Commands.Base;

namespace TimeTrackerUI.ViewModels.Commands
{
    class Command<T> : CommandBase where T : class
    {
        private readonly Action<T> _action;

        public Command(Action<T> action) => this._action = action;

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => this._action.Invoke(parameter as T);
    }
}
