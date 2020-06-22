using IdeaRS.OpenModel.Connection;
using IdeaStatiCa.ConnectionClient.Commands;
using IdeaStatiCa.ConnectionClient.Model;
using IdeaStatiCa.Plugin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace ConnectionHiddenCalculation
{
	/// <summary>
	/// Main view model of the example
	/// </summary>
	public class MainVM : INotifyPropertyChanged, IConHiddenCalcModel
	{
		#region private fields
		public event PropertyChangedEventHandler PropertyChanged;
		bool isIdea;
		string statusMessage;
		string ideaStatiCaDir;
		ObservableCollection<ConnectionVM> connections;
		string results;
		ConnHiddenClientFactory CalcFactory { get; set; }
		ConnectionHiddenCheckClient IdeaConnectionClient { get; set; }
		IConnHiddenCheck service;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public MainVM()
		{
			connections = new ObservableCollection<ConnectionVM>();
			ideaStatiCaDir = Properties.Settings.Default.IdeaStatiCaDir;
			if (Directory.Exists(ideaStatiCaDir))
			{
				string ideaConnectionFileName = Path.Combine(ideaStatiCaDir, "IdeaConnection.exe");
				if (File.Exists(ideaConnectionFileName))
				{
					IsIdea = true;
					StatusMessage = string.Format("IdeaStatiCa installation was found in '{0}'", ideaStatiCaDir);
					CalcFactory = new ConnHiddenClientFactory(ideaStatiCaDir);
				}
			}

			if (!IsIdea)
			{
				StatusMessage = string.Format("ERROR IdeaStatiCa doesn't exist in '{0}'", ideaStatiCaDir);
			}

			OpenProjectCmd = new OpenProjectCommand(this);
			ImportIOMCmd = new ImportIOMCommand(this);
			CloseProjectCmd = new CloseProjectCommand(this);
			CalculateConnectionCmd = new CalculateConnectionCommand(this);
			ConnectionGeometryCmd = new ConnectionGeometryCommand(this);
			SaveAsProjectCmd = new SaveAsProjectCommand(this);
			ConnectionToTemplateCmd = new ConnectionToTemplateCommand(this);
			ApplyTemplateCmd = new ApplyTemplateCommand(this);

			GetMaterialsCmd = new GetMaterialsCommand(this);
			GetCrossSectionsCmd = new GetCrossSectionsCommand(this);
			GetBoltAssembliesCmd = new GetBoltAssembliesCommand(this);
			CreateBoltAssemblyCmd = new CreateBoltAssemblyCommand(this);
		}
		#endregion

		#region Commands
		public ICommand OpenProjectCmd { get; set; }
		public ICommand ImportIOMCmd { get; set; }
		public ICommand CloseProjectCmd { get; set; }
		public ICommand CalculateConnectionCmd { get; set; }
		public ICommand ConnectionGeometryCmd { get; set; }
		public ICommand SaveAsProjectCmd { get; set; }
		public ICommand ConnectionToTemplateCmd { get; set; }
		public ICommand ApplyTemplateCmd { get; set; }
		public ICommand GetMaterialsCmd { get; set; }
		public ICommand GetCrossSectionsCmd { get; set; }
		public ICommand GetBoltAssembliesCmd { get; set; }
		public ICommand CreateBoltAssemblyCmd { get; set; }
		#endregion

		#region IConHiddenCalcModel

		/// <summary>
		/// Indicate if the installation of IdeaStatiCa exits
		/// </summary>
		public bool IsIdea
		{
			get => isIdea;

			set
			{
				isIdea = value;
				NotifyPropertyChanged("IsIdea");
			}
		}

		public bool IsService
		{
			get => Service != null;
		}

		public string Results
		{
			get => results;
			set
			{
				results = value;
				NotifyPropertyChanged("Results");
			}
		}


		public IConnHiddenCheck GetConnectionService()
		{
			if (Service != null)
			{
				return Service;
			}

			IdeaConnectionClient = CalcFactory.Create();
			Service = IdeaConnectionClient;
			return Service;
		}

		public void CloseConnectionService()
		{
			if (Service == null)
			{
				return;
			}

			IdeaConnectionClient.CloseProject();
			IdeaConnectionClient.Close();
			IdeaConnectionClient = null;
			Service = null;

			Results = string.Empty;
			Connections.Clear();
		}

		public void SetStatusMessage(string msg)
		{
			Application.Current.Dispatcher.BeginInvoke(
			 (ThreadStart)delegate
			 {
				 this.StatusMessage = msg;
			 });
		}

		public void SetResults(object res)
		{
			Application.Current.Dispatcher.BeginInvoke(
			 (ThreadStart)delegate
			 {
				 var jsonSetting = new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(), Culture = CultureInfo.InvariantCulture };

				 if (res is ConnectionResultsData cbfemResults)
				 {
					 
					 var jsonFormating = Formatting.Indented;
					 this.Results = JsonConvert.SerializeObject(cbfemResults, jsonFormating, jsonSetting);
				 }
				 else if (res is IdeaRS.OpenModel.Connection.ConnectionData conData)
				 {
					 var jsonFormating = Formatting.Indented;
					 Results = JsonConvert.SerializeObject(conData, jsonFormating, jsonSetting);
				 }
				 else
				 {
					 this.Results = (res == null ? string.Empty : res.ToString());
				 }
			 });
		}

		public void SetConProjectData(ConProjectInfo projectData)
		{
			List<ConnectionVM> connectionsVm = new List<ConnectionVM>();
			// get information obaout all aconections in the project
			foreach (var con in projectData.Connections)
			{
				connectionsVm.Add(new ConnectionVM(con));
			}

			this.Connections = new ObservableCollection<ConnectionVM>(connectionsVm);
		}

		#endregion

		#region View model's properties and methods

		private IConnHiddenCheck Service
		{
			get => service;
			set
			{
				service = value;
				NotifyPropertyChanged("Service");
			}
		}

		/// <summary>
		/// The list of view models for all connections in the project
		/// </summary>
		public ObservableCollection<ConnectionVM> Connections
		{
			get => connections;
			set
			{
				connections = value;
				NotifyPropertyChanged("Connections");
			}
		}

		/// <summary>
		/// Notification in the status bar
		/// </summary>
		public string StatusMessage
		{
			get => statusMessage;
			set
			{
				statusMessage = value;
				NotifyPropertyChanged("StatusMessage");
			}
		}

		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		} 
		#endregion
	}
}
