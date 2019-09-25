# IOM Example - ConcreteColumn

This example describes how to define a concrete column in IOM (IDEA StatiCa Open Model).


Let's create a standard console application in MS Visual Studio. Select __File__ > __New__ > __Project__ from the menu bar. In the dialog, select the __Visual C#__ node followed by the __Get Started__ node. Then select the __Console App__ project template.

## Add the IdeaRS.OpenModel NuGet package

OpenModel is published as [the nuget package](https://www.nuget.org/packages/IdeaStatiCa.OpenModel/). To install this package, you can use either the Package Manager UI or the Package Manager Console.

For more information, see [Install and use a package in Visual Studio](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio)

There is also documentation related to [IdeaRS.OpenModel](https://idea-statica.github.io/iom/iom-api/latest/index.html) on Github.

## Create a new project
IOM data has to contain basic information of a new project, such as a project name, description, code type etc.

![alt text][projdata]


## Materials
To create new project, these types of materials have to be created:
-	new concrete material
```csharp
//Concrete material
MatConcreteEc2 mat = new MatConcreteEc2();
mat.Name = "C30/37";
mat.UnitMass = 2500.0;
mat.E = 32836.6e6;
mat.G = 13667000000.0;
mat.Poisson = 0.2;
mat.SpecificHeat = 0.6;
mat.ThermalExpansion = 0.00001;
mat.ThermalConductivity = 45;
mat.Fck = Conversions.MPaToSystem(30.0);
mat.CalculateDependentValues = true;
openModel.AddObject(mat);
```

![alt text][concreteprop]


-   new material of reinforcement

![alt text][reinforcementprop]


## Cross-section
The next step is to define the shape and dimension of cross-section and type of material.

![alt text][cross-section]


## Design member
### Member data
Setting of exposure classes, humidity and other important factors for the calculations (for example creep).

![alt text][member data]

### Imperfections
For calculation of second order effects, effective length of column and other parameters have to be set.

![alt text][imperfections]


## Internal forces
For assessment of limit states, actual internal forces into the analyzed cross-section need to be insert.

![alt text][forces]


## Second order effect
To calculate second order effects is necessary to insert the correct values of bending moment on the top and bottom of the column.

![alt text][secondorder]


## Reinforcement
Reinforcement is define as stirrups and longitudinal bars.

![alt text][reinforcement]

### Stirrups
Setting shape, diameter and material of stirrup.

![alt text][stirrups]

### Longitudinal reinforcement
Define position, material, diameter and quantity of longitudinal reinforcement.

![alt text][longreinforcement]


## Calculation control
This setting define, which type of assessment will be used and corresponding results will be displayed.

![alt text][calccontrol]


## Results
Results are displayed in tables with considered values for each assessment and some values are shown in figures.

![alt text][results]


## Sections
It is possible to have many types of design members and reinforced cross-sections in one project. In the “Sections” a cross-section can be assigned to the selected design member. Name of the section can be change, too.

![alt text][sections]


## Design members
There is a summary of all Design members in a project, where is possible to change name of member and some Member data.

![alt text][members]


## Reinforced cross-sections
A list of all reinforced cross-sections in project.

![alt text][rcs]



[projdata]: Images/Column/1.PNG "Project data"
[concreteprop]: Images/Column/15.PNG "Concrete"
[reinforcementprop]: Images/Column/16.PNG "Reinforcement"
[cross-section]: Images/Column/2.PNG "Cross-section"
[member data]: Images/Column/3.PNG "Member data"
[imperfections]: Images/Column/4.PNG "Imperfections"
[forces]: Images/Column/5.PNG "Internal forces"
[secondorder]: Images/Column/6.PNG "Second order effect"
[reinforcement]: Images/Column/7.PNG "Reinforcement"
[stirrups]: Images/Column/8.PNG "Stirrups"
[longreinforcement]: Images/Column/9.PNG "Longitudinal reinforcement"
[calccontrol]: Images/Column/10.PNG "Calulation control"
[results]: Images/Column/11.PNG "Results"
[sections]: Images/Column/12.PNG "Sections"
[members]: Images/Column/13.PNG "Members"
[rcs]: Images/Column/14.PNG "Reinforced cross-sections"
