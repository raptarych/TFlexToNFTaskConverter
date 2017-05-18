namespace TFlexToNFTaskConverter.Models.TFlexNestingTask
{
    /// <summary>
    /// Сущность позиции детали на раскрое TFlex
    /// </summary>
    public class ResultPartPosition
    {
        public bool Rotated { get; set; }
        public double AngleDeg { get; set; }
        public bool Inverted { get; set; }
        public Point Position { get; set; }
        public int PartID { get; set; }
        public int SheetID { get; set; }
        public int PartIndex { get; set; }
    }
}