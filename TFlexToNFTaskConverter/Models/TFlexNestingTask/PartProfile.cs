using System.Collections.Generic;
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
         XmlArrayItem(Type = typeof(CircleContour)),
         XmlArrayItem(Type = typeof(RectangularContour))]
        public List<Contour> Contours { get; set; } = new List<Contour>();

        /// <summary>
        /// Сугубо для парсера NF
        /// </summary>
        [XmlIgnore]
        public string ItemName { get; set; }

        public void RotateAroundPoint(double ang, Point p = null)
        {
            foreach (var c in Contours) c.RotateAroundPoint(ang, p);
        }
    }
}
