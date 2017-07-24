using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    public class SettingsModel
    {
        public string UnitNotation { get; set; } = "mm";
        public string ImportLayerName { get; set; } = "Раскрой";
        public ExportSettingsModel ExportSettings { get; set; } = new ExportSettingsModel();
    }
}
