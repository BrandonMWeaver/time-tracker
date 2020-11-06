using System;
using System.Collections.Generic;
using System.Text;
using TimeTrackerUI.ViewModels.Commands.Base;

namespace TimeTrackerUI.ViewModels.Commands
{
    class ParameterlessCommand : CommandBase
    {
        private readonly Action _action;

        public ParameterlessCommand(Action action) => this._action = action;

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => this._action.Invoke();
    }
}
