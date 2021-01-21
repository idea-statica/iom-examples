namespace IdeaStatiCa.ConnectionClient.Model
{
	/// <summary>
	/// Represents connection parameters
	/// </summary>
	public class ConnectionParameters
	{
		string parametersJson;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="json"></param>
		public ConnectionParameters(string json)
		{
			parametersJson = json;
		}

		/// <summary>
		/// Json string represention connection parameters
		/// </summary>
		public string ParametersJson { get => parametersJson; set => parametersJson = value; }
	}
}
