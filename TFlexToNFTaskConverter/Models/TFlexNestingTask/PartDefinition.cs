using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Сущность детали TFlex/Раскрой
    /// </summary>
    public class PartDefinition
    {
        public int ID { get; set; }
        public int Count { get; set; } = 1;
        public string FilePath { get; set; }
        [XmlElement]
        public PartProfile OriginalPartProfile { get; set; }

        public double AngleStep { get; set; } = 0;
        public bool DisableTurn { get; set; } = true;
        public bool OverturnAllowed { get; set; } = false;
        public string Name { get; set; }
        public string Notation { get; set; }
        public int CountToStock { get; set; } = 0;
        public bool Exclude { get; set; } = false;
        public bool IsLinearPart { get; set; } = false;
        public bool InheritAngle { get; set; } = true;
    }
}
