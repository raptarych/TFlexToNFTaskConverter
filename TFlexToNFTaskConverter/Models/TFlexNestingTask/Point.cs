namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{

    /// <summary>
    /// Сущность точки TFlex/Раскрой
    /// </summary>
    public class Point
    {
        public Point() {}

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public static Point operator -(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);
        public static Point operator +(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);
    }
}