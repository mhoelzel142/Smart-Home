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
        public DisplaySensors()
        {
            this.InitializeComponent();
            StartAsyncTimedWork();
        }

        //public void GetDevices()
        //{
        //    DeviceContext db = new DeviceContext();
        //    var devices = db.Devices.ToList();
        //    DisplayThermometers.ItemsSource = devices;
        //}

        private async void StartAsyncTimedWork()
        {

            DeviceContext db = new DeviceContext();
            DevicesViewModel viewModel = new DevicesViewModel();
            viewModel.Devices = new ObservableCollection<Device>(db.Devices.ToList());

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                async () => { DisplayThermometers.ItemsSource = viewModel.Devices; });


            // Start the timer on a loop
            Timer timer = new Timer(10000);
            timer.Elapsed += async delegate
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    async () => { DisplayThermometers.ItemsSource = viewModel.Devices; });
                foreach (var device in viewModel.Devices)
                {
                    Uri uri = new Uri(device.DeviceIp);
                    HttpClient client = new HttpClient();
                    client.BaseAddress = uri;
                    try
                    {
                        var response = client.GetAsync(client.BaseAddress).Result.Content.ReadAsStringAsync();
                        var temperature = response.Result.IndexOf("Fahrenheit");

                        //var currentTemperature = response.Result;
                        var currentTemperature = response.Result.Substring(temperature + 14, 4) + "° F";
                        // We only expect a number/float here, so if it contains letters, ignore it:
                        if (!currentTemperature.Contains("aile") && !currentTemperature.Contains("Fail") && !currentTemperature.Contains("n"))
                        {
                            device.DeviceTemperature = currentTemperature;
                            db.Entry(device).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    

                    // Re-instantiate the viewModel's collection of Devices
                    viewModel.Devices = new ObservableCollection<Device>(db.Devices.ToList());

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

        private async void DisplayThermometers_ItemClick(object sender, ItemClickEventArgs e)
        {
            Device device = (Device)DisplayThermometers.SelectedItem;

            MessageDialog dialog = new MessageDialog("Selected Index: " + device?.Id);
            await dialog.ShowAsync();

        }
    }
}
