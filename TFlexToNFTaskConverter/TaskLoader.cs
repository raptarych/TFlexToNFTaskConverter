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
            return new Point { X = x, Y =  -y, B = b };
        }

        private string FormatPathForItems(string rawPath, string dirName)
        {
            if (Uri.IsWellFormedUriString(rawPath, UriKind.Absolute)) return rawPath;
            return $"{dirName}\\{rawPath.Split('\\').Last()}";
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
                        if (!double.TryParse(value, out double width)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: invalid parameter WIDTH");
                        currentLine++;
                        if (!double.TryParse(GetValue(taskFile.ReadLine()), out double length)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: expected parameter LENGTH");
                        currentLine++;
                        if (!int.TryParse(GetValue(taskFile.ReadLine()), out int sheetCount)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: expected parameter SHEETCOUNT");
                        currentLine++;

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
                            SheetProfile = ReadNFProfile(FormatPathForItems(value, dirName))
                        };
                        sheet.Name = sheet.SheetProfile.ItemName;
                        if (!int.TryParse(GetValue(taskFile.ReadLine()), out int sheetCount)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: expected parameter SHEETCOUNT");
                        currentLine++;
                        sheet.Count = sheetCount;
                        task.Sheets.Add(sheet);
                    }
                    else if (key == "ITEM2ITEMDIST")
                    {
                        if (!double.TryParse(value, out double d2dResult)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: invalid parameter ITEM2ITEMDIST");
                        task.FigureParams.PartDistance = d2dResult;
                    }
                    else if (key == "ITEMFILE")
                    {
                        var itemFileName = Uri.IsWellFormedUriString(value, UriKind.Absolute) ? value : $"{dirName}\\{value}";
                        if (!int.TryParse(GetValue(taskFile.ReadLine()), out int itemQuant)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: invalid parameter ITEMQUANT");
                        if (!int.TryParse(GetValue(taskFile.ReadLine()), out int rotate)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: invalid parameter ROTATE");
                        var rotStep = GetValue(taskFile.ReadLine());
                        if (!int.TryParse(GetValue(taskFile.ReadLine()), out int reflect)) throw new Exception($"{dirName}\\{taskFileName}:{currentLine}: invalid parameter REFLECT");

                        int.TryParse(
                            new string(value.Reverse()
                                .SkipWhile(ch => ch != '.')
                                .Skip(1)
                                .TakeWhile(ch => ch != '\\').Reverse().ToArray()), out int id);
                        if (id <= 0) id = task.GetNewPartId();
                        var part = new PartDefinition
                        {
                            ID = id,
                            OriginalPartProfile = ReadNFProfile(FormatPathForItems(itemFileName, dirName)),
                            Count = itemQuant,
                            DisableTurn = rotate == 0,
                            OverturnAllowed = reflect == 1
                        };
                        part.Name = part.OriginalPartProfile.ItemName;
                        switch (rotStep)
                        {
                            case "PI":
                                part.AngleStep = 180;
                                break;
                            case "PI/2":
                                part.AngleStep = 90;
                                break;
                            case "NO":
                                part.AngleStep = 360;
                                break;
                        }
                        task.Parts.Add(part);
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
                            if (Math.Abs(firstPoint.B - 1) < 0.001 || Math.Abs(firstPoint.B + 1) < 0.001)
                            {
                                var secondPoint = ParsePoint(GetValue(sheetFile.ReadLine()));
                                var circleContour = new CircleContour
                                {
                                    Orientation = !partProfile.Contours.Any() ? "Positive" : "Negative",
                                    Center = (firstPoint + secondPoint) / 2,
                                    Radius = (Math.Abs(firstPoint.X - secondPoint.X) + Math.Abs(firstPoint.Y - secondPoint.Y)) / 2
                                };
                                partProfile.Contours.Add(circleContour);
                                continue;
                            }
                        }
                        var contour = new FigureContour { Orientation = !partProfile.Contours.Any() ? "Positive" : "Negative" };
                        Point lastPoint = null;
                        while (linesLeftToParse > 0)
                        {
                            var point = ParsePoint(GetValue(sheetFile.ReadLine()));
                            linesLeftToParse--;

                            //Arc
                            if (point.B > 0 || point.B < 0)
                            {
                                var startPoint = point;
                                var endPoint = ParsePoint(GetValue(sheetFile.ReadLine()));
                                point = endPoint;
                                linesLeftToParse--;

                                //немного геометрии - вычисление центра дуги O, и затем радиуса
                                //A - начало дуги, B - конец дуги, M - средняя точка между A и B
                                var ang = Math.Atan(startPoint.B) * -4;
                                var AB = endPoint - startPoint;
                                var AM = AB / 2;
                                var Normed = AM.Normalize();
                                var MO = Normed.Rotate(Math.PI / 2 * (ang > 0 ? -1 : 1)) * AM.Length * Math.Tan((Math.Abs(ang) - Math.PI) / 2);
                                var O = startPoint + AM + MO;

                                var arc = new ContourArc
                                {
                                    Begin = startPoint,
                                    End = endPoint,
                                    Angle = ang,
                                    Ccw = ang > 0,
                                    Center = O,
                                    Radius = (O - startPoint).Length
                                };

                                contour.Objects.Add(arc);
                            }
                            //Line
                            else
                            {
                                if (lastPoint != null)    
                                {
                                    var lineObj = new ContourLine { Begin = lastPoint, End = point };
                                    contour.Objects.Add(lineObj);
                                }
                            }
                            lastPoint = point;
                        }
                        contour.CloseContour();
                        partProfile.Contours.Add(contour);
                    }
                }
            }
            return partProfile;
        }
    }
}
