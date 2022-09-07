namespace ElectricityPriceApi.Configuration;

public class CosmosConfiguration
{
    public string? DatabaseName { get; set; }
    public string? ContainerName { get; set; }
    public string? Account { get; set; }
    public string? Key { get; set; }
}