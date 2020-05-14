using IdeaRS.OpenModel;
using IdeaRS.OpenModel.Result;
using IdeaStatiCa.Plugin;
using IOM.GeneratorExample;
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

			if (!Directory.Exists(IdeaInstallDir))
			{
				Console.WriteLine("IDEA StatiCa installation was not found in '{0}'", IdeaInstallDir);
				return;
			}

			Console.WriteLine("IDEA StatiCa installation directory is '{0}'", IdeaInstallDir);

			Console.WriteLine("Start generate example of IOM...");

			// create IOM and results
			OpenModel example = SteelFrameExample.CreateIOM();
			OpenModelResult result = Helpers.GetResults();

			string iomFileName = "example.xml";
			string iomResFileName = "example.xmlR";

			// save to the files
			example.SaveToXmlFile(iomFileName);
			result.SaveToXmlFile(iomResFileName);


			var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			var fileConnFileNameFromLocal = Path.Combine(desktopDir, "connectionFromIOM-local.ideaCon");

			var calcFactory = new ConnHiddenClientFactory(IdeaInstallDir);

			var client = calcFactory.Create();
			try
			{
				// it creates connection project from IOM 
				Console.WriteLine("Creating Idea connection project ");
				client.CreateConProjFromIOM(iomFileName, iomResFileName, fileConnFileNameFromLocal);
				Console.WriteLine("Generated project was saved to the file '{0}'", fileConnFileNameFromLocal);
			}
			catch(Exception e)
			{
				Console.WriteLine("Error '{0}'", e.Message);
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
