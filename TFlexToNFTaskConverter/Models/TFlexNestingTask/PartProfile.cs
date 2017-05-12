using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models
{
    /// <summary>
    /// Сущность профиля детали TFlex/Раскрой
    /// </summary>
    public class PartProfile
    {
        public double Area { get; set; }
        [XmlArrayAttribute("Contours")]
        [XmlArrayItem("Contour")]
        public List<FigureContour> Contours { get; set; }
    }
}
