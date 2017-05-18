using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Сущность результатов раскроя TFlex
    /// </summary>
    public class NestingResult
    {
        public int TotalParts { get; set; }
        public int TotalPlacedParts { get; set; }
        public int TotalSheets { get; set; }
        public double TotalEffectiveKIM { get; set; }
        public double KIM { get; set; }
        [XmlArray("Layouts")]
        [XmlArrayItem("SheetLayout")]
        public List<SheerLayout> Layouts { get; set; } = new List<SheerLayout>();
    }
}
