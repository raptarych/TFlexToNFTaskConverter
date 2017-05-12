using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models
{

    /// <summary>
    /// Сущность контура детали TFlex/Раскрой
    /// </summary>
    public class FigureContour
    {
        [XmlElement("Orientation")]
        public string _orientation { get; set; }

        public TFlexOrientationType Orientation
        {
            get
            {
                switch (_orientation)
                {
                    case "Negative":
                        return TFlexOrientationType.Negative;
                    case "Positive":
                        return TFlexOrientationType.Positive;   
                    default:
                        return TFlexOrientationType.Unknown;
                }
            }
        }

        [XmlArray("Objects")]
        [XmlArrayItem("ContourObject")]
        public List<ContourLine> Objects { get; set; }
    }

    public enum TFlexOrientationType
    {
        Unknown = 0,
        Negative = 1,
        Positive = 2
    }
}
