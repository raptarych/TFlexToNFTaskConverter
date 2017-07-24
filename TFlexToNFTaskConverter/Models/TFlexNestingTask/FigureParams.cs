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
        public double PartDistance { get; set; } = 5;
        public bool PartInPartMode { get; set; } = true;
        public bool OverlapCuts { get; set; } = false;
        public int MaterialMargin { get; set; } = 0;
        public double RotationStep { get; set; } = 10;
        public double Resolution { get; set; } = 0.1d;
        public string StartFrom { get; set; } = "LeftBottom";
        public int OptimizationLevel { get; set; } = 20;
        public string Direction { get; set; } = "DirectionX";
        public int Flexure { get; set; } = 0;
        public double PresentationPrecisionRatio { get; set; } = 0.005d;
    }
}
