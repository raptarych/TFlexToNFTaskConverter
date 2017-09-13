using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                    var jsonConverter = new NestingConverter();
                    jsonConverter.SaveFromNfToJson();
                    var dirs = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory);
                    foreach (var dir in dirs)
                    {
                        var fileName = dir + "\\nest_000.nres";
                        if (File.Exists(fileName))
                        {
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
                            var itemArea = task.Parts.Sum(i => CalcHelper.CalcArea(i.PartProfile) * i.Count);
                            var factRatio = itemArea / (maxSize * listY);
                            /*var rightSide = task.RightSide();*/
                            Console.WriteLine($"Size: {domainSize}x{listY} ({domainSize*listY}), fact size: {maxSize}x{listY} ({maxSize * listY}), NF ratio: {nfRatio}, areas {itemArea}, ratio fact: {factRatio}");

                            var pr = new Process
                            {
                                StartInfo =
                                {
                                    FileName = "json2dbs.bat",
                                    Arguments = $@"--output={dirName}.json --force {dirName}\to_dbs.json"
                                }
                            };
                            pr.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write($"{ex.Message}\n{ex.StackTrace}\n");
            }

        }

        static void Main(string[] args)
        {
            var conHandler = new ConWorker();
            conHandler.AddHandler(CommandHandler);
            conHandler.Start();
        }
    }
}
