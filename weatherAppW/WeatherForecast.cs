using System.Collections.Generic;

namespace weatherAppW
{
    public class WeatherForecast
    {
        public List<List> List { get; set; } //liste de prévisions
    }

    public class Temp
    {
        public double Min { get; set; }
        public double Max { get; set; }
    }
    public class Weather
    {
        public string Description { get; set; } // description temps
        public string Icon { get; set; } // icones temps
    }
    public class List
    {
        //jours en millisecondes
        public long Dt { get; set; }
        //pression en hecto pascal
        public double Speed { get; set; }
        public Temp Temp { get; set; }
        public List<Weather> Weather { get; set; } // liste de la catégorie weather
    }

}
