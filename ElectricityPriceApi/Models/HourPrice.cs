using System;

namespace ElectricityPriceApi.Models
{
    public class HourPrice
    {
        public float PriceAmount { get; set; }
        public DateTime StartTime { get; set; }
    }
}