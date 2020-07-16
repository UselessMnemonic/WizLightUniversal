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
    public partial class MainPage : ContentPage
    {
        private Action quitAction;
        private IPreferencesProvider preferencesProvider;

        public MainPage(Action quitAction, IPreferencesProvider preferencesProvider)
        {
            InitializeComponent();
            this.quitAction = quitAction;
            this.preferencesProvider = preferencesProvider;
        }

        public void Quit_Clicked(object sender, EventArgs e)
        {
            quitAction.Invoke();
        }

        private async void Preferences_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PreferencesPage(preferencesProvider));
        }
    }
}