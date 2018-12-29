using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thermostat.Model;

namespace Thermostat.UWP.ViewModels
{
    class ForecastViewModel
    {

        public ObservableCollection<Forecast> Forecasts { get; set; }
        public DateTime Today = DateTime.Now;
    }
}
