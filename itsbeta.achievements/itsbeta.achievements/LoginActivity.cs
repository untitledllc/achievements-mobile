using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Webkit;
using Android.Views.Animations;
using System.IO;
using FlurryLib;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        Animation buttonClickAnimation;
        FlurryClient _flurryClient;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _flurryClient = new FlurryClient();
            AppInfo._display = WindowManager.DefaultDisplay;
            
            AppInfo.IsLocaleRu = false;
            var locale =  Java.Util.Locale.Default.DisplayLanguage;
            if (locale == "русский")
            {
                AppInfo.IsLocaleRu = true;
            }


            AppInfo._badgesCount = 0;
            AppInfo._subcategCount = 0;
            AppInfo._bonusesCount = 0;
            
            Vibrator vibe = (Vibrator)this.GetSystemService(Context.VibratorService);


            SetContentView(Resource.Layout.loginactivitylayout);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, Android.Resource.Animation.FadeIn);

            ImageButton loginButton = FindViewById<ImageButton>(Resource.Id.login);
            TextView signUpTextView = FindViewById<TextView>(Resource.Id.signUpTextView);
            TextView signInTextView = FindViewById<TextView>(Resource.Id.login_signinTextView);
            TextView centerTextView = FindViewById<TextView>(Resource.Id.login_centerTextView);

            if (!AppInfo.IsLocaleRu)
            {
                signUpTextView.Text = "About project";
                signInTextView.Text = "               Sign in with";
                centerTextView.Text = "Collect all your achievements";
            }


            //login_centerTextView
            signUpTextView.Click += delegate
            {
                Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://itsbeta.com/"));
                StartActivity(browserIntent);
            };

            loginButton.Click += delegate
            {
                loginButton.StartAnimation(buttonClickAnimation);
                vibe.Vibrate(50);
                Finish();
                StartActivity(typeof(LoginWebActivity));
            };
        }


        protected override void OnStart()
        {
            base.OnStart();
            try
            {
                _flurryClient.OnStartActivity(this);
            }
            catch { }
        }

        protected override void OnStop()
        {
            base.OnStop();
            try
            {
                _flurryClient.OnStopActivity(this);
            }
            catch { }
        }
    }
}

