using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Фигурный контур TFlex
    /// </summary>
    public class ContourSheet : SheetDefinition
    {
        public PartProfile SheetProfile { get; set; }
    }
}
