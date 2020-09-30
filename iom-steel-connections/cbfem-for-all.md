#### The examples of using API for running CBFEM analysis from .NET applications

Both examples use [IdeaStatiCa.Plugin](https://github.com/idea-statica/ideastatica-plugin) for running CBFEM analysis. Running this example it requires IDEA StatiCa v 20.0 (or higher) on an user's PC. Free trial version version can be obtained [here](https://www.ideastatica.com/free-trial).

### Example [ConHiddenCheckConsole](https://github.com/idea-statica/iom-examples/tree/master/ConnCalcExamples/ConHiddenCheckConsole)
It is very simple console application example. *ConnCalculatorConsole.exe* requires the path to IDEA Statica installation directory and optionly the path to the idea connection project. If any project is passed the default 
project for this example is calculated.

![ConnectionHiddenCalculation](https://github.com/idea-statica/iom-examples/blob/gh-pages/iom-steel-connections/Images/hidden-check-console.PNG?raw=true)

Running CBFEM from the commad line :

```
ConnCalculatorConsole.exe "C:\Program Files\IDEA StatiCa\StatiCa 20.0" "c:\test.ideaCon"
```

### Example [ConnectionHiddenCalculation](https://github.com/idea-statica/iom-examples/tree/master/ConnCalcExamples/ConnectionHiddenCalculation)

It is more complex example which presents more features which are provided by IDEA StaiCa API. To be able to run this example set the path to Idea StatiCa directory in the project settings.

![ConnectionHiddenCalculation](https://github.com/idea-statica/iom-examples/blob/gh-pages/iom-steel-connections/Images/conn-hidden-calculation.PNG?raw=true)

Communication of a 3rd party application (the example ConnectionHiddenCalculation in this case) with ConnectionCalcService which runs in a another process.
![ConnectionHiddenCalculation](https://github.com/idea-statica/ideastatica-plugin/blob/master/Images/ConnectionHiddenCalculation.svg?raw=true)

In the project [IdeaStatiCa.ConnectionClient](https://github.com/idea-statica/iom-examples/tree/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient) there are commands which control [ConnHiddenCheck Service](https://github.com/idea-statica/ideastatica-plugin/blob/master/IdeaStatiCa.Plugin/IConnHiddenCheck.cs). These commands show how to  :

* [Open IDEA Connection project](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/OpenProjectCommand.cs)
* [Import IDEA Connection from IOM](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/ImportIOMCommand.cs)
* [Close IDEA Connection project](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/CloseProjectCommand.cs)
* [Calculate a connection](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/CalculateConnectionCommand.cs)
* [Get geometry of a connection](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/ConnectionGeometryCommand.cs)
* [Create template from a connection](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/ConnectionToTemplateCommand.cs)
* [Apply template](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/ApplyTemplateCommand.cs)

There are new methods for getting information about materials, cross-sections and bolt assemblies in idea connection project in [IConnHiddenCheck](https://github.com/idea-statica/ideastatica-plugin/blob/master/IdeaStatiCa.Plugin/IConnHiddenCheck.cs) since version **IDEA StatiCa v20.0.81**. It is also possible to add new bolt assemblies into a connection project - see method :

```C#
		/// <summary>
		/// Add the new bolt assembly. Its type is defined by its name (e.g. 'M12 4.6')
		/// </summary>
		/// <param name="boltAssemblyName"></param>
		/// <returns></returns>
		[OperationContract]
		int AddBoltAssembly(string boltAssemblyName);
```        

The method *ApplyTemplate* has the new parameter *connTemplateSetting*. It defines materials which are used when template is applied. 

![Get materials from project](https://github.com/idea-statica/iom-examples/blob/gh-pages/iom-steel-connections/Images/hidden-check-get_material.png?raw=true)

The examples of using these new *IConnHiddenCheck* methods can be found in examples :

* [GetMaterialsInProject](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/GetMaterialsCommand.cs)
* [GetCrossSectionsInProject](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/GetCrossSectionsCommand.cs)
* [GetBoltAssembliesInProject](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/GetBoltAssembliesCommand.cs)
* [AddBoltAssembly](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/CreateBoltAssemblyCommand.cs)

### Improvements in version IDEA StatiCa v 20.1

API users can apply simple template to a connection.

![ConnectionHiddenCalculation](https://github.com/idea-statica/iom-examples/blob/gh-pages/iom-steel-connections/Images/apply-simple-template.PNG?raw=true)

There is the example how to call the service in the command : [Apply Simple template](https://github.com/idea-statica/iom-examples/blob/release-20i/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/ApplySimpleTemplateCommands.cs)



