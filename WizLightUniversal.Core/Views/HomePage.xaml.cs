using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using WizLightUniversal.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OpenWiz;
using System.Diagnostics;

namespace WizLightUniversal.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly ObservableCollection<WizLightModel> lights;
        private int lastUsedHomeID;
        private volatile WizDiscoveryService discoveryService;
        private volatile bool refreshInProgress;
        private Timer AmbientTimer;

        // Constructor
        public HomePage()
        {
            InitializeComponent();
            lights = new ObservableCollection<WizLightModel>();
            listView.ItemsSource = lights;
            refreshInProgress = false;
            lastUsedHomeID = 0;
            Refresh();
            AmbientTimer = new Timer(AmbientTimerCallback, this, Timeout.Infinite, Timeout.Infinite);
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
                if (!AmbientCheckbox.IsChecked)
                {
                    WizControlPage controlPage = new WizControlPage((WizLightModel)e.SelectedItem);
                    listView.SelectedItem = null;
                    await Navigation.PushAsync(controlPage);
                }
                else
                {
                    listView.SelectedItem = null;
                }
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

        // Called when the ambient checkbox is changed
        private void AmbientCheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (AmbientCheckbox.IsChecked)
            {
                foreach (var light in lights)
                {
                    light.SavePilot();
                }
                AmbientTimer.Change(0, AMBIENT_UPDATE_PERIOD_MS);
            }
            else
            {
                AmbientTimer.Change(Timeout.Infinite, 0);
                foreach (var light in lights)
                {
                    light.RestorePilot();
                }
            }
        }

        // Called when an ambient update is needed
        private const int AMBIENT_UPDATE_PERIOD_MS = 200;
        private volatile bool UpdatingAmbience = false;
        private void AmbientTimerCallback(object state)
        {
            if (UpdatingAmbience) return;
            UpdatingAmbience = true;
            WizState ambientState = new WizState() { Method = WizMethod.setPilot, Params = new WizParams() };
            System.Drawing.Color c = PreferencesProvider.Default.ScreenAmbientColor;
            if (c.GetBrightness() < 0.1)
            {
                ambientState.Params.State = false;
            }
            else
            {
                ambientState.Params.R = c.R;
                ambientState.Params.G = c.G;
                ambientState.Params.B = c.B;
                ambientState.Params.C = (int)(c.GetSaturation() > 0.5 ? 0.0 : 25 * (1 - c.GetSaturation()));
                ambientState.Params.W = 0;
                ambientState.Params.Dimming = (int)(100 * c.GetBrightness());
            }
            Debug.WriteLine("Ambient Control: " + ambientState);
            foreach (var light in lights)
            {
                light.QueueUpdate(ambientState);
            }
            UpdatingAmbience = false;
        }
    }
}