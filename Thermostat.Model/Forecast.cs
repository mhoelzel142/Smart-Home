using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Thermostat.Model
{
    public class ExtendedForecast
    {
        public int cod { get; set; }
        public decimal message { get; set; }
        public int cnt { get; set; }
        public ObservableCollection<Forecast> list { get; set; }
        public City coty { get; set; }
    }

    public class Forecast
    {
        public long dt { get; set; }
        public Main main { get; set; }
        public List<Weather> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public Rain rain { get; set; }
        public Snow snow { get; set; }
        public Sys sys { get; set; }
        public DateTime dt_txt { get; set; }
        public string dayofweek { get; set; }
    }

    public class Main
    {
        public decimal temp { get; set; }
        public decimal temp_min { get; set; }
        public decimal temp_max { get; set; }
        public decimal pressure { get; set; }
        public decimal sea_level { get; set; }
        public decimal grnd_level { get; set; }
        public decimal humidity { get; set; }
        public decimal temp_kf { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Clouds
    {
        string all { get; set; }
    }

    public class Wind
    {
        public decimal speed { get; set; }
        public decimal deg { get; set; }
    }

    public class Rain
    {
        public decimal threeHour { get; set; }
    }

    public class Snow
    {
        public decimal threeHour { get; set; }
    }

    public class Sys
    {
        public string pod { get; set; }
    }

    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public Coord coord { get; set; }
        public string country { get; set; }
    }

    public class Coord
    {
        public decimal lat { get; set; }
        public decimal lon { get; set; }
    }
}
