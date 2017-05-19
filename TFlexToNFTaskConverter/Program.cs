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

        static string GetExtension(string S) => string.Join("", S.Reverse().TakeWhile(ch => ch != '.').Reverse()).ToLowerInvariant();
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
                var extension = GetExtension(fileName);
                if (extension == "tfnesting")
                {
                    Console.WriteLine($"\nLoading {fileName}...");
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
            if (args.Length == 0)
            {
                var conHandler = new ConWorker();
                conHandler.AddHandler(CommandHandler);
                conHandler.Start();
                return;
            }
            
            if (args.Length == 2)
            {
                if (GetExtension(args[0]) == "tfnesting" && !string.IsNullOrEmpty(args[1]))
                {
                    //чуть костыльнул - может быть потом нормально перепишу
                    CommandHandler($"load {args[0]}");
                    CommandHandler($"save -nf {args[1]}");
                    return;
                } else Console.WriteLine("Bad arguments\n");

            } else Console.WriteLine("Less then 2 arguments!\n");
            Console.WriteLine("Using:");
            Console.WriteLine("TFlexToNFTaskConverter.exe task.tfnesting out_nf_folder");
        }
    }
}
