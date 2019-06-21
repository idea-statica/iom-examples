# IOM Example

This example describes how to create the IOM.

To start, we need create a standard console application. Launch Visual Studio. Select __File__ > __New__ > __Project__ from the menu bar. In the dialog, select the __Visual C#__ node followed by the __Get Started__ node. Then select the __Console App__ project template.

## Add the IdeaRS.OpenModel NuGet package

OpenModel is published as [the nuget package](https://www.nuget.org/packages/IdeaStatiCa.OpenModel/). To install the package, you can use either the Package Manager UI or the Package Manager Console.

For more information, see [Install and use a package in Visual Studio](https://docs.microsoft.com/cs-cz/nuget/quickstart/install-and-use-a-package-in-visual-studio)

There is also the documentation of [IdeaRS.OpenModel](https://idea-statica.github.io/iom/iom-api/html/ef39c768-a155-5401-182f-d445fe9723b5.htm).

## Example
Our main task will be to create the structure you can see in the picture below. The model consists of several columns, beams and some load cases.

![alt text][structure]

Let's create our IOM!

### Settings

Reference to the settings in open model is [here](https://github.com/idea-statica/iom/blob/master/IdeaRS.OpenModel/OriginSettings.cs).

You can set some default IOM parameters like project name, description etc.

```csharp
// create new instance of the open model
OpenModel model = new OpenModel();

model.OriginSettings = new OriginSettings() { CrossSectionConversionTable = CrossSectionConversionTable.SCIA };
model.OriginSettings.ProjectName = "Project";
model.OriginSettings.Author = "IDEA StatiCa s.r.o.";
model.OriginSettings.ProjectDescription = "Training example";
```

### Nodes

Individual nodes are placed in the structure as follows:

![alt text][nodes]

Table of all nodes with given coordinates:

|  Node  |  X  |  Y  |  Z  |
| ------ | --- | --- | --- |
|   N1   |  -2 |  3  |  0  |
|   N2   |  -2 |  3  |  3  |
|   N3   |  2  |  3  |  0  |
|   N4   |  2  |  3  |  3  |
|   N5   |  6  |  3  |  0  |
|   N6   |  6  |  3  |  3  |
|   N7   | -2  |  3  |  6  |
|   N8   |  2  |  3  |  6  |
|   N9   |  6  |  3  |  6  |

Create 3D point and fill properties: id, name and coordinates.

```csharp
// create N1
Point3D N1 = new Point3D() { X = -2, Y = 3, Z = 0 };
N1.Name = "N1";
N1.Id = 1;
model.AddObject(N1);

// create N2
Point3D N2 = new Point3D() { X = -2, Y = 3, Z = 3 };
N2.Name = "N2";
N2.Id = 2;
model.AddObject(N2);

// and so on...
```

### Materials

*Reference to the materials in open model is [here](https://github.com/idea-statica/iom/tree/master/IdeaRS.OpenModel/Libraries/Material).*

```csharp
MatSteelEc2 material = new MatSteelEc2();

// set properties
material.Id = 1;
material.Name = "S355";
material.E = 210000000000;
material.G = material.E / (2 * (1 + 0.3));
material.Poisson = 0.3;
material.UnitMass = 7850;
material.SpecificHeat = 0.6;
material.ThermalExpansion = 0.000012;
material.ThermalConductivity = 45;
material.IsDefaultMaterial = false;
material.OrderInCode = 0;
material.StateOfThermalExpansion = ThermalExpansionState.Code;
material.StateOfThermalConductivity = ThermalConductivityState.Code;
material.StateOfThermalSpecificHeat = ThermalSpecificHeatState.Code;
material.StateOfThermalStressStrain = ThermalStressStrainState.Code;
material.StateOfThermalStrain = ThermalStrainState.Code;
material.fy = 355000000;
material.fu = 510000000;
material.fy40 = 335000000;
material.fu40 = 470000000;
material.DiagramType = SteelDiagramType.Bilinear;

// add material to the model
model.AddObject(material);
```

### Cross sections

*Reference to the cross sections in open model is [here](https://github.com/idea-statica/iom/tree/master/IdeaRS.OpenModel/Libraries/CrossSection).*

Model has two types of cross sections: HE200B and HE240B. To create a single cross-section you need to know the material from previous section.

```csharp
// only one material is in the model
MatSteel material = model.MatSteel.FirstOrDefault();

CrossSectionParameter css = new CrossSectionParameter();

css.Id = 1;
css.Name = "HE200B";
css.CrossSectionRotation = 0;
css.CrossSectionType = CrossSectionType.RolledI;

css.Parameters.Add(new ParameterString() { Name = "UniqueName", Value = "HE200B" });
css.Material = new ReferenceElement(material);

// add cross sections to the model
model.AddObject(css);
```

### Members

There are two ways how members can be defined in the model:

__1. Member is composed of start node and end node.__

![alt text][first_member]

The code below describes the creation of first member (__M1__) that is composed of start node (__N2__) and end node (__N4__).

*Please notice, for better readability there are also helper functions like __CreateLineSegment3D__, __CreateElement1D__ and __CreateMember1D__ that you can find [here](https://github.com/idea-statica/iom-examples/blob/master/IOMExample/IOMExample/Helpers.cs).*

```csharp
// find appropriate cross sections
var css_he_200b = model.CrossSection.FirstOrDefault(item => item.Name == "HE200B");

// create line segments
LineSegment3D segment = CreateLineSegment3D(model, "N2", "N4");

// create polylines
PolyLine3D polyline = new PolyLine3D();
polyline.Id = model.GetMaxId(polyline) + 1;
polyline.Segments.Add(new ReferenceElement(segment));

// add polylines and segments to the model
model.AddObject(polyline);
model.AddObject(segment);

// create 1D elements
Element1D element = CreateElement1D(model, css, segment);
model.AddObject(element);

// create 1D members
Member1D member = CreateMember1D(model, 1, Member1DType.Beam, element);
model.Member1D.Add(member);

// create and return connected member
ConnectedMember M1 = new ConnectedMember();

M1.Id = 1;
M1.MemberId = new ReferenceElement(member);

model.AddObject(M1);
```

__2. Continuous member is composed of start node, middle node and end node.__ 

![alt text][second_member]

The code below describes the creation of second member (__M2__) that is composed of start node (__N1__), middle node (__N2__) and end node (__N7__).

```csharp
// find appropriate cross sections
var css_he_240b = model.CrossSection.FirstOrDefault(item => item.Name == "HE240B");

// create line segment from N1 to N2
LineSegment3D segment1 = CreateLineSegment3D(model, "N1", "N2");
model.AddObject(segment1);

// create line segment from N2 to N7
LineSegment3D segment2 = CreateLineSegment3D(model, "N2", "N7");
model.AddObject(segment2);

// create polylines
PolyLine3D polyline = new PolyLine3D();
polyline.Id = model.GetMaxId(polyline) + 1;
polyline.Segments.Add(new ReferenceElement(segment1));
polyline.Segments.Add(new ReferenceElement(segment2));
model.AddObject(polyline);

// create 1D elements
Element1D element1 = CreateElement1D(model, css, segment1);
model.AddObject(element1);

Element1D element2 = CreateElement1D(model, css, segment2);
model.AddObject(element2);

// create 1D members
Member1D member = CreateMember1D(model, 2, Member1DType.Column, element1, element2);
model.Member1D.Add(member);

// create and return connected member
ConnectedMember M2 = new ConnectedMember();

M2.Id = id;
M2.MemberId = new ReferenceElement(member);

model.AddObject(M2);
```

### Coordinate system

*Reference to the coordinate system is [here](https://idea-statica.github.io/iom/coord-system.html).*

The image and code below describes how to change local coordinate system in line segment.

![alt text][coordinates]

```csharp
// add local coordinate system to the LineSegment3D
CoordSystemByPoint coord = new CoordSystemByPoint();

coord.Point = new Point3D() { X = 100000, Y = 0, Z = 0 };
coord.InPlane = Plane.ZX;

LineSegment3D segment3D = new LineSegment3D();
segment3D.LocalCoordinateSystem = coord;
```

### Connection point

To make connection point you need instance of specific node and required members. 

![alt text][connection_point]

![alt text][unbalanced_forces]

```csharp
// create first connection point
ConnectionPoint CP1 = new ConnectionPoint();

CP1.Node = new ReferenceElement(model.Point3D.FirstOrDefault(n => n.Name == "N2"));
CP1.Id = model.GetMaxId(CP1) + 1;
CP1.Name = "CON " + CP1.Id.ToString();

// members from previous section
CP1.ConnectedMembers.Add(M1);
CP1.ConnectedMembers.Add(M3);

model.AddObject(CP1);
```

### Load cases

```csharp
// create first load group
LoadGroupEC LG1 = new LoadGroupEC(); ;

LG1.Id = 1;
LG1.Name = "PERM1";
LG1.Relation = Relation.Standard;
LG1.GroupType = LoadGroupType.Permanent;
LG1.GammaQ = 1.35;
LG1.Dzeta = 0.85;
LG1.GammaGInf = 1;
LG1.GammaGSup = 1.35;
model.AddObject(LG1);

// create first load case
LoadCase LC1 = new LoadCase();

LC1.Id = 1;
LC1.Name = "SelfWeight";
LC1.LoadType = LoadCaseType.Permanent;
LC1.Type = LoadCaseSubType.PermanentStandard;
LC1.Variable = VariableType.Standard;
LC1.LoadGroup = new ReferenceElement(LG1);

// create second load case
LoadCase LC2 = new LoadCase();

LC2.Id = 2;
LC2.Name = "PernamentLoading";
LC2.LoadType = LoadCaseType.Permanent;
LC2.Type = LoadCaseSubType.PermanentStandard;
LC2.Variable = VariableType.Standard;
LC2.LoadGroup = new ReferenceElement(LG1);

// create second load group
LoadGroupEC LG2 = new LoadGroupEC(); ;

LG2.Id = 2;
LG2.Name = "VAR1";
LG2.Relation = Relation.Exclusive;
LG2.GroupType = LoadGroupType.Variable;
LG2.GammaQ = 1.5;
LG2.Dzeta = 0.85;
LG2.GammaGInf = 0;
LG2.GammaGSup = 1.5;
LG2.Psi0 = 0.7;
LG2.Psi1 = 0.5;
LG2.Psi2 = 0.3;
model.AddObject(LG2);

// create third load case
LoadCase LC3 = new LoadCase();

LC3.Id = 3;
LC3.Name = "LiveLoad";
LC3.LoadType = LoadCaseType.Variable;
LC3.Type = LoadCaseSubType.VariableStatic;
LC3.Variable = VariableType.Standard;
LC3.LoadGroup = new ReferenceElement(LG2);

// add load cases to the model
model.AddObject(LC1);
model.AddObject(LC2);
model.AddObject(LC3);
```

### Combinations

```csharp
// create first combination input
CombiInputEC CI1 = new CombiInputEC();

CI1.Id = model.GetMaxId(CI1) + 1;
CI1.Name = "Co.#1";
CI1.Description = "SelfWeight + PernamentLoading + LiveLoad";
CI1.TypeCombiEC = TypeOfCombiEC.ULS;
CI1.TypeCalculationCombi = TypeCalculationCombiEC.Linear;

CombiItem item = new CombiItem();
item.Id = 1;
item.Coeff = 1;
item.LoadCase = new ReferenceElement(model.LoadCase.FirstOrDefault(l => l.Name == "SelfWeight"));
CI1.Items.Add(item);

item = new CombiItem();
item.Id = 2;
item.Coeff = 1;
item.LoadCase = new ReferenceElement(model.LoadCase.FirstOrDefault(l => l.Name == "PernamentLoading"));
CI1.Items.Add(item);

item = new CombiItem();
item.Id = 3;
item.Coeff = 1;
item.LoadCase = new ReferenceElement(model.LoadCase.FirstOrDefault(l => l.Name == "LiveLoad"));
CI1.Items.Add(item);

model.AddObject(CI1);
      
// and so on...     
```

### Results

Results which were generated by a FEA application (internal forces) can be optionally saved as in the format of [OpenModelResult](https://idea-statica.github.io/iom/iom-api/html/a3529228-0caa-4961-7b11-96c215d94bcf.htm). It contains internal forces on the Member1Ds. The relationships between the IOM and the IOM Results are defined by ID of objects.

The file (*.xmlR*) with results can be found [here](https://github.com/idea-statica/iom-examples/blob/master/IOMExample/IOMExample/Results/results.xmlR). Format is as follows:

```xml
<?xml version="1.0" encoding="utf-16"?>
<OpenModelResult xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <ResultOnMembers>
    <ResultOnMembers>
      <Members>
        <ResultOnMember>
          <Member>
            <MemberType>Member1D</MemberType>
            <Id>1</Id>
          </Member>
          <ResultType>InternalForces</ResultType>
          <Results>
            <ResultBase xsi:type="ResultOnSection">
              <AbsoluteRelative>Absolute</AbsoluteRelative>
              <Position>0</Position>
              <Results>
                <SectionResultBase xsi:type="ResultOfInternalForces">
                  <Loading>
                    <LoadingType>LoadCase</LoadingType>
                    <Id>1</Id>
                    <Items>
                      <ResultOfLoadingItem>
                        <Coefficient>1</Coefficient>
                      </ResultOfLoadingItem>
                    </Items>
                  </Loading>
                  <N>242.96484375</N>
                  <Qy>0</Qy>
                  <Qz>1176.9375</Qz>
                  <Mx>0</Mx>
                  <My>-727.5482177734375</My>
                  <Mz>0</Mz>
                </SectionResultBase>
                <SectionResultBase xsi:type="ResultOfInternalForces">
                  <Loading>
                    <LoadingType>LoadCase</LoadingType>
                    <Id>2</Id>
                    <Items>
                      <ResultOfLoadingItem>
                        <Coefficient>1</Coefficient>
                      </ResultOfLoadingItem>
                    </Items>
                  </Loading>
                  <N>20200.224609375</N>
                  <Qy>0</Qy>
                  <Qz>97851.2109375</Qz>
                  <Mx>0</Mx>
                  <My>-60488.7421875</My>
                  <Mz>0</Mz>
                </SectionResultBase>
              </Results>
            </ResultBase>
          <Results>
        </ResultOnMember>
      <Members>
    <ResultOnMembers>
  <ResultOnMembers>
</OpenModelResult>
```

[structure]: https://github.com/idea-statica/iom-examples/blob/master/IOMExample/IOMExample/Images/structure.PNG "Structure"
[nodes]: https://github.com/idea-statica/iom-examples/blob/master/IOMExample/IOMExample/Images/nodes.PNG "Nodes"
[first_member]: https://github.com/idea-statica/iom-examples/blob/master/IOMExample/IOMExample/Images/first_member.PNG "Member"
[second_member]: https://github.com/idea-statica/iom-examples/blob/master/IOMExample/IOMExample/Images/second_member.PNG "Continuous member"
[connection_point]: https://github.com/idea-statica/iom-examples/blob/master/IOMExample/IOMExample/Images/connection_point.PNG "Connection point"
[unbalanced_forces]: https://github.com/idea-statica/iom-examples/blob/master/IOMExample/IOMExample/Images/unbalanced_forces.PNG "Unbalanced forces"
[coordinates]: https://github.com/idea-statica/iom-examples/blob/master/IOMExample/IOMExample/Images/coordinates.PNG "Coordinates"
