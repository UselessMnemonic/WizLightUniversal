using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using WizLightUniversal.Core;

namespace WizLightUniversal.Windows
{
    public class WinPreferencesProvider : PreferencesProvider
    {
        public override int HomeID
        {
            get { return Properties.Settings.Default.HomeID; }
            set { Properties.Settings.Default.HomeID = value; Properties.Settings.Default.Save(); }
        }
    }
}