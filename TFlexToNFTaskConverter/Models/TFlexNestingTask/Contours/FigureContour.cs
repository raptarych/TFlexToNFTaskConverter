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

        public void CloseContour()
        {
            var firstPoint = Objects.FirstOrDefault()?.Begin;
            var lastPoint = Objects.LastOrDefault()?.End;
            if (!firstPoint?.Equals(lastPoint) ?? false)
                Objects.Add(new ContourLine() {Begin = lastPoint, End = firstPoint});
        }

        public override void RotateAroundPoint(double ang, Point p)
        {
            foreach (var obj in Objects) obj.RotateAroundPoint(ang, p);
        }
    }
}
