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
using itsbeta.achievements.gui;
using System.IO;
using Android.Views.Animations;
using Android.Graphics;
using ItsBeta.Core;
using System.Threading;
using ZXing.Mobile;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public partial class MainScreenActivity : Activity
    {
        static Animation _buttonClickAnimation;
        static Animation _fadeoutClickAnimation;
        static Button _inactiveListButton;
        static Button _inactiveAllButton;
        static Vibrator _vibe;
        static TextView _refreshAchTextView;
        public static Context _context;

        public static bool _isAchListItemClicked = false;
        public static TextView _foundActionTextView;
        public static bool _isLogout = false;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _context = this;
            SetContentView(Resource.Layout.SecondScreenActivityLayout);
            _foundActionTextView = new TextView(this);
            _vibe = (Vibrator)this.GetSystemService(Context.VibratorService);

            ImageButton profileImageButton = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_ProfileScreenImageButton);
            ImageButton profileImageButtonFake = FindViewById<ImageButton>(Resource.Id.secondscr_NavBar_ProfileScreenImageButtonFake);
            TextView badgesCount = FindViewById<TextView>(Resource.Id.NavBar_AchievesCountTextView);

            badgesCount.Text = AppInfo._badgesCount.ToString();
            profileImageButtonFake.Click += delegate { _vibe.Vibrate(50); profileImageButton.StartAnimation(_buttonClickAnimation); StartActivity(typeof(ProfileActivity)); };


            //.............................................................................
            _refreshAchTextView = new TextView(this);
            _badgePopupWindow = new PopupWindow(this);
            _vibe = (Vibrator)this.GetSystemService(Context.VibratorService);
            _inactiveListButton = FindViewById<Button>(Resource.Id.secondscr_inactiveListButton);
            _inactiveAllButton = FindViewById<Button>(Resource.Id.secondscr_inactiveAllButton);
            _inactiveAllButton.Visibility = ViewStates.Gone;
            _inactiveListButton.Visibility = ViewStates.Gone;

            _buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);
            _fadeoutClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeOut);
            _categoryViewRelativeLayout = FindViewById<RelativeLayout>(Resource.Id.secondscr_categrowrelativeLayout);
            _subcategoryViewRelativeLayout = FindViewById<RelativeLayout>(Resource.Id.secondscr_projectsrelativeLayout);

            GetCategoryView();
            GetProjectsView();
            GetAchievementsView();
            GetActivationDialog();

            _refreshProjectsAndAchTextView.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(_refreshProjectsAndAchTextView_TextChanged);
            _refreshAchTextView.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(_refreshAchTextView_TextChanged);
            _subcategoryViewRelativeLayout.Click += new EventHandler(_subcategoryViewRelativeLayout_Click);
            _inactiveListButton.Click += new EventHandler(_inactiveListButton_Click);
            _inactiveAllButton.Click += new EventHandler(_inactiveAllButton_Click);
            _achievementsListView.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(achievementsListView_ItemClick);

            
        }

        void _inactiveAllButton_Click(object sender, EventArgs e)
        {
            _inactiveAllButton.Visibility = ViewStates.Gone;
        }

        void _inactiveListButton_Click(object sender, EventArgs e)
        {
            if (_isProjectsListOpen)    
            {
                _subcategoryViewRelativeLayout_Click(new object(),new EventArgs());
            }
            if (isCategoriesListOpen)
            {
                _categoryViewRelativeLayout_Click(new object(), new EventArgs());
            }
        }

        public override void Finish()
        {
            base.Finish();
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (_isLogout)
            {
                Finish();
                _isLogout = false;
                StartActivity(typeof(LoginActivity));
            }
        }

        public override void OnBackPressed()
        {
            if (_categoriesListView.Visibility == ViewStates.Visible)
            {
                _categoriesListView.Visibility = ViewStates.Gone;
                _inactiveListButton.Visibility = ViewStates.Gone;
                isCategoriesListOpen = false;
                return;
            }
            if (_subcategoriesListView.Visibility == ViewStates.Visible)
            {
                _subcategoriesListView.Visibility = ViewStates.Gone;
                _inactiveListButton.Visibility = ViewStates.Gone;
                _isProjectsListOpen = false;
                return;
            }
            if (_badgePopupWindow.IsShowing)
            {
                _badgePopupWindow.Dismiss(); return;
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}