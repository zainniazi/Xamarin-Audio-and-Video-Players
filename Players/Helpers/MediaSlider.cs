using System;
using Xamarin.Forms;

namespace Players.Helpers
{
    public class MediaSlider : Slider
    {
        public static readonly BindableProperty IsPressedProperty =
            BindableProperty.Create("IsPressed", typeof(bool), typeof(MediaSlider), default(bool), BindingMode.TwoWay);
        public bool IsPressed
        {
            get => (bool)GetValue(IsPressedProperty);
            set => SetValue(IsPressedProperty, value);
        }
    }
}
