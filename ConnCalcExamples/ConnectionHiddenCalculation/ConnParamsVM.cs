using IdeaStatiCa.ConnectionClient.ConHiddenCalcCommands;
using IdeaStatiCa.ConnectionClient.Model;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ConnectionHiddenCalculation
{
	public class ConnParamsVM : INotifyPropertyChanged, IUpdatedConnection
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private IConHiddenCalcModel calcModel;
		private ConnectionParameters connParameters;

		public ConnParamsVM(IConHiddenCalcModel calcModel, ConnectionParameters connParameters)
		{
			Debug.Assert(calcModel != null);
			Debug.Assert(connParameters != null);
			this.connParameters = connParameters;
			var updateCommand = new UpdateConnParamsCommand(calcModel);
			updateCommand.UpdateFinished += UpdateCommand_UpdateFinished;
			this.UpdateParametersCmd = updateCommand;
		}

		public event EventHandler UpdateFinished;

		public string ConnParamsJson
		{
			get => connParameters?.ParametersJson;

			set
			{
				connParameters.ParametersJson = value;
				NotifyPropertyChanged("ConnParamsJson");
			}
		}

		public Guid ConnectionId
		{
			get => connParameters.ConnectionId;
		}

		public ICommand UpdateParametersCmd { get; set; }

		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		private void UpdateCommand_UpdateFinished(object sender, EventArgs e)
		{
			if (UpdateFinished != null)
			{
				UpdateFinished.Invoke(this, new EventArgs());
			}
		}
	}
}
