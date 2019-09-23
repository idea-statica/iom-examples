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

			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.AssemblyResolve += new ResolveEventHandler(IdeaResolveEventHandler);

			Console.WriteLine("Start generate example of IOM...");

			// create IOM and results
			OpenModel example = Example.CreateIOM();
			OpenModelResult result = Helpers.GetResults();

			// save to the files
			result.SaveToXmlFile("example.xmlR");
			example.SaveToXmlFile("example.xml");

			var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			var fileConnFileNameFromLocal = Path.Combine(desktopDir, "connectionFromIOM-local.ideaCon");

			string ideaConLinkFullPath = System.IO.Path.Combine(IdeaInstallDir, "IdeaStatiCa.IOMToConnection.dll");
			var conLinkAssembly = Assembly.LoadFrom(ideaConLinkFullPath);
			object obj = conLinkAssembly.CreateInstance("IdeaStatiCa.IOMToConnection.IOMToConnection");
			dynamic d = obj;


			var initMethod = (obj).GetType().GetMethod("Init");
			initMethod.Invoke(obj, null);


			Console.WriteLine("Generating IDEA Connection project loacally");

			var method = (obj).GetType().GetMethod("Import");
			object[] array = new object[3];
			array[0] = example;
			array[1] = result;
			array[2] = fileConnFileNameFromLocal;
			method.Invoke(obj, array);


			/*#region Generatig IDEA Connection loacally
			IdeaStatiCa.IOMToConnection.IOMToConnection iOMToConnection = new IdeaStatiCa.IOMToConnection.IOMToConnection();
			IdeaStatiCa.IOMToConnection.IOMToConnection.Init();
			iOMToConnection.Import(example, result, fileConnFileNameFromLocal);
			#endregion*/

			// end console application
			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}

		private static Assembly IdeaResolveEventHandler(object sender, ResolveEventArgs args)
		{
			AssemblyName asmName = new AssemblyName(args.Name);

			string assemblyFileName = System.IO.Path.Combine(IdeaInstallDir, asmName.Name + ".dll");
			if (System.IO.File.Exists(assemblyFileName))
			{
				return Assembly.LoadFile(assemblyFileName);
			}

			return args.RequestingAssembly;
		}
	}
}
