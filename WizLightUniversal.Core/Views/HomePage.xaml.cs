using OpenWiz;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WizLightUniversal.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WizLightUniversal.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly ObservableCollection<WizLightModel> lights;
        private volatile WizDiscoveryService discoveryService;
        private volatile bool refreshInProgress;

        // Constructor
        public HomePage()
        {
            InitializeComponent();
            lights = new ObservableCollection<WizLightModel>();
            listView.ItemsSource = lights;
            refreshInProgress = false;
            SpawnDiscoveryService();
            Refresh();
        }

        // refresh the light list fron LAN
        public async void Refresh()
        {
            if (!refreshInProgress)
            {
                refreshInProgress = true;
                if (discoveryService != null)
                {
                    discoveryService.Start(WhenLightDiscovered);
                    await Task.Delay(2000);
                    discoveryService.Stop();
                }
                refreshInProgress = false;
            }
        }

        // called when a light is discovered
        public void WhenLightDiscovered(WizHandle handle)
        {
            foreach (var light in lights)
            {
                if (light.MAC.ToLower() == handle.Mac.ToLower()) return;
            }
            WizLightModel model = new WizLightModel(handle);
            model.ShouldUpdate = true;
            lights.Add(model);
        }

        // called when preferences are selected
        async void PreferencesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PreferencesPage());
        }

        // called when a refresh is requested
        void RefreshButton_Clicked(object sender, EventArgs e) => Refresh();

        // called when a light is selected
        async void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                WizControlPage controlPage = new WizControlPage((WizLightModel)e.SelectedItem);
                listView.SelectedItem = null;
                await Navigation.PushAsync(controlPage);
            }
        }

        // starts the discovery service and pauses
        public void SpawnDiscoveryService()
        {
            if (PreferencesProvider.Default.HostNIC != null && PreferencesProvider.Default.HostIP != null)
            {
                string ip = PreferencesProvider.Default.HostIP.ToString();
                byte[] mac = PreferencesProvider.Default.HostNIC.GetPhysicalAddress().GetAddressBytes();
                discoveryService = null;
                GC.Collect();
                discoveryService = new WizDiscoveryService(PreferencesProvider.Default.HomeID, ip, mac);
            }
        }
    }
}