using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizLightUniversal.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WizLightUniversal.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private Collection<WizLightModel> lights;

        public HomePage()
        {
            InitializeComponent();
            lights = new Collection<WizLightModel>();
            lights.Add(new WizLightModel());
            listView.ItemsSource = lights;
        }

        public void Refresh()
        {
            //TODO
        }

        async void PreferencesButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PreferencesPage());
        }

        void RefreshButton_Clicked(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}