using System.Text.Json.Serialization;

namespace ConsoleSample.JsonRdgProtocol.DataModel
{
    public class TableItem
    {
        [JsonInclude]public string Name { get; set; }
        [JsonInclude]public string TranslationCurrent { get; set; }
        [JsonInclude]public string TranslationEn { get; set; }
        [JsonInclude]public int Alterability { get; set; }
    }
}