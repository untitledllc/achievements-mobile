using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

namespace Achievements.AndroidPlatform
{
    [Activity(Label = "Achievements", MainLauncher = true,
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginScreenActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.LoginActivityLayout);

            Button signInButton = FindViewById<Button>(Resource.Id.signin_button);

            signInButton.Click += delegate 
            { 
                Finish();
                StartActivity(typeof(MainActivity));
            };
        }
    }
}