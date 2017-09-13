using System;
using System.Collections.Generic;
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
        public void Convert(string dir)
        {
            var taskFilename = Directory.GetFiles(dir).FirstOrDefault(file => Program.GetExtension(file) == "task");
            if (taskFilename == null) return;
            var items = new List<JsonDbsModel>();
            using (var taskFile = File.OpenText(taskFilename))
            {
                while (!taskFile.EndOfStream)
                {
                    var line = taskFile.ReadLine();
                    if (line == null) continue;
                    if (line.StartsWith("DOMAINFILE:"))
                    {
                        var domainPath = line.Split('\t')[1];
                        if(Path.IsPathRooted(domainPath))
                        {
                            using (var itemFile = File.OpenText(domainPath)) items.Add(ReadItem(itemFile));
                        }
                        else
                        {
                            using (var itemFile = File.OpenText($"{dir}\\{domainPath}")) items.Add(ReadItem(itemFile));
                        }
                    }
                    if (line.StartsWith("WIDTH"))
                    {
                        var width = double.Parse(line.Split('\t')[1].Replace(".", ","));
                        line = taskFile.ReadLine();
                        var height = double.Parse(line.Split('\t')[1].Replace(".", ","));
                        items.Add(new JsonDbsModel
                        {
                            PartId = "list",
                            Paths = new List<List<List<double>>>
                            {
                                new List<List<double>>
                                {
                                    new List<double> { 0, 0, 0 },
                                    new List<double> { width, 0, 0 },
                                    new List<double> { width, height, 0 },
                                    new List<double> {0, height, 0 },
                                    new List<double> { 0, 0, 0 }
                                }
                            }
                        });

                    }
                    if (!line.StartsWith("ITEMFILE:")) continue;
                    var path = line.Split('\t')[1];
                    if (Path.IsPathRooted(path))
                    {
                        using (var itemFile = File.OpenText(path)) items.Add(ReadItem(itemFile));
                    }
                    else
                    {
                        using (var itemFile = File.OpenText($"{dir}\\{path}")) items.Add(ReadItem(itemFile));
                    }
                }
            }
            using (var jsonFile = File.CreateText($"{dir}\\to_dbs.json"))
            {
                jsonFile.Write(JsonConvert.SerializeObject(items));
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
            return item;
        }
    }
}
