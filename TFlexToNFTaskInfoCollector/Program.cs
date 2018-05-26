using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GemBox.Spreadsheet;
using TFlexToNFTaskConverter;

namespace TFlexToNFTaskInfoCollector
{
    class Program
    {
        static void CommandHandler(string S)
        {
            try
            {
                var commandName = string.Join("", S.TakeWhile(ch => ch != ' '));
                if (commandName == "calc")
                {
                    SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                    var excelFile = new ExcelFile();
                    var workSheet = excelFile.Worksheets.Add("NestingResults");
                    var rowNum = 0;
                    var jsonConverter = new NestingConverter();
                    jsonConverter.SaveFromNfToDbs();
                    var dirs = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);
                    foreach (var dir in dirs)
                    {
                        var fileName = dir + "\\nest_000.nres";
                        if (File.Exists(fileName))
                        {
                            rowNum++;
                            var domainSize = 0d;
                            var maxSize = 0d;
                            var listY = 0d;
                            var nfRatio = 0d;
                            using (var file = new StreamReader(fileName))
                            {
                                while (!file.EndOfStream)
                                {
                                    var line = file.ReadLine();
                                    if (string.IsNullOrEmpty(line)) continue;
                                    var split = line.Split(' ').ToList();
                                    if (split.FirstOrDefault() == "Material")
                                    {
                                        double.TryParse(split.LastOrDefault()?.Replace(".", ","), out nfRatio);
                                        continue;
                                    }
                                    if (split.Count < 2) continue;
                                    double.TryParse(split.FirstOrDefault()?.Replace(".", ","), out double X);
                                    double.TryParse(split.FirstOrDefault()?.Replace(".", ","), out double Y);
                                    if (X > domainSize) domainSize = X;
                                    if (X > maxSize && Math.Abs(X - domainSize) > 0.1) maxSize = X;
                                    if (listY <= 0 && Y > 0) listY = Y;
                                }
                            }

                            var dirName = dir.Split('\\').ToList().Last();
                            var loader = new TaskLoader();
                            var task = loader.LoadTFlexTask(dirName + ".tfnesting");
                            var tFlexRatio = task.Results.FirstOrDefault()?.KIM ?? 0;
                            CalculateDbsNesting(dirName);
                            var siriusRatio = "";
                            var resultReportPath = "report\\report_result.htm";
                            if (File.Exists(resultReportPath))
                            {
                                var resultsHtml = File.ReadAllText(resultReportPath);
                                siriusRatio = resultsHtml?.Split(' ').LastOrDefault()?.Replace("</b></body></html>\r\n", "");
                            }
                            
                            Console.WriteLine(
                                $"Size: {domainSize}x{listY} ({domainSize * listY}), fact size: {maxSize}x{listY} ({maxSize * listY}), NF ratio: {nfRatio}, TFlex ratio: {tFlexRatio}");
                            workSheet.Cells[rowNum, 0].Value = dirName;
                            workSheet.Cells[rowNum, 1].Value = $"{domainSize}x{listY}";
                            workSheet.Cells[rowNum, 2].Value = task.Parts.Sum(i => i.Count);
                            workSheet.Cells[rowNum, 4].Value = nfRatio;
                            workSheet.Cells[rowNum, 5].Value = domainSize - maxSize;
                            workSheet.Cells[rowNum, 6].Value = tFlexRatio;
                            workSheet.Cells[rowNum, 8].Value = siriusRatio;
                        }
                    }
                    excelFile.Save("results.xlsx");
                }
            }
            catch (Exception ex)
            {
                Console.Write($"{ex.Message}\n{ex.StackTrace}\n");
            }

        }

        public static void ConvertToDbs(string dirName, string fileName)
        {
            var pr = new Process
            {
                StartInfo =
                {
                    FileName = "json2dbs.bat",
                    Arguments = $@"--output={dirName}.json --force {dirName}\{fileName}.json"
                }
            };
            pr.Start();
        }

        public static void CalculateDbsNesting(string dirName)
        {
            var pr = new Process
            {
                StartInfo =
                {
                    FileName = "ncl.exe",
                    Arguments = $@"-c {dirName}\nest.kol"
                }
            };
            pr.Start();
            pr.WaitForExit();
        }

        static void Main(string[] args)
        {
            var conHandler = new ConWorker();
            conHandler.AddHandler(CommandHandler);
            conHandler.Start();
        }

        static void Shit()
        {
            ("testset").Replace("asd", "asd");
        }
    }
}
