using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
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

        // Constructor
        public WinPreferencesProvider() : base() { }
    }
}