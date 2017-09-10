using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    public class RectangularContour : Contour
    {
        public double Width { get; set; }
        public double Length { get; set; }
        public override void RotateAroundPoint(double ang, Point p)
        {
            //do nothing
        }
    }
}
