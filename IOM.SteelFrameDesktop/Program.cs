using IOM.SteelFrame;
using System;
using System.IO;
using System.Reflection;
using IdeaRS.OpenModel;
using IdeaRS.OpenModel.Result;

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

			//string ideaConLinkFullPath = System.IO.Path.Combine(IdeaInstallDir, "IdeaStatiCa.IOMToConnection.dll");
			//var conLinkAssembly = Assembly.LoadFrom(ideaConLinkFullPath);
			//object obj = conLinkAssembly.CreateInstance("IdeaStatiCa.IOMToConnection.IOMToConnection");
			//dynamic d = obj;

			//// Initializtion
			//var initMethod = (obj).GetType().GetMethod("Init");
			//initMethod.Invoke(obj, null);

			//Console.WriteLine("Generating IDEA Connection project locally");

			//// Invoking method Import by reflection
			//var methodImport = (obj).GetType().GetMethod("Import");
			//object[] array = new object[3];
			//array[0] = example;
			//array[1] = result;
			//array[2] = fileConnFileNameFromLocal;
			//methodImport.Invoke(obj, array);

			Console.WriteLine("Writing Idea connection project to file '{0}'", fileConnFileNameFromLocal);

			// end console application
			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
