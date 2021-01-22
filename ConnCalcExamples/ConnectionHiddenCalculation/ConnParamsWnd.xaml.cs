using System;
using System.Windows;

namespace ConnectionHiddenCalculation
{
	/// <summary>
	/// Interaction logic for ConcParamsWnd.xaml
	/// </summary>
	public partial class ConnParamsWnd : Window
	{
		private readonly ConnParamsVM viewModel;
		public ConnParamsWnd()
		{
			InitializeComponent();
		}

		public ConnParamsWnd(ConnParamsVM viewModel) : this()
		{
			this.viewModel = viewModel;
			this.viewModel.UpdateFinished += ViewModel_UpdateFinished;
			DataContext = viewModel;
		}

		private void ViewModel_UpdateFinished(object sender, EventArgs e)
		{
			Dispatcher.BeginInvoke((Action)(() =>
			{
				DialogResult = true;
				Close();
			}));
		}
	}
}
