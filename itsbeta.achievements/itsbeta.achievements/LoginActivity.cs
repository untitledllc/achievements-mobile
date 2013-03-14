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

namespace itsbeta.achievements
{
    [Activity(Label = "itsbeta", MainLauncher = true,
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity
    {
        Animation buttonClickAnimation;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            AppInfo._display = WindowManager.DefaultDisplay;
            AppInfo._badgesCount = 0;
            AppInfo._subcategCount = 0;
            AppInfo._bonusesCount = 0;
            

            Vibrator vibe = (Vibrator)this.GetSystemService(Context.VibratorService);

            if (File.Exists(@"/data/data/ru.hintsolutions.itsbeta/data.txt"))
            {
                LoginWebActivity.isPlayerExist = true;
                LoginWebActivity.isAppBadgeEarned = true;

                AppInfo._user.Fullname = File.ReadAllLines(@"/data/data/ru.hintsolutions.itsbeta/data.txt")[0];
                AppInfo._user.BirthDate = File.ReadAllLines(@"/data/data/ru.hintsolutions.itsbeta/data.txt")[1];
                AppInfo._user.FacebookUserID = File.ReadAllLines(@"/data/data/ru.hintsolutions.itsbeta/data.txt")[2];
                AppInfo._user.ItsBetaUserId = File.ReadAllLines(@"/data/data/ru.hintsolutions.itsbeta/data.txt")[3];
                AppInfo._user.City = File.ReadAllLines(@"/data/data/ru.hintsolutions.itsbeta/data.txt")[4];

                Finish();
                StartActivity(typeof(FirstBadgeActivity));
            }

            
            SetContentView(Resource.Layout.LoginActivityLayout);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, Android.Resource.Animation.FadeIn);


            ImageButton loginButton = FindViewById<ImageButton>(Resource.Id.login);
            TextView signUpTextView = FindViewById<TextView>(Resource.Id.signUpTextView);

            signUpTextView.Click += delegate
            {
                Intent browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://www.facebook.com/r.php"));
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
    }
}

