using IdeaStatiCa.ConnectionClient.Commands;
using IdeaStatiCa.ConnectionClient.Model;
using System;

namespace IdeaStatiCa.ConnectionClient.ConHiddenCalcCommands
{
	public class UpdateConnParamsCommand : ConnHiddenCalcCommandBase
	{
		public event EventHandler UpdateFinished;

		public UpdateConnParamsCommand(IConHiddenCalcModel model) : base(model)
		{
		}

		public override bool CanExecute(object parameter)
		{
			return (Model.IsIdea && Model.IsService && !IsCommandRunning);
		}

		public override void Execute(object parameter)
		{
			Model.SetResults("Setting parameters for connection");

			NotifyCommandFinished();
		}

		private void NotifyCommandFinished()
		{
			if(UpdateFinished != null)
			{
				UpdateFinished.Invoke(this, new EventArgs());
			}
		}
	}
}
