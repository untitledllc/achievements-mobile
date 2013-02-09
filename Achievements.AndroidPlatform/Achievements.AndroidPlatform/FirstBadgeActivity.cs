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
using ItsBeta.Core;

namespace itsbeta.achievements
{
    [Activity(Label = "123", Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class FirstBadgeActivity : Activity
    {
        public static Achieves.ParentCategory[] _achievesArray;
        public static Achieves _achievesInfo;

        Animation buttonClickAnimation;

        string access_token = "059db4f010c5f40bf4a73a28222dd3e3";

        string player_id = AppInfo._user.ItsBetaUserId;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _achievesInfo = new Achieves(access_token, player_id);
            _achievesArray = _achievesInfo.ParentCategoryArray();

            MainActivity._selectedCategoriesDictionary = new Dictionary<string, bool>();
            MainActivity._selectedSubCategoriesDictionary = new Dictionary<string, bool>();

            if (!LoginWebActivity.isPlayerExist)
            {

                buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
                SetContentView(Resource.Layout.FirstBadgeLayout);

                TextView userName = FindViewById<TextView>(Resource.Id.usertextView);
                if (AppInfo._user.Fullname != null)
                {
                    userName.Text = AppInfo._user.Fullname;
                }

                Button badgeReadyButton = FindViewById<Button>(Resource.Id.badgereadybutton);

                badgeReadyButton.Click += delegate
                {
                    badgeReadyButton.StartAnimation(buttonClickAnimation);
                    Finish();
                    StartActivity(typeof(MainActivity));
                };
            }
            else
            {
                Finish();
                StartActivity(typeof(MainActivity));
            }

        }
    }
}