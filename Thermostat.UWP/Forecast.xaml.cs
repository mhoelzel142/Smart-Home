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
using Thermostat.Model;
using Thermostat.UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Thermostat.UWP
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

        public async void GetForecast()
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
            string requestUri = "data/2.5/forecast?zip=54915&appid=" + apiKey; // API Key here
            var result = client.GetAsync(requestUri).Result;
            var content = result.Content.ReadAsStringAsync();
            var text = content.Result;
            var weatherData = JsonConvert.DeserializeObject<ExtendedForecast>(text);

            // Change temperature from Kelvin to Fahrenheit, with 1 decimal place
            foreach (var item in weatherData.list)
            {
                // Convert temperatures from Kelvin to Fahrenheit
                item.main.temp = convertToFahrenheit(item.main.temp);
                item.main.temp_min = convertToFahrenheit(item.main.temp_min);
                item.main.temp_max = convertToFahrenheit(item.main.temp_max);

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
