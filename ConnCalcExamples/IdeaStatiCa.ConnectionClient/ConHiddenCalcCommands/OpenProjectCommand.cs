using IdeaStatiCa.ConnectionClient.Model;
using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace IdeaStatiCa.ConnectionClient.Commands
{
	public class OpenProjectCommand : ConnHiddenCalcCommandBase
	{
		public OpenProjectCommand(IConHiddenCalcModel model) : base (model)
		{
		}

		public override bool CanExecute(object parameter)
		{
			return (Model.IsIdea && !Model.IsService );
		}

		public override void Execute(object parameter)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "IdeaConnection | *.ideacon";
			if (openFileDialog.ShowDialog() == true)
			{
				try
				{
					Debug.WriteLine("Creating the instance of IdeaRS.ConnectionService.Service.ConnectionSrv");

					var Service = Model.GetConnectionService();

					Debug.WriteLine("Opening the project file '{0}'", openFileDialog.FileName);
					Service.OpenProject(openFileDialog.FileName);

					var projectInfo = Service.GetProjectInfo();
					Model.SetConProjectData(projectInfo);

				}
				catch (Exception e)
				{
					Debug.Assert(false, e.Message);
					Model.SetStatusMessage(e.Message);
					Model.CloseConnectionService();
				}
			}
		}
	}
}
