using IdeaStatiCa.ConnectionClient.Model;
using System;

namespace IdeaStatiCa.ConnectionClient.Commands
{
	public class ConnectionToTemplateCommand : ConnHiddenCalcCommandBase
	{
		public ConnectionToTemplateCommand(IConHiddenCalcModel model) : base(model)
		{
		}

		public override bool CanExecute(object parameter)
		{
			return (Model.IsIdea && Model.IsService && !IsCommandRunning);
		}

		public override void Execute(object parameter)
		{
			throw new NotImplementedException();
		}
	}
}
