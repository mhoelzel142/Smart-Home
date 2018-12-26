using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using Microsoft.EntityFrameworkCore;

namespace Thermostat.Model
{
    public class DeviceContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=devices.db");
        }
    }

    public class Device : ViewModelBase
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIp { get; set; }
        public string DeviceMac { get; set; }
        public string DeviceTemperature { get; set; }
        public string DeviceHumidity { get; set; }
    }
}
