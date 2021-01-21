using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
