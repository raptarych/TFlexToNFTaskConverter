using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    public abstract class SheetDefinition
    {
        public int Count { get; set; }
        public bool Exclude { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
