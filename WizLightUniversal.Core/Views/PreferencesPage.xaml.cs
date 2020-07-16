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
    public partial class PreferencesPage : ContentPage
    {
        private IPreferencesProvider provider;

        public PreferencesPage(IPreferencesProvider provider)
        {
            InitializeComponent();
            this.provider = provider;
            if (provider.HomeID > 0)
            {
                HomeIDEntry.Text = provider.HomeID.ToString();
            }
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            int homeId = 0;
            string text = HomeIDEntry.Text;

            if (text.Length == 6 && int.TryParse(text, out homeId))
            {
                provider.HomeID = homeId;
            }

            await Navigation.PopModalAsync();
        }
    }
}