using System;
using IdeaRS.OpenModel;
using IdeaRS.OpenModel.Result;

namespace IOMExample
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
			
			Console.WriteLine("Done. Press any key to exit.");
			Console.ReadKey();
		}
	}
}
