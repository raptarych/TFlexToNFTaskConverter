using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{

    /// <summary>
    /// Сущность контура детали TFlex/Раскрой
    /// </summary>
    public class FigureContour : Contour
    {
        [XmlArray("Objects")]
        [XmlArrayItem(Type = typeof(ContourObject)),
        XmlArrayItem(Type = typeof(ContourArc)),
        XmlArrayItem(Type = typeof(ContourLine))]
        public List<ContourObject> Objects { get; set; } = new List<ContourObject>();
    }
}
