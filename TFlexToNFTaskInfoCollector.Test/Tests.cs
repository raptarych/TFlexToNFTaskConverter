using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFlexToNFTaskConverter.Models;
using TFlexToNFTaskConverter.Models.TFlexNestingTask;

namespace TFlexToNFTaskInfoCollector.Test
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void CalcSquareArea()
        {
            var outerContour = new FigureContour
            {
                Objects = new List<ContourObject>
                {
                    new ContourLine { Begin = new Point {X = 0, Y = 0}, End = new Point {X = 0, Y = 1000} },
                    new ContourLine { Begin = new Point {X = 0, Y = 1000}, End = new Point {X = 1000, Y = 1000} },
                    new ContourLine { Begin = new Point {X = 1000, Y = 1000}, End = new Point {X = 1000, Y = 0} },
                    new ContourLine { Begin = new Point {X = 1000, Y = 0}, End = new Point {X = 0, Y = 0} }
                }
            };

            var innerContour = new FigureContour
            {
                Objects = new List<ContourObject>
                {
                    new ContourLine { Begin = new Point {X = 10, Y = 10}, End = new Point {X = 10, Y = 110} },
                    new ContourLine { Begin = new Point {X = 10, Y = 110}, End = new Point {X = 110, Y = 110} },
                    new ContourLine { Begin = new Point {X = 110, Y = 110}, End = new Point {X = 110, Y = 10} },
                    new ContourLine { Begin = new Point {X = 110, Y = 10}, End = new Point {X = 10, Y = 10} }
                }
            };

            var partProfile = new PartProfile
            {
                Contours = new List<Contour> { outerContour, innerContour }
            };
            Assert.AreEqual(1000000 - 10000, CalcHelper.CalcArea(partProfile));
        }
    }
}
