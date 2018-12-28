using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Thermostat.Model;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Thermostat.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditSensor : Page
    {
        public EditSensor()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Device device = e.Parameter as Device;
            PopulateFields(device);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DisplaySensors));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DeviceContext db = new DeviceContext();
            var deviceString = HiddenForDeviceId.Text;
            int deviceId = Convert.ToInt32(deviceString);
            var device = db.Devices.Find(deviceId);
            var colors = Helpers.Colors.GetAvailableColors();

            device.DeviceName = TxtDeviceName.Text;
            device.DeviceIp = TxtDeviceIp.Text;
            device.DeviceTileColor = colors[cbComboBox.SelectedIndex];
            db.Entry(device).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();

            BackButton_Click(null, null);
        }

        private void PopulateFields(Device device)
        {
            if (device.DeviceName == null) device.DeviceName = "Give this sensor a name";
            TxtDeviceName.Text = device.DeviceName;
            TxtDeviceIp.Text = device.DeviceIp;
            DisplayDeviceIp.Text = device.DeviceIp;
            DisplayDeviceName.Text = device.DeviceName;
            HiddenForDeviceId.Text = device.Id.ToString();
            confirmDeleteDialog.Tag = device;

            ObservableCollection<string> colors = Helpers.Colors.GetAvailableColors();
            ObservableCollection<Colors> itemList = new ObservableCollection<Colors>();
            foreach(var color in colors)
            {
                Colors c = new Colors()
                {
                    colorName = color
                };
                itemList.Add(c);
            }
            cbComboBox.ItemsSource = itemList;
            cbComboBox.SelectedIndex = colors.IndexOf(device.DeviceTileColor);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await confirmDeleteDialog.ShowAsync();
        }

        private void ConfirmDeleteDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var button = sender as ContentDialog;
            Device device = (Device)button.Tag;
            DeviceContext db = new DeviceContext();
            db.Devices.Remove(device);
            db.SaveChanges();

            BackButton_Click(null, null);
        }
    }

    public class Colors
    {
        public string colorName { get; set; }
    }
}
