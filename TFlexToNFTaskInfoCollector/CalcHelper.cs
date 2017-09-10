using System;
using System.Collections.Generic;
using System.Linq;
using TFlexToNFTaskConverter.Models;
using TFlexToNFTaskConverter.Models.TFlexNestingTask;

namespace TFlexToNFTaskInfoCollector
{
    public static class CalcHelper
    {
        //TODO
        /*public static double CalcRightSide(TFlexTask task, bool isTflex = true)
        {
            if (isTflex)
            {
                
            }
        }*/

        public static double CalcArea(PartProfile partProfile)
        {
            var contourAreas = partProfile.Contours.Select(CalcContourArea).ToList();
            return contourAreas.FirstOrDefault() - contourAreas.Where((d, i) => i > 0).Sum();
        }
        private static double CalcContourArea(Contour partProfile)
        {
            if (partProfile is FigureContour fContour)
            {
                return CalcFigureContour(fContour);
            } else if (partProfile is CircleContour cContour)
            {
                return 4 * Math.PI * cContour.Radius;
            } else if (partProfile is RectangularContour rContour)
                return rContour.Length * rContour.Width;
            else return 0;
        }

        private static double CalcFigureContour(FigureContour fContour)
        {
            var points = new List<Point>();
            foreach (var p in fContour.Objects)
            {
                
                if (points.LastOrDefault()?.Equals(p.Begin) ?? true) points.Add(p.Begin);
                points.Add(p.End);

                if (p is ContourArc arc)
                {
                    var area = (arc.Radius * arc.Radius) * 0.5 *
                               (Math.PI * arc.Angle / 180 - Math.Sin(arc.Angle * Math.PI / 180));
                }
                
            }


            return CalcPoints(points);
        }

        private static double CalcPoints(List<Point> points)
        {
            //http://algolist.manual.ru/maths/geom/polygon/area.php
            var result = 0d;
            for (var k = 0; k < points.Count - 1; k++)
            {
                var currentPoint = points[k];
                var nextPoint = points[k + 1];
                result += (currentPoint.X + nextPoint.X) * (currentPoint.Y - nextPoint.Y);
            }
            return 0.5 * Math.Abs(result);
        }
    }
}
