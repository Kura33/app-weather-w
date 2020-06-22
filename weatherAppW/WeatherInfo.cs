using System.Collections.Generic;

namespace weatherAppW
{
    internal class WeatherInfo
    {
        // class temps
        public class Weather
        {
            public string Description { get; set; }
            public string Icon { get; set; } // icones temps
        }

        // class barometre
        public class Main
        {
            public double Temp { get; set; }
            public double Humidity { get; set; }
        }

        // class vent
        public class Wind
        {
            public double Speed { get; set; }
        }

        //class pays
        public class Sys
        {
            public string Country { get; set; }
        }

        // class root regroupant les différentes données
        public class Root
        {
            public string Name { get; set; }
            public Sys Sys { get; set; }
            public long Dt { get; set; }
            public Wind Wind { get; set; }
            public Main Main { get; set; }
            public List<Weather> Weather { get; set; }

        }
    }
}
