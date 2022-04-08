using System.Text.Json.Serialization;

namespace ConsoleSample.JsonRdgProtocol.DataModel
{
    public class TableItem
    {
        public string Name { get; set; }
        public string TranslationCurrent { get; set; }
        public string TranslationEn { get; set; }
        public object Alterability { get; set; }
    }
}