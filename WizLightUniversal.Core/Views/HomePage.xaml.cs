using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using WizLightUniversal.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OpenWiz;
using System.Net;

namespace WizLightUniversal.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly ObservableCollection<WizLightModel> lights;
        private int lastUsedHomeID;
        private volatile WizDiscoveryService discoveryService;
        private volatile bool refreshInProgress;

        // Constructor
        public HomePage()
        {
            InitializeComponent();
            lights = new ObservableCollection<WizLightModel>();
            listView.ItemsSource = lights;
            refreshInProgress = false;
            lastUsedHomeID = 0;
            Refresh();
        }

        // refresh the light list fron LAN
        public async void Refresh()
        {
            if (!refreshInProgress)
            {
                refreshInProgress = true;

                if (lastUsedHomeID != PreferencesProvider.Default.HomeID)
                {
                    lights.Clear();
                    lastUsedHomeID = PreferencesProvider.Default.HomeID;
                    SpawnDiscoveryService();
                }

                if (discoveryService != null)
                {
                    discoveryService.Start(WhenLightDiscovered);
                    await Task.Delay(2000);
                    discoveryService.Stop();
                    refreshInProgress = false;
                }
            }
        }

        // called when a light is discovered
        public void WhenLightDiscovered(WizHandle handle)
        {
            foreach (var light in lights)
            {
                if (light.MAC == handle.Mac) return;
            }
            WizLightModel model = new WizLightModel(handle);
            lights.Add(model);
        }

        // called when preferences are selected
        async void PreferencesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PreferencesPage());
        }

        // called when a refresh is requested
        void RefreshButton_Clicked(object sender, EventArgs e)
        {
            Refresh();
        }

        // called when a light is selected
        async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                WizControlPage controlPage = new WizControlPage((WizLightModel) e.SelectedItem);
                listView.SelectedItem = null;
                await Navigation.PushAsync(controlPage);
            }
        }

        // starts the discovery service and pauses
        public void SpawnDiscoveryService()
        {
            if (lastUsedHomeID != 0)
            {
                (NetworkInterface nic, IPAddress ip) = PreferencesProvider.Default.NetworkInformation;
                if (nic != null && ip != null)
                {
                    discoveryService = null;
                    GC.Collect();
                    discoveryService = new WizDiscoveryService(lastUsedHomeID, ip.ToString(), nic.GetPhysicalAddress().GetAddressBytes());
                }
            }
        }
    }
}