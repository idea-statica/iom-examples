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
```csharp
//Common project data
var projectData = new ProjectData();
projectData.Name = "Column project";
projectData.Date = new DateTime(2019, 6, 4);

//Additionl data for Ec
var projectDataEc = new ProjectDataEc();
projectDataEc.AnnexCode = NationalAnnexCode.NoAnnex;
projectDataEc.FatigueCheck = false;
projectDataEc.FatigueAnnexNN = false;
projectData.CodeDependentData = projectDataEc;

openModel.ProjectData = projectData;

//Concrete project data
var projectDataConcrete = new ProjectDataConcreteEc2();
projectDataConcrete.CodeEN1992_2 = false;
projectDataConcrete.CodeEN1992_3 = false;
openModel.ProjectDataConcrete = projectDataConcrete;
```


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
```csharp
//Reinforcement material
MatReinforcementEc2 matR = new MatReinforcementEc2();
matR.Name = "B 500B";
matR.UnitMass = 7850.0;
matR.E = Conversions.GPaToSystem(200);
matR.Poisson = 0.2;
matR.G = Conversions.GPaToSystem(83.333);
matR.SpecificHeat = 0.6;
matR.ThermalExpansion = 0.00001;
matR.ThermalConductivity = 45;
matR.Fyk = Conversions.MPaToSystem(500);
matR.CoeffFtkByFyk = 1.08;
matR.Epsuk = 0.025;
matR.Type = ReinfType.Bars;
matR.BarSurface = ReinfBarSurface.Ribbed;
matR.Class = ReinfClass.B;
matR.Fabrication = ReinfFabrication.HotRolled;
matR.DiagramType = ReinfDiagramType.BilinerWithAnInclinedTopBranch;
openModel.AddObject(matR);
```

![alt text][reinforcementprop]


## Cross-section
The next step is to define the shape and dimension of cross-section and type of material.

```csharp
CrossSectionParameter css = new CrossSectionParameter(); //creating instance of cross-section defined by parameters
css.Name = "CSS 1";
css.Id = openModel.GetMaxId(css) + 1;
css.CrossSectionType = CrossSectionType.Rect;
css.Parameters.Add(new ParameterDouble() { Name = "Height", Value = 0.5 });
css.Parameters.Add(new ParameterDouble() { Name = "Width", Value = 0.5 });
css.Material = new ReferenceElement(mat);
openModel.AddObject(css);
```

![alt text][cross-section]

## Reinforced cross-sections

After defining the concrete cross section the reinforcement is set into this one. The reinforced section is defined in this way and referenced the concrete-cross-section.

```csharp
A list of all reinforced cross-sections in project.
//Reinforced section - concrete with reinforcement
ReinforcedCrossSection rcs = new ReinforcedCrossSection();
rcs.Name = "R 1";
rcs.CrossSection = new ReferenceElement(css);
openModel.AddObject(rcs);
```

![alt text][reinforcement]

## Reinforcement
Reinforcement is define as stirrups and longitudinal bars.

### Longitudinal reinforcement
Define position, material, diameter and quantity of longitudinal reinforcement.

```csharp
ReinforcedBar bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = -0.1939;
bar.Point.Y = 0.1939;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = -0.1939;
bar.Point.Y = -0.1939;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = 0.1939;
bar.Point.Y = -0.1939;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = 0.1939;
bar.Point.Y = 0.1939;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = -0.0613;
bar.Point.Y = -0.198;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = 0.0613;
bar.Point.Y = -0.198;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = 0.0613;
bar.Point.Y = 0.198;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = -0.0613;
bar.Point.Y = 0.198;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = -0.198;
bar.Point.Y = 0.0613;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = -0.198;
bar.Point.Y = -0.0613;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = 0.198;
bar.Point.Y = -0.0613;
rcs.Bars.Add(bar);

bar = new ReinforcedBar();
bar.Diameter = 0.020;
bar.Material = new ReferenceElement(matR);
bar.Point = new Point2D();
bar.Point.X = 0.198;
bar.Point.Y = 0.0613;
rcs.Bars.Add(bar);
```

![alt text][longreinforcement]

### Stirrups
Setting shape, diameter and material of stirrup.

```csharp
var stirrup = new Stirrup();
stirrup.Diameter = 0.012;
stirrup.DiameterOfMandrel = 4.0;
stirrup.Distance = 0.2;
stirrup.IsClosed = true;
stirrup.Material = new ReferenceElement(matR);
var poly = new PolyLine2D();

poly.StartPoint = new Point2D();
poly.StartPoint.X = -0.214;
poly.StartPoint.Y = 0.214;
var segment = new LineSegment2D();
segment.EndPoint = new Point2D();
segment.EndPoint.X = -0.214;
segment.EndPoint.Y = -0.214;
poly.Segments.Add(segment);

segment = new LineSegment2D();
segment.EndPoint = new Point2D();
segment.EndPoint.X = 0.214;
segment.EndPoint.Y = -0.214;
poly.Segments.Add(segment);

segment = new LineSegment2D();
segment.EndPoint = new Point2D();
segment.EndPoint.X = 0.214;
segment.EndPoint.Y = 0.214;
poly.Segments.Add(segment);

segment = new LineSegment2D();
segment.EndPoint = new Point2D();
segment.EndPoint.X = -0.214;
segment.EndPoint.Y = 0.214;
poly.Segments.Add(segment);

stirrup.Geometry = poly;
rcs.Stirrups.Add(stirrup);
```

![alt text][stirrups]


## Design member

Design member contains information about whole checked member. The first must be defined design member and then the member data are set into this design member.

