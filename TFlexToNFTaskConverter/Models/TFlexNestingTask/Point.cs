using System;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{

    /// <summary>
    /// Сущность точки TFlex/Раскрой
    /// </summary>
    public class Point
    {
        public Point() {}

        public Point(double x, double y, double b = 0)
        {
            X = x;
            Y = y;
            B = b;
        }

        public double X { get; set; }
        public double Y { get; set; }
        [XmlIgnore]
        public double B { get; set; }
        [XmlIgnore]
        public bool IsEmpty => Math.Abs(X) < 0.0001 && Math.Abs(Y) < 0.0001 && Math.Abs(B) < 0.0001;
        [XmlIgnore]
        public double Length => Math.Sqrt(X * X + Y * Y);
        public Point Normalize() => this / Length;
        public Point Rotate(double angle) => new Point(X * Math.Cos(angle) - Y * Math.Sin(angle), Y * Math.Cos(angle) + X * Math.Sin(angle));
        public static Point operator -(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);
        public static Point operator +(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);
        public static Point operator /(Point p1, double i) => new Point(p1.X / i, p1.Y / i);
        public static Point operator *(Point p1, double i) => new Point(p1.X * i, p1.Y * i);
    }
}