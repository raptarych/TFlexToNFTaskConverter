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
        public TFlexTask LoadNfTask(string dirName)
        {
            var reader = new NFTaskReader();
            var task = reader.Read(dirName);
            return task;
        }
    }
}
