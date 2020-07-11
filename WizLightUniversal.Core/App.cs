using System.Diagnostics;
using Xamarin.Forms;

namespace WizLightUniversal.Core
{
    public class App : Application
    {
        public App()
        {
            MainPage = new Views.MainPage();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Debug.WriteLine("Application started");
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            Debug.WriteLine("Application sleeps");
        }

        protected override void OnResume()
        {
            base.OnResume();
            Debug.WriteLine("Application resumes");
        }
    }
}