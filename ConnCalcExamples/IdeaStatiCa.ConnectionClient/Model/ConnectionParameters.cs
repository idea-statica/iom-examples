using System;

namespace IdeaStatiCa.ConnectionClient.Model
{
	/// <summary>
	/// Represents connection parameters
	/// </summary>
	public class ConnectionParameters
	{
		string parametersJson;
		readonly Guid connectionId;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="json"></param>
		public ConnectionParameters(Guid connectionId, string json)
		{
			this.connectionId = connectionId;
			parametersJson = json;
		}

		/// <summary>
		/// Json string represention connection parameters
		/// </summary>
		public string ParametersJson { get => parametersJson; set => parametersJson = value; }

		public Guid ConnectionId => connectionId;
	}
}
