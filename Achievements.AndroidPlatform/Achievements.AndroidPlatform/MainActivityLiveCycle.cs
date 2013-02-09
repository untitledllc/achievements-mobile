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
    [Activity(Label = "Achievements", 
        Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public partial class MainActivity : Activity
    {
        
        public static TextView RefreshEventListTextView;
        public static TextView AchieveListSelectedEventTextView;

        public static Display _display;
        bool _isBarCategoriesListOpen = false;
        ListView _categoriesListView;

        public static Dictionary<string,bool> _selectedCategoriesDictionary = new Dictionary<string,bool>();
        public static Dictionary<string, bool> _selectedSubCategoriesDictionary = new Dictionary<string, bool>();

        Animation categoriesChosedAnimation;
        Animation buttonClickAnimation;

        ImageButton _navigationBarImageButton;
        LinearLayout categoriesLinearLayout;
        ImageButton _navigationBarMenuImageButton;

        public static Achieves.ParentCategory[] _achievesArray;
        public static Achieves _achievesInfo;

        protected override void OnCreate(Bundle bundle)
        {
            _achievesInfo = FirstBadgeActivity._achievesInfo;
            _achievesArray = FirstBadgeActivity._achievesInfo.ParentCategoryArray();

            base.OnCreate(bundle);


            _display = WindowManager.DefaultDisplay;

            SetContentView(Resource.Layout.MainActivityLayout);

            categoriesChosedAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.SlideInLeft);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);

            categoriesLinearLayout = FindViewById<LinearLayout>(Resource.Id.SelectedCategoriesLinearLayout);
            _navigationBarImageButton = FindViewById<ImageButton>(Resource.Id.NavigationBarImageButton);
            _navigationBarMenuImageButton = FindViewById<ImageButton>(Resource.Id.NavigationBarMenuImageButton);

            
            _navigationBarMenuImageButton.Click += delegate { StartActivity(typeof(ProfileActivity)); };

            #region CategoriesList Local
            CreateCategoriesViewObject();
            #endregion

            #region SubCategories Local
            CreateSubCategoriesViewObject();
            #endregion

            #region AchievementsList Local
            CreateAchievementsViewObject();

            #endregion

            RefreshEventListTextView = new TextView(this);
            RefreshEventListTextView.TextChanged += delegate
            {
                CreateAchievementsViewObject();
            };

            AchieveListSelectedEventTextView = new TextView(this);

            AchieveListSelectedEventTextView.TextChanged += new EventHandler<Android.Text.TextChangedEventArgs>(achievementsListView_Click);
        }

        public static bool isFinishFromProfile = false;
        protected override void OnResume()
        {
            base.OnResume();
            if (isFinishFromProfile)
            {
                _selectedCategoriesDictionary = new Dictionary<string, bool>();
                _selectedSubCategoriesDictionary = new Dictionary<string, bool>();
                isFinishFromProfile = false;
                LoginActivity.isCommingFromProfile = true;
                Finish();
                StartActivity(typeof(LoginActivity));
            }
        }

    }

}