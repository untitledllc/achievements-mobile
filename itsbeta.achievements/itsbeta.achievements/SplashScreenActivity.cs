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
using System.IO;
using System.Timers;

namespace itsbeta.achievements
{
    [Activity(Label = "itsbeta", MainLauncher = true,
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : Activity
    {
        private Timer _freezeTimer;

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            AppInfo._badgesCount = 0;
            AppInfo._subcategCount = 0;
            AppInfo._bonusesCount = 0;

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
            else
            {
                SetContentView(Resource.Layout.splashactivitylayout);

                _freezeTimer = new Timer();
                _freezeTimer.Interval = 1500;
                _freezeTimer.Start();
                _freezeTimer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            }            
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //timer.Stop();
            _freezeTimer.Stop();
            StartActivity(typeof(LoginActivity));
            OnDestroy();
        }


    }
}