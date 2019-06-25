using System.Linq;
using IdeaRS.OpenModel;
using IdeaRS.OpenModel.Connection;
using IdeaRS.OpenModel.CrossSection;
using IdeaRS.OpenModel.Geometry3D;
using IdeaRS.OpenModel.Loading;
using IdeaRS.OpenModel.Material;
using IdeaRS.OpenModel.Model;

namespace IOM.SteelFrame
{
	public static class Example
	{
		/// <summary>
		/// Create example of the IOM
		/// </summary>
		/// <returns>Valid open model</returns>
		public static OpenModel CreateIOM()
		{
			OpenModel model = new OpenModel();

			// add setting
			AddSettingsToIOM(model);

			// add nodes
			AddNodesToIOM(model);

			// add materials
			AddMaterialsToIOM(model);

			// add cross section
			AddCrossSectionToIOM(model);

			// add connection point with members
			AddConnectionPointsToIOM(model);

			// add load cases
			AddLoadCasesToIOM(model);

			// add combinations
			AddCombinationsToIOM(model);

			return model;
		}

		/// <summary>
		/// Add settings to the IDEA open model
		/// </summary>
		/// <param name="model">Open model</param>
		private static void AddSettingsToIOM(OpenModel model)
		{
			model.OriginSettings = new OriginSettings();

			model.OriginSettings.CrossSectionConversionTable = CrossSectionConversionTable.SCIA;
			model.OriginSettings.CountryCode = CountryCode.ECEN;
			model.OriginSettings.ProjectName = "Project";
			model.OriginSettings.Author = "IDEA StatiCa s.r.o.";
			model.OriginSettings.ProjectDescription = "Training example";
		}

		/// <summary>
		/// Add nodes to the IDEA open model
		/// </summary>
		/// <param name="model">Open model</param>
		private static void AddNodesToIOM(OpenModel model)
		{
			Point3D N1 = new Point3D() { X = -2, Y = 3, Z = 0 };
			N1.Name = "N1";
			N1.Id = 1;
			model.AddObject(N1);

			Point3D N2 = new Point3D() { X = -2, Y = 3, Z = 3 };
			N2.Name = "N2";
			N2.Id = 2;
			model.AddObject(N2);

			Point3D N3 = new Point3D() { X = 2, Y = 3, Z = 0 };
			N3.Name = "N3";
			N3.Id = 3;
			model.AddObject(N3);

			Point3D N4 = new Point3D() { X = 2, Y = 3, Z = 3 };
			N4.Name = "N4";
			N4.Id = 4;
			model.AddObject(N4);

			Point3D N5 = new Point3D() { X = 6, Y = 3, Z = 0 };
			N5.Name = "N5";
			N5.Id = 5;
			model.AddObject(N5);

			Point3D N6 = new Point3D() { X = 6, Y = 3, Z = 3 };
			N6.Name = "N6";
			N6.Id = 6;
			model.AddObject(N6);

			Point3D N7 = new Point3D() { X = -2, Y = 3, Z = 6 };
			N7.Name = "N7";
			N7.Id = 7;
			model.AddObject(N7);

			Point3D N8 = new Point3D() { X = 2, Y = 3, Z = 6 };
			N8.Name = "N8";
			N8.Id = 8;
			model.AddObject(N8);

			Point3D N9 = new Point3D() { X = 6, Y = 3, Z = 6 };
			N9.Name = "N9";
			N9.Id = 9;
			model.AddObject(N9);
		}

		/// <summary>
		/// Add materials to the IDEA open model
		/// </summary>
		/// <param name="model">Open model</param>
		private static void AddMaterialsToIOM(OpenModel model)
		{
			MatSteelEc2 material = new MatSteelEc2();

			material.Id = 1;
			material.Name = "S355";
			material.E = 210000000000.00003;
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
			material.fy = 355000000.00000006;
			material.fu = 510000000.00000006;
			material.fy40 = 335000000.00000006;
			material.fu40 = 470000000.00000006;
			material.DiagramType = SteelDiagramType.Bilinear;

			model.AddObject(material);
		}

		/// <summary>
		/// Add cross section to the IDEA open model
		/// </summary>
		/// <param name="model">Open model</param>
		private static void AddCrossSectionToIOM(OpenModel model)
		{
			// only one material is in the model
			MatSteel material = model.MatSteel.FirstOrDefault();

			// create first cross section
			CrossSectionParameter css1 = Helpers.CreateCSSParameter(1, "HE200B", material);

			// create second cross section
			CrossSectionParameter css2 = Helpers.CreateCSSParameter(2, "HE240B", material);
			
			// add cross sections to the model
			model.AddObject(css1);
			model.AddObject(css2);
		}

