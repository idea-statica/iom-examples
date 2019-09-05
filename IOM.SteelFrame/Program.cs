using System;
using IdeaRS.OpenModel;
using IdeaRS.OpenModel.Result;

namespace IOM.SteelFrame
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Start generate example of IOM...");

			// create IOM and results
			OpenModel example = Example.CreateIOM();
			OpenModelResult result = Helpers.GetResults();

			// save to the files
			result.SaveToXmlFile("example.xmlR");
			example.SaveToXmlFile("example.xml");

			//add reference on  IdeaStatiCa.IOMToConnection.dll and uncomment
			/*IdeaStatiCa.IOMToConnection.IOMToConnection iOMToConnection = new IdeaStatiCa.IOMToConnection.IOMToConnection();
			IdeaStatiCa.IOMToConnection.IOMToConnection.Init();
			iOMToConnection.Import(example, result, @"C:\temp\example.ideaCon");*/

			//create project file on server and store local
			//Example.CreateOnServer(example, result, @"C:\temp\example.ideaCon");

			// end console application
			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
