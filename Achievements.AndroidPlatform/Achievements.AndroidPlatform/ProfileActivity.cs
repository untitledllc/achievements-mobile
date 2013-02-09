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
using Android.Webkit;
using System.IO;

namespace itsbeta.achievements
{
    [Activity(Label = "Achievements", Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProfileActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ProfileActivityLayout);

            TextView userName = FindViewById<TextView>(Resource.Id.userName);
            userName.Text = AppInfo._user.Fullname;

            TextView userInfo = FindViewById<TextView>(Resource.Id.UserInfo);
            userInfo.Text = GetUserAge(AppInfo._user.BirthDate).ToString();

            Button logoutButton = FindViewById<Button>(Resource.Id.logoutbutton);
            logoutButton.Click += delegate
            {
                MainActivity.isFinishFromProfile = true;
                CookieManager.Instance.RemoveAllCookie();
                File.Delete(@"/data/data/itsbeta.achievements/data.txt");
                Finish();
                StartActivity(typeof(LoginActivity));
            };

        }

        int GetUserAge(string date)
        {
            int daynow = DateTime.Now.Day;
            int monthnow = DateTime.Now.Month;
            int yearnow = DateTime.Now.Year;

            var inputdate = date.Split('/');

            int inputday;
            int inputmonth;
            int inputyear;

            int.TryParse(inputdate[1], out inputday);
            int.TryParse(inputdate[0], out inputmonth);
            int.TryParse(inputdate[2], out inputyear);

            int age = yearnow - inputyear;

            return age;
        }
    }
}