using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizLightUniversal.Core.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WizLightUniversal.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizControlPage : ContentPage
    {
        public WizControlPage(WizLightModel model)
        {
            BindingContext = model;
            InitializeComponent();

            TempSlider.SetBinding(Slider.ValueProperty, "Temperature", BindingMode.TwoWay, new RangeToPercentConverter(model.MinimumTemperature, model.MaximumTemperature));
        }

        private class RangeToPercentConverter : IValueConverter
        {
            private int sourceMinimum, sourceMaximum;
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