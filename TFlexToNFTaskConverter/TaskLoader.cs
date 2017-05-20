using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TFlexToNFTaskConverter.Models;
using TFlexToNFTaskConverter.Models.TFlexNestingTask;

namespace TFlexToNFTaskConverter
{
    class TaskLoader
    {
        public TFlexTask LoadTFlexTask(string fileName)
        {
            Console.WriteLine($"\nLoading {fileName}...");
            var deserializer = new XmlSerializer(typeof(TFlexTask));
            TextReader textReader = new StreamReader(fileName);
            var entity = (TFlexTask)deserializer.Deserialize(textReader);
            Console.WriteLine($"{fileName} loaded into buffer!");
            Console.WriteLine($"Name: {entity.Name}\nProject type: {entity.ProjectType}");
            Console.WriteLine($"Parts count: {entity.Parts.Count}");
            Console.WriteLine($"Sheets: {entity.Sheets.Count}");
            Console.WriteLine($"Results: {entity.Results.Count}");
            return entity;
        }

        public string GetKey(string line) => new string(line?.TakeWhile(ch => ch != ':').ToArray());

        public string GetValue(string line) => new string(line?.SkipWhile(ch => ch != '\t').Skip(1).ToArray());

        public Point ParsePoint(string value)
        {
            var split = value.Split('\t');
            double.TryParse(split[0].Replace('.', ','), out double x);
            double.TryParse(split[1].Replace('.', ','), out double y);
            double.TryParse(split[2].Replace('.', ','), out double b);
            return new Point(x, y, b);
        }

        public TFlexTask LoadNfTask(string dirName)
        {
            var taskFileName = Directory.GetFiles(dirName).FirstOrDefault(file => Program.GetExtension(file) == "task");
            if (string.IsNullOrEmpty(taskFileName)) throw new Exception("Can't find task file :(");

            var task = new TFlexTask();
            using (var taskFile = new StreamReader(taskFileName))
            {
                var currentLine = 0;
                while (!taskFile.EndOfStream)
                {

                    var line = taskFile.ReadLine();
                    currentLine++;
                    var key = GetKey(line);
                    var value = GetValue(line);

                    if (key == "TASKNAME") task.Name = value;
                    else if (key == "WIDTH")
                    {
                        if (!int.TryParse(value, out int width)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: invalid parameter WIDTH");
                        if (!int.TryParse(GetValue(taskFile.ReadLine()), out int length)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: expected parameter LENGTH");
                        if (!int.TryParse(GetValue(taskFile.ReadLine()), out int sheetCount)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: expected parameter SHEETCOUNT");

                        var sheet = new RectangularSheet
                        {
                            ID = task.GetNewSheetId(),
                            Length = length,
                            Width = width,
                            Count = sheetCount
                        };
                        sheet.Name = sheet.ID.ToString();
                        task.Sheets.Add(sheet);
                        
                    } else if (key == "DOMAINFILE")
                    {
                        var sheet = new ContourSheet
                        {
                            ID = task.GetNewSheetId(),
                            SheetProfile = ReadNFProfile($"{dirName}\\{value}")
                        };
                        sheet.Name = sheet.SheetProfile.ItemName;
                        task.Sheets.Add(sheet);
                    }
                }
            }
            return task;
        }

        private PartProfile ReadNFProfile(string filePath)
        {
            var partProfile = new PartProfile();
            using (var sheetFile = new StreamReader(filePath))
            {
                var currentLine = 0;
                while (!sheetFile.EndOfStream)
                {
                    var line = sheetFile.ReadLine();
                    currentLine++;
                    var key = GetKey(line);
                    var value = GetValue(line);
                    if (key == "ITEMNAME") partProfile.ItemName = value;
                    if (key == "VERTQUANT")
                    {
                        if (!int.TryParse(value, out int linesLeftToParse)) throw new Exception($"{filePath}:{currentLine}: invalid parameter VERTQUANT");
                        //Circle
                        if (linesLeftToParse == 2)
                        {
                            var firstPoint = ParsePoint(GetValue(sheetFile.ReadLine()));
                            if ((Math.Abs(firstPoint.B - 1) < 0.001 || Math.Abs(firstPoint.B + 1) < 0.001) && linesLeftToParse == 1)
                            {
                                var secondPoint = ParsePoint(GetValue(sheetFile.ReadLine()));
                                var circleContour = new CircleContour();
                                circleContour.Orientation = TFlexOrientationType.Positive;
                                circleContour.Center = (firstPoint + secondPoint) / 2;
                                circleContour.Radius = (Math.Abs(firstPoint.X - secondPoint.X) + Math.Abs(firstPoint.Y - secondPoint.Y)) / 2;
                                partProfile.Contours.Add(circleContour);
                            }
                            continue;
                        }
                        var contour = new FigureContour { Orientation = TFlexOrientationType.Positive };
                        var lastPoint = new Point();
                        while (linesLeftToParse > 0)
                        {
                            var point = ParsePoint(GetValue(sheetFile.ReadLine()));
                            linesLeftToParse--;

                            //Arc
                            if (point.B > 0 || point.B < 0)
                            {
                                var arc = new ContourArc();
                                var startPoint = point;
                                var endPoint = ParsePoint(GetValue(sheetFile.ReadLine()));
                                point = endPoint;
                                linesLeftToParse--;

                                var ang = Math.Atan(startPoint.B) * 720 / Math.PI;
                                arc.Angle = ang;
                                arc.Ccw = ang > 0;

                                //немного геометрии - вычисление центра дуги O, и затем радиуса
                                //A - начало дуги, B - конец дуги, M - средняя точка между A и B
                                var AB = endPoint - startPoint;
                                var AM = AB / 2;
                                var MO = AM.Normalize().Rotate(90 * (ang > 0 ? 1 : -1)) * AM.Length * Math.Tan(Math.Abs(ang) / 2 - 90);
                                var O = startPoint + AM + MO;

                                arc.Center = O;
                                arc.Radius = (arc.Center - startPoint).Length;
                                
                                contour.Objects.Add(arc);
                            }
                            //Line
                            else if (!lastPoint.IsEmpty)    
                            {
                                var lineObj = new ContourLine { Begin = lastPoint, End = point };
                                contour.Objects.Add(lineObj);
                            }
                            lastPoint = point;
                        }
                        partProfile.Contours.Add(contour);
                    }
                }
            }
            return partProfile;
        }
    }
}