		/// <summary>
		/// Add members and connection points to the IDEA open model
		/// </summary>
		/// <param name="model">Open model</param>
		private static void AddConnectionPointsToIOM(OpenModel model)
		{
			// find appropriate cross sections
			var css_he_240b = model.CrossSection.FirstOrDefault(item => item.Name == "HE240B");
			var css_he_200b = model.CrossSection.FirstOrDefault(item => item.Name == "HE200B");

			// member for left floor beam
			ConnectedMember M1 = Helpers.CreateMember(model, 1, Member1DType.Beam, css_he_200b, "N2", "N4");

			// member for right floor beam
			ConnectedMember M2 = Helpers.CreateMember(model, 2, Member1DType.Beam, css_he_200b, "N4", "N6");

			// member for left column
			ConnectedMember M3 = Helpers.CreateMember(model, 3, Member1DType.Column, css_he_240b, "N1", "N2", "N7");

			// member for middle column
			ConnectedMember M4 = Helpers.CreateMember(model, 4, Member1DType.Column, css_he_240b, "N3", "N4", "N8");

			// member for right column
			ConnectedMember M5 = Helpers.CreateMember(model, 5, Member1DType.Column, css_he_240b, "N5", "N6", "N9");

			// member for upper continuous beam
			ConnectedMember M6 = Helpers.CreateMember(model, 6, Member1DType.Beam, css_he_200b, "N7", "N8", "N9");

			// add members to the model
			model.AddObject(M1);
			model.AddObject(M2);
			model.AddObject(M3);
			model.AddObject(M4);
			model.AddObject(M5);
			model.AddObject(M6);

			// create first connection point
			ConnectionPoint CP1 = new ConnectionPoint();

			CP1.Node = new ReferenceElement(model.Point3D.FirstOrDefault(n => n.Name == "N2"));
			CP1.Id = model.GetMaxId(CP1) + 1;
			CP1.Name = "CON " + CP1.Id.ToString();

			CP1.ConnectedMembers.Add(M1);
			CP1.ConnectedMembers.Add(M3);

			model.AddObject(CP1);

			// create second connection point
			ConnectionPoint CP2 = new ConnectionPoint();

			CP2.Node = new ReferenceElement(model.Point3D.FirstOrDefault(n => n.Name == "N4"));
			CP2.Id = model.GetMaxId(CP2) + 1;
			CP2.Name = "CON " + CP2.Id.ToString();

			CP2.ConnectedMembers.Add(M1);
			CP2.ConnectedMembers.Add(M2);
			CP2.ConnectedMembers.Add(M4);

			model.AddObject(CP2);
		}

		/// <summary>
		/// Add load cases to the IDEA open model
		/// </summary>
		/// <param name="model">Open model</param>
		private static void AddLoadCasesToIOM(OpenModel model)
		{
			// create LG1
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

			// create LC1
			LoadCase LC1 = new LoadCase();

			LC1.Id = 1;
			LC1.Name = "SelfWeight";
			LC1.LoadType = LoadCaseType.Permanent;
			LC1.Type = LoadCaseSubType.PermanentStandard;
			LC1.Variable = VariableType.Standard;
			LC1.LoadGroup = new ReferenceElement(LG1);

			// create LC2
			LoadCase LC2 = new LoadCase();

			LC2.Id = 2;
			LC2.Name = "PernamentLoading";
			LC2.LoadType = LoadCaseType.Permanent;
			LC2.Type = LoadCaseSubType.PermanentStandard;
			LC2.Variable = VariableType.Standard;
			LC2.LoadGroup = new ReferenceElement(LG1);

			// create LG2
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

			// create LC3
			LoadCase LC3 = new LoadCase();

			LC3.Id = 3;
			LC3.Name = "LiveLoad";
			LC3.LoadType = LoadCaseType.Variable;
			LC3.Type = LoadCaseSubType.VariableStatic;
			LC3.Variable = VariableType.Standard;
			LC3.LoadGroup = new ReferenceElement(LG2);

			// add load cases
			model.AddObject(LC1);
			model.AddObject(LC2);
			model.AddObject(LC3);
		}

		/// <summary>
		/// Add combinations to the IDEA open model
		/// </summary>
		/// <param name="model">OpenModel</param>
		private static void AddCombinationsToIOM(OpenModel model)
		{
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

			// create second combination input
			CombiInputEC CI2 = new CombiInputEC();

			CI2.Id = model.GetMaxId(CI2) + 1;
			CI2.Name = "Co.#2";
			CI2.Description = "SelfWeight";
			CI2.TypeCombiEC = TypeOfCombiEC.ULS;
			CI2.TypeCalculationCombi = TypeCalculationCombiEC.Linear;

			item = new CombiItem();
			item.Id = 1;
			item.Coeff = 1;
			item.LoadCase = new ReferenceElement(model.LoadCase.FirstOrDefault(l => l.Name == "SelfWeight"));
			CI2.Items.Add(item);

			model.AddObject(CI2);
		}
	}
}
