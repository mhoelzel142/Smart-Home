using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using GalaSoft.MvvmLight.Threading;
using Microsoft.EntityFrameworkCore;

namespace Thermostat.Model
{
    public class DeviceContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Database.db");
        }
    }

    public class Device : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIp { get; set; }
        public string DeviceMac { get; set; }

        private string _deviceTemperature;
        public string DeviceTemperature {
            get => _deviceTemperature;
            set {
                if(value != _deviceTemperature)
                {
                    _deviceTemperature = value;
                    DispatcherHelper.CheckBeginInvokeOnUI(() => NotifyPropertyChanged());
                }
            }
        }

        // NotifyPropertyChanged on UI thread (using Galasoft.MvvmLight DispatcherHelper) to update tiles without refreshing UI
        private string _deviceHumidity;
        public string DeviceHumidity
        {
            get => _deviceHumidity;
            set
            {
                if (value != _deviceHumidity)
                {
                    _deviceHumidity = value;
                    DispatcherHelper.CheckBeginInvokeOnUI(() => NotifyPropertyChanged());
                }
            }
        }
        public string DeviceTileColor { get; set; }
        public string DeviceTextColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
