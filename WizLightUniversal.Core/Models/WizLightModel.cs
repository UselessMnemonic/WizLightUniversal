using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xamarin.Forms.Shapes;

namespace WizLightUniversal.Core.Models
{
    class WizLightModel
    {
        public string IP { get; set; }
        public string MAC { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        public int WarmWhite { get; set; }
        public int CoolWhite { get; set; }
        public bool Power { get; set; }

        public WizLightModel()
        {
            Red = 0;
            Green = 0;
            Blue = 0;
            WarmWhite = 0;
            CoolWhite = 0;
            Power = true;
            MAC = "00:00:00:00";
            IP = "255.255.255.255";
        }
    }
}
