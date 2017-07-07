using System;
using Xamarin.Forms;

namespace Players.Helpers
{
    public class AudioSlider : Slider
    {
        public static readonly BindableProperty IsPressedProperty =
            BindableProperty.Create("IsPressed", typeof(bool), typeof(AudioSlider), default(bool), BindingMode.TwoWay);
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set
            {
                SetValue(IsPressedProperty, value);
            }
        }
    }
}
