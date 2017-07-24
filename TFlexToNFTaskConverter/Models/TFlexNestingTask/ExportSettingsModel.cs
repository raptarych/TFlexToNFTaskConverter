namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    public class ExportSettingsModel
    {
        public bool UsePartColors { get; set; } = true;
        public bool CreateHatches { get; set; } = true;
        public bool CreateGroups { get; set; } = true;
        public bool CreateDims { get; set; } = false;
        public bool CreateGuillotineCuts { get; set; } = false;
        public bool GenerateInActiveDocument { get; set; } = false;
        public bool GenerateOnCurrentPage { get; set; } = false;
        public bool GenerateOnOnePage { get; set; } = false;
        public string PrototypeName { get; set; }
    }
}