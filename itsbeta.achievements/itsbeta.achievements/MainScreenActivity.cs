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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SecondScreenActivityLayout);

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

            _refreshProjectsTextView.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(_refreshProjectsTextView_TextChanged);
            _subcategoryViewRelativeLayout.Click += new EventHandler(_subcategoryViewRelativeLayout_Click);
            _inactiveListButton.Click += new EventHandler(_inactiveListButton_Click);
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
    }
}