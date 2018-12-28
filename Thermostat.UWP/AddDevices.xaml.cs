using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
using Microsoft.Extensions.Logging;
using Thermostat.Model;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Thermostat.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddDevices : Page
    {
        public AddDevices()
        {
            this.InitializeComponent();

        }

        /// <summary>
        /// When clicked, begins searching for network devices which match the Espressif 8266 parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Button_Click(object sender, RoutedEventArgs e)
        {
            StartProgressBar();
            await System.Threading.Tasks.Task.Run(() => GetNetworkDevices());

        }


        /// <summary>
        /// Loops through IP addresses 192.168.1.0 to 192.168.1.255, looking for devices matching Espressif 8266. 
        /// </summary>
        public async void GetNetworkDevices()
        {
            int numNetworkDevicesFound = 0;
            int numMatchingDevicesFound = 0;

            DeviceContext db = new DeviceContext();

            // Loop through all available IP addresses in the 192.168.1 subnet
            for (int i = 0; i < 256; i++)
            {
                var found = numMatchingDevicesFound;
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => { NumberDevicesFound.Text = found.ToString(); });

                if (i == 255)
                {
                    await Task.Run(() => ShowCompletedDialog(found));
                    await Task.Run(() => StopProgressBar());
                }
                else
                {

                    // Calculate percentage complete and send to progress bar in UI
                    double percentComplete = (i / 256.0);
                    await Task.Run(() => UpdateProgressBar(percentComplete));
                    
                    // Put this code in another method
                    HttpClient client = new HttpClient()
                    {
                        BaseAddress = new Uri("http://192.168.1." + i),
                        Timeout = TimeSpan.FromMilliseconds(400)
                    };
                    try
                    {
                        var result = client.GetAsync(client.BaseAddress).Result;
                        if ((int)result.StatusCode == 200)
                        {
                            var content = result.Content.ReadAsStringAsync();

                            if (content.Result.Contains("Temperature"))
                            {
                                if (!db.Devices.Any(x => x.DeviceIp == client.BaseAddress.ToString()))
                                {


                                    // Get a list of system colors
                                    ObservableCollection<string> colors = new ObservableCollection<string>();
                                    colors = Helpers.Colors.GetAvailableColors();

                                    // Assign a color randomly using Random.Next
                                    Random rnd = new Random();
                                    var colorIndex = rnd.Next(colors.Count - 1);

                                    var response = content.Result;
                                    var temperature = response.IndexOf("Fahrenheit");
                                    var currentTemperature = response.Substring(temperature + 14, 4) + "° F";
                                    // We only expect a number/float here, so if it contains letters, ignore it:
                                    if (!currentTemperature.Contains("aile") && !currentTemperature.Contains("Fail") && !currentTemperature.Contains("n"))
                                    {

                                        db.Devices.Add(new Device()
                                        {
                                            DeviceIp = client.BaseAddress.ToString(),
                                            DeviceName = "Unnamed Sensor",
                                            DeviceTemperature = currentTemperature,
                                            // Get the color
                                            DeviceTileColor = colors[colorIndex]
                                        });
                                        await db.SaveChangesAsync();
                                        numMatchingDevicesFound++;
                                    }
                                }
                            }
                        }
                        // Get rid of HttpClient to free up memory
                        client.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        client.Dispose();
                        numNetworkDevicesFound++;

                    }
                }
            }
        }



        /// <summary>
        /// Starts the progress ring
        /// </summary>
        public void StartProgressBar()
        {
            // Hide the button and show other UI elements while searching for new devices
            ProgressStatusText.Visibility = Visibility.Visible;
            BtnAddDevice.Visibility = Visibility.Collapsed;
            ProgressRing.Visibility = Visibility.Visible;
            ProgressRing.IsActive = true;
            NumDevicesFoundLabel.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///  Stops and hides the progress ring, if it is already started
        /// </summary>
        public async void StopProgressBar()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                // Once completed, hide all UI elements again
                if (ProgressRing.IsActive)
                {
                    ProgressRing.Visibility = Visibility.Collapsed;
                    ProgressBar.Value = 100;
                    ProgressStatusText.Visibility = Visibility.Collapsed;
                    ProgressStatusText.Visibility = Visibility.Collapsed;
                    ProgressRing.IsActive = false;
                }
            });
        }

        /// <summary>
        /// Shows a "searching complete" dialog and shows the number of matching network devices found (number of Espressif ESP8266 devices)
        /// </summary>
        /// <param name="numDevicesFound"></param>
        public async void ShowCompletedDialog(int numDevicesFound)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                MessageDialog dialog = new MessageDialog($"Operation completed. Found {numDevicesFound} devices");
                await dialog.ShowAsync();
            });
        }

        /// <summary>
        /// Updates the progress bar with the completion percentage
        /// </summary>
        /// <param name="percent">A double representing the operation completion progress percentage.</param>
        public async void UpdateProgressBar(double percent)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => { ProgressBar.Value = percent * 100; });
        }

        /// <summary>
        /// Obsolete. Shows a dialog box with a text input to name the device. This will be replaced with a new view to show/name all devices on one page. 
        /// </summary>
        /// <param name="title">The text to display in the dialog box's title</param>
        /// <returns>A string value of TextBox.Text</returns>
        private async Task<string> InputTextDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox();
            inputTextBox.AcceptsReturn = false;
            inputTextBox.Height = 32;
            ContentDialog dialog = new ContentDialog();
            dialog.Content = inputTextBox;
            dialog.Title = title;
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Ok";
            dialog.SecondaryButtonText = "Cancel";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                return inputTextBox.Text;
            else
                return "";
        }
    }
}
