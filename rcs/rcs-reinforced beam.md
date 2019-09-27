# IOM Example - Reinforced Beam

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
To create new project, these types of materials have to be defined:
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
The next step is to define the shape and dimensions of cross-section and type of material.

![alt text][cross-section]


## Reinforced cross-sections
After defining the concrete cross-section, reinforcement is set into this one. The reinforced section is defined in this way and referenced the concrete cross-section.

![alt text][rcs]


## Reinforcement
Reinforcement is defined as stirrups and longitudinal bars.

![alt text][reinforcement]


### Longitudinal reinforcement
Define position, material, diameter and quantity of longitudinal reinforcement.

![alt text][longreinforcement]

### Stirrups
Setting shape and material of stirrup.

![alt text][stirrups]


## Design member
Design member contains information about whole checked member. In the first step, it is required to define design member and then the member data are set into the design member.

### Member data
Setting of exposure classes, humidity and other important factors for the calculations (for example creep).

![alt text][member data]

### Flectural slendeness
In this dialog, it is required to set clear distance between faces of the supports and support conditions to check deflection of the beam.

![alt text][slenderness]


## Sections, Extremes, Internal forces
The reinforced cross-section and the check member are defined for the checked section. 
Extremes of internal forces (for ULS and SLS calculation) are set in the checked section data there.

For assessment of limit states, actual internal forces into the analyzed cross-section need to be insert.

![alt text][sections]
![alt text][forces]


## Calculation control
This setting define, which type of assessment will be used and corresponding results will be displayed.

![alt text][calccontrol]


## Concrete setup
Creating the code setup used for assessment of cross-section including national annex settings.

![alt text][concretesetup]


## Results
Results are displayed in tables with considered values for each assessment and some values are shown in figures.

![alt text][results]




[projdata]: Images/ReinforcedBeam/1.PNG "Project data"
[concreteprop]: Images/ReinforcedBeam/14.PNG "Concrete"
[reinforcementprop]: Images/ReinforcedBeam/15.PNG "Reinforcement"
[cross-section]: Images/ReinforcedBeam/2.PNG "Cross-section"
[member data]: Images/ReinforcedBeam/3.PNG "Member data"
[slenderness]: Images/ReinforcedBeam/4.PNG "Slenderness"
[forces]: Images/ReinforcedBeam/5.PNG "Internal forces"
[reinforcement]: Images/ReinforcedBeam/6.PNG "Reinforcement"
[stirrups]: Images/ReinforcedBeam/7.PNG "Stirrups"
[longreinforcement]: Images/ReinforcedBeam/8.PNG "Longitudinal reinforcement"
[calccontrol]: Images/ReinforcedBeam/9.PNG "Calulation control"
[results]: Images/ReinforcedBeam/10.PNG "Results"
[sections]: Images/ReinforcedBeam/11.PNG "Sections"
[members]: Images/ReinforcedBeam/12.PNG "Members"
[rcs]: Images/ReinforcedBeam/13.PNG "Reinforced cross-sections"
[concretesetup]: Images/ReinforcedBeam/16.PNG "Concrete setup"
