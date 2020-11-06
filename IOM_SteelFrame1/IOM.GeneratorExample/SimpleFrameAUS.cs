﻿using IdeaRS.OpenModel;
using IdeaRS.OpenModel.Connection;
using IdeaRS.OpenModel.CrossSection;
using IdeaRS.OpenModel.Geometry3D;
using IdeaRS.OpenModel.Material;
using IdeaRS.OpenModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOM.GeneratorExample
{
	public static class SimpleFrameAUS
	{
		public static OpenModel CreateIOM()
		{
			OpenModel model = new OpenModel();

			// add nodes
			AddNodesToIOM(model);

			// add materials
			AddMaterialsToIOM(model);

			// add cross section
			AddCrossSectionToIOM(model);

			// add connection point with members
			//AddConnectionPointsToIOM(model);

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
			model.OriginSettings.CountryCode = CountryCode.Australia;
			model.OriginSettings.ProjectName = "Project";
			model.OriginSettings.Author = "IDEA StatiCa s.r.o.";
			model.OriginSettings.ProjectDescription = "Aust";
		}

		/// <summary>
		/// Add nodes to the IDEA open model
		/// </summary>
		/// <param name="model">Open model</param>
		private static void AddNodesToIOM(OpenModel model)
		{
			Point3D N1 = new Point3D() { X = 0, Y = 0, Z = 0 };
			N1.Name = "N1";
			N1.Id = 1;
			model.AddObject(N1);

			Point3D N2 = new Point3D() { X = 0, Y = 0, Z = 3 };
			N2.Name = "N2";
			N2.Id = 2;
			model.AddObject(N2);

			Point3D N3 = new Point3D() { X = 0, Y = 0, Z = 6 };
			N3.Name = "N3";
			N3.Id = 3;
			model.AddObject(N3);

			Point3D N4 = new Point3D() { X = 3, Y = 0, Z = 3 };
			N4.Name = "N4";
			N4.Id = 4;
			model.AddObject(N4);
		}

		/// <summary>
		/// Add materials to the IDEA open model
		/// </summary>
		/// <param name="model">Open model</param>
		private static void AddMaterialsToIOM(OpenModel model)
		{
			MatSteelAUS material = new MatSteelAUS();

			material.Id = 1;
			material.Name = "C350";
			material.LoadFromLibrary = true;

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
			CrossSectionParameter css1 = Helpers.CreateCSSParameter(1, "125TFB", material);

			// create second cross section
			CrossSectionParameter css2 = Helpers.CreateCSSParameter(2, "100TFB", material);

			// add cross sections to the model
			model.AddObject(css1);
			model.AddObject(css2);
		}

		/// <summary>
		/// Add members and connection points to the IDEA open model
		/// </summary>
		/// <param name="model">Open model</param>
		private static void CreateFrameGeometry(OpenModel model)
		{
			// find appropriate cross sections
			var columnCss = model.CrossSection.FirstOrDefault(item => item.Name == "125TFB");
			var beamCss = model.CrossSection.FirstOrDefault(item => item.Name == "100TFB");

			// member - column bottom part
			ConnectedMember M1 = Helpers.CreateMember(model, 1, Member1DType.Column, columnCss, "N1", "N2");

			// member - column upper part
			ConnectedMember M2 = Helpers.CreateMember(model, 2, Member1DType.Column, columnCss, "N2", "N3");

			// member - beam
			ConnectedMember M3 = Helpers.CreateMember(model, 3, Member1DType.Beam, beamCss, "N2", "N4");

			// add members to the model
			model.AddObject(M1);
			model.AddObject(M2);
			model.AddObject(M3);

			// create the connection point in the node N2
			ConnectionPoint CP1 = new ConnectionPoint();

			CP1.Node = new ReferenceElement(model.Point3D.FirstOrDefault(n => n.Name == "N2"));
			CP1.Id = model.GetMaxId(CP1) + 1;
			CP1.Name = "CON " + CP1.Id.ToString();

			CP1.ConnectedMembers.Add(M1);
			CP1.ConnectedMembers.Add(M3);

			model.AddObject(CP1);
		}
	}
}
