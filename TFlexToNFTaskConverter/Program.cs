using System;
using System.IO;
using System.Linq;
using TFlexToNFTaskConverter.Models;

namespace TFlexToNFTaskConverter
{
    class Program
    {
        private static TFlexTask Buffer;

        public static string GetExtension(string s)
        {
            s = s.ToLowerInvariant();
            var extension = string.Join("", s.Reverse().TakeWhile(ch => ch != '.').Reverse()).ToLowerInvariant();
            if (s == extension || string.IsNullOrEmpty(extension)) return "";
            return extension.Equals(s) ? string.Empty : extension;
        }
        static void CommandHandler(string s)
        {
            try
            {
                var commandName = string.Join("", s.TakeWhile(ch => ch != ' '));
                var fileName = string.Join("", s.SkipWhile(ch => ch != ' ').Skip(1));
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
                    } else if (commandName.ToLowerInvariant() == "-json")
                    {
                        fileName = string.Join("", fileName.SkipWhile(ch => ch != ' ').Skip(1));
                        if (string.IsNullOrEmpty(fileName)) return;
                        var converter = new NestingConverter();
                        converter.SaveToJson(Buffer, fileName);
                    }
                }
                if (commandName == "toJson")
                {
                    
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
                
                //чуть костыльнул - может быть потом нормально перепишу
                CommandHandler($"load {args[0]}");
                if (args[1].EndsWith(".tfnesting"))
                    CommandHandler($"save -tflex {args[1]}");
                else if (args[1].EndsWith(".json"))
                    CommandHandler($"save -json {args[1]}");
                else
                    CommandHandler($"save -nf {args[1]}");
                return;


            }
            Console.WriteLine("Less then 2 arguments!\n");
            Console.WriteLine("Using:");
            Console.WriteLine("TFlexToNFTaskConverter.exe task.tfnesting out_nf_folder");
        }
    }
}
