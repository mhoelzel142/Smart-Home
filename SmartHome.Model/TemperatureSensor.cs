using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartHome.Model
{
    /// <summary>
    /// The base class for a DHT22 Temperature Sensor
    /// Each device has a json encoded value for Device Type, Temperature, and Humidity. 
    /// This class allows the values to be decoded into a strongly typed object.
    /// </summary>
    class TemperatureSensor : INotifyPropertyChanged
    {

        /// <summary>
        /// The type of device - private field
        /// </summary>
        private string _deviceType;

        /// <summary>
        /// The type of device (DHT11, DHT22, etc)
        /// </summary>
        public string DeviceType
        {
            get => _deviceType;
            set
            {
                if(value != _deviceType)
                {
                    _deviceType = value;
                }
            }
        }

        /// <summary>
        /// The current temperature of the device - private field
        /// </summary>
        private decimal _temperature;

        /// <summary>
        /// The current temperature of the device
        /// </summary>
        public decimal Temperature
        {
            get => _temperature;
            set {
                if(value != _temperature)
                {
                    _temperature = value;
                    DispatcherHelper.CheckBeginInvokeOnUI(() => NotifyPropertyChanged());
                }
            }
        }


        /// <summary>
        /// The device humidity - private field
        /// </summary>
        private decimal _humidity;

        /// <summary>
        /// The current humidity of the device
        /// </summary>
        public decimal Humidity {
            get => _humidity;
            set {
                if(value != _humidity)
                {
                    _humidity = value;
                    DispatcherHelper.CheckBeginInvokeOnUI(() => NotifyPropertyChanged());

                }
            }
        }

        /// <summary>
        /// Notifies any dependent view items that the property has changed, and updates them acordingly
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the view that the property of an item has changed, and updates it accordingly.
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
