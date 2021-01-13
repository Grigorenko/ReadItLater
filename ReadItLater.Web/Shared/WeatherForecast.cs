using System;
using System.Collections.Generic;
using System.Text;

namespace ReadItLater.Web.Shared
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    public class ItemC
    {
        public ItemC()
        {

        }
        public ItemC((string, string) obj)
        {
            Title = obj.Item1;
            Image = obj.Item2;
        }

        public string Title { get; set; }
        public string Image { get; set; }
    }
}
