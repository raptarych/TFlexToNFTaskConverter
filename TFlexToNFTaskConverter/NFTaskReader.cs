using System;
using System.IO;
using System.Linq;
using TFlexToNFTaskConverter.Models;
using TFlexToNFTaskConverter.Models.TFlexNestingTask;

namespace TFlexToNFTaskConverter
{
    // ReSharper disable once InconsistentNaming
    public class NFTaskReader : NFFileReader
    {
        private string _dirName;
        private readonly TFlexTask _task = new TFlexTask();
        private int _currentLine;
        private string _key;
        private string _value;

        private string FormatPathForItems(string rawPath)
        {
            if (Uri.IsWellFormedUriString(rawPath, UriKind.Absolute)) return rawPath;
            return $"{_dirName}\\{rawPath.Split('\\').Last()}";
        }

        private void ParseRectangleSheet()
        {
            if (!double.TryParse(_value, out double width)) throw new Exception($"{_dirName}\\{FileName}:{_currentLine}: invalid parameter WIDTH");
            _currentLine++;
            if (!double.TryParse(GetValue(StreamReader.ReadLine()), out double length)) throw new Exception($"{_dirName}\\{FileName}:{_currentLine}: expected parameter LENGTH");
            _currentLine++;
            if (!int.TryParse(GetValue(StreamReader.ReadLine()), out int sheetCount)) throw new Exception($"{_dirName}\\{FileName}:{_currentLine}: expected parameter SHEETCOUNT");
            _currentLine++;

            var sheet = new RectangularSheet
            {
                ID = _task.GetNewSheetId(),
                Length = length,
                Width = width,
                Count = sheetCount
            };
            sheet.Name = sheet.ID.ToString();
            _task.Sheets.Add(sheet);
        }

        private void ParseSheet()
        {
            var itemReader = new NFItemReader(FormatPathForItems(_value), true);
            var sheet = new ContourSheet
            {
                ID = _task.GetNewSheetId(),
                SheetProfile = itemReader.Read()
            };
            sheet.Name = sheet.SheetProfile.ItemName;
            if (!int.TryParse(GetValue(StreamReader.ReadLine()), out int sheetCount)) throw new Exception($"{_dirName}\\{FileName}:{_currentLine}: expected parameter SHEETCOUNT");
            _currentLine++;
            sheet.Count = sheetCount;
            _task.Sheets.Add(sheet);
        }

        private void ParseItemDistance()
        {
            if (!double.TryParse(_value, out double d2DResult)) throw new Exception($"{_dirName}\\{FileName}:{_currentLine}: invalid parameter ITEM2ITEMDIST");
            _task.FigureParams.PartDistance = d2DResult;
        }

        private int GetItemId()
        {
            int.TryParse(
                new string(_value.Reverse()
                    .SkipWhile(ch => ch != '.')
                    .Skip(1)
                    .TakeWhile(ch => ch != '\\').Reverse().ToArray()), out int id);
            if (id <= 0) id = _task.GetNewPartId();
            return id;
        }

        private void ParseItem()
        {
            var itemFileName = Uri.IsWellFormedUriString(_value, UriKind.Absolute) ? _value : $"{_dirName}\\{_value}";
            if (!int.TryParse(GetValue(StreamReader.ReadLine()), out int itemQuant)) throw new Exception($"{_dirName}\\{FileName}:{_currentLine}: invalid parameter ITEMQUANT");
            if (!int.TryParse(GetValue(StreamReader.ReadLine()), out int rotate)) throw new Exception($"{_dirName}\\{FileName}:{_currentLine}: invalid parameter ROTATE");
            var rotStep = GetValue(StreamReader.ReadLine());
            if (!int.TryParse(GetValue(StreamReader.ReadLine()), out int reflect)) throw new Exception($"{_dirName}\\{FileName}:{_currentLine}: invalid parameter REFLECT");

            int id = GetItemId();

            var itemReader = new NFItemReader(FormatPathForItems(itemFileName));
            var partProfile = itemReader.Read();

            var part = new PartDefinition
            {
                ID = id,
                OriginalPartProfile = partProfile,
                Count = itemQuant,
                DisableTurn = rotate == 0,
                OverturnAllowed = reflect == 1
            };
            part.Name = part.OriginalPartProfile.ItemName;
            switch (rotStep)
            {
                case "PI":
                    part.AngleStep = 180;
                    break;
                case "PI/2":
                    part.AngleStep = 90;
                    break;
                case "NO":
                    part.AngleStep = 360;
                    break;
            }
            _task.Parts.Add(part);
        }

        /// <summary>
        /// Чтение папки с NF-раскроем и конвертация его в формат TFlex
        /// </summary>
        /// <returns></returns>
        public TFlexTask Read(string dirName)
        {
            _dirName = dirName;
            FileName = Directory.GetFiles(_dirName).FirstOrDefault(file => Program.GetExtension(file) == "task");
            if (string.IsNullOrEmpty(FileName)) return null;
            using (StreamReader = new StreamReader(FileName))
            {
                _currentLine = 0;
                while (!StreamReader.EndOfStream)
                {

                    var line = StreamReader.ReadLine();
                    _currentLine++;
                    _key = GetKey(line);
                    _value = GetValue(line);

                    if (_key == "TASKNAME") _task.Name = _value;
                    else if (_key == "WIDTH")
                    {
                        ParseRectangleSheet();

                    }
                    else if (_key == "DOMAINFILE")
                    {
                        ParseSheet();
                    }
                    else if (_key == "ITEM2ITEMDIST")
                    {
                        ParseItemDistance();
                    }
                    else if (_key == "ITEMFILE")
                    {
                        ParseItem();
                    }
                }
            }
            return _task;
        }
    }
}
