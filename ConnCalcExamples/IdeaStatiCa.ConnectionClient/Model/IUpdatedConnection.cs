using System;

namespace IdeaStatiCa.ConnectionClient.Model
{
	public interface IUpdatedConnection
	{
		/// <summary>
		/// Identifier of the connection in a project
		/// </summary>
		Guid ConnectionId { get; }

		/// <summary>
		/// values of the parameters
		/// </summary>
		string ConnParamsJson { get; }
	}
}
