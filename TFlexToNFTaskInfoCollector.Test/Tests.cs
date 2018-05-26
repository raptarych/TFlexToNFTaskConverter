using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        [TestMethod]
        public void Shit()
        {
            var k = 3;
            var numsList = new List<int>();
            for (int i = 1; i <= 1000; i++) numsList.Add(i);
            var nums = numsList.ToArray();

            var singleCount = nums.Where(i => i % k == 0).Count();
            //nums = nums.Where(i => i % k != 0).ToArray();
            var subsequences = 0;
            for (var index = 0; index < nums.Length; index++)
            {
                var localOffset = 0;
                for (var count = 1; count <= nums.Length - index; count++)
                {
                    var shit = nums.Skip(index + localOffset).Take(count - localOffset);
                    var sum = shit.Sum();
                    if (sum < k) continue;
                    if (sum % k == 0)
                    {
                        subsequences++;
                        localOffset = localOffset + shit.Count();
                    }
                }
            }

            Assert.IsTrue(subsequences > 0);
            
        }

        [TestMethod]
        public void Shit2()
        {
            var minIndex = 1;
            var maxIndex = 5;

            var testsCount = 5;

            var minDetails = 14;
            var maxDetails = 20;
            var random = new Random();

            var partSubstrs = new Dictionary<int, string>();
            for (var testNum = 0; testNum < testsCount; testNum++)
            {
                while (true)
                {
                    var partNum = new Random().Next(minIndex, maxIndex);
                    if (!partSubstrs.ContainsKey(partNum))
                    {
                        var substr = $"{partNum}x{random.Next(minDetails, maxDetails)}";
                        partSubstrs.Add(partNum, substr);
                        break;
                    }
                }
            }

            Debug.WriteLine($"{string.Join(", ", partSubstrs.Values)}, всего {partSubstrs.Values.Select(i => int.Parse(i.Split('x')[1]))} деталей");
        }
    }
}
