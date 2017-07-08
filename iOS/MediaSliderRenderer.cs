using System;
using Players.Helpers;
using Players.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MediaSlider), typeof(MediaSliderRenderer))]
namespace Players.iOS
{
    public class MediaSliderRenderer : SliderRenderer
    {
        MediaSlider MainSlider => (MediaSlider)Element;
        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetThumbImage(new UIKit.UIImage("SliderPointer"), UIKit.UIControlState.Normal);
                Control.TouchDown += Control_TouchDown;
                Control.TouchUpInside += Control_TouchUpInside;
                Control.TouchUpOutside += Control_TouchUpOutside;
            }
        }
        void Control_TouchUpOutside(object sender, EventArgs e)
        {
            MainSlider.IsPressed = false;
        }

        void Control_TouchUpInside(object sender, EventArgs e)
        {
            MainSlider.IsPressed = false;
        }

        void Control_TouchDown(object sender, EventArgs e)
        {
            MainSlider.IsPressed = true;
        }
    }
}
