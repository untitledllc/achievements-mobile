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
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class FirstBadgeActivity : Activity
    {
        public static Achieves.ParentCategory[] _achievesArray;
        public static Achieves _achievesInfo;

        Animation buttonClickAnimation;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (LoginWebActivity.isPlayerExist)
            {

                buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
                SetContentView(Resource.Layout.FirstBadgeActivityLayout);

                TextView userName = FindViewById<TextView>(Resource.Id.FirstBadge_ProfileNameTextView);
                if (AppInfo._user.Fullname != null)
                {
                    userName.Text = AppInfo._user.Fullname;
                }

                ImageButton badgeReadyButton = FindViewById<ImageButton>(Resource.Id.BadgeSheet_CloseImageButton);
                ImageButton badgeReadyButtonFake = FindViewById<ImageButton>(Resource.Id.BadgeSheet_CloseImageButtonFake);

                badgeReadyButton.Click += delegate
                {
                    badgeReadyButtonFake.StartAnimation(buttonClickAnimation);
                    Finish();
                    StartActivity(typeof(SecondScreenActivity));
                };
            }
            else
            {
                Finish();
                StartActivity(typeof(SecondScreenActivity));
            }

        }
    }
}