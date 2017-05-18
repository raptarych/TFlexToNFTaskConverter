using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TFlexToNFTaskConverter.Models;

namespace TFlexToNFTaskConverter
{
    class Program
    {
        private static TFlexTask Buffer;
        static void CommandHandler(string S)
        {
            var commandName = string.Join("", S.TakeWhile(ch => ch != ' '));
            var fileName = string.Join("",S.SkipWhile(ch => ch != ' ').Skip(1));
            if (commandName == "load")
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("File doesn't exist!");
                    return;
                }
                var extension = string.Join("", fileName.Reverse().TakeWhile(ch => ch != '.').Reverse()).ToLowerInvariant();
                if (extension == "tfnesting")
                {
                    Console.WriteLine($"Loading {fileName}...");
                    var deserializer = new XmlSerializer(typeof(TFlexTask));
                    TextReader textReader = new StreamReader(fileName);
                    var entity = (TFlexTask) deserializer.Deserialize(textReader);
                    Console.WriteLine($"{fileName} loaded into buffer!");
                    Console.WriteLine($"Name: {entity.Name}\nProject type: {entity.ProjectType}");
                    Console.WriteLine($"Parts count: {entity.Parts.Count}");
                    Console.WriteLine($"Sheets: {entity.Sheets.Count}");
                    Console.WriteLine($"Results: {entity.Results.Count}");

                    Buffer = entity;
                }
            }
            if (commandName == "save")
            {
                commandName = string.Join("", fileName.TakeWhile(ch => ch != ' '));
                if (string.IsNullOrEmpty(commandName)) return;

                if (commandName.ToLowerInvariant() == "-nf")
                {
                    fileName = string.Join("", fileName.SkipWhile(ch => ch != ' ').Skip(1));
                    if (string.IsNullOrEmpty(fileName)) return;
                    var converter = new NestingConverter();
                    converter.SaveToNestingFactory(Buffer, fileName, Directory.GetCurrentDirectory());
                }
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
