using System;
using System.IO;
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

			var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

			//To be able to generate IDEA Connection project loacaly you need to add the reference of the assembly IdeaStatiCa.IOMToConnection.dll
			//which is included in the setup if IDEA StatiCa v. 10.1 and uncomment the following lines */

			#region Generatig IDEA Connection loacally
			//Console.WriteLine("Generating IDEA Connection project loacally");
			//var fileConnFileNameFromLocal = Path.Combine(desktopDir, "connectionFromIOM-local.ideaCon");
			//IdeaStatiCa.IOMToConnection.IOMToConnection iOMToConnection = new IdeaStatiCa.IOMToConnection.IOMToConnection();
			//IdeaStatiCa.IOMToConnection.IOMToConnection.Init();
			//iOMToConnection.Import(example, result, fileConnFileNameFromLocal);
			#endregion

			#region Generatig IDEA Connection by web service
			Console.WriteLine("Generating IDEA Connection project by web service");
			var fileConnFileNameFromWeb = Path.Combine(desktopDir, "connectionFromIOM-web.ideaCon");
			Example.CreateOnServer(example, result, fileConnFileNameFromWeb);
			#endregion

			// end console application
			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
