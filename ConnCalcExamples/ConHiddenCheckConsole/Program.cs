using IdeaStatiCa.Plugin;
using System;
using System.IO;

namespace ConnCalculatorConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start");

			if(args.Length < 1)
			{
				Console.WriteLine("The path to Idea StatiCa installation dir is required as the argument");
				return;
			}

			if (!Directory.Exists(args[0]))
			{
				Console.WriteLine(string.Format("Missing Idea StatiCa installation in '{0}'", args[0]));
				return;
			}

			Console.WriteLine(string.Format("Using Idea StatiCa from '{0}'", args[0]));

			using (var calcFactory = new ConnHiddenClientFactory(args[0]))
			{
				var client = calcFactory.Create();

				client.OpenProject("testProj.ideaCon");

				// get detail about idea connection project
				var projInfo = client.GetProjectInfo();

				if (projInfo != null && projInfo.Connections != null)
				{
					// iterate all connections in the project
					foreach (var con in projInfo.Connections)
					{
						Console.WriteLine(string.Format("Starting calculation of connection {0}", con.Identifier));

						// calculate a get results for each connection in the project
						var conRes = client.Calculate(con.Identifier);
						Console.WriteLine("Calculation is done");

						// get the geometry of the connection
						var connectionModel = client.GetConnectionModel(con.Identifier);
					}
				}

				client.CloseProject();

				client.Close();
			}

			Console.WriteLine("End");
			var x = Console.ReadLine();
		}
	}
}
