#### The example of creating IDEA Connection project from IOM locally - see project [IOM.SteelFrameDesktop](https://github.com/idea-statica/iom-examples/tree/master/IOM_SteelFrame1/IOM.SteelFrameDesktop)

IOM including steel frame and geometry of connections is generated in IOM.GeneratorExample

```C#
			// create IOM and results
			OpenModel example = SteelFrameExample.CreateIOM();
			OpenModelResult result = Helpers.GetResults();

			string iomFileName = "example.xml";
			string iomResFileName = "example.xmlR";

			// save to the files
			example.SaveToXmlFile(iomFileName);
			result.SaveToXmlFile(iomResFileName);
```

The instance of the class *ConnHiddenClientFactory* is responsible for communication with local installation of Idea StatiCa. The path to the installation directory is passed in the constructor

```C#
			IdeaInstallDir = IOM.SteelFrameDesktop.Properties.Settings.Default.IdeaInstallDir;
			Console.WriteLine("IDEA StatiCa installation directory is '{0}'", IdeaInstallDir);

			var calcFactory = new ConnHiddenClientFactory(IdeaInstallDir);
```

The installation directory of Idea StatiCa v 20.0 (or higher) is set in the project setting of *IOM.SteelFrameDesktop*. Idea Connection project is created by calling method *CreateConProjFromIOM*

```C#
				client.CreateConProjFromIOM(iomFileName, iomResFileName, fileConnFileNameFromLocal);
				Console.WriteLine("Generated project was saved to the file '{0}'", fileConnFileNameFromLocal);
```




