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
        private Bitmap ScreenBitmap;
        private Graphics ScreenGraphics;
        private Bitmap AmbientBitmap;
        private Graphics AmbientGraphics;

        public override int HomeID
        {
            get { return Properties.Settings.Default.HomeID; }
            set { Properties.Settings.Default.HomeID = value; Properties.Settings.Default.Save(); }
        }

        public override Color ScreenAmbientColor
        {
            get
            {
                if (AmbientBitmap == null)
                {
                    ScreenBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Size.Width, Screen.PrimaryScreen.Bounds.Size.Height);
                    ScreenGraphics = Graphics.FromImage(ScreenBitmap);
                    AmbientBitmap = new Bitmap(1, 1);
                    AmbientGraphics = Graphics.FromImage(AmbientBitmap);
                    AmbientGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                }
                ScreenGraphics.CopyFromScreen(0, 0, 0, 0, ScreenBitmap.Size, CopyPixelOperation.SourceCopy);
                AmbientGraphics.DrawImage(ScreenBitmap, 0, 0, 1, 1);
                return AmbientBitmap.GetPixel(0, 0);
            }
        }
    }
}