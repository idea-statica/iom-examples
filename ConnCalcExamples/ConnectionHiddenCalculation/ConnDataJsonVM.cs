using IdeaStatiCa.ConnectionClient.ConHiddenCalcCommands;
using IdeaStatiCa.ConnectionClient.Model;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace ConnectionHiddenCalculation
{
	public class ConnDataJsonVM : INotifyPropertyChanged, IConnectionDataJson
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private ConnectionDataJson connData;

		public ConnDataJsonVM(IUpdateCommand updateCommand, ConnectionDataJson connParameters)
		{
			Debug.Assert(updateCommand != null);
			Debug.Assert(connParameters != null);
			this.connData = connParameters;
			this.UpdateConnectionCmd = updateCommand;
			UpdateConnectionCmd.UpdateFinished += UpdateCommand_UpdateFinished;
		}

		public event EventHandler UpdateFinished;

		public string DataJson
		{
			get => connData?.DataJson;

			set
			{
				connData.DataJson = value;
				NotifyPropertyChanged("DataJson");
			}
		}

		public Guid ConnectionId
		{
			get => connData.ConnectionId;
		}

		public IUpdateCommand UpdateConnectionCmd { get; set; }

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
