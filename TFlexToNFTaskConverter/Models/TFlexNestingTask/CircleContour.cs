﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Базовая сущность контура окружности
    /// </summary>
    public class CircleContour : Contour
    {
        public Point Center { get; set; }
        public double Radius { get; set; }
    }
}
