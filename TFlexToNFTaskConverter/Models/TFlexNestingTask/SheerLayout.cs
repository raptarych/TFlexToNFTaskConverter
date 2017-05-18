using System.Collections.Generic;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Сущность домена на готовом раскрое TFlex
    /// </summary>
    public class SheerLayout
    {
        public int SheerID { get; set; }
        public int SheerIndex { get; set; }
        public double KIM { get; set; }
        public double EffectiveKIM { get; set; }
        [XmlArray("PartPositions")]
        [XmlArrayItem("PartPosition")]
        public List<ResultPartPosition> PartPositions { get; set; }
    }
}