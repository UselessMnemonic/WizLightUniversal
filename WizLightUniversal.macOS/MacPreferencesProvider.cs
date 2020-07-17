using Foundation;
using WizLightUniversal.Core;

namespace WizLightUniversal.macOS
{
    public class MacPreferencesProvider : PreferencesProvider
    {
        private NSUserDefaults defaults;

        public override int HomeID
        {
            get
            {
                return (int) defaults.IntForKey("HomeID");
            }

            set
            {
                defaults.SetInt(value, "HomeID");
            }
        }

        public MacPreferencesProvider()
        {
            defaults = new NSUserDefaults();
        }
    }
}