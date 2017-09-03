using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFlexToNFTaskConverter.Models;

namespace TFlexToNFTaskConverter
{
    public abstract class NFFileReader
    {
        protected string FileName;
        protected StreamReader StreamReader;

        protected string GetKey(string line) => new string(line?.TakeWhile(ch => ch != ':').ToArray());
        protected string GetValue(string line) => new string(line?.SkipWhile(ch => ch != '\t').Skip(1).ToArray());
    }
}
