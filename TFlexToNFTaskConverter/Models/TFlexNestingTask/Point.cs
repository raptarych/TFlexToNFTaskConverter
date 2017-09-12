using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{

    /// <summary>
    /// Сущность точки TFlex/Раскрой
    /// </summary>
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        [XmlIgnore]
        public double B { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public bool IsEmpty => Math.Abs(X) < 0.0001 && Math.Abs(Y) < 0.0001 && Math.Abs(B) < 0.0001;
        [XmlIgnore]
        [JsonIgnore]
        public double Length => Math.Sqrt(X * X + Y * Y);
        public Point Normalize() => this / Length;
        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                var p2 = obj as Point;
                return Math.Abs(X - p2.X) < 0.00001 && Math.Abs(Y - p2.Y) < 0.00001 && Math.Abs(B - p2.B) < 0.0000001;
            }
            return base.Equals(obj);
        }

        protected bool Equals(Point other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && B.Equals(other.B);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ B.GetHashCode();
                return hashCode;
            }
        }

        public Point Rotate(double angle) => new Point {X = X * Math.Cos(angle) - Y * Math.Sin(angle), Y = Y * Math.Cos(angle) + X * Math.Sin(angle)};
        public static Point operator -(Point p1, Point p2) => new Point { X = p1.X - p2.X, Y = p1.Y - p2.Y};
        public static Point operator +(Point p1, Point p2) => new Point { X = p1.X + p2.X, Y = p1.Y + p2.Y };
        public static Point operator /(Point p1, double i) => new Point { X = p1.X / i, Y = p1.Y / i };
        public static Point operator *(Point p1, double i) => new Point { X = p1.X * i, Y = p1.Y * i };

        public override string ToString() => $"[{X},{Y}]";
    }
}