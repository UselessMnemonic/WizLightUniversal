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
            IP = "255.255.255.255";
            MAC = "ff:ff:ff:ff:ff:ff";
            MinimumTemperature = 2000;
            MaximumTemperature = 6500;

            Red = 255;
            Green = 255;
            Blue = 255;

            WarmWhite = 0;
            CoolWhite = 0;
            Temperature = MinimumTemperature;

            Brightness = 100;
            Power = false;

            UsingColor = true;
            UsingTemp = false;
            UsingScene = false;
        }
    }
}
