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
    public partial class MainNavigationPage : NavigationPage
    {
        public HomePage HomePage { get; }

        public MainNavigationPage() : base(new HomePage())
        {
            HomePage = (HomePage) RootPage;
        }
    }
}