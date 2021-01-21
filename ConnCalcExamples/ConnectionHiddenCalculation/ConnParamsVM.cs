using IdeaStatiCa.ConnectionClient.Model;
using System.ComponentModel;
using System.Diagnostics;

namespace ConnectionHiddenCalculation
{
	public class ConnParamsVM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private IConHiddenCalcModel calcModel;
		private ConnectionParameters connParameters;

		public ConnParamsVM(IConHiddenCalcModel calcModel, ConnectionParameters connParameters)
		{
			Debug.Assert(calcModel != null);
			Debug.Assert(connParameters != null);
			this.connParameters = connParameters;
		}

		public string ConnParamsJson
		{
			get => connParameters?.ParametersJson;

			set
			{
				connParameters.ParametersJson = value;
				NotifyPropertyChanged("ConnParamsJson");
			}
		}

		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