```csharp
var checkMember = new CheckMember1D(); //Design member data object
openModel.AddObject(checkMember);
```

### Member data
Setting of exposure classes, humidity and other important factors for the calculations (for example creep).

```csharp
//Concrete member data
var memberData = new ConcreteMemberDataEc2(); //Member data base common object
memberData.MemberType = ConcreteMemberType.Column;
memberData.RelativeHumidity = 0.65;
memberData.CreepCoeffInfinityValue = InputValue.Calculated;
memberData.MemberImportance = MemberImportance.Major;

memberData.ExposureClassesData = new ExposureClassesDataEc2(); //Exposure classes
memberData.ExposureClassesData.NoCorrosionCheck = false;
memberData.ExposureClassesData.CarbonationCheck = true;
memberData.ExposureClassesData.Carbonation = ExposureClassEc2.XC3;
memberData.ExposureClassesData.ChloridesCheck = true;
memberData.ExposureClassesData.Chlorides = ExposureClassEc2.XD1;
memberData.ExposureClassesData.ChloridesFromSeaCheck = false;
memberData.ExposureClassesData.FreezeAttackCheck = false;
memberData.ExposureClassesData.ChemicalAttackCheck = false;

memberData.Element = new ReferenceElement(checkMember);
openModel.AddObject(memberData);

//Beam data are not necessary but must be created a default one
memberData.BeamData = new BeamDataEc2();
```

![alt text][member data]

### Imperfections
For calculation of second order effects, effective length of column and other parameters have to be set.

```csharp
//Concrete member data
memberData.ColumnData = new ColumnDataEc2();
memberData.ColumnData.L = 3.0;
memberData.ColumnData.EffectiveLength = InputValue.UserInput;
memberData.ColumnData.L0Y = 3.0;
memberData.ColumnData.L0Z = 3.0;

memberData.ColumnData.SecondOrderEffectInput = InputValue.Calculated;
memberData.ColumnData.GeometricImperfectionsULS = true;
memberData.ColumnData.GeometricImperfectionsSLS = false;
memberData.ColumnData.EffectConsidered = EffectConsideredType.IsolatedMember;
memberData.ColumnData.ImperfectionDirection = ImperfectionDirection.FromSetup;

memberData.ColumnData.Calculation2ndOrderEffect = true;
memberData.ColumnData.BracedY = false;
memberData.ColumnData.BracedZ = false;
memberData.ColumnData.SecondOrderEffectMethod = SecondOrderEffectMethodEc2.NominalCurvature;
memberData.ColumnData.ValueTypeOfcY = ValueTypec.UserDefined;
memberData.ColumnData.UserValuecY = 9.8696;
memberData.ColumnData.ValueTypeOfcZ = ValueTypec.UserDefined;
memberData.ColumnData.UserValuecZ = 9.8696;
```

![alt text][imperfections]


## Sections, Extremes, Internal forces, Second order effect
For the check section data the reinforcement cross-section and the check member is defined.
In the check section data there are defined extremes of internal forces for ULS and SLS calculation.

For assessment of limit states, actual internal forces into the analyzed cross-section need to be insert.
To calculate second order effects is necessary to insert the correct values of bending moment on the top and bottom of the column.

```csharp
//Standard section
var singleCheckSection = new StandardCheckSection();
singleCheckSection.Description = "S 1";
singleCheckSection.ReinfSection = new ReferenceElement(rcs);
singleCheckSection.CheckMember = new ReferenceElement(checkMember);

//add extreme to section
var sectionExtreme = new StandardCheckSectionExtreme();
sectionExtreme.Fundamental = new LoadingULS();
sectionExtreme.Fundamental.InternalForces = new IdeaRS.OpenModel.Result.ResultOfInternalForces() { N = -3750.0e3, My = 112.7e3, Mz = -52.0e3 };
sectionExtreme.Fundamental.InternalForcesBegin = new IdeaRS.OpenModel.Result.ResultOfInternalForces() { My = 22.0e3, Mz = -5.0e3 };
sectionExtreme.Fundamental.InternalForcesEnd = new IdeaRS.OpenModel.Result.ResultOfInternalForces() { My = 18.0e3, Mz = 10.0e3 };
singleCheckSection.Extremes.Add(sectionExtreme);

openModel.AddObject(singleCheckSection);
```

![alt text][forces]
![alt text][secondorder]


## Calculation control
This setting define, which type of assessment will be used and corresponding results will be displayed.

```csharp
memberData.CalculationSetup = new CalculationSetup();
memberData.CalculationSetup.UlsDiagram = true;
memberData.CalculationSetup.UlsShear = false;
memberData.CalculationSetup.UlsTorsion = false;
memberData.CalculationSetup.UlsInteraction = true;
memberData.CalculationSetup.SlsStressLimitation = true;
memberData.CalculationSetup.SlsCrack = true;
memberData.CalculationSetup.Detailing = true;
memberData.CalculationSetup.UlsResponse = true;
memberData.CalculationSetup.SlsStiffnesses = false;
memberData.CalculationSetup.MNKappaDiagram = false;
```

![alt text][calccontrol]


## Concrete setup
Creating the norm code setup for the check of concrete including settings of the annex.

```csharp
//Concrete setup
var setup = new ConcreteSetupEc2();
setup.Annex = NationalAnnexCode.NoAnnex;
openModel.ConcreteSetup = setup;
```

![alt text][concretesetup]


## Results
Results are displayed in tables with considered values for each assessment and some values are shown in figures.

```csharp
...
```


![alt text][results]


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
[concretesetup]: Images/Column/x.PNG "Concrete setup"
