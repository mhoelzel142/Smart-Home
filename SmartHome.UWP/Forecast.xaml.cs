using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using SmartHome.Model;
using SmartHome.UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SmartHome.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForecastTest : Page
    {
        public ForecastTest()
        {
            this.InitializeComponent();
            GetForecast();
        }

        public void GetForecast()
        {
            // Instantiate the viewModel list
            ForecastViewModel viewModel = new ForecastViewModel { Forecasts = new ObservableCollection<Forecast>() };

            HttpClient client = new HttpClient()
            {
                BaseAddress =
                    new Uri(
                        "https://api.openweathermap.org/")
            };
            var apiKey = ResourceLoader.GetForCurrentView().GetString("ApiKey");
            string requestUri = "data/2.5/forecast?zip=54915&appid=" + apiKey + "&units=imperial"; // API Key here

            var result = client.GetAsync(requestUri).Result.Content.ReadAsStringAsync().Result;
            var weatherData = JsonConvert.DeserializeObject<ExtendedForecast>(result);
            var weatherList = weatherData.list;

            // Group the results by day so they can be enumerated on a per-day basis
            var groupedWeatherByDay = weatherList.GroupBy(
                    w => w.dt_txt.ToString("MMM dd, yyyy"),
                    (key) => new { weatherList = key });

            // Get the high and low for current day. TODO: This needs to be improved, make a 2nd api request maybe?
            var firstItem = groupedWeatherByDay.First();
            todayLowTemp.Text = Math.Round(firstItem.First().weatherList.main.temp_min).ToString();
            todayHighTemp.Text = Math.Round(firstItem.First().weatherList.main.temp_max).ToString();


            //var grouped = weatherList.GroupBy(x => x.dt_txt.ToString("MMM dd, yyyy")).Select(y => y.Key).ToList();

            // Change temperature from Kelvin to Fahrenheit, with 1 decimal place
            foreach (var oneDayWeather in groupedWeatherByDay)
            {
                // Get the minimum and maximum temperature from the list for each day
                var dayMaxTemp = oneDayWeather.Max(x => x.weatherList.main.temp_max);
                var dayMinTemp = oneDayWeather.Min(x => x.weatherList.main.temp_min);
                var currentTemp = oneDayWeather.First().weatherList.main.temp;
                foreach (var single in oneDayWeather)
                {
                    var item = single.weatherList;

                    // Don't need this anymore - OWM has an extra paramter &units=imperial to return Fahrenheit
                    // Convert temperatures from Kelvin to Fahrenheit
                    //item.main.temp = convertToFahrenheit(item.main.temp);
                    
                    //item.main.temp_min = convertToFahrenheit(item.main.temp_min);
                    //item.main.temp_max = convertToFahrenheit(item.main.temp_max);

                    // convert OpenWeatherMap icon from a sprite into a png image from the Images folder
                    item.weather[0].icon = GetUsableWeatherIcon(item.weather[0].icon);


                    // Get day of week for forecast
                    if (item.dt_txt >= DateTime.Now)
                    {
                        // Expected input: 2018/01/01 06:00:00
                        // Expected output: Tuesday, January 01 (or January 1) 
                        var forecastDate = item.dt_txt.ToString("dddd, MMMM dd");
                        item.dayofweek = forecastDate;
                    }

                    // assign the min and max temp to the item
                    item.main.temp_max = Math.Floor(dayMaxTemp);
                    item.main.temp_min = Math.Floor(dayMinTemp);

                    // Show 1 temperature per day
                    var index = weatherData.list.IndexOf(item);
                    if (((index + 7) % 8) == 0)
                    {
                        viewModel.Forecasts.Add(item);
                    }
                }
                //viewModel.Forecasts = weatherData.list;
                DisplayWeather.ItemsSource = viewModel.Forecasts;
            }
        }

        private string GetUsableWeatherIcon(string icon)
        {
            string newIcon;
            switch (icon)
            {
                case "01d":
                    newIcon = "Images/clear.png";
                    return newIcon;
                case "02d":
                    newIcon = "Images/partly-cloudy.png";
                    return newIcon;
                case "03d":
                    newIcon = "Images/scattered-clouds.png";
                    return newIcon;
                case "04d":
                    newIcon = "Images/cloudy.png";
                    return newIcon;
                case "09d":
                    newIcon = "Images/light-rain.png";
                    return newIcon;
                case "10d":
                    newIcon = "Images/rain.png";
                    return newIcon;
                case "11d":
                    newIcon = "Images/thunderstorm.png";
                    return newIcon;
                case "13d":
                    newIcon = "Images/snow.png";
                    return newIcon;
                case "01n":
                    newIcon = "Images/clear.png";
                    return newIcon;
                case "02n":
                    newIcon = "Images/partly-cloudy.png";
                    return newIcon;
                case "03n":
                    newIcon = "Images/scattered-clouds.png";
                    return newIcon;
                case "04n":
                    newIcon = "Images/cloudy.png";
                    return newIcon;
                case "09n":
                    newIcon = "Images/light-rain.png";
                    return newIcon;
                case "10n":
                    newIcon = "Images/rain.png";
                    return newIcon;
                case "11n":
                    newIcon = "Images/thunderstorm.png";
                    return newIcon;
                case "13n":
                    newIcon = "Images/snow.png";
                    return newIcon;
                default:
                    newIcon = icon;
                    return newIcon;
            }
        }

        private decimal convertToFahrenheit(decimal kelvinTemp)
        {

            var fahrenheitTemp = ((decimal)1.8 * (kelvinTemp - 273)) + 32;
            return Math.Floor(fahrenheitTemp);
        }
    }
}
