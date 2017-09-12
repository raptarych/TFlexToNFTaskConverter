using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Базовая сущность контура
    /// </summary>
    public class Contour
    {
        public string Orientation { get; set; }
        public virtual string Type => GetType().Name;
    }
}
