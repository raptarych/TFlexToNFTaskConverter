﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TFlexToNFTaskConverter.Models.TFlexNestingTask;

namespace TFlexToNFTaskConverter.Models
{
    /// <summary>
    /// Модель для сериализации XML-ки формата .tfnesting 
    /// </summary>
    [XmlRoot("Project")]
    public class TFlexTask
    {
        public string Name { get; set; }
        public string ProjectType { get; set; } = "FigureCutting";
        [XmlArray("Parts")]
        public List<PartDefinition> Parts { get; set; } = new List<PartDefinition>();

        [XmlArray("Sheets")]
        [XmlArrayItem(Type = typeof(SheetDefinition)),
         XmlArrayItem(Type = typeof(RectangularSheet)),
         XmlArrayItem(Type = typeof(ContourSheet))]
        public List<SheetDefinition> Sheets { get; set; } = new List<SheetDefinition>();

        public FigureParams FigureParams { get; set; } = new FigureParams();
        [XmlArray("Results")]
        [XmlArrayItem("NestingResult")]
        public List<NestingResult> Results { get; set; } = new List<NestingResult>();

        public SettingsModel Settings { get; set; } = new SettingsModel();
        public GuillotineParamsModel GuillotineParams { get; set; } = new GuillotineParamsModel();

        public int GetNewSheetId() => Sheets.Any() ? Sheets.Max(sheet => sheet.ID) + 1 : 0;
        public int GetNewPartId() => Parts.Any() ? Parts.Max(part => part.ID) + 1 : 0;

        public double RightSide(bool isTflex = true)
        {
            var maxY = 0d;
            var maxName = "";
            var resultParts = Results.SelectMany(i => i.Layouts).SelectMany(i => i.PartPositions);
            foreach (var partResult in resultParts)
            {
                var part = Parts.FirstOrDefault(i => i.ID == partResult.PartID);
                if (part == null) continue;
                var partProfile = part.PartProfile;
                partProfile.RotateAroundPoint(partResult.AngleDeg);
                foreach (var contour in part.PartProfile.Contours)
                {
                    if (contour is FigureContour lCont)
                    {
                        lCont.Objects.ForEach(o =>
                        {
                            var localMaxY = Math.Max(o.Begin.Y + partResult.Position.Y, o.End.Y + partResult.Position.Y);
                            if (localMaxY > maxY)
                            {
                                maxY = localMaxY;
                                maxName = part.Name;
                            }
                        });
                    }
                }
            }

            return maxY;
        }
    }
}
