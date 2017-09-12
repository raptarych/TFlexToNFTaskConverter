using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Базовая сущность элементарного объекта контура (линия)
    /// </summary>
    public abstract class ContourObject
    {
        public Point Begin { get; set; }
        public Point End { get; set; }
        public virtual string Type => GetType().Name;
    }
}
