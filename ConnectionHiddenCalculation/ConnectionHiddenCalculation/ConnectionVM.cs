using System;
using System.ComponentModel;

namespace ConnectionHiddenCalculation
{
	public class ConnectionVM : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		dynamic dynamicItem;

		public ConnectionVM(object item)
		{
			this.dynamicItem = item;
		}

		public string Name
		{
			get => (string)(dynamicItem.Header.Name);
		}

		public Guid ConnectionId
		{
			get => (Guid)(dynamicItem.Header.ConnectionID);
		}

		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
