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
    public partial class WizControlPage : ContentPage
    {
        public WizControlPage(WizLightModel model)
        {
            InitializeComponent();
            this.BindingContext = model;
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory.ToString());
        }
    }
}