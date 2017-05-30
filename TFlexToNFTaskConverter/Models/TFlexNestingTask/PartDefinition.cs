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
        public int Count { get; set; }
        public string FilePath { get; set; }
        [XmlElement]
        public PartProfile OriginalPartProfile { get; set; }
        public double AngleStep { get; set; }
        public bool DisableTurn { get; set; }
        public bool OverturnAllowed { get; set; }
        public string Name { get; set; }
    }
}
