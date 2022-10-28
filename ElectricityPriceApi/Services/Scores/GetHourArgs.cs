namespace ElectricityPriceApi.Services.Scores;

public class GetHourArgs
{
    public GetHourArgs(DateTime localTime, int score, Area area, string currency)
    {
        LocalTime = localTime;
        Area = area;
        Score = score;
        Currency = currency;
    }

    public DateTime LocalTime { get; }
    public Area Area { get; }
    public int Score { get; }
    public string Currency { get; }
}