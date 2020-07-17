using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WizLightUniversal.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
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