using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Timers;
using Windows.ApplicationModel.Background;
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
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new DevicesViewModel();
            MainFrame.Navigate(typeof(DisplaySensors));
        }
        public DevicesViewModel ViewModel { get; set; }

        

        private void ExpandMenu(object sender, RoutedEventArgs e)
        {
            if (splitView.IsPaneOpen)
            {
                splitView.IsPaneOpen = false;
            }

            else if (!splitView.IsPaneOpen)
            {
                splitView.IsPaneOpen = true;
            }
        }

        private void GetNavMenuItems()
        {
            var navItems = new List<NavMenuItem>()
            {

                new NavMenuItem()
                {
                    Symbol = Symbol.Home,
                    Label = "Display",
                    DestinationPage = typeof(DisplaySensors)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Globe,
                    Label = "Forecast",
                    DestinationPage = typeof(ForecastTest)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Add,
                    Label = "Add Sensors",
                    DestinationPage = typeof(AddDevices)
                }
            };

            NavLinksList.ItemsSource = navItems;
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            GetNavMenuItems();
        }

        private void NavLinksList_OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is NavMenuItem menuItem && menuItem.IsNavigation)
            {
                MainFrame.Navigate(menuItem.DestinationPage);
            }
        }
    }

    public class NavMenuItem
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar => (char)this.Symbol;
        public Type DestinationPage { get; set; }
        public object Arguments { get; set; }

        public bool IsNavigation => DestinationPage != null;
    }
}
