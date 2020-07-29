using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WizLightUniversal.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainNavigationPage : NavigationPage
    {
        public HomePage HomePage { get; }

        public MainNavigationPage() : base(new HomePage())
        {
            HomePage = (HomePage)RootPage;
            Popped += OnPageUnload;
        }

        // TODO: Find the source of all that used up memory... 40MB at least!
        public void OnPageUnload(object sender, NavigationEventArgs e)
        {
            GC.Collect();
        }
    }
}