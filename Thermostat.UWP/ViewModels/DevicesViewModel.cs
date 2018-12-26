using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Thermostat.Model;
using Thermostat.Model.Annotations;

namespace Thermostat.UWP.ViewModels
{
    public class DevicesViewModel
    {
        public Device Device { get; set; }
        private ObservableCollection<Device> _devices;

        public ObservableCollection<Device> Devices
        {
            get => _devices;
            set
            {
                if (value != _devices)
                {
                    _devices = value;
                    OnPropertyChanged(nameof(Devices));
                }
            }
        } 

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
