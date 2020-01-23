using IdeaRS.OpenModel;
using IdeaRS.OpenModel.Result;
using IdeaStatiCa.Plugin;
using IOM.SteelFrame;
using System;
using System.IO;

namespace IOM.SteelFrameDesktop
{
	class Program
	{
		private static string IdeaInstallDir;
		public static void Main(string[] args)
		{
			IdeaInstallDir = IOM.SteelFrameDesktop.Properties.Settings.Default.IdeaInstallDir;

			Console.WriteLine("IDEA StatiCa installation directory is '{0}'", IdeaInstallDir);

			Console.WriteLine("Start generate example of IOM...");

			// create IOM and results
			OpenModel example = Example.CreateIOM();
			OpenModelResult result = Helpers.GetResults();

			// save to the files
			result.SaveToXmlFile("example.xmlR");
			example.SaveToXmlFile("example.xml");

			var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			var fileConnFileNameFromLocal = Path.Combine(desktopDir, "connectionFromIOM-local.ideaCon");

			var calcFactory = new ConnHiddenClientFactory(IdeaInstallDir);

			var client = calcFactory.Create();
			try
			{
				// it creates connection project from IOM 

				Console.WriteLine("Creating Idea connection project which will be saved to the file '{0}'", fileConnFileNameFromLocal);
				client.CreateConProjFromIOM(example, result, fileConnFileNameFromLocal);
			}
			finally
			{
				if (client != null)
				{
					client.Close();
				}
			}

			// end console application
			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
