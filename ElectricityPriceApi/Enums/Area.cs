using Newtonsoft.Json.Converters;

namespace ElectricityPriceApi.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum Area
{
    No1,
    No2,
    No3,
    No4,
    No5
}