using System;
using System.Globalization;
using WizLightUniversal.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WizLightUniversal.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizControlPage : ContentPage
    {
        // Constructor
        public WizControlPage(WizLightModel model)
        {
            BindingContext = model;
            InitializeComponent();

            // Set binding programmatically
            TempSlider.SetBinding(Slider.ValueProperty, "Temperature", BindingMode.TwoWay, new RangeToPercentConverter(model.MinimumTemperature, model.MaximumTemperature));
        }

        // That damn temeprature slider gives me a bad headache. The value is reset
        // anytime the minimum bound is set. Instead, I'll use constant bounds on the slider
        // and convert in between
        private class RangeToPercentConverter : IValueConverter
        {
            private readonly int sourceMinimum;
            private readonly int sourceMaximum;

            public RangeToPercentConverter(int sourceMinimum, int sourceMaximum)
            {
                this.sourceMaximum = sourceMaximum;
                this.sourceMinimum = sourceMinimum;
            }

            object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return 100 * ((int)value - sourceMinimum) / (sourceMaximum - sourceMinimum);
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return ((double)value * (sourceMaximum - sourceMinimum) / 100) + sourceMinimum;
            }
        }
    }
}