using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ElectricityPriceApi.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum Format
{
    json,
    xml
}