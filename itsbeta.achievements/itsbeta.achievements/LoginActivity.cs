using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Webkit;

namespace itsbeta.achievements
{
    [Activity(Label = "itsbeta", MainLauncher = true, Icon = "@drawable/Login_logo",
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.LoginActivityLayout);

            ImageButton loginButton = FindViewById<ImageButton>(Resource.Id.login);

            loginButton.Click += delegate
            {
                //Finish();
                StartActivity(typeof(LoginWebActivity));
            };
        }

        
    }
}

