using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Сущность детали TFlex/Раскрой
    /// </summary>
    public class PartDefinition
    {
        public int ID { get; set; }
        public int Count { get; set; }
        public string FilePath { get; set; }
        public PartProfile PartProfile { get; set; }
        public int AngleStep { get; set; }
        public bool DisableTurn { get; set; }
        public bool OverturnAllowed { get; set; }
        public string Name { get; set; }
    }
}
