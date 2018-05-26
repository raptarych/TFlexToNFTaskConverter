using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TFlexToNFTaskConverter
{
    public class JsonDbsModel
    {
        [JsonProperty(PropertyName = "partid")]
        public string PartId { get; set; }

        [JsonProperty(PropertyName = "paths")]
        public List<List<List<double>>> Paths { get; set; } = new List<List<List<double>>>();
    }
    public class NFToJsonConverter
    {
        public static void ConvertToDbs(string dirName, string fileName)
        {
            var pr = new Process
            {
                StartInfo =
                {
                    FileName = "json2dbs.bat",
                    Arguments = $@"--output={dirName}\{fileName} --force {dirName}\{fileName}.json"
                }
            };
            pr.Start();
        }

        public void Convert(string dir)
        {
            var taskFilename = Directory.GetFiles(dir).FirstOrDefault(file => Program.GetExtension(file) == "task");
            if (taskFilename == null) return;
            var kolList = new List<string>();
            var items = new List<JsonDbsModel>();
            using (var taskFile = File.OpenText(taskFilename))
            {
                var kolStringToWrite = "";
                
                while (!taskFile.EndOfStream)
                {
                    var line = taskFile.ReadLine();
                    if (line == null) continue;
                    if (line.StartsWith("DOMAINFILE:"))
                    {
                        var domainPath = line.Split('\t')[1];
                        var item = new JsonDbsModel();
                        if(Path.IsPathRooted(domainPath))
                        {
                            using (var itemFile = File.OpenText(domainPath)) item = ReadItem(itemFile);
                        }
                        else
                        {
                            using (var itemFile = File.OpenText($"{dir}\\{domainPath}")) item = ReadItem(itemFile);
                        }
                        using (var jsonFile = File.CreateText($"{dir}\\list.json"))
                        {
                            jsonFile.Write(JsonConvert.SerializeObject(new List<JsonDbsModel> { item }));
                            var dirName = dir.Split('\\').LastOrDefault();
                            ConvertToDbs(dirName, "list");
                            kolList.Add($"{dir}\\list.dbs 1 0");
                            items.Add(item);
                        }
                    }
                    if (line.StartsWith("WIDTH"))
                    {
                        var width = double.Parse(line.Split('\t')[1].Replace(".", ","));
                        line = taskFile.ReadLine();
                        var height = double.Parse(line.Split('\t')[1].Replace(".", ","));
                        var item = new JsonDbsModel
                        {
                            PartId = "list",
                            Paths = new List<List<List<double>>>
                            {
                                new List<List<double>>
                                {
                                    new List<double> { 0, 0, 0 },
                                    new List<double> { width, 0, 0 },
                                    new List<double> { width, height, 0 },
                                    new List<double> { 0, height, 0 },
                                    new List<double> { 0, 0, 0 }
                                }
                            }
                        };
                        using (var jsonFile = File.CreateText($"{dir}\\list.json"))
                        {
                            jsonFile.Write(JsonConvert.SerializeObject(new List<JsonDbsModel> { item }));
                            var dirName = dir.Split('\\').LastOrDefault();
                            ConvertToDbs(dirName, "list");
                            kolList.Add($"{dir}\\list.dbs 1 0");
                            items.Add(item);
                        }
                    }
                    if (line.StartsWith("ITEMQUANT"))
                    {
                        kolList.Add(kolStringToWrite + $" {line.Split('\t')[1]} 1");
                    }
                    if (!line.StartsWith("ITEMFILE:")) continue;
                    var path = line.Split('\t')[1];
                    var itemModel = new JsonDbsModel();
                    var itemName = path.Split('\\').LastOrDefault()?.Replace(".item", "");
                    if (Path.IsPathRooted(path))
                    {
                        using (var itemFile = File.OpenText(path)) itemModel = ReadItem(itemFile);
                    }
                    else
                    {
                        using (var itemFile = File.OpenText($"{dir}\\{path}")) itemModel = ReadItem(itemFile);
                    }
                    using (var jsonFile = File.CreateText($"{dir}\\{itemName}.json"))
                    {
                        jsonFile.Write(JsonConvert.SerializeObject(new List<JsonDbsModel> { itemModel }));
                        var dirName = dir.Split('\\').LastOrDefault();
                        ConvertToDbs(dirName, itemName);
                        kolStringToWrite = $"{dir}\\{itemName}.dbs";
                        items.Add(itemModel);
                    }
                }
            }
            using (var kolFile = File.CreateText($"{dir}\\nest.kol"))
            {
                kolFile.Write(string.Join("\n", kolList));
            }
            using (var dbsFile = File.CreateText($"{dir}\\nest.json"))
            {
                dbsFile.Write(JsonConvert.SerializeObject(items));
                var dirName = dir.Split('\\').LastOrDefault();
                ConvertToDbs(dirName, "nest");
            }
        }

        public JsonDbsModel ReadItem(StreamReader stream)
        {
            var item = new JsonDbsModel();
            var currentContour = new List<List<double>>();
            while (!stream.EndOfStream)
            {
                var line = stream.ReadLine();
                if (line == null) return null;
                if (line.StartsWith("ITEMNAME:"))
                {
                    item.PartId = line.Split('\t')[1];
                }
                else if (line.StartsWith("VERTQUANT:"))
                {
                    if (currentContour.Any())
                    {
                        var lastPoint = currentContour.Last();
                        var firstPoint = currentContour.First();
                        if (lastPoint[0] != firstPoint[0] || lastPoint[1] != firstPoint[1]) currentContour.Add(firstPoint);
                        item.Paths.Add(currentContour);
                        currentContour = new List<List<double>>();
                    }
                    
                    var vertexCount = int.Parse(line.Split('\t')[1]);
                    for (var i = 0; i < vertexCount; i++)
                    {
                        var vertexLine = stream.ReadLine();
                        var vertexData = vertexLine?.Split('\t').Skip(1).Select(vtrx => double.Parse(vtrx.Replace(".", ","))).ToList();
                        currentContour.Add(vertexData);
                    }
                    
                }
            }
            if (currentContour.Any())
            {
                var lastPoint = currentContour.Last();
                var firstPoint = currentContour.First();
                if (lastPoint[0] != firstPoint[0] || lastPoint[1] != firstPoint[1]) currentContour.Add(firstPoint);
                item.Paths.Add(currentContour);
            }
            return item;
        }
    }
}
