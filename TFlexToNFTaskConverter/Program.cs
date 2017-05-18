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
                    Console.WriteLine($"Name: {entity.Name}\nProject type: {entity.ProjectType}");
                    Console.WriteLine($"Parts count: {entity.Parts.Count}");
                    Console.WriteLine($"Sheets: {entity.Sheets.Count}");
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
