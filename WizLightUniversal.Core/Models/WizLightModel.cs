using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xamarin.Forms.Shapes;

namespace WizLightUniversal.Core.Models
{
    public class WizLightModel
    {
        public string IP { get; set; }
        public string MAC { get; set; }
        public int MinimumTemperature { get; set; }
        public int MaximumTemperature { get; set; }

        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public int WarmWhite { get; set; }
        public int CoolWhite { get; set; }
        public int Temperature { get; set; }

        public int Brightness { get; set; }
        public bool Power { get; set; }

        public bool UsingTemp { get; set; }
        public bool UsingColor { get; set; }
        public bool UsingScene { get; set; }

        public WizLightModel()
        {
            IP = "0.0.0.0";
            MAC = "00:00:00:00";
            MinimumTemperature = 0;
            MaximumTemperature = 100;

            Red = 0;
            Green = 0;
            Blue = 0;

            WarmWhite = 0;
            CoolWhite = 0;
            Temperature = MinimumTemperature;

            Brightness = 0;
            Power = false;

            UsingColor = true;
            UsingTemp = false;
            UsingScene = false;
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}
