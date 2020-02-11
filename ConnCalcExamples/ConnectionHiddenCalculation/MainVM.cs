﻿using IdeaRS.OpenModel.Connection;
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

namespace ConnectionHiddenCalculation
{
	/// <summary>
	/// Main view model of the example
	/// </summary>
	public class MainVM : INotifyPropertyChanged
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

			OpenProjectCmd = new CustomCommand(this.CanOpen, this.Open);
			CloseProjectCmd = new CustomCommand(this.CanClose, this.Close);
			CalculateConnectionCmd = new CustomCommand(this.CanCalculate, this.Calculate);
			ConnectionGeometryCmd = new CustomCommand(this.CanGetGeometry, this.GetGeometry);
			SaveAsProjectCmd = new CustomCommand(this.CanSaveAsProject, this.SaveAsProject);
		}

		#endregion

		#region Commands
		public CustomCommand OpenProjectCmd { get; set; }
		public CustomCommand CloseProjectCmd { get; set; }
		public CustomCommand CalculateConnectionCmd { get; set; }
		public CustomCommand ConnectionGeometryCmd { get; set; }
		public CustomCommand SaveAsProjectCmd { get; set; }
		#endregion

		#region Properties
		public IConnHiddenCheck Service
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

		#region Command handlers
		/// <summary>
		/// Is it possible to open a new project
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public bool CanOpen(object param)
		{
			return (IsIdea && Service == null);
		}

		/// <summary>
		/// Open idea connection project
		/// </summary>
		/// <param name="param"></param>
		public void Open(object param)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "IdeaConnection | *.ideacon";
			if (openFileDialog.ShowDialog() == true)
			{
				try
				{
					Debug.WriteLine("Creating the instance of IdeaRS.ConnectionService.Service.ConnectionSrv");

					IdeaConnectionClient = CalcFactory.Create();
					Service = IdeaConnectionClient;

					Debug.WriteLine("Opening the project file '{0}'", openFileDialog.FileName);
					Service.OpenProject(openFileDialog.FileName);

					List<ConnectionVM> connectionsVm = GetConnectionViewModels();

					this.Connections = new ObservableCollection<ConnectionVM>(connectionsVm);
				}
				catch (Exception e)
				{
					Debug.Assert(false, e.Message);
					StatusMessage = e.Message;
					if (Service != null)
					{
						((IDisposable)Service)?.Dispose();
						Service = null;
					}
				}
			}
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

		/// <summary>
		/// Is there a project to close ?
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public bool CanClose(object param)
		{
			return Service != null;
		}

		/// <summary>
		/// Close the current idea connection project
		/// </summary>
		/// <param name="param"></param>
		public void Close(object param)
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
		#endregion

		private List<ConnectionVM> GetConnectionViewModels()
		{
			List<ConnectionVM> connectionsVm = new List<ConnectionVM>();
			// get information obaout all aconections in the project
			var projectData = Service.GetProjectInfo();
			foreach (var con in projectData.Connections)
			{
				connectionsVm.Add(new ConnectionVM(con));
			}

			return connectionsVm;
		}

		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
