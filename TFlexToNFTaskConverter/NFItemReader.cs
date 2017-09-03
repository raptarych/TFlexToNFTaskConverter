using System;
using System.IO;
using System.Linq;
using TFlexToNFTaskConverter.Models.TFlexNestingTask;

namespace TFlexToNFTaskConverter
{
    // ReSharper disable once InconsistentNaming
    public class NFItemReader : NFFileReader
    {
        private int _linesLeftToParse;
        private PartProfile _partProfile;
        private Point _lastPoint;
        private FigureContour _contour;
        private readonly bool _isSheet;
        private Point _point;

        public NFItemReader(string fileName, bool isSheet = false)
        {
            _isSheet = isSheet;
            FileName = fileName;
        }

        protected Point ParsePoint(string value)
        {
            var split = value.Split('\t');
            double.TryParse(split[0].Replace('.', ','), out double x);
            double.TryParse(split[1].Replace('.', ','), out double y);
            double.TryParse(split[2].Replace('.', ','), out double b);
            return new Point { X = _isSheet ? -y : x, Y = _isSheet ? x : -y, B = b };
        }

        public bool ParseCircle()
        {
            if (_linesLeftToParse == 2)
            {
                var firstPoint = ParsePoint(GetValue(StreamReader.ReadLine()));
                if (Math.Abs(firstPoint.B - 1) < 0.001 || Math.Abs(firstPoint.B + 1) < 0.001)
                {
                    var secondPoint = ParsePoint(GetValue(StreamReader.ReadLine()));
                    var circleContour = new CircleContour
                    {
                        Orientation = !_partProfile.Contours.Any() ? "Positive" : "Negative",
                        Center = (firstPoint + secondPoint) / 2,
                        Radius = (Math.Abs(firstPoint.X - secondPoint.X) + Math.Abs(firstPoint.Y - secondPoint.Y)) / 2
                    };
                    _partProfile.Contours.Add(circleContour);
                    return true;
                }
                return false;
            }
            return false;
        }

        private ContourArc CalculateContourArc(Point startPoint, Point endPoint)
        {
            //немного геометрии - вычисление центра дуги O, и затем радиуса
            //A - начало дуги, B - конец дуги, M - средняя точка между A и B
            var ang = Math.Atan(startPoint.B) * -4;
            var ab = endPoint - startPoint;
            var am = ab / 2;
            var normed = am.Normalize();
            var mo = normed.Rotate(Math.PI / 2 * (ang > 0 ? -1 : 1)) * am.Length * Math.Tan((Math.Abs(ang) - Math.PI) / 2);
            var o = startPoint + am + mo;

            return new ContourArc
            {
                Begin = startPoint,
                End = endPoint,
                Angle = ang,
                Ccw = ang > 0,
                Center = o,
                Radius = (o - startPoint).Length
            };
        }

        public void ParseArc()
        {
            var startPoint = _point;
            var endPoint = ParsePoint(GetValue(StreamReader.ReadLine()));
            _point = endPoint;
            _linesLeftToParse--;

            var arc = CalculateContourArc(startPoint, endPoint);

            _contour.Objects.Add(arc);
        }

        private void ParseLine()
        {
            if (_lastPoint != null)
            {
                var lineObj = new ContourLine { Begin = _lastPoint, End = _point };
                _contour.Objects.Add(lineObj);
            }
        }

        public void ParseArcOrLine()
        {
            _contour = new FigureContour { Orientation = !_partProfile.Contours.Any() ? "Positive" : "Negative" };
            _lastPoint = null;
            while (_linesLeftToParse > 0)
            {
                _point = ParsePoint(GetValue(StreamReader.ReadLine()));
                _linesLeftToParse--;

                if (_point.B > 0 || _point.B < 0)
                    ParseArc();
                else
                    ParseLine();
                _lastPoint = _point;
            }
            _contour.CloseContour();
            _partProfile.Contours.Add(_contour);
        }

        public PartProfile Read()
        {
            _partProfile = new PartProfile();
            using (StreamReader = new StreamReader(FileName))
            {
                var currentLine = 0;
                while (!StreamReader.EndOfStream)
                {
                    var line = StreamReader.ReadLine();
                    currentLine++;
                    var key = GetKey(line);
                    var value = GetValue(line);
                    if (key == "ITEMNAME") _partProfile.ItemName = value;
                    if (key == "VERTQUANT")
                    {
                        if (!int.TryParse(value, out _linesLeftToParse)) throw new Exception($"{FileName}:{currentLine}: invalid parameter VERTQUANT");
                        if (ParseCircle()) continue;
                        ParseArcOrLine();
                    }
                }
            }
            return _partProfile;
        }

    }
}
