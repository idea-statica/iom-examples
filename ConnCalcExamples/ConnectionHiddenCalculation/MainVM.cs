using ConnectionHiddenCalculation.Commands;
using IdeaRS.OpenModel.Connection;
using IdeaStatiCa.Plugin;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Input;

namespace ConnectionHiddenCalculation
{
	public interface IConHiddenCalcModel
	{
		bool IsService { get; }

		bool IsIdea { get; }

		IConnHiddenCheck GetConnectionService();

		void CloseConnectionService();

		void SetConProjectData(ConProjectInfo projectData);

		void SetStatusMessage(string msg);

		
	}

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
			ImportIOMCmd = new CustomCommand(this.CanImportIOM, this.ImportIOM);
			CloseProjectCmd = new CloseProjectCommand(this);
			CalculateConnectionCmd = new CustomCommand(this.CanCalculate, this.Calculate);
			ConnectionGeometryCmd = new CustomCommand(this.CanGetGeometry, this.GetGeometry);
			SaveAsProjectCmd = new CustomCommand(this.CanSaveAsProject, this.SaveAsProject);
			ConnectionToTemplateCmd = new CustomCommand(this.CanConnectionToTemplate, this.ConnectionToTemplate);
			ApplyTemplateCmd = new CustomCommand(this.CanApplyTemplate, this.ApplyTemplate);
		}
		#endregion

		#region Commands
		public ICommand OpenProjectCmd { get; set; }
		public CustomCommand ImportIOMCmd { get; set; }
		public ICommand CloseProjectCmd { get; set; }
		public CustomCommand CalculateConnectionCmd { get; set; }
		public CustomCommand ConnectionGeometryCmd { get; set; }
		public CustomCommand SaveAsProjectCmd { get; set; }
		public CustomCommand ConnectionToTemplateCmd { get; set; }
		public CustomCommand ApplyTemplateCmd { get; set; }
		#endregion

		#region Properties
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

		#endregion

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
			this.StatusMessage = msg;
		}

		private void ImportIOM(object obj)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "IOM | *.xml";
			openFileDialog.CheckFileExists = true;
			if (openFileDialog.ShowDialog() == true)
			{
				Debug.WriteLine("Creating the instance of IdeaRS.ConnectionService.Service.ConnectionSrv");

				IdeaConnectionClient = CalcFactory.Create();
				Service = IdeaConnectionClient;

				string iomFileName = openFileDialog.FileName;
				string resultsFileName = Path.ChangeExtension(openFileDialog.FileName, ".xmlR");

				string tempProjectFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Path.GetRandomFileName());
				try
				{
					// create temporary idea connection project
					IdeaConnectionClient.CreateConProjFromIOM(iomFileName, resultsFileName, tempProjectFileName);

					// open it
					Debug.WriteLine("Opening the project file '{0}'", tempProjectFileName);
					Service.OpenProject(tempProjectFileName);

					var projectInfo = Service.GetProjectInfo();
					SetConProjectData(projectInfo);
				}
				finally
				{
					// delete temp file
					if (File.Exists(tempProjectFileName))
					{
						File.Delete(tempProjectFileName);
					}
				}
			}
		}

		private bool CanImportIOM(object arg)
		{
			return (IsIdea && Service == null);
		}

		private void SaveAsProject(object obj)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "IdeaConnection | *.ideacon";
			if (saveFileDialog.ShowDialog() == true)
			{
				Service.SaveAsProject(saveFileDialog.FileName);
			}
		}

		private bool CanSaveAsProject(object arg)
		{
			return Service != null;
		}

		public bool CanCalculate(object param)
		{
			return (IsIdea && Service != null);
		}

		/// <summary>
		/// Run CBFEM and return brief results
		/// </summary>
		/// <param name="param">View model of the selected connection</param>
		public void Calculate(object param)
		{
			var res = string.Empty;
			Results = "Running CBFEM";
			try
			{
				var conVM = (ConnectionVM)param;
				object resData = Service.Calculate(conVM.ConnectionId);
				ConnectionResultsData cbfemResults = (ConnectionResultsData)resData;
				if (cbfemResults != null)
				{
					var jsonSetting = new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() };
					var jsonFormating = Formatting.Indented;
					Results = JsonConvert.SerializeObject(cbfemResults, jsonFormating, jsonSetting);
				}
			}
			catch (Exception e)
			{
				StatusMessage = e.Message;
			}
		}

		public bool CanGetGeometry(object param)
		{
			return (IsIdea && Service != null);
		}

		/// <summary>
		/// Get geometry of th selected connection
		/// </summary>
		/// <param name="param">View model of the selected connection</param>
		public void GetGeometry(object param)
		{
			try
			{
				var conVM = (ConnectionVM)param;
				IdeaRS.OpenModel.Connection.ConnectionData conData = Service.GetConnectionModel(conVM.ConnectionId);
				if (conData != null)
				{
					var jsonSetting = new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() };
					var jsonFormating = Formatting.Indented;
					Results = JsonConvert.SerializeObject(conData, jsonFormating, jsonSetting);
				}
			}
			catch (Exception e)
			{
				StatusMessage = e.Message;
			}
		}

		private void ApplyTemplate(object obj)
		{
			
		}

		private bool CanApplyTemplate(object arg)
		{
			return (IsIdea && Service != null);
		}

		private void ConnectionToTemplate(object obj)
		{
			
		}

		private bool CanConnectionToTemplate(object arg)
		{
			return (IsIdea && Service != null);
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

		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
