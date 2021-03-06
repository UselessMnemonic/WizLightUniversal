﻿using System;
using Xamarin.Forms;

namespace WizLightUniversal.Core.Views
{
    public class ToggleButton : Button
    {
        public event EventHandler<ToggledEventArgs> Toggled;

        public static BindableProperty IsToggledProperty =
            BindableProperty.Create("IsToggled", typeof(bool), typeof(ToggleButton),
                                    false, propertyChanged: OnIsToggledChanged);

        public ToggleButton() : base()
        {
            Clicked += (sender, args) => IsToggled ^= true;
        }

        public bool IsToggled
        {
            set => SetValue(IsToggledProperty, value);
            get => (bool)GetValue(IsToggledProperty);
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            VisualStateManager.GoToState(this, "ToggledOff");
        }

        static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ToggleButton toggleButton = (ToggleButton)bindable;
            bool isToggled = (bool)newValue;

            // Fire event
            toggleButton.Toggled?.Invoke(toggleButton, new ToggledEventArgs(isToggled));

            // Set the visual state
            _ = VisualStateManager.GoToState(toggleButton, isToggled ? "ToggledOn" : "ToggledOff");
        }
    }
}
