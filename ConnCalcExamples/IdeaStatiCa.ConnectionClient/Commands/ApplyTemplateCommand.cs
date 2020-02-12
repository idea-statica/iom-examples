using IdeaStatiCa.ConnectionClient.Model;

namespace IdeaStatiCa.ConnectionClient.Commands
{
	public class ApplyTemplateCommand : ConnHiddenCalcCommandBase
	{
		public ApplyTemplateCommand(IConHiddenCalcModel model) : base(model)
		{
		}

		public override bool CanExecute(object parameter)
		{
			return (Model.IsIdea && Model.IsService && !IsCommandRunning);
		}

		public override void Execute(object parameter)
		{
		}
	}
}
