using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thermostat.UWP.Helpers
{
    public class Colors
    {
        public static ObservableCollection<string> GetAvailableColors()
        {
            var colors = new ObservableCollection<string>
            {
                "Aqua",
                "Aquamarine",
                "BlueViolet",
                "Chartreuse",
                "Coral",
                "Crimson",
                "DarkCyan",
                "DarkMagenta",
                "DarkTurquoise",
                "DeepPink",
                "DeepSkyBlue",
                "Gold",
                "GreenYellow",
                "Indigo",
                "LightCoral",
                "LightSeaGreen",
                "Lime",
                "Magenta",
                "Maroon",
                "MidnightBlue",
                "OliveDrab",
                "Orange",
                "OrangeRed",
                "Orchid",
                "RoyalBlue",
                "SeaGreen",
                "Sienna",
                "SlateBlue",
                "Teal",
                "Tomato",
                "Yellow",
                "YellowGreen"
            };
            return colors;
        }
    }
}
