using System;
using System.IO;
using System.Xml.Serialization;
using TFlexToNFTaskConverter.Models;

namespace TFlexToNFTaskConverter
{
    class TaskLoader
    {
        private void LogToConsole(TFlexTask entity)
        {
            Console.WriteLine($"{entity.Name} loaded into buffer!");
            Console.WriteLine($"Name: {entity.Name}\nProject type: {entity.ProjectType}");
            Console.WriteLine($"Parts count: {entity.Parts.Count}");
            Console.WriteLine($"Sheets: {entity.Sheets.Count}");
            Console.WriteLine($"Results: {entity.Results.Count}");
        }
        public TFlexTask LoadTFlexTask(string fileName)
        {
            Console.WriteLine($"\nLoading {fileName}...");
            var deserializer = new XmlSerializer(typeof(TFlexTask));
            TextReader textReader = new StreamReader(fileName);
            var entity = (TFlexTask)deserializer.Deserialize(textReader);
            LogToConsole(entity);
            return entity;
        }
        public TFlexTask LoadNfTask(string dirName)
        {
            var reader = new NFTaskReader();
            var task = reader.Read(dirName);
            LogToConsole(task);
            return task;
        }
    }
}
