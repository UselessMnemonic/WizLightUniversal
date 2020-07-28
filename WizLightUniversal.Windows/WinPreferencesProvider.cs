using System;
using System.IO;
using WizLightUniversal.Core;

namespace WizLightUniversal.Windows
{
    // The preferences provider offered by Windows 
    public class WinPreferencesProvider : PreferencesProvider
    {
        public override int HomeID
        {
            get { return Properties.Settings.Default.HomeID; }
            set { Properties.Settings.Default.HomeID = value; Properties.Settings.Default.Save(); }
        }
    }
}