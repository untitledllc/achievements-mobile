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

            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            SetContentView(Resource.Layout.LoginActivityLayout);

            ImageButton loginButton = FindViewById<ImageButton>(Resource.Id.login);

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

