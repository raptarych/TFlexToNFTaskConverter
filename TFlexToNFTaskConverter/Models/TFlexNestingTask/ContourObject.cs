﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    public abstract class ContourObject
    {
        public Point Begin { get; set; }
        public Point End { get; set; }
    }
}
