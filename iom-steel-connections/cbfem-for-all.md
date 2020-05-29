#### The examples of using API for running CBFEM analysis from .NET applications

Both examples use [IdeaStatiCa.Plugin](https://github.com/idea-statica/ideastatica-plugin) for running CBFEM analysis. They also require [installation](https://github.com/idea-statica/ideastatica-plugin) of IDEA StatiCa v 20.0 on an user's PC. 

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

In the project [IdeaStatiCa.ConnectionClient](https://github.com/idea-statica/iom-examples/tree/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient) there are commands which control [ConnHiddenCheck Service](https://github.com/idea-statica/ideastatica-plugin/blob/master/IdeaStatiCa.Plugin/IConnHiddenCheck.cs). These commands show how to  :

* [Open IDEA Connection project](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/OpenProjectCommand.cs)
* [Import IDEA Connection from IOM](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/ImportIOMCommand.cs)
* [Close IDEA Connection project](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/CloseProjectCommand.cs)
* [Calculate a connection](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/CalculateConnectionCommand.cs)
* [Get geometry of a connection](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/ConnectionGeometryCommand.cs)
* [Create template from a connection](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/ConnectionToTemplateCommand.cs)
* [Apply template](https://github.com/idea-statica/iom-examples/blob/master/ConnCalcExamples/IdeaStatiCa.ConnectionClient/ConHiddenCalcCommands/ApplyTemplateCommand.cs)





