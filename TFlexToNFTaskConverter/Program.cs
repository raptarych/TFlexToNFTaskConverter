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

        public static string GetExtension(string S)
        {
            var extension = string.Join("", S.Reverse().TakeWhile(ch => ch != '.').Reverse()).ToLowerInvariant();
            if (S == extension || string.IsNullOrEmpty(extension)) return "";
            return extension;
        }
        static void CommandHandler(string S)
        {
            try
            {
                var commandName = string.Join("", S.TakeWhile(ch => ch != ' '));
                var fileName = string.Join("", S.SkipWhile(ch => ch != ' ').Skip(1));
                if (commandName == "load")
                {
                    var loader = new TaskLoader();


                    var extension = GetExtension(fileName);
                    if (extension == "tfnesting")
                    {
                        Buffer = loader.LoadTFlexTask(fileName);
                        return;
                    }
                    if (extension == "" && Directory.Exists(fileName))
                    {
                        Buffer = loader.LoadNfTask(fileName);
                        return;
                    }

                    if (!File.Exists(fileName))
                    {
                        Console.WriteLine("File doesn't exist!");
                        return;
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
                    } else if (commandName.ToLowerInvariant() == "-tflex")
                    {
                        fileName = string.Join("", fileName.SkipWhile(ch => ch != ' ').Skip(1));
                        if (string.IsNullOrEmpty(fileName)) return;
                        var converter = new NestingConverter();
                        converter.SaveToTFlex(Buffer, fileName, Directory.GetCurrentDirectory());
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
