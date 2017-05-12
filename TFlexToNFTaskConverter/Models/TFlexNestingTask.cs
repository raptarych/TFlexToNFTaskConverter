using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models
{
    /// <summary>
    /// Модель для сериализации XML-ки формата .tfnesting 
    /// </summary>
    [XmlRootAttribute("Project")]
    public class TFlexNestingTask
    {
        [XmlArrayAttribute("Parts")]
        public List<PartDefinition> Parts { get; set; }
    }
}
