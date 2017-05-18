using System;
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
        public string ProjectType { get; set; }
        [XmlArray("Parts")]
        public List<PartDefinition> Parts { get; set; } = new List<PartDefinition>();

        [XmlArray("Sheets")]
        [XmlArrayItem(Type = typeof(SheetDefinition)),
         XmlArrayItem(Type = typeof(RectangularSheet)),
         XmlArrayItem(Type = typeof(ContourSheet))]
        public List<SheetDefinition> Sheets { get; set; } = new List<SheetDefinition>();

        public FigureParams FigureParams { get; set; }
        [XmlArray("Results")]
        [XmlArrayItem("NestingResult")]
        public List<NestingResult> Results { get; set; } = new List<NestingResult>();

    }
}
