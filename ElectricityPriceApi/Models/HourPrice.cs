using System;

namespace ElectricityPriceApi.Models;

public class HourPrice
{
    public DateTime Time { get; set; }
    public float Price { get; set; }
}

public class HourPriceScore : HourPrice
{
    public HourPriceScore()
    {
            
    }

    public HourPriceScore(HourPrice hourPrice)
    {
        Time = hourPrice.Time;
        Price = hourPrice.Price;
    }

    public int Score { get; set; }
}