

namespace TFlexToNFTaskConverter.Models
{

    /// <summary>
    /// Сущность элемента контура (линии) TFlex/Раскрой
    /// </summary>
    public class ContourLine
    {
        public Point Begin { get; set; }
        public Point End { get; set; }
    }
}