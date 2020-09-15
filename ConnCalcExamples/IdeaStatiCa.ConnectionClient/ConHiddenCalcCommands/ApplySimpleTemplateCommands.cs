using IdeaStatiCa.ConnectionClient.Commands;
using IdeaStatiCa.ConnectionClient.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdeaStatiCa.ConnectionClient.ConHiddenCalcCommands
{
	public class ApplySimpleTemplateCommand : ConnHiddenCalcCommandBase
	{
		public ApplySimpleTemplateCommand(IConHiddenCalcModel model) : base(model)
		{
		}

		public override bool CanExecute(object parameter)
		{
			return (Model.IsIdea && Model.IsService && !IsCommandRunning);
		}

		public override void Execute(object parameter)
		{
			var res = string.Empty;
			Model.SetResults("Apply Simple Template");
			IsCommandRunning = true;

			string connTemplateFileName = string.Empty;

			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Idea Connection Template| *.contemp";
			if (openFileDialog.ShowDialog() != true)
			{
				return;
			}
			else
			{
				connTemplateFileName = openFileDialog.FileName;
			}
				
			List<int> AttachedMembers = new List<int>();
			AttachedMembers.Add(2);
			//AttachedMembers.Add(3);

			var mainMember = 1;


			var applySimpleTemplateTask = Task.Run(() =>
			{
				try
				{
					var connection = (IConnectionId)parameter;
					var service = Model.GetConnectionService();

					var resData = service.ApplySimpleTemplate(connection.ConnectionId, connTemplateFileName, Model.TemplateSetting, mainMember, AttachedMembers);

					Model.SetResults(resData);
				}
				catch (Exception e)
				{
					Model.SetStatusMessage(e.Message);
				}
				finally
				{
					IsCommandRunning = false;
				}
			});

		}
	}
}
