using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Thermostat.Model;
using Thermostat.UWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Thermostat.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DisplaySensors : Page
    {

        Timer timer = new Timer(5000);

        public DisplaySensors()
        {
            this.InitializeComponent();
            //AddDevices();
            StartAsyncTimedWork();
        }

        //public void GetDevices()
        //{
        //    DeviceContext db = new DeviceContext();
        //    var devices = db.Devices.ToList();
        //    DisplayThermometers.ItemsSource = devices;
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            timer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            timer.Stop();
        }

        private void AddDevices()
        {
            DeviceContext db = new DeviceContext();
            // Get a list of system colors
            ObservableCollection<string> colors = new ObservableCollection<string>();
            colors = Helpers.Colors.GetAvailableColors();

            // Assign a color randomly using Random.Next
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                var colorIndex = rnd.Next(colors.Count - 1);
                var d = new Device() { 
                
                    Id = 50 + i,
                    DeviceName = "Test Device",
                    DeviceTemperature = "72.15",
                    DeviceHumidity = i.ToString(),
                    DeviceIp = "http://192.168.1." + i,
                    DeviceTileColor = colors[colorIndex]
                };
                db.Devices.Add(d);
                db.SaveChanges();
            }
        }

        private async void StartAsyncTimedWork()
        {
            // Instantiate objects
            DeviceContext db = new DeviceContext();
            DevicesViewModel viewModel = new DevicesViewModel();
            viewModel.Devices = new ObservableCollection<Device>(db.Devices.ToList());

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => { DisplayThermometers.ItemsSource = viewModel.Devices; });


            // Start the timer on a 10 second loop

            // On each loop completion, execute the code to check the temperatures and update UI
            timer.Elapsed += async delegate
            {
                // Re-set the itemssource as viewModel.Devices - May not need this after MvvmLight and BeginInvokeOnUI()
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () => { DisplayThermometers.ItemsSource = viewModel.Devices; });

                foreach (var device in viewModel.Devices)
                {
                    Uri uri = new Uri(device.DeviceIp);
                    HttpClient client = new HttpClient();
                    client.BaseAddress = uri;
                    try
                    {
                        var response = client.GetAsync(client.BaseAddress).Result.Content.ReadAsStringAsync();
                        var temperature = response.Result.IndexOf("Fahrenheit");
                        var humidity = response.Result.IndexOf("Humidity");

                        //var currentTemperature = response.Result;
                        var currentTemperature = response.Result.Substring(temperature + 14, 4) + "° F";
                        var currentHumidity = response.Result.Substring(humidity + 13, 4) + "%";
                        // We only expect a number/float here, so if it contains letters, ignore it:
                        if (!currentTemperature.Contains("aile") && !currentTemperature.Contains("Fail") && !currentTemperature.Contains("n"))
                        {
                            device.DeviceTemperature = currentTemperature;
                            device.DeviceHumidity = currentHumidity;
                            //device.DeviceTemperature = "15";
                            db.Entry(device).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
                            // Re - instantiate the viewModel's collection of Devices

                        }
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                        () =>
                        {
                            DisplayThermometers.ItemsSource = viewModel.Devices;
                        });
                    }
                    catch(InvalidOperationException e)
                    {
                        Console.WriteLine(e);
                    }
                    catch (Exception e)
                    {
                        foreach(var d in viewModel.Devices)
                        {
                            d.DeviceTemperature = "15.6";
                        }
                        // Commented out to see if propertyChanged works
                        //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                        //() =>
                        //{
                        //    DisplayThermometers.ItemsSource = viewModel.Devices;
                        //});
                        Console.WriteLine(e);
                    }



                    //DisplayThermometers.ItemsSource = viewModel.Devices;

                    //viewModel.Devices.CollectionChanged += delegate { DisplayThermometers.Items?.Remove(Devices); };
                    //Devices.CollectionChanged += delegate { DisplayThermometers.Items.Add(Devices); };
                    //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    //    async () =>
                    //    {
                    //        DisplayThermometers.UpdateLayout();
                    //        DisplayThermometers.InvalidateArrange();
                    //        DisplayThermometers.ItemsSource = viewModel.Devices;
                    //    });
                }
            };
            timer.Start();
        }

        void GetDeviceTemperatures()
        {

        }

        private void DisplayThermometers_ItemClick(object sender, ItemClickEventArgs e)
        {
            timer.Stop();
            Device device = (Device)e.ClickedItem;
            sensorDisplayFrame.Navigate(typeof(EditSensor), device);

        }
    }
}
