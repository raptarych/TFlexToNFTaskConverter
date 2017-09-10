using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Окружность
    /// </summary>
    public class CircleContour : Contour
    {
        public Point Center { get; set; }
        public double Radius { get; set; }
        public override void RotateAroundPoint(double ang, Point p = null) => Center = Center.Rotate(ang, p);
    }
}
