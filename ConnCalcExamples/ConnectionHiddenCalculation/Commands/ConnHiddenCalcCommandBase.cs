using System;
using System.Windows.Input;

namespace ConnectionHiddenCalculation.Commands
{
	public abstract class ConnHiddenCalcCommandBase : ICommand
	{
		
		public ConnHiddenCalcCommandBase(IConHiddenCalcModel model)
		{
			Model = model;
		}

		public IConHiddenCalcModel Model { get; private set; }

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}

		public abstract bool CanExecute(object parameter);

		public abstract void Execute(object parameter);
	}
}
