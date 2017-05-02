using System;
using Android.App;
using Android.Content;
using Xamarin.Forms.Platform.Android;
using App1.Custom.ButtonCustom;
using Xamarin.Forms;
using App1.Droid;
using Object = Java.Lang.Object;
using View = Android.Views.View;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Android.Runtime;

[assembly: ExportRenderer(typeof(FacebookLoginButtonCustom), typeof(FacebookLoginButtonRenderer))]

namespace App1.Droid
{
    public class FacebookLoginButtonRenderer : ButtonRenderer
    {
        private static Activity _activity;


        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            _activity = this.Context as Activity;


            if (this.Control != null)
            {
                Android.Widget.Button button = this.Control;
                button.SetOnClickListener(ButtonClickListener.Instance.Value);
            }
        }

        private class ButtonClickListener : Object, IOnClickListener
        {
            public static readonly Lazy<ButtonClickListener> Instance = new Lazy<ButtonClickListener>(() => new ButtonClickListener());

            public void OnClick(View v)
            {
                var myIntent = new Intent(_activity, typeof(FacebookLoginActivity));
                _activity.StartActivityForResult(myIntent, 0);
            }

            
        }
    }
}