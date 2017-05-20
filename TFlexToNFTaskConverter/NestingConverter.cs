using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TFlexToNFTaskConverter.Models;
using TFlexToNFTaskConverter.Models.TFlexNestingTask;

namespace TFlexToNFTaskConverter
{
    /// <summary>
    /// Класс для конвертирования задания и разультатов раскроя
    /// </summary>
    class NestingConverter
    {
        /// <summary>
        /// Вспомогательный метод - приведение координаты к съедаемому NF виду
        /// </summary>
        private string FormatNumber(double x) => x != 0 ? (-x).ToString("##.#####").Replace(",", ".") : "0";

        /// <summary>
        /// Запись точки в файл формата NF
        /// </summary>
        private void AddVertex(StreamWriter partFile, Point p, double b = 0) =>
            partFile.Write(string.Join("\t", "VERTEX:", FormatNumber(-p.X), FormatNumber(p.Y), FormatNumber(b)) + "\n");

        /// <summary>
        /// Запись детали в отдельный файл
        /// </summary>
        private void WriteItemToFile(StreamWriter partFile, PartProfile part)
        {
            foreach (var contour in part.Contours)
            {
                switch (contour)
                {
                    case RectangularContour rect:
                        partFile.Write(string.Join("\t", "VERTQUANT:", 4) + "\n");
                        AddVertex(partFile, new Point());
                        AddVertex(partFile, new Point { X = rect.Width });
                        AddVertex(partFile, new Point { X = rect.Width, Y = rect.Length });
                        AddVertex(partFile, new Point { Y = rect.Length });

                        break;
                    case CircleContour circle:
                        partFile.Write(string.Join("\t", "VERTQUANT:", 2) + "\n");

                        var leftPoint = new Point { X = circle.Radius - circle.Center.X, Y = circle.Center.Y };
                        var rightPoint = new Point { X = circle.Radius + circle.Center.X, Y = circle.Center.Y };

                        AddVertex(partFile, leftPoint, 1);
                        AddVertex(partFile, rightPoint, 1);
                        break;
                    case FigureContour figureContour:
                        partFile.Write(string.Join("\t", "VERTQUANT:", figureContour.Objects.Count + 1) + "\n");

                        //т.к. точка Begin текущего элемента контура равна End предыдущего элемента - нет смысла её писать в файл для всех элементов контуров кроме первого
                        var isFirst = true; 

                        foreach (var obj in figureContour.Objects)
                        {
                            switch (obj)
                            {
                                case ContourArc arc:
                                    var bulge = Math.Tan(arc.Angle / 4) * (arc.Ccw ? -1 : 1);
                                    if (isFirst)
                                    {
                                        AddVertex(partFile, arc.Begin, bulge);
                                        isFirst = false;
                                    }
                                    AddVertex(partFile, arc.End);
                                    break;
                                default:
                                    if (isFirst)
                                    {
                                        AddVertex(partFile, obj.Begin);
                                        isFirst = false;
                                    }
                                    AddVertex(partFile, obj.End);
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Экспорт задания на раскрой в папку в формате NF
        /// </summary>
        /// <param name="input">Задание в формате TFlex</param>
        /// <param name="destination">Название папки</param>
        /// <param name="folderPath">Путь к папке</param>
        /// 
        public void SaveToNestingFactory(TFlexTask input, string destination, string folderPath)
        {
            if (!Directory.Exists(folderPath)) throw new Exception("Directory doesn't exits");
            destination = $"{folderPath}\\{destination}";
            if (Directory.Exists(destination))
            {
                Console.WriteLine("Directory is already exists. Overwrite it? y/n");
                var yesOrNo = Console.ReadLine();
                if (yesOrNo.ToLowerInvariant().StartsWith("y"))
                {
                    foreach (var fileToDelete in Directory.GetFiles(destination))
                        File.Delete(fileToDelete);

                }
                else return;
            }
            Directory.CreateDirectory(destination);

            foreach (var part in input.Parts)
            {
                var filePath = $"{destination}\\{part.ID}.item";

                using (var partFile = File.CreateText(filePath))
                {
                    partFile.Write(string.Join("\t", "ITEMNAME:", part.Name) + "\n");
                    WriteItemToFile(partFile, part.PartProfile);
                    partFile.Close();
                }
            }

            foreach (var sheet in input.Sheets.Where(sheet => !(sheet is RectangularSheet)))
            {
                var filePath = $"{destination}\\{sheet.ID}_sheet.item";
                using (var sheetFile = File.CreateText(filePath))
                {
                    sheetFile.Write(string.Join("\t", "ITEMNAME:", sheet.Name ?? sheet.ID.ToString()) + "\n");
                    if (sheet is ContourSheet cont)
                        WriteItemToFile(sheetFile, cont.SheetProfile);
                    sheetFile.Close();
                }
            }

            var taskPath = $"{destination}\\{input.Name ?? "nest"}.task";
            if (!Uri.IsWellFormedUriString(taskPath, UriKind.RelativeOrAbsolute)) taskPath = $"{destination}\\task.task";
            using (var taskFile = File.CreateText(taskPath))
            {
                taskFile.Write(string.Join("\t", "TASKNAME:", input.Name ?? "nest") + "\n");
                taskFile.Write(string.Join("\t", "TIMELIMIT:", 3600000) + "\n");
                taskFile.Write(string.Join("\t", "TASKTYPE:", "Sheet") + "\n");
                foreach (var sheet in input.Sheets)
                {
                    switch (sheet)
                    {
                        case ContourSheet cont:
                            taskFile.Write(string.Join("\t", "DOMAINFILE:", $"{cont.ID}_sheet.item") + "\n");
                            break;
                        case RectangularSheet rect:
                            taskFile.Write(string.Join("\t", "WIDTH:", FormatNumber(rect.Width)) + "\n");
                            taskFile.Write(string.Join("\t", "LENGTH:", FormatNumber(rect.Length)) + "\n");
                            break;
                    }
                    taskFile.Write(string.Join("\t", "SHEETQUANT:", sheet.Count) + "\n");
                }
                taskFile.Write(string.Join("\t", "ITEM2DOMAINDIST:", 5) + "\n");    //TODO
                taskFile.Write(string.Join("\t", "ITEM2ITEMDIST:", 5) + "\n");      //TODO
                foreach (var item in input.Parts)
                {
                    taskFile.Write(string.Join("\t", "ITEMFILE:", $"{item.ID}.item") + "\n");
                    taskFile.Write(string.Join("\t", "ITEMQUANT:", item.Count) + "\n");
                    taskFile.Write(string.Join("\t", "ROTATE:", item.DisableTurn ? 0 : 1) + "\n");
                    string rotationStep;
                    if (item.AngleStep % 360 == 0) rotationStep = "NO";
                    else if (item.AngleStep % 180 == 0) rotationStep = "PI";
                    else if (item.AngleStep % 90 == 0) rotationStep = "PI/2";
                    else rotationStep = "FREE";


                    taskFile.Write(string.Join("\t", "ROTSTEP:", rotationStep) + "\n");
                    taskFile.Write(string.Join("\t", "REFLECT:", item.OverturnAllowed ? 1 : 0) + "\n");
                }
                taskFile.Close();
            }
        }

        public void SaveToTFlex(TFlexTask input, string fileName, string folderPath)
        {
            fileName = $"{folderPath}\\{fileName}";
            if (!fileName.EndsWith(".tfnesting")) fileName = $"{fileName}.tfnesting";

            var serializer = new XmlSerializer(typeof(TFlexTask));
            using (var xmlFile = File.CreateText(fileName))
                serializer.Serialize(xmlFile, input);
            
            
        }
    }
}
