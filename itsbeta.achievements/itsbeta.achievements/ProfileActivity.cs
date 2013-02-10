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
using Android.Views.Animations;
using Android.Webkit;
using System.IO;
using System.Collections;
using itsbeta.achievements.gui;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProfileActivity : Activity
    {
        Animation buttonClickAnimation;
        public static ProfileActivity _context;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            SetContentView(Resource.Layout.ProfileScreenActivityLayout);

            _context = this;

            ImageButton logoutImageButton = FindViewById<ImageButton>(Resource.Id.profilescr_NavBar_LogoutImageButton);

            logoutImageButton.Click += delegate
            {
                CookieManager.Instance.RemoveAllCookie();
                SecondScreenActivity._context.Finish();
                File.Delete(@"/data/data/itsbeta.achievements/data.txt");
                Finish();
                StartActivity(typeof(LoginActivity));
            };

            TextView userFullname = FindViewById<TextView>(Resource.Id.profilescr_usernameTextView);
            TextView userData = FindViewById<TextView>(Resource.Id.profilescr_userAgelocTextView);

            TextView badgesCount = FindViewById<TextView>(Resource.Id.profilescr_allbadgescountTextView);
            TextView bonusesCount = FindViewById<TextView>(Resource.Id.profilescr_bonusCountTextView);
            TextView subCategoriesCount = FindViewById<TextView>(Resource.Id.profilescr_subcategCountTextView);

            userFullname.Text = AppInfo._user.Fullname;
            userData.Text = GetUserAge(AppInfo._user.BirthDate);

            badgesCount.Text = AppInfo._badgesCount.ToString();
            bonusesCount.Text = AppInfo._bonusesCount.ToString();
            subCategoriesCount.Text = AppInfo._subcategCount.ToString();


            IList<CategoriesListData> categoriesList = new List<CategoriesListData>();

            for (int i = 0; i < AppInfo._achievesInfo.CategoriesCount; i++)
            {
                categoriesList.Add(new CategoriesListData()
                {
                    CategoryNameText = String.Format("{0}",
                        AppInfo._achievesInfo.CategoryArray[i].DisplayName),
                });
            }

            ListView categoriesListView = FindViewById<ListView>(Resource.Id.profilescr_categListView);

            ProfileScrCategoriesListItemAdapter categoriesAdapter = new ProfileScrCategoriesListItemAdapter(this, Resource.Layout.ProfileScreenParentRow,
                categoriesList);

            categoriesListView.Adapter = categoriesAdapter;
            categoriesListView.DividerHeight = 0;

        }

        string GetUserAge(string date)
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
            if (inputmonth > monthnow)
            {
                age--;
            }
            if (inputmonth == monthnow && inputday > daynow)
            {
                age--;
            }

            return age.ToString();
        }
    }
}