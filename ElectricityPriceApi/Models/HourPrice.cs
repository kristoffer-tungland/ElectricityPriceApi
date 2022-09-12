using System;

namespace ElectricityPriceApi.Models
{
    public class HourPrice
    {
        public DateTime Time { get; set; }
        public float Price { get; set; }
    }
}