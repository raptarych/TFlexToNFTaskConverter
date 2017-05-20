using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Параметра раскроя TFlex
    /// </summary>
    public class FigureParams
    {
        public double PartDistance { get; set; }
        public bool PartInPartMode { get; set; }
        public double RotationStep { get; set; }
        public string Direction { get; set; }
        public double PresentationPrecisionRatio { get; set; }
    }
}
