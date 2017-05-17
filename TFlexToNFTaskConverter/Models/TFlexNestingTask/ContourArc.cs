using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Сущность элемента контура (дуги) TFlex/Раскрой
    /// </summary>
    public class ContourArc : ContourObject
    {
        public Point Center { get; set; }
        public double Radius { get; set; }
        public double Angle { get; set; }
        public bool Ccw { get; set; }
    }
}
