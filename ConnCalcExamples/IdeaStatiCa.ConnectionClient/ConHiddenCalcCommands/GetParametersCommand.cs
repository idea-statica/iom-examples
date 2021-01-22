﻿using IdeaStatiCa.ConnectionClient.Commands;
using IdeaStatiCa.ConnectionClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeaStatiCa.ConnectionClient.ConHiddenCalcCommands
{
	public class GetParametersCommand : ConnHiddenCalcCommandBase
	{
		public GetParametersCommand(IConHiddenCalcModel model) : base(model)
		{
		}

		public override bool CanExecute(object parameter)
		{
			return (Model.IsIdea && Model.IsService && !IsCommandRunning);
		}

		public override void Execute(object parameter)
		{
			var res = string.Empty;
			IsCommandRunning = true;
			Model.SetResults("Getting geometry parametes of the connection");
			var calculationTask = Task.Run(() =>
			{
				try
				{
					var conVM = (IConnectionId)parameter;
					var Service = Model.GetConnectionService();

					string parametersJson = Service.GetParametersJSON(conVM.ConnectionId);
					var conParameters = new ConnectionDataJson(Guid.Parse(conVM.ConnectionId) ,parametersJson);
					Model.SetResults(conParameters);

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
