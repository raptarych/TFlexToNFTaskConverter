using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Сущность профиля детали TFlex/Раскрой
    /// </summary>
    public class PartProfile
    {
        public double Area { get; set; }
        [XmlArray("Contours")]
        [XmlArrayItem(Type = typeof(Contour)),
         XmlArrayItem(Type = typeof(FigureContour)),
         XmlArrayItem(Type = typeof(CircleContour))]
        public List<Contour> Contours { get; set; }
    }
}
