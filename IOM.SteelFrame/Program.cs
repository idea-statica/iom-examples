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
