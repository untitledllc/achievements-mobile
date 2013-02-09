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
using System.IO;
using System.Threading;

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

            if (!File.Exists(@"/data/data/itsbeta.achievements/data.txt"))
            {
                List<string> config = new List<string>();

                config.Add(AppInfo._user.Fullname);
                config.Add(AppInfo._user.BirthDate);
                config.Add(AppInfo._user.FacebookUserID);
                config.Add(AppInfo._user.ItsBetaUserId);

                File.WriteAllLines(@"/data/data/istbeta.achievements/data.txt", config.ToArray(), Encoding.UTF8);
            }

            SetContentView(Resource.Layout.SecondScreenActivityLayout);
            ProgressDialog mDialog = new ProgressDialog(this);
            mDialog.SetMessage("Loading...");
            mDialog.SetCancelable(false);
            mDialog.Show();

            

            ThreadStart threadStart = new ThreadStart(treadStartVoid);
            new Thread(threadStart).Start();

            
            

            if (!LoginWebActivity.isPlayerExist)
            {
                mDialog.Hide();
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
                mDialog.Hide();
                Finish();
                StartActivity(typeof(SecondScreenActivity));
            }

        }

        void treadStartVoid()
        {
            AppInfo._achievesInfo = new Achieves(AppInfo._access_token, AppInfo._user.ItsBetaUserId);
        }
    }
}